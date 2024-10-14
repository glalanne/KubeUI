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
public static partial class DialogWindow_MarkupExtensions
{
//================= Properties ======================//
 // IsManagedResizerVisible

/*BindFromExpressionSetterGenerator*/
public static T IsManagedResizerVisible<T>(this T control, Func<System.Boolean> func, Action<System.Boolean>? onChanged = null, [CallerArgumentExpression("func")] string? expression = null) where T : Ursa.Controls.DialogWindow 
   => control._set(Ursa.Controls.DialogWindow.IsManagedResizerVisibleProperty, func, onChanged, expression);

/*MagicalSetterGenerator*/
public static T IsManagedResizerVisible<T>(this T control,System.Boolean value, BindingMode? bindingMode = null, IValueConverter? converter = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.DialogWindow 
=> control._setEx(Ursa.Controls.DialogWindow.IsManagedResizerVisibleProperty, ps, () => control.IsManagedResizerVisible = value, bindingMode, converter, bindingSource);

/*BindSetterGenerator*/
public static T IsManagedResizerVisible<T>(this T control, IBinding binding) where T : Ursa.Controls.DialogWindow 
   => control._set(Ursa.Controls.DialogWindow.IsManagedResizerVisibleProperty, binding);

/*AvaloniaPropertyBindSetterGenerator*/
public static T IsManagedResizerVisible<T>(this T control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) where T : Ursa.Controls.DialogWindow 
   => control._set(Ursa.Controls.DialogWindow.IsManagedResizerVisibleProperty, avaloniaProperty, bindingMode, converter, overrideView);

/*MagicalSetterWithConverterGenerator*/
public static T IsManagedResizerVisible<TValue,T>(this T control, TValue value, FuncValueConverter<TValue, System.Boolean> converter, BindingMode? bindingMode = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.DialogWindow 
=> control._setEx(Ursa.Controls.DialogWindow.IsManagedResizerVisibleProperty, ps, () => control.IsManagedResizerVisible = converter.TryConvert(value), bindingMode, converter, bindingSource);



//================= Styles ======================//
 // IsManagedResizerVisible

/*ValueStyleSetterGenerator*/
public static Style<T> IsManagedResizerVisible<T>(this Style<T> style, System.Boolean value) where T : Ursa.Controls.DialogWindow 
=> style._addSetter(Ursa.Controls.DialogWindow.IsManagedResizerVisibleProperty, value);

/*BindingStyleSetterGenerator*/
public static Style<T> IsManagedResizerVisible<T>(this Style<T> style, IBinding binding) where T : Ursa.Controls.DialogWindow 
=> style._addSetter(Ursa.Controls.DialogWindow.IsManagedResizerVisibleProperty, binding);



}
