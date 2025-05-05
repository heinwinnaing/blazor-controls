using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace BlazorControls.Components;
public partial class Radiobox<TValue>
    : ComponentBase
{
    [Parameter, EditorRequired]
    public TValue? Id { get; set; }
    [Parameter, EditorRequired] public string FieldName { get; set; } = null!;
    [Parameter, EditorRequired] public TValue Value { get; set; } = default!;
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool Disabled { get; set; } = false;
    [Parameter] public bool Checked { get; set; } = false;
    [Parameter] public EventCallback<ChangeEventArgs> OnChanged { get; set; }
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

    private async Task OnCheckedChanged(ChangeEventArgs e)
    {
        _= BindConverter.TryConvertTo<TValue>(e.Value, CultureInfo.CurrentCulture, out TValue? newValue);
        await OnChanged.InvokeAsync(e);
        await ValueChanged.InvokeAsync(newValue);
    }

    protected CssClassBuilder CssClassBuilder => new CssClassBuilder();
    protected string CssClasses => CssClassBuilder
        .Add("input-check")
        .Add(Disabled ? "disabled" : "")
        .ToString();
}
