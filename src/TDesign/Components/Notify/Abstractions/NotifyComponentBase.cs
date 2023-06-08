﻿namespace TDesign.Notification;
/// <summary>
/// 消息组件的基类。
/// </summary>
public abstract class NotifyComponentBase : TDesignAdditionParameterWithChildContentComponentBase
{
    /// <summary>
    /// 提醒主题。
    /// </summary>
    [Parameter] public virtual Theme? Theme { get; set; }
    /// <summary>
    /// 图标。
    /// </summary>
    [Parameter] public virtual object? Icon { get; set; }

    /// <summary>
    /// 获取对应的主题样式。
    /// </summary>
    protected string? GetThemeClass
    {
        get
        {
            if (Theme is null)
            {
                return string.Empty;
            }
            if (Theme == Theme.Primary)
            {
                return "info";
            }
            if (Theme == Theme.Danger)
            {
                return "error";
            }
            return Theme.Value;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        Icon ??= GetTIconByTheme;
    }

    /// <summary>
    /// 获取主题对应的图标。
    /// </summary>
    protected virtual IconName? GetTIconByTheme
    {
        get
        {
            if (Theme is null)
            {
                return IconName.InfoCircle;
            }
            if (Theme == Theme.Success)
            {
                return IconName.CheckCircleFilled;
            }
            if (Theme == Theme.Danger)
            {
                return IconName.CloseCircleFilled;
            }
            return IconName.InfoCircleFilled;
        }
    }
}
