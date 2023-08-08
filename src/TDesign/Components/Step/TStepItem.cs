﻿using Microsoft.AspNetCore.Components.Rendering;
using System.Diagnostics.CodeAnalysis;

namespace TDesign;

/// <summary>
/// 表示步骤的项。必须在 <see cref="TStepGroup"/> 组件中使用。
/// </summary>
[CssClass("t-steps-item")]
[ChildComponent(typeof(TStepGroup))]
public class TStepItem : TDesignAdditionParameterComponentBase, IHasChildContent
{
    /// <summary>
    /// 级联参数。
    /// </summary>
    [CascadingParameter][NotNull] public TStepGroup CascadingStepGroup { get; set; }
    /// <summary>
    /// 步骤的状态。
    /// </summary>
    [ParameterApiDoc("步骤的状态", Value = "NotStart")]
    [Parameter][CssClass("t-steps-item--")] public StepStatus Status { get; set; } = StepStatus.NotStart;
    /// <summary>
    /// 设置可点击的样式。
    /// </summary>
    [ParameterApiDoc("是否使用可点击的样式")]
    [Parameter][CssClass("t-steps-item--clickable")] public bool Clickable { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [ParameterApiDoc("步骤条的任意内容")]
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 设置图标名称。默认自动计算当前步骤所在的数字。
    /// <para>
    /// 当 <see cref="TStepGroup.Dot"/> 是 <c>true</c> 时无效。
    /// </para>
    /// </summary>
    [ParameterApiDoc("图标名称。默认自动计算当前步骤所在的数字，当 TStepGroup.Dot 为 true 时无效")]
    [Parameter] public object? Icon { get; set; }
    /// <summary>
    /// 设置描述内容。
    /// </summary>
    [ParameterApiDoc("描述部分的内容")]
    [Parameter] public RenderFragment? DescriptionContent { get; set; }
    /// <summary>
    /// 设置附加内容。
    /// </summary>
    [ParameterApiDoc("其余附加部分的内容")]
    [Parameter] public RenderFragment? ExtraContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", inner =>
        {
            BuildTIcon(inner, 0);
            BuildContent(inner, 1);

        }, new { @class = HtmlHelper.Instance.Class().Append("t-steps-item__inner") });
    }

    private void BuildContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", content =>
        {
            content.CreateElement(0, "div", ChildContent, new { @class = "t-steps-item__title" });
            content.CreateElement(1, "div", DescriptionContent, new { @class = "t-steps-item__description" }, DescriptionContent is not null);
            content.CreateElement(2, "div", ExtraContent, new { @class = "t-steps-item__extra" }, ExtraContent is not null);

        }, new { @class = "t-steps-item__content" });
    }

    /// <summary>
    /// 构建步骤的图标。
    /// </summary>
    /// <param name="builder"></param>
    private void BuildTIcon(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", icon =>
        {
            if (CascadingStepGroup.Dot)
            {
                return;
            }

            if (Status == StepStatus.Error)
            {
                icon.CreateComponent<TIcon>(0, attributes: new { Name = IconName.CloseCircle });
                return;
            }

            if (Icon is not null)
            {
                icon.CreateComponent<TIcon>(0, attributes: new { Name = Icon });
                return;
            }

            icon.CreateElement(0, "div", num =>
            {
                switch (Status)
                {
                    case StepStatus.Finish:
                        num.CreateComponent<TIcon>(0, attributes: new { Name = IconName.Check });
                        break;
                    default:
                        //数字
                        var number = CascadingStepGroup.ChildComponents.ToList().FindIndex(m => m == this);
                        num.AddContent(0, number + 1);
                        break;
                }

            }, new { @class = "t-steps-item__icon--number" });
        },
                    new
                    {
                        @class = HtmlHelper.Instance.Class()
                        .Append("t-steps-item__icon")
                        .Append($"t-steps-item--{Status.GetCssClass()}")
                    });
    }
}

/// <summary>
/// 步骤的当前状态。
/// </summary>
public enum StepStatus
{
    /// <summary>
    /// 默认，未开始状态。
    /// </summary>
    [CssClass("default")] NotStart,
    /// <summary>
    /// 正在进行，会高亮步骤。
    /// </summary>
    [CssClass("process")] Process,
    /// <summary>
    /// 完成。
    /// </summary>
    Finish,
    /// <summary>
    /// 错误
    /// </summary>
    Error,
}