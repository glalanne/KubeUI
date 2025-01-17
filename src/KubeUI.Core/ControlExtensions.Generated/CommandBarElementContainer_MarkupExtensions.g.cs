#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Avalonia.Markup.Declarative;
[global::System.CodeDom.Compiler.GeneratedCode("AvaloniaExtensionGenerator", "1.0.0.0")]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public static partial class CommandBarElementContainer_MarkupExtensions
{
//================= Properties ======================//
 // DynamicOverflowOrder

/*BindFromExpressionSetterGenerator*/
public static T DynamicOverflowOrder<T>(this T control, Func<System.Int32> func, Action<System.Int32>? onChanged = null, [CallerArgumentExpression("func")] string? expression = null) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
   => control._set(FluentAvalonia.UI.Controls.CommandBarElementContainer.DynamicOverflowOrderProperty, func, onChanged, expression);

/*MagicalSetterGenerator*/
public static T DynamicOverflowOrder<T>(this T control,System.Int32 value, BindingMode? bindingMode = null, IValueConverter? converter = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
=> control._setEx(FluentAvalonia.UI.Controls.CommandBarElementContainer.DynamicOverflowOrderProperty, ps, () => control.DynamicOverflowOrder = value, bindingMode, converter, bindingSource);

/*BindSetterGenerator*/
public static T DynamicOverflowOrder<T>(this T control, IBinding binding) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
   => control._set(FluentAvalonia.UI.Controls.CommandBarElementContainer.DynamicOverflowOrderProperty, binding);

/*AvaloniaPropertyBindSetterGenerator*/
public static T DynamicOverflowOrder<T>(this T control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
   => control._set(FluentAvalonia.UI.Controls.CommandBarElementContainer.DynamicOverflowOrderProperty, avaloniaProperty, bindingMode, converter, overrideView);

/*MagicalSetterWithConverterGenerator*/
public static T DynamicOverflowOrder<TValue,T>(this T control, TValue value, FuncValueConverter<TValue, System.Int32> converter, BindingMode? bindingMode = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
=> control._setEx(FluentAvalonia.UI.Controls.CommandBarElementContainer.DynamicOverflowOrderProperty, ps, () => control.DynamicOverflowOrder = converter.TryConvert(value), bindingMode, converter, bindingSource);


 // IsCompact

/*BindFromExpressionSetterGenerator*/
public static T IsCompact<T>(this T control, Func<System.Boolean> func, Action<System.Boolean>? onChanged = null, [CallerArgumentExpression("func")] string? expression = null) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
   => control._set(FluentAvalonia.UI.Controls.CommandBarElementContainer.IsCompactProperty, func, onChanged, expression);

/*MagicalSetterGenerator*/
public static T IsCompact<T>(this T control,System.Boolean value, BindingMode? bindingMode = null, IValueConverter? converter = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
=> control._setEx(FluentAvalonia.UI.Controls.CommandBarElementContainer.IsCompactProperty, ps, () => control.IsCompact = value, bindingMode, converter, bindingSource);

/*BindSetterGenerator*/
public static T IsCompact<T>(this T control, IBinding binding) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
   => control._set(FluentAvalonia.UI.Controls.CommandBarElementContainer.IsCompactProperty, binding);

/*AvaloniaPropertyBindSetterGenerator*/
public static T IsCompact<T>(this T control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
   => control._set(FluentAvalonia.UI.Controls.CommandBarElementContainer.IsCompactProperty, avaloniaProperty, bindingMode, converter, overrideView);

/*MagicalSetterWithConverterGenerator*/
public static T IsCompact<TValue,T>(this T control, TValue value, FuncValueConverter<TValue, System.Boolean> converter, BindingMode? bindingMode = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
=> control._setEx(FluentAvalonia.UI.Controls.CommandBarElementContainer.IsCompactProperty, ps, () => control.IsCompact = converter.TryConvert(value), bindingMode, converter, bindingSource);



//================= Styles ======================//
 // IsCompact

/*ValueStyleSetterGenerator*/
public static Style<T> IsCompact<T>(this Style<T> style, System.Boolean value) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
=> style._addSetter(FluentAvalonia.UI.Controls.CommandBarElementContainer.IsCompactProperty, value);

/*BindingStyleSetterGenerator*/
public static Style<T> IsCompact<T>(this Style<T> style, IBinding binding) where T : FluentAvalonia.UI.Controls.CommandBarElementContainer 
=> style._addSetter(FluentAvalonia.UI.Controls.CommandBarElementContainer.IsCompactProperty, binding);



}
