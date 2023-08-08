﻿using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;

/// <summary>
/// 表示栅格的列。
/// </summary>
[ChildComponent(typeof(TRow))]
[CssClass("t-col")]
public class TColumn : TDesignAdditionParameterComponentBase, IHasChildContent
{
    /// <summary>
    /// 获取或设置父组件 <see cref="TRow"/> 的级联参数的值。
    /// </summary>
    [CascadingParameter] public TRow CascadingRow { get; set; }
    /// <summary>
    /// 获取或设置栅格的宽度。
    /// </summary>
    [ParameterApiDoc("栅格的宽度", Value ="Is1")]
    [Parameter][CssClass("t-col-")] public ColumnSpan Span { get; set; } = ColumnSpan.Is1;
    /// <summary>
    /// 获取或设置栅格的偏移量。
    /// </summary>
    [ParameterApiDoc("栅格的偏移量")]
    [Parameter][CssClass("t-col-offset-")] public ColumnSpan? Offset { get; set; }
    /// <summary>
    /// 获取或设置向左边拉动的单元格宽度。
    /// </summary>
    [ParameterApiDoc("向左边拉动的单元格宽度")]
    [Parameter][CssClass("t-col-pull-")] public ColumnSpan? Pull { get; set; }
    /// <summary>
    /// 获取或设置向右边推动的单元格宽度。
    /// </summary>
    [ParameterApiDoc("向右边推动的单元格宽度")]
    [Parameter][CssClass("t-col-push-")] public ColumnSpan? Push { get; set; }
    /// <summary>
    /// 获取或设置单元格的排序。
    /// </summary>
    [ParameterApiDoc("单元格的排序")]
    [Parameter][CssClass("t-col-order-")] public ColumnSpan? Order { get; set; }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc("任意的内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", ChildContent);
    }
}
