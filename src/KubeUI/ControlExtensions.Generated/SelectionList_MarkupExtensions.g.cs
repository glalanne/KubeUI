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
public static partial class SelectionList_MarkupExtensions
{
//================= Properties ======================//
 // IndicatorProperty

/*BindFromExpressionSetterGenerator*/
public static T Indicator<T>(this T control, Func<Avalonia.Controls.Control> func, Action<Avalonia.Controls.Control>? onChanged = null, [CallerArgumentExpression("func")] string? expression = null) where T : Ursa.Controls.SelectionList
   => control._set(Ursa.Controls.SelectionList.IndicatorProperty, func, onChanged, expression);

/*MagicalSetterGenerator*/
public static T Indicator<T>(this T control, Avalonia.Controls.Control value, BindingMode? bindingMode = null, IValueConverter? converter = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.SelectionList
=> control._setEx(Ursa.Controls.SelectionList.IndicatorProperty, ps, () => control.Indicator = value, bindingMode, converter, bindingSource);

/*BindSetterGenerator*/
public static T Indicator<T>(this T control, IBinding binding) where T : Ursa.Controls.SelectionList
   => control._set(Ursa.Controls.SelectionList.IndicatorProperty, binding);

/*AvaloniaPropertyBindSetterGenerator*/
public static T Indicator<T>(this T control, AvaloniaProperty avaloniaProperty, BindingMode? bindingMode = null, IValueConverter? converter = null, ViewBase? overrideView = null) where T : Ursa.Controls.SelectionList
   => control._set(Ursa.Controls.SelectionList.IndicatorProperty, avaloniaProperty, bindingMode, converter, overrideView);

/*MagicalSetterWithConverterGenerator*/
public static T Indicator<T,TValue>(this T control, TValue value, FuncValueConverter<TValue, Avalonia.Controls.Control> converter, BindingMode? bindingMode = null, object? bindingSource = null, [CallerArgumentExpression("value")] string? ps = null) where T : Ursa.Controls.SelectionList
=> control._setEx(Ursa.Controls.SelectionList.IndicatorProperty, ps, () => control.Indicator = converter.TryConvert(value), bindingMode, converter, bindingSource);



//================= Events ======================//

//================= Styles ======================//
 // IndicatorProperty

/*ValueStyleSetterGenerator*/
public static Style<T> Indicator<T>(this Style<T> style, Avalonia.Controls.Control value) where T : Ursa.Controls.SelectionList
=> style._addSetter(Ursa.Controls.SelectionList.IndicatorProperty, value);

/*BindingStyleSetterGenerator*/
public static Style<T> Indicator<T>(this Style<T> style, IBinding binding) where T : Ursa.Controls.SelectionList
=> style._addSetter(Ursa.Controls.SelectionList.IndicatorProperty, binding);



}