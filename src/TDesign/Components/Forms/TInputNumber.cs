﻿namespace TDesign;
/// <summary>
/// 数字输入框由增加、减少按钮、数值输入组成。每次点击增加按钮（或减少按钮），数字增长（或减少）的量是恒定的。
/// <para>
/// 支持数字类型：<see cref="int"/>、<see cref="short"/>、<see cref="long"/>、<see cref="decimal"/>、<see cref="double"/>、<see cref="float"/>
/// </para>
/// </summary>
/// <typeparam name="TValue">数字类型。</typeparam>
[CssClass("t-input-number")]
public class TInputNumber<TValue> : TDesignInputComonentBase<TValue?>
{
    /// <summary>
    /// 允许的泛型类型
    /// </summary>
#pragma warning disable S2743 // Static fields should not be used in generic types
    private static readonly Type[] SupportTypes = new[] { typeof(int), typeof(short), typeof(long), typeof(decimal), typeof(double), typeof(float) };
#pragma warning restore S2743 // Static fields should not be used in generic types
    /// <summary>
    /// 输入框文本
    /// </summary>
    private string? _inputString;
    /// <summary>
    /// 初始化 <see cref="TInputNumber{TValue}"/> 类的新实例。
    /// </summary>
    public TInputNumber()
    {
        if (!SupportTypes.Contains(typeof(TValue)))
        {
            throw new NotSupportedException($"{nameof(TValue)} 仅支持 short, int, long, decimal, double 和 float 类型。");
        }
    }
    /// <summary>
    /// 设置输入框的文本对齐方式。
    /// </summary>
    [ParameterApiDoc("输入框的文本对齐方式",Value="Center")]
    [Parameter] public HorizontalAlignment Align { get; set; } = HorizontalAlignment.Center;
    /// <summary>
    /// 设置每一次增减的跨度值。
    /// </summary>
    [ParameterApiDoc("每一次增减的跨度值",Value=1)]
    [Parameter] public TValue? Step { get; set; } = (TValue)Convert.ChangeType(1, typeof(TValue));
    /// <summary>
    /// 设置输入的最大值限制。
    /// </summary>
    [ParameterApiDoc("输入的最大值限制", Value = int.MaxValue)]
    [Parameter] public TValue? Max { get; set; } = (TValue)Convert.ChangeType(int.MaxValue, typeof(TValue));
    /// <summary>
    /// 设置输入的最小值限制。
    /// </summary>
    [ParameterApiDoc("输入的最小值限制", Value = int.MinValue)]
    [Parameter] public TValue? Min { get; set; } = (TValue)Convert.ChangeType(int.MinValue, typeof(TValue));
    /// <summary>
    /// 设置排列形式和模式。
    /// </summary>
    [ParameterApiDoc("排列形式和模式", Value = "Row")]
    [Parameter][CssClass("t-input-number--")] public InputNumberTheme Theme { get; set; } = InputNumberTheme.Row;
    /// <summary>
    /// 设置输入框后缀显示的文本。
    /// </summary>
    [ParameterApiDoc("输入框后缀显示的文本")]
    [Parameter] public string? SuffixText { get; set; }
    /// <summary>
    /// 设置输入框前面显示的文本。
    /// </summary>
    [ParameterApiDoc("输入框前面显示的文本")]
    [Parameter] public string? PrefixText { get; set; }

    /// <summary>
    /// 设置输入框出现的提示的文本。
    /// </summary>
    [ParameterApiDoc("输入框出现的提示的文本")]
    [Parameter] public string? Tip { get; set; }
    /// <summary>
    /// 表示提示的对齐方式。
    /// </summary>
    [ParameterApiDoc("提示的对齐方式",Value= "Input")]
    [Parameter] public TipAlign? TipAlign { get; set; } = TDesign.TipAlign.Input;

    /// <summary>
    /// 占位字符串。
    /// </summary>
    [ParameterApiDoc("占位字符串")]
    [Parameter] public string? Placeholder { get; set; }

    /// <summary>
    /// 重写以构建组件的内容。
    /// </summary>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        dynamic? _value = Value;
        dynamic? _step = Step;
        var _arrowUpDisabled = IsOutOfMax();
        var _arrowDownDisabled = IsOutOfMin();

        BuildButton(builder, sequence + 1, IconName.Remove, _arrowDownDisabled || Disabled, Theme != InputNumberTheme.Normal, a =>
        {
            Value = (TValue)(_value - _step);
        });

