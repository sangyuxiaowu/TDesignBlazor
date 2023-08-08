﻿using Microsoft.AspNetCore.Components.Rendering;

namespace TDesign;
/// <summary>
/// 表示列表项。必须放在 <see cref="TList"/> 组件中。
/// </summary>
[ChildComponent(typeof(TList))]
[CssClass("t-list-item")]
public class TListItem : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 列表项正文部分的内容。
    /// </summary>
    [ParameterApiDoc("列表项正文部分的内容")]
    [Parameter] public override RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// 列表项标题部分的内容。
    /// </summary>
    [ParameterApiDoc("列表项标题部分的内容")]
    [Parameter] public RenderFragment? TitleContent { get; set; }
    /// <summary>
    /// 列表项头像部分的内容。
    /// </summary>
    [ParameterApiDoc("列表项头像部分的内容")]
    [Parameter] public RenderFragment? AvatarContent { get; set; }
    /// <summary>
    /// 列表项操作部分的内容。
    /// </summary>
    [ParameterApiDoc("列表项操作部分的内容")]
    [Parameter] public RenderFragment? OperationContent { get; set; }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        builder.CreateElement(sequence, "div", main =>
        {
            main.CreateElement(0, "div", BuildContent, new { @class = "t-list-item__content" });

            builder.CreateElement(1, "div", OperationContent, new { @class = "t-list-item__action" });
        }, new { @class = "t-list-item-main" });
    }

    private void BuildContent(RenderTreeBuilder builder)
    {
        if (TitleContent is not null || AvatarContent is not null)
        {
            builder.CreateElement(0, "div", meta =>
            {
                meta.CreateElement(0, "div", AvatarContent, new { @class = "t-list-item__meta-avatar" }, AvatarContent is not null);

                meta.CreateElement(1, "div", content =>
                {
                    content.CreateElement(0, "div", TitleContent, new { @class = "t-list-item__meta-title" }, TitleContent is not null);

                    content.CreateElement(1, "p", ChildContent, new { @class = "t-list-item__meta-description" });
                }, new { @class = "t-list-item__meta-content" });
            }, new { @class = "t-list-item__meta" });
        }
        else
        {
            builder.AddContent(0, ChildContent);
        }
    }
}
