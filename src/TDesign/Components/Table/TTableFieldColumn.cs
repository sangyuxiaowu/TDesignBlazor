﻿using System.Linq.Expressions;

namespace TDesign;

/// <summary>
/// 表示可以输出数据的表格列的基类。必须在 <see cref="TTable{TItem}"/> 组件中使用。
/// </summary>
/// <typeparam name="TItem">数据的类型。</typeparam>
/// <typeparam name="TField">绑定的字段类型。</typeparam>
public class TTableFieldColumn<TItem,TField> : TTableColumnBase<TItem>,IHasChildContent<TItem>
{
    private Expression<Func<TItem, TField>>? _lastAssignedField;

    /// <summary>
    /// 设置数据源的字段名称。
    /// </summary>
    [ParameterApiDoc("数据源的字段名称", Type = "Expression<Func<TItem, TField>>")]
    [Parameter, EditorRequired] public Expression<Func<TItem, TField>> Field { get; set; } = default;

    /// <summary>
    /// 设置字段输出的格式。要符合 <see cref="IFormattable"/> 的定义。
    /// </summary>
    [ParameterApiDoc("字段输出的格式")]
    [Parameter] public string? Format { get; set; }
    [ParameterApiDoc("自定义显示模板", Type = "RenderFragment<TItem>")]
    [Parameter]public RenderFragment<TItem>? ChildContent { get; set; }

    protected Func<TItem, string?>? CellTextFunc { get; private set; }

    /// <inheritdoc/>
    protected override void AfterSetParameters(ParameterView parameters)
    {
        base.AfterSetParameters(parameters);

        Header ??= (Field.Body as MemberExpression)?.Member?.Name;
    }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if ( _lastAssignedField != Field )
        {
            _lastAssignedField = Field;

            var compiledFieldExpression = Field.Compile();
            if ( !string.IsNullOrEmpty(Format) )
            {
                var nullableType = Nullable.GetUnderlyingType(typeof(TField)); //获取可为空的字段类型。
                if ( !typeof(IFormattable).IsAssignableFrom(nullableType ?? typeof(TField)) )
                {
                    throw new TDesignComponentException(this, $"提供了 {nameof(Format)} 参数的值，但是字段 {typeof(TField)} 没有实现 {nameof(IFormattable)} 接口");
                }
                CellTextFunc = item => ((IFormattable?)compiledFieldExpression(item!))?.ToString(Format, default);
            }
            else
            {

                CellTextFunc = item => compiledFieldExpression(item!)?.ToString();
            }
        }

        ChildContent ??= value => builder => builder.AddContent(0, CellTextFunc!(value));
    }

    /// <inheritdoc/>
    protected internal override RenderFragment? GetCellContent(int rowIndex, TItem item)
    => builder => builder.AddContent(0, ChildContent, item);
}
