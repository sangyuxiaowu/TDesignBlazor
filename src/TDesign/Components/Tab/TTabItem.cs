﻿namespace TDesign;
/// <summary>
/// 选项卡的项。
/// </summary>
[ChildComponent(typeof(TTab))]
[CssClass("t-tab-panel")]
public class TTabItem : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 用于自动化获取父组件。
    /// </summary>
    [CascadingParameter] public TTab CascadingTab { get; set; }
    /// <summary>
    /// 选项卡标题。
    /// </summary>
    [EditorRequired][Parameter] public string Title { get; set; }
    ///// <summary>
    ///// 选项卡宽度，单位时 px。
    ///// </summary>
    //[Parameter] public int? Width { get; set; } = 82;
    /// <summary>
    /// 选项卡内容的内边距，默认时 25px。
    /// </summary>
    [Parameter] public int Padding { get; set; } = 25;
    /// <summary>
    /// 禁用选项卡。
    /// </summary>
    [Parameter] public bool Disabled { get; set; }
    /// <summary>
    /// 选项卡标题的图标。
    /// </summary>
    [Parameter] public object? Icon { get; set; }

    internal int Index { get; private set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Index = CascadingTab.ChildComponents.Count - 1;
    }

    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        if (CascadingTab.SwitchIndex.HasValue && CascadingTab.SwitchIndex.Value == Index)
        {
            builder.CreateElement(sequence, "p", content =>
            {
                content.AddContent(1, ChildContent);
            }, new { style = $"padding:{Padding}px" });
        }
    }
}
