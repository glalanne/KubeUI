using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Avalonia.Data;
using Dock.Model.Controls;
using Dock.Model.Core;
using DynamicData;
using DynamicData.Binding;
using FluentAvalonia.UI.Controls;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia.Fluent;
using k8s;
using k8s.KubeConfigModels;
using k8s.Models;
using KubeUI.Client;
using KubeUI.Client.Informer;
using KubeUI.Controls;
using Swordfish.NET.Collections;

namespace KubeUI.ViewModels;

[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
internal class DemoResourceListViewModel : ResourceListViewModel<V1Pod> { public DemoResourceListViewModel() { } }

public partial class ResourceListViewModel<T> : ViewModelBase, IDisposable where T : class, IKubernetesObject<V1ObjectMeta>, new()
{
    private readonly ILogger<ResourceListViewModel<T>> _logger;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private Client.Cluster _cluster;

    [ObservableProperty]
    private GroupApiVersionKind _kind;

    [ObservableProperty]
    private ConcurrentObservableDictionary<NamespacedName, T> _objects;

    [ObservableProperty]
    private object _selectedItem;

    [ObservableProperty]
    private object _selectedNamespaces;

    [ObservableProperty]
    private ReadOnlyObservableCollection<KeyValuePair<NamespacedName, T>> _dataGridObjects;

    [ObservableProperty]
    private ConcurrentObservableDictionary<NamespacedName, V1Namespace> _namespaces;

    [ObservableProperty]
    private string _searchQuery;

    [ObservableProperty]
    private ResourceListViewDefinition<T> _viewDefinition;

    private IDisposable _filter;

    [GeneratedRegex("types '(.+)' and '(.+)'", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex TypeErrorRegex();

    public ResourceListViewModel()
    {
        Factory = Application.Current.GetRequiredService<IFactory>();
        _logger = Application.Current.GetRequiredService<ILogger<ResourceListViewModel<T>>>();
        _dialogService = Application.Current.GetRequiredService<IDialogService>();
    }

    public void Initialize()
    {
        _filter = Objects.ToObservableChangeSet<ConcurrentObservableDictionary<NamespacedName, T>, KeyValuePair<NamespacedName, T>>()
            .Bind(out var filteredObjects)
            .Subscribe();

        DataGridObjects = filteredObjects;

        Namespaces = Cluster.GetObjectDictionary<V1Namespace>();

        ViewDefinition = GetViewDefinition<T>();
    }

    private void SetFilter()
    {
        _filter.Dispose();

        _filter = Objects.ToObservableChangeSet<ConcurrentObservableDictionary<NamespacedName, T>, KeyValuePair<NamespacedName, T>>()
            .Filter(GenerateFilter())
            .Bind(out var filteredObjects)
            .Subscribe();

        DataGridObjects = filteredObjects;
    }

    private Func<KeyValuePair<NamespacedName, T>, bool> GenerateFilter()
    {
        var param = Expression.Parameter(typeof(KeyValuePair<NamespacedName, T>), "p");

        var key = Expression.PropertyOrField(param, "Key");

        var value = Expression.PropertyOrField(param, "Value");

        BinaryExpression? body = null;

        BinaryExpression? namespaceFilter = null;

        BinaryExpression? searchFilter = null;

        if (SelectedNamespaces != null)
        {
            namespaceFilter = Expression.Equal(
                    Expression.PropertyOrField(key, "Namespace"),
                    Expression.Constant(((V1Namespace)SelectedNamespaces).Name())
               );
        }

        if(!string.IsNullOrEmpty(SearchQuery))
        {
            var method = typeof(string).GetMethod(nameof(string.IndexOf), [typeof(string), typeof(StringComparison)]);

            foreach (var query in SearchQuery.Split(' '))
            {
                if (string.IsNullOrEmpty(query))
                {
                    continue;
                }

                BinaryExpression? wordFilter = null;

                foreach (var column in ViewDefinition.Columns)
                {
                    var someValue = Expression.Constant(query, typeof(string));

                    var colType = column.GetType();

                    var columnDisplay = (Func<T, string>)colType.GetProperty(nameof(ResourceListViewDefinitionColumn<V1Pod, string>.Display)).GetValue(column);

                    columnDisplay ??= (Func<T, string>)colType.GetProperty(nameof(ResourceListViewDefinitionColumn<V1Pod, string>.Field)).GetValue(column);

                    var funcCall = Expression.Call(Expression.Constant(columnDisplay), columnDisplay.GetType().GetMethod("Invoke"), value);

                    var expression = Expression.GreaterThanOrEqual(Expression.Call(funcCall, method, someValue, Expression.Constant(StringComparison.OrdinalIgnoreCase)), Expression.Constant(0));

                    wordFilter = wordFilter == null ? expression : Expression.OrElse(wordFilter, expression);
                }

                searchFilter = searchFilter == null ? wordFilter : Expression.AndAlso(searchFilter, wordFilter);
            }
        }

        if (namespaceFilter != null && searchFilter == null)
        {
            body = namespaceFilter;
        }
        else if (namespaceFilter == null && searchFilter != null)
        {
            body = searchFilter;
        }
        else if (namespaceFilter != null && searchFilter != null)
        {
            body = Expression.AndAlso(namespaceFilter, searchFilter);
        }
        else
        {
            body = Expression.Equal(Expression.Constant(true), Expression.Constant(true));
        }

        return Expression.Lambda<Func<KeyValuePair<NamespacedName, T>, bool>>(body, param).Compile();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.PropertyName == nameof(SelectedNamespaces))
        {
            SetFilter();
        }

        if (e.PropertyName == nameof(SearchQuery))
        {
            SetFilter();
        }
    }

    private ResourceListViewDefinition<T> GetViewDefinition<T>() where T : class, IKubernetesObject<V1ObjectMeta>, new()
    {
        var resourceType = typeof(T);

        ResourceListViewDefinition<T> definition = new();

        if (resourceType == typeof(V1Node))
        {
            definition.ShowNamespaces = false;

            definition.Columns = new()
            {
                new ResourceListViewDefinitionColumn<V1Node, string>()
                {
                    Name = "Name",
                    Field = x => x.Metadata.Name,
                    Sort = SortDirection.Ascending,
                    Width = "4*",
                },
                new ResourceListViewDefinitionColumn<V1Node, string>()
                {
                    Name = "CPU",
                    Field = x => x.Status.Allocatable["cpu"].Value,
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Node, string>()
                {
                    Name = "Memory",
                    Field = x => x.Status.Allocatable["memory"].Value,
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Node, string>()
                {
                    Name = "Disk",
                    Field = x => x.Status.Allocatable["ephemeral-storage"].Value,
                    Width = nameof(DataGridLengthUnitType.SizeToCells)
                },
                new ResourceListViewDefinitionColumn<V1Node, string>()
                {
                    Name = "Taints",
                    Field = x => x.Metadata.Annotations.TryGetValue("scheduler.alpha.kubernetes.io/taints", out var value) ? value : "",
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Node, string>()
                {
                    Name = "Roles",
                    Field = x => x.Metadata.Annotations.Any(x => x.Key.StartsWith("node-role.kubernetes.io/")) ? x.Metadata.Annotations.Where(x => x.Key.StartsWith("node-role.kubernetes.io/")).Select(x => x.Value).Aggregate((x,y) => x + ", " + y) : "",
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Node, string>()
                {
                    Name = "Version",
                    Field = x => x.Status.NodeInfo.KubeletVersion,
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Node, string>()
                {
                    Name = "Status",
                    Field = x => x.Status.Conditions.FirstOrDefault(x => x.Type == "Ready")?.Reason ?? "",
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Node, DateTime?>()
                {
                    Name = "Age",
                    CustomControl = typeof(AgeCell),
                    Field = x => x.Metadata.CreationTimestamp,
                    Display = x => x.Metadata.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    Width = "80"
                }
            };
        }
        else if (resourceType == typeof(V1Namespace))
        {
            definition.ShowNamespaces = false;

            definition.Columns = new()
            {
                new ResourceListViewDefinitionColumn<V1Namespace, string>()
                {
                    Name = "Name",
                    Field = x => x.Metadata.Name,
                    Sort = SortDirection.Ascending,
                    Width = "4*",
                },
                new ResourceListViewDefinitionColumn<V1Namespace, string>()
                {
                    Name = "Labels",
                    Field = x => x.Metadata.Labels.Select(x => x.Key + "=" + x.Value).Aggregate((x,y) => x + ", " + y),
                    Width = "2*"
                },
                new ResourceListViewDefinitionColumn<V1Namespace, string>()
                {
                    Name = "Status",
                    Field = x => x.Status.Phase,
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Namespace, DateTime?>()
                {
                    Name = "Age",
                    CustomControl = typeof(AgeCell),
                    Field = x => x.Metadata.CreationTimestamp,
                    Display = x => x.Metadata.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss"),
                    Width = "80"
                }
            };
        }
        else if (resourceType == typeof(V1Pod))
        {
            definition.Columns = new()
            {
                new ResourceListViewDefinitionColumn<V1Pod, string>()
                {
                    Name = "Name",
                    Field = x => x.Metadata.Name,
                    Sort = SortDirection.Ascending,
                    Width = "4*",
                },
                new ResourceListViewDefinitionColumn<V1Pod, int>()
                {
                    Name = "Containers",
                    CustomControl = typeof(PodContainerCell),
                    Field = x => x.Spec.Containers.Count + ((x.Spec.InitContainers?.Count) ?? 0),
                    Display = x => (x.Spec.Containers.Count + ((x.Spec.InitContainers?.Count) ?? 0)).ToString(),
                    Width = nameof(DataGridLengthUnitType.SizeToCells)
                },
                new ResourceListViewDefinitionColumn<V1Pod, string>()
                {
                    Name = "Namespace",
                    Field = x => x.Metadata.NamespaceProperty,
                    Width = "*"
                },
                new ResourceListViewDefinitionColumn<V1Pod, int>()
                {
                    Name = "Restarts",
                    Field = x => x.Status.ContainerStatuses.Sum(x => x.RestartCount),
                    Display = x => x.Status.ContainerStatuses?.Sum(x => x.RestartCount).ToString() ?? "0",
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Pod, string>()
                {
                    Name = "Controlled By",
                    Field = x => x.Metadata.OwnerReferences.FirstOrDefault()?.Name ?? "",
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Pod, string>()
                {
                    Name = "Node",
                    Field = x => x.Spec.NodeName,
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Pod, string>()
                {
                    Name = "QoS",
                    Field = x => x.Status.QosClass,
                    Width = nameof(DataGridLengthUnitType.SizeToCells)
                },
                new ResourceListViewDefinitionColumn<V1Pod, string>()
                {
                    Name = "Status",
                    Field = x => x.Status.Phase,
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Pod, DateTime?>()
                {
                    Name = "Age",
                    CustomControl = typeof(AgeCell),
                    Field = x => x.Metadata.CreationTimestamp,
                    Display = x => x.Metadata.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    Width = "80"
                }
            };

            definition.MenuItems = new()
            {
                new()
                {
                    Header = "View Console",
                    ItemSourcePath = "SelectedItem.Value.Spec.Containers",
                    MenuItem = new()
                    {
                        HeaderBinding = new Binding(nameof(V1Container.Name)),
                        CommandPath = nameof(ResourceListViewModel<V1Pod>.ViewConsoleCommand),
                        CommandParameterPath = nameof(V1Container.Name),
                    }
                },
                new()
                {
                    Header = "View Logs",
                    ItemSourcePath = "SelectedItem.Value.Spec.Containers",
                    MenuItem = new()
                    {
                        HeaderBinding = new Binding(nameof(V1Container.Name)),
                        CommandPath = nameof(ResourceListViewModel<V1Pod>.ViewLogsCommand),
                        CommandParameterPath = nameof(V1Container.Name),
                    }
                },
                new()
                {
                    Header = "Port Forwarding",
                    ItemSourcePath = "SelectedItem.Value.Spec.Containers",
                    MenuItem = new()
                    {
                        HeaderBinding = new Binding(nameof(V1Container.Name)),
                        ItemSourcePath = nameof(V1Container.Ports),
                        MenuItem = new()
                        {
                            HeaderBinding = new MultiBinding()
                            {
                                Bindings = [ new Binding(nameof(V1ContainerPort.Name)), new Binding(nameof(V1ContainerPort.ContainerPort)) ],
                                StringFormat = "{0} - {1}"
                            },
                            CommandPath = nameof(ResourceListViewModel<V1Pod>.PortForwardCommand),
                            CommandParameterPath = nameof(V1ContainerPort.ContainerPort),
                        }
                    }
                }
            };
        }
        else if (resourceType == typeof(V1Deployment))
        {
            definition.Columns = new()
            {
                new ResourceListViewDefinitionColumn<V1Deployment, string>()
                {
                    Name = "Name",
                    Field = x => x.Metadata.Name,
                    Sort = SortDirection.Ascending,
                    Width = "2*",
                },
                new ResourceListViewDefinitionColumn<V1Deployment, string>()
                {
                    Name = "Namespace",
                    Field = x => x.Metadata.NamespaceProperty,
                    Width = "*",
                },
                new ResourceListViewDefinitionColumn<V1Deployment, int>()
                {
                    Name = "Pods",
                    Display = x => $"{x.Status?.AvailableReplicas.GetValueOrDefault()}/{x.Spec?.Replicas}",
                    Field = x => x.Status.AvailableReplicas.GetValueOrDefault(),
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Deployment, int>()
                {
                    Name = "Replicas",
                    Display = x => x.Spec.Replicas.GetValueOrDefault().ToString(),
                    Field = x => x.Spec.Replicas.GetValueOrDefault(),
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Deployment, string>()
                {
                    Name = "Available",
                    Field = x => x.Status.Conditions.FirstOrDefault(x => x.Type == "Available")?.Status ?? "",
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<V1Deployment, DateTime?>()
                {
                    Name = "Age",
                    CustomControl = typeof(AgeCell),
                    Field = x => x.Metadata.CreationTimestamp,
                    Display = x => x.Metadata.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    Width = "80"
                }
            };
        }
        else if (resourceType == typeof(V1CustomResourceDefinition))
        {
            definition.ShowNamespaces = false;

            definition.Columns = new()
            {
                new ResourceListViewDefinitionColumn<V1CustomResourceDefinition, string>()
                {
                    Name = "Name",
                    Field = x => x.Spec.Names.Kind,
                    Sort = SortDirection.Ascending,
                    Width = "2*",
                },
                new ResourceListViewDefinitionColumn<V1CustomResourceDefinition, string>()
                {
                    Name = "Group",
                    Field = x => x.Spec.Group,
                    Width = "*",
                },
                new ResourceListViewDefinitionColumn<V1CustomResourceDefinition, string>()
                {
                    Name = "Version",
                    Field = x => x.Spec.Versions.First(x => x.Storage).Name,
                    Width = nameof(DataGridLengthUnitType.SizeToCells)
                },
                new ResourceListViewDefinitionColumn<V1CustomResourceDefinition, string>()
                {
                    Name = "Scope",
                    Field = x => x.Spec.Scope,
                    Width = nameof(DataGridLengthUnitType.SizeToCells)
                },
                new ResourceListViewDefinitionColumn<V1CustomResourceDefinition, DateTime?>()
                {
                    Name = "Age",
                    CustomControl = typeof(AgeCell),
                    Field = x => x.Metadata.CreationTimestamp,
                    Display = x => x.Metadata.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    Width = "80"
                }
            };
        }
        else if (resourceType == typeof(Corev1Event))
        {
            definition.Columns = new()
            {
                new ResourceListViewDefinitionColumn<Corev1Event, string>()
                {
                    Name = "Type",
                    Field = x => x.Type,
                    Width = nameof(DataGridLengthUnitType.SizeToCells)
                },
                new ResourceListViewDefinitionColumn<Corev1Event, string>()
                {
                    Name = "Message",
                    Field = x => x.Message,
                    Width = "4*"
                },
                new ResourceListViewDefinitionColumn<Corev1Event, string>()
                {
                    Name = "Namespace",
                    Field = x => x.Metadata.NamespaceProperty,
                    Width = "*"
                },
                new ResourceListViewDefinitionColumn<Corev1Event, string>()
                {
                    Name = "Involved Object",
                    Field = x => x.InvolvedObject.Name,
                    Width = "*"
                },
                new ResourceListViewDefinitionColumn<Corev1Event, string>()
                {
                    Name = "Source",
                    Field = x => x.Source.Component ?? "",
                    Width = "*"
                },
                new ResourceListViewDefinitionColumn<Corev1Event, int>()
                {
                    Name = "Count",
                    Display = x => (x.Count ?? 0).ToString(),
                    Field = x => x.Count ?? 0,
                    Width = nameof(DataGridLengthUnitType.SizeToHeader)
                },
                new ResourceListViewDefinitionColumn<Corev1Event, DateTime?>()
                {
                    Name = "Last Seen",
                    CustomControl = typeof(LastSeenCell),
                    Field = x => x.LastTimestamp,
                    Display = x => x.LastTimestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    Sort = SortDirection.Descending,
                    Width = "80"
                },
                new ResourceListViewDefinitionColumn<Corev1Event, DateTime?>()
                {
                    Name = "Age",
                    CustomControl = typeof(AgeCell),
                    Field = x => x.Metadata.CreationTimestamp,
                    Display = x => x.Metadata.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    Width = "80"
                },
            };
        }
        else
        {
            definition.Columns = new();

            // Add Name Column
            var nameColumn = new ResourceListViewDefinitionColumn<T, string>()
            {
                Name = "Name",
                Field = x => x.Metadata.Name,
                Sort = SortDirection.Ascending,
                Width = "2*"
            };

            definition.Columns.Add(nameColumn);

            // Check if resource type is CRD
            // TODO: Find a better way to identify CRDs
            if (resourceType.Namespace.StartsWith("KubeUI.Models"))
            {
                // Generate CRD Columns
                var crd = Cluster.GetObject<V1CustomResourceDefinition>(null, Kind.PluralNameGroup);

                var version = crd.Spec.Versions.First(x => x.Storage);

                //Check if its a namespaced crd
                if (crd.Spec.Scope == "Namespaced")
                {
                    // Add Namespace Column
                    var nsColumn = new ResourceListViewDefinitionColumn<T, string>()
                    {
                        Name = "Namespace",
                        Field = x => x.Metadata.NamespaceProperty,
                        Width = "*"
                    };

                    definition.Columns.Add(nsColumn);
                }
                else
                {
                    definition.ShowNamespaces = false;
                }

                if (version.AdditionalPrinterColumns != null)
                {
                    foreach (var item in version.AdditionalPrinterColumns)
                    {
                        start:

                        try
                        {
                            if (item.JsonPath == ".metadata.creationTimestamp")
                            {
                                continue;
                            }

                            if (item.Type == "string")
                            {
                                var exp = JsonPathLINQ.JsonPathLINQ.GetExpression<T, string>(item.JsonPath, true);

                                var colDef = new ResourceListViewDefinitionColumn<T, string>()
                                {
                                    Name = item.Name,
                                    Field = exp.Compile(),
                                    Width = "*"
                                };

                                definition.Columns.Add(colDef);
                            }
                            else if (item.Type == "number")
                            {
                                var exp = JsonPathLINQ.JsonPathLINQ.GetExpression<T, double>(item.JsonPath, true);

                                var colDef = new ResourceListViewDefinitionColumn<T, double>()
                                {
                                    Name = item.Name,
                                    Display = TransformToFuncOfString<T>(exp.Body, exp.Parameters).Compile(),
                                    Field = exp.Compile(),
                                    Width = "*"
                                };

                                definition.Columns.Add(colDef);
                            }
                            else if (item.Type == "integer" && item.Format == "int64")
                            {
                                var exp = JsonPathLINQ.JsonPathLINQ.GetExpression<T, long>(item.JsonPath, true);

                                var colDef = new ResourceListViewDefinitionColumn<T, long>()
                                {
                                    Name = item.Name,
                                    Display = TransformToFuncOfString<T>(exp.Body, exp.Parameters).Compile(),
                                    Field = exp.Compile(),
                                    Width = "*"
                                };

                                definition.Columns.Add(colDef);
                            }
                            else if (item.Type == "integer")
                            {
                                var exp = JsonPathLINQ.JsonPathLINQ.GetExpression<T, int>(item.JsonPath, true);

                                var colDef = new ResourceListViewDefinitionColumn<T, int>()
                                {
                                    Name = item.Name,
                                    Display = TransformToFuncOfString<T>(exp.Body, exp.Parameters).Compile(),
                                    Field = exp.Compile(),
                                    Width = "*"
                                };

                                definition.Columns.Add(colDef);
                            }
                            else if (item.Type == "date")
                            {
                                var exp = JsonPathLINQ.JsonPathLINQ.GetExpression<T, DateTime>(item.JsonPath, true);

                                var colDef = new ResourceListViewDefinitionColumn<T, DateTime>()
                                {
                                    Name = item.Name,
                                    Field = exp.Compile(),
                                    Width = "*"
                                };

                                definition.Columns.Add(colDef);
                            }
                            else if (item.Type == "boolean")
                            {
                                var exp = JsonPathLINQ.JsonPathLINQ.GetExpression<T, bool>(item.JsonPath, true);

                                var colDef = new ResourceListViewDefinitionColumn<T, bool>()
                                {
                                    Name = item.Name,
                                    Display = TransformToFuncOfString<T>(exp.Body, exp.Parameters).Compile(),
                                    Field = exp.Compile(),
                                    Width = "*"
                                };

                                definition.Columns.Add(colDef);
                            }
                            else
                            {
                                _logger.LogWarning("CRD Column Type not supported: {type}", item.Type);
                            }
                        }
                        catch (InvalidOperationException ex) when (ex.Message.StartsWith("No coercion operator is defined between types", StringComparison.Ordinal))
                        {
                            // The type defined in the AdditionalPrinterColumn is not correct

                            var match = TypeErrorRegex().Match(ex.Message);
                            if (match.Success)
                            {
                                var type = Type.GetType(match.Groups[1].Value);

                                if (type.IsGenericType)
                                {
                                    type = type.GenericTypeArguments[0];
                                }

                                if (type == typeof(string))
                                {
                                    item.Type = "string";
                                }
                                else if (type == typeof(double))
                                {
                                    item.Type = "number";
                                }
                                if (type == typeof(int))
                                {
                                    item.Type = "integer";
                                }
                                else if (type == typeof(long) )
                                {
                                    item.Type = "integer";
                                    item.Format = "int64";
                                }
                                else if (type == typeof(DateTime))
                                {
                                    item.Type = "date";
                                }
                                else if (type == typeof(bool))
                                {
                                    item.Type = "boolean";
                                }
                                else
                                {
                                    _logger.LogCritical(ex, "Unable to generate CRD Column: {Name} with type {Type}", item.Name, type);
                                    continue;
                                }

                                goto start;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogCritical(ex, "Unable to generate CRD Column: {Name}", item.Name);
                        }
                    }
                }
            }

            // Add Age Column
            var ageColumn = new ResourceListViewDefinitionColumn<T, DateTime?>()
            {
                Name = "Age",
                CustomControl = typeof(AgeCell),
                Field = x => x.Metadata.CreationTimestamp,
                Display = x => x.Metadata.CreationTimestamp?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                Width = "80"
            };

            definition.Columns.Add(ageColumn);
        }

        return definition;
    }

    private static Expression<Func<T, string>> TransformToFuncOfString<T>(Expression expression, ReadOnlyCollection<ParameterExpression> parameters)
    {
        // Convert the body of the original expression to return a string
        var bodyAsString = Expression.Call(expression, nameof(string.ToString), Type.EmptyTypes);

        // Create a new lambda expression
        return Expression.Lambda<Func<T, string>>(bodyAsString, parameters);
    }

    #region Actions

    [RelayCommand(CanExecute = nameof(CanDelete))]
    private async Task Delete(IList items)
    {
        ContentDialogSettings settings = new()
        {
            Title = Resources.ResourceListViewModel_Delete_Title,
            Content = string.Format(Resources.ResourceListViewModel_Delete_Content, items.Count),
            PrimaryButtonText = Resources.ResourceListViewModel_Delete_Primary,
            SecondaryButtonText = Resources.ResourceListViewModel_Delete_Secondary,
            DefaultButton = ContentDialogButton.Secondary
        };

        var result = await _dialogService.ShowContentDialogAsync(this, settings);

        if (result == ContentDialogResult.Primary)
        {
            foreach (var item in items.Cast<KeyValuePair<NamespacedName, T>>().ToList())
            {
                await Cluster.Delete<T>(item.Value);
            }
        }
    }

    private bool CanDelete(IList items)
    {
        return items.Count != 0;
        //var coll = items as IEnumerable;

        //foreach (var item in coll)
        //{
        //    var i = item as T;
        //    var cani = Cluster.CanDelete<T>(i).Result;

        //    if (!cani)
        //    {
        //        return false;
        //    }
        //}
    }

    [RelayCommand(CanExecute = nameof(CanView))]
    private void View(object item)
    {
        var root = Factory.GetDockable<IRootDock>("Root");
        var pinnedDoc = root.RightPinnedDockables?.FirstOrDefault(x => x.Id == "Properties");

        if (pinnedDoc != null)
        {
            Factory.RemoveDockable(pinnedDoc, true);
        }

        var doc = Factory.GetDockable<IDock>("RightDock");

        var existingDock = doc.VisibleDockables.FirstOrDefault(x => x.Id == "Properties");

        if (existingDock != null)
        {
            Factory.RemoveDockable(existingDock, true);
        }

        var instance = Application.Current.GetRequiredService<ResourcePropertiesViewModel<T>>();
        instance.Cluster = Cluster;
        instance.Object = ((KeyValuePair<NamespacedName, T>)(item)).Value;
        instance.Kind = Kind;
        instance.CanFloat = false;

        Factory?.InsertDockable(doc, instance, 0);
        Factory?.PinDockable(instance);
        Factory?.PreviewPinnedDockable(instance);
    }

    private bool CanView(object item)
    {
        return item != null;
    }

    [RelayCommand(CanExecute = nameof(CanViewYaml))]
    private void ViewYaml(object item)
    {
        var vm = Application.Current.GetRequiredService<ResourceYamlViewModel>();
        vm.Cluster = Cluster;
        vm.Object = ((KeyValuePair<NamespacedName, T>)item).Value;
        vm.Id = $"{nameof(ViewYaml)}-{Cluster.Name}-{((KeyValuePair<NamespacedName, T>)SelectedItem).Key}";

        Factory.AddToBottom(vm);
    }

    private bool CanViewYaml(object item)
    {
        return item != null;
    }

    [RelayCommand(CanExecute = nameof(CanViewLogs))]
    private async Task ViewLogs(string containerName)
    {
        var vm = Application.Current.GetRequiredService<PodLogsViewModel>();
        vm.Cluster = Cluster;
        vm.Object = ((KeyValuePair<NamespacedName, V1Pod>)SelectedItem).Value;
        vm.ContainerName = containerName.ToString();
        vm.Id = $"{nameof(ViewLogs)}-{Cluster.Name}-{((KeyValuePair<NamespacedName, V1Pod>)SelectedItem).Key}-{containerName}";

        if (Factory.AddToBottom(vm))
        {
        try
        {
            await vm.Connect();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error viewing logs");
            return;
        }
        }
    }

    private bool CanViewLogs(string containerName)
    {
        return !string.IsNullOrEmpty(containerName);
    }

    [RelayCommand(CanExecute = nameof(CanViewConsole))]
    private async Task ViewConsole(string containerName)
    {
        var vm = Application.Current.GetRequiredService<PodConsoleViewModel>();
        vm.Cluster = Cluster;
        vm.Object = ((KeyValuePair<NamespacedName, V1Pod>)SelectedItem).Value;
        vm.ContainerName = containerName;
        vm.Id = $"{nameof(ViewConsole)}-{Cluster.Name}-{((KeyValuePair<NamespacedName, V1Pod>)SelectedItem).Key}-{containerName}";

        if (Factory.AddToBottom(vm))
        {
        try
        {
            await vm.Connect();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error connecting to console");
            return;
        }
        }
    }

    private bool CanViewConsole(string containerName)
    {
        return !string.IsNullOrEmpty(containerName);
    }

    [RelayCommand(CanExecute = nameof(CanPortForward))]
    private async Task PortForward(int containerPort)
    {
        var pf = Cluster.AddPortForward(((KeyValuePair<NamespacedName, V1Pod>)SelectedItem).Key.Namespace, ((KeyValuePair<NamespacedName, V1Pod>)SelectedItem).Key.Name, containerPort);

        ContentDialogSettings settings = new()
        {
            Title = Resources.ResourceListViewModel_PortForward_Title,
            Content = string.Format(Resources.ResourceListViewModel_PortForward_Content, containerPort, pf.LocalPort),
            PrimaryButtonText = Resources.ResourceListViewModel_PortForward_Primary,
            SecondaryButtonText = Resources.ResourceListViewModel_PortForward_Secondary,
            DefaultButton = ContentDialogButton.Secondary
        };

        var result = await _dialogService.ShowContentDialogAsync(this, settings);

        if (result == ContentDialogResult.Primary)
        {
            var window = (Window)_dialogService.DialogManager.GetMainWindow()!.RefObj;
            await window!.Launcher.LaunchUriAsync(new Uri($"http://localhost:{pf.LocalPort}"));
        }
    }

    private bool CanPortForward(int containerPort)
    {
        return containerPort > 0;
    }

    #endregion

    public void Dispose()
    {
        _filter.Dispose();
    }
}

public class ResourceListViewDefinitionColumn<T, T2> where T : class, IKubernetesObject<V1ObjectMeta>, new()
{
    public required string Name { get; set; }

    public required Func<T, T2> Field { get; set; }

    public Func<T, string>? Display { get; set; }

    public SortDirection Sort { get; set; }

    public Type? CustomControl { get; set; }

    public string? Width { get; set; }
}

public enum SortDirection
{
    None,
    Ascending,
    Descending
}

public class ResourceListViewMenuItem
{
    public string? Header { get; set; }

    public IBinding? HeaderBinding { get; set; }

    public string? CommandPath { get; set; }

    public string? CommandParameterPath { get; set; }

    public string? ItemSourcePath { get; set; }

    public ResourceListViewMenuItem? MenuItem { get; set; }
}

public class ResourceListViewDefinition<T> where T : class, IKubernetesObject<V1ObjectMeta>, new()
{
    public List<object> Columns { get; set; }

    public List<ResourceListViewMenuItem> MenuItems { get; set; }

    public bool DefaultMenuItems { get; set; } = true;

    public bool ShowNamespaces { get; set; } = true;
}