        builder.CreateComponent<TInputText<TValue>>(sequence + 2, attributes: new
        {
            Value,
            ValueExpression,
            ValueChanged,
            Size,
            Placeholder,
            Alignment = Align,
            Status = Disabled ? Status.Error : Status,
            SuffixText,
            PrefixText,
            AutoWidth,
            Readonly,
            Disabled,
            TipContent = TipAlign is not null && TipAlign == TDesign.TipAlign.Input ? HtmlHelper.Instance.CreateContent(Tip) : HtmlHelper.Instance.CreateContent(""),
            @Type = InputType.Number,
            Step,
            Min,
            Max,
            onblur = HtmlHelper.Instance.Callback().Create(this, async () =>
            {
                await ConvertNumberAsync(_inputString);
            }),
            oninput = HtmlHelper.Instance.Callback().Create<ChangeEventArgs>(this, async args =>
            {
                _inputString = args?.Value?.ToString() ?? "";
                await ConvertNumberAsync(_inputString);
            }),
        });
        BuildButton(builder, sequence + 3, IconName.Add, _arrowUpDisabled || Disabled, Theme != InputNumberTheme.Normal, a =>
        {
            Value = (TValue)(_value + _step);
        });


        builder.CreateElement(sequence + 4, "div", Tip, new { @class = $"t-input__tips t-input__tips--{Status.GetCssClass()}" }, TipAlign == TDesign.TipAlign.Left);
    }
    /// <summary>
    /// 是否超出最大值
    /// </summary>
    /// <returns></returns>
    private bool IsOutOfMax()
    {
        dynamic? _value = Value;
        return Max != null && (bool)(_value >= Max);
    }

    /// <summary>
    /// 是否超出最小值
    /// </summary>
    /// <returns></returns>
    private bool IsOutOfMin()
    {
        dynamic? _value = Value;
        return Min != null && (bool)(_value <= Min);
    }

    /// <summary>
    /// 转换数值类型
    /// </summary>
    /// <param name="inputString">输入字符串</param>
    /// <returns></returns>
    private async Task ConvertNumberAsync(string? inputString)
    {
        _ = this.TryParseValueFromString(inputString,out var error);
        if (IsOutOfMax())
        {
            this.ChangeValue(Max);
        }
        if (IsOutOfMin())
        {
            this.ChangeValue(Min);
        }
    }



    /// <summary>
    /// 构建增减按钮。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    /// <param name="iconName">图标名称。</param>
    /// <param name="disabled">是否禁用。</param>
    /// <param name="condition">组件创建的条件。</param>
    /// <param name="click">点击事件。</param>
    private void BuildButton(RenderTreeBuilder builder, int sequence, object iconName, bool disabled, bool condition, Action<MouseEventArgs>? click = default)
    {
        builder.CreateComponent<TButton>(sequence + 1, content =>
        {
            content.CreateComponent<TIcon>(0, attributes: new
            {
                Name = iconName,
            });
        },
        new
        {
            Varient = ButtonVarient.Outline,
            Shape = ButtonShape.Square,
            @class = HtmlHelper.Instance.Class()
            .Append($"t-input-number__decrease", iconName.ToString() == IconName.Remove.ToString())
            .Append($"t-input-number__increase", iconName.ToString() == IconName.Add.ToString())
            .Append($"t-is-disabled", iconName.ToString() == IconName.Remove.ToString() && disabled)
            .Append($"t-is-disabled", iconName.ToString() == IconName.Add.ToString() && disabled),
            onclick = HtmlHelper.Instance.Callback().Create<MouseEventArgs>(this, e => click?.Invoke(e)),
            Disabled = disabled,

        }, condition);
    }
}

/// <summary>
///  主题
/// </summary>
public enum InputNumberTheme
{
    /// <summary>
    /// 按钮横向排列
    /// </summary>
    Row,
    /// <summary>
    /// 按钮行内纵向排列
    /// </summary>
    [CssClass("column t-is-controls-right")] Column,
    /// <summary>
    /// 没有按钮，通过上下键控制
    /// </summary>
    Normal
}

/// <summary>
/// 提示的对齐方式。
/// </summary>
public enum TipAlign
{
    /// <summary>
    /// 左对齐。
    /// </summary>
    Left,
    /// <summary>
    /// 输入框对齐。
    /// </summary>
    Input
}