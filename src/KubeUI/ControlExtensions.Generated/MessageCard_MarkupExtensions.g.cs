#nullable enable
using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Avalonia.Markup.Declarative;
[global::System.CodeDom.Compiler.GeneratedCode("AvaloniaExtensionGenerator", "11.1.3.0")]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public static partial class MessageCard_MarkupExtensions
{
//================= Properties ======================//
 // IsClosedProperty

/*BindFromExpressionSetterGenerator*/
public static T IsClosed<T>(this T control, Func<System.Boolean> func, Action<System.Boolean>? onChanged = null, [CallerArgumentExpression("func")] string? expression = null) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.IsClosedProperty, func, onChanged, expression);

/*MagicalSetterGenerator*/
public static T IsClosed<T>(this T control, System.Boolean value, BindingMode? bindingMode = null, IValueConverter? converter = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.MessageCard
=> control._setEx(Ursa.Controls.MessageCard.IsClosedProperty, ps, () => control.IsClosed = value, bindingMode, converter, bindingSource);

/*BindSetterGenerator*/
public static T IsClosed<T>(this T control, IBinding binding) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.IsClosedProperty, binding);

/*AvaloniaPropertyBindSetterGenerator*/
public static T IsClosed<T>(this T control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.IsClosedProperty, avaloniaProperty, bindingMode, converter, overrideView);

/*MagicalSetterWithConverterGenerator*/
public static T IsClosed<T,TValue>(this T control, TValue value, FuncValueConverter<TValue, System.Boolean> converter, BindingMode? bindingMode = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.MessageCard
=> control._setEx(Ursa.Controls.MessageCard.IsClosedProperty, ps, () => control.IsClosed = converter.TryConvert(value), bindingMode, converter, bindingSource);


 // NotificationTypeProperty

/*BindFromExpressionSetterGenerator*/
public static T NotificationType<T>(this T control, Func<Avalonia.Controls.Notifications.NotificationType> func, Action<Avalonia.Controls.Notifications.NotificationType>? onChanged = null, [CallerArgumentExpression("func")] string? expression = null) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.NotificationTypeProperty, func, onChanged, expression);

/*MagicalSetterGenerator*/
public static T NotificationType<T>(this T control, Avalonia.Controls.Notifications.NotificationType value, BindingMode? bindingMode = null, IValueConverter? converter = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.MessageCard
=> control._setEx(Ursa.Controls.MessageCard.NotificationTypeProperty, ps, () => control.NotificationType = value, bindingMode, converter, bindingSource);

/*BindSetterGenerator*/
public static T NotificationType<T>(this T control, IBinding binding) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.NotificationTypeProperty, binding);

/*AvaloniaPropertyBindSetterGenerator*/
public static T NotificationType<T>(this T control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.NotificationTypeProperty, avaloniaProperty, bindingMode, converter, overrideView);

/*MagicalSetterWithConverterGenerator*/
public static T NotificationType<T,TValue>(this T control, TValue value, FuncValueConverter<TValue, Avalonia.Controls.Notifications.NotificationType> converter, BindingMode? bindingMode = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.MessageCard
=> control._setEx(Ursa.Controls.MessageCard.NotificationTypeProperty, ps, () => control.NotificationType = converter.TryConvert(value), bindingMode, converter, bindingSource);


 // ShowIconProperty

/*BindFromExpressionSetterGenerator*/
public static T ShowIcon<T>(this T control, Func<System.Boolean> func, Action<System.Boolean>? onChanged = null, [CallerArgumentExpression("func")] string? expression = null) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.ShowIconProperty, func, onChanged, expression);

/*MagicalSetterGenerator*/
public static T ShowIcon<T>(this T control, System.Boolean value, BindingMode? bindingMode = null, IValueConverter? converter = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.MessageCard
=> control._setEx(Ursa.Controls.MessageCard.ShowIconProperty, ps, () => control.ShowIcon = value, bindingMode, converter, bindingSource);

/*BindSetterGenerator*/
public static T ShowIcon<T>(this T control, IBinding binding) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.ShowIconProperty, binding);

/*AvaloniaPropertyBindSetterGenerator*/
public static T ShowIcon<T>(this T control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.ShowIconProperty, avaloniaProperty, bindingMode, converter, overrideView);

/*MagicalSetterWithConverterGenerator*/
public static T ShowIcon<T,TValue>(this T control, TValue value, FuncValueConverter<TValue, System.Boolean> converter, BindingMode? bindingMode = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.MessageCard
=> control._setEx(Ursa.Controls.MessageCard.ShowIconProperty, ps, () => control.ShowIcon = converter.TryConvert(value), bindingMode, converter, bindingSource);


 // ShowCloseProperty

/*BindFromExpressionSetterGenerator*/
public static T ShowClose<T>(this T control, Func<System.Boolean> func, Action<System.Boolean>? onChanged = null, [CallerArgumentExpression("func")] string? expression = null) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.ShowCloseProperty, func, onChanged, expression);

/*MagicalSetterGenerator*/
public static T ShowClose<T>(this T control, System.Boolean value, BindingMode? bindingMode = null, IValueConverter? converter = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.MessageCard
=> control._setEx(Ursa.Controls.MessageCard.ShowCloseProperty, ps, () => control.ShowClose = value, bindingMode, converter, bindingSource);

/*BindSetterGenerator*/
public static T ShowClose<T>(this T control, IBinding binding) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.ShowCloseProperty, binding);

/*AvaloniaPropertyBindSetterGenerator*/
public static T ShowClose<T>(this T control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) where T : Ursa.Controls.MessageCard
   => control._set(Ursa.Controls.MessageCard.ShowCloseProperty, avaloniaProperty, bindingMode, converter, overrideView);

/*MagicalSetterWithConverterGenerator*/
public static T ShowClose<T,TValue>(this T control, TValue value, FuncValueConverter<TValue, System.Boolean> converter, BindingMode? bindingMode = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.MessageCard
=> control._setEx(Ursa.Controls.MessageCard.ShowCloseProperty, ps, () => control.ShowClose = converter.TryConvert(value), bindingMode, converter, bindingSource);



//================= Events ======================//
 // MessageClosed

/*ActionToEventGenerator*/
    public static T OnMessageClosed<T>(this T control, Action<Avalonia.Interactivity.RoutedEventArgs> action) where T : Ursa.Controls.MessageCard => 
        control._setEvent((System.EventHandler<Avalonia.Interactivity.RoutedEventArgs>) ((arg0, arg1) => action(arg1)), h => control.MessageClosed += h);



//================= Styles ======================//
 // IsClosedProperty

/*ValueStyleSetterGenerator*/
public static Style<T> IsClosed<T>(this Style<T> style, System.Boolean value) where T : Ursa.Controls.MessageCard
=> style._addSetter(Ursa.Controls.MessageCard.IsClosedProperty, value);

/*BindingStyleSetterGenerator*/
public static Style<T> IsClosed<T>(this Style<T> style, IBinding binding) where T : Ursa.Controls.MessageCard
=> style._addSetter(Ursa.Controls.MessageCard.IsClosedProperty, binding);


 // NotificationTypeProperty

/*ValueStyleSetterGenerator*/
public static Style<T> NotificationType<T>(this Style<T> style, Avalonia.Controls.Notifications.NotificationType value) where T : Ursa.Controls.MessageCard
=> style._addSetter(Ursa.Controls.MessageCard.NotificationTypeProperty, value);

/*BindingStyleSetterGenerator*/
public static Style<T> NotificationType<T>(this Style<T> style, IBinding binding) where T : Ursa.Controls.MessageCard
=> style._addSetter(Ursa.Controls.MessageCard.NotificationTypeProperty, binding);


 // ShowIconProperty

/*ValueStyleSetterGenerator*/
public static Style<T> ShowIcon<T>(this Style<T> style, System.Boolean value) where T : Ursa.Controls.MessageCard
=> style._addSetter(Ursa.Controls.MessageCard.ShowIconProperty, value);

/*BindingStyleSetterGenerator*/
public static Style<T> ShowIcon<T>(this Style<T> style, IBinding binding) where T : Ursa.Controls.MessageCard
=> style._addSetter(Ursa.Controls.MessageCard.ShowIconProperty, binding);


 // ShowCloseProperty

/*ValueStyleSetterGenerator*/
public static Style<T> ShowClose<T>(this Style<T> style, System.Boolean value) where T : Ursa.Controls.MessageCard
=> style._addSetter(Ursa.Controls.MessageCard.ShowCloseProperty, value);

/*BindingStyleSetterGenerator*/
public static Style<T> ShowClose<T>(this Style<T> style, IBinding binding) where T : Ursa.Controls.MessageCard
=> style._addSetter(Ursa.Controls.MessageCard.ShowCloseProperty, binding);



}
