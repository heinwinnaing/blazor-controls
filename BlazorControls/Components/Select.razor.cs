using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using System.Linq.Expressions;

namespace BlazorControls.Components;

public class SelectOtpion<TValue>
{
    public TValue Value { get; set; } = default!;
    public string Label { get; set; } = default!;
}

public partial class Select<TValue>
    : ComponentBase
{
    [Parameter, EditorRequired] public List<SelectOtpion<TValue>> Options { get; set; } = new();
    [Parameter] public string? FieldName { get; set; }
    [Parameter] public TValue Value { get; set; } = default!;
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

    [CascadingParameter] private EditContext? EditContext { get; set; }
    [Parameter] public Expression<Func<TValue>>? ValidationFor { get; set; }
    public IEnumerable<string>? ValidationErrors { get; set; }

    private async Task OnValueChanged(ChangeEventArgs e)
    {
        _ = BindConverter.TryConvertTo(e.Value, CultureInfo.CurrentCulture, out TValue newValue);
        await ValueChanged.InvokeAsync(newValue);

        FieldIdentifier? fid = EditContext?.Field(FieldName ?? "");
        if (fid is not null)
        {
            //EditContext?.Validate();
            EditContext?.NotifyFieldChanged(fid.Value);
            ValidationErrors = EditContext?.GetValidationMessages(fid.Value);
        }
    }

    protected override void OnInitialized()
    {
        if (EditContext is not null
            && ValidationFor is not null)
        {
            EditContext.OnValidationRequested += HandleValidationRequested;
        }
        base.OnInitialized();
    }

    private void HandleValidationRequested(object? sender,
        ValidationRequestedEventArgs args)
    {
        FieldIdentifier? fid = EditContext?.Field(FieldName ?? "");
        if (fid is not null)
        {
            EditContext?.NotifyFieldChanged(fid.Value);
            ValidationErrors = EditContext?.GetValidationMessages(fid.Value);
        }
    }
    protected CssClassBuilder CssClassBuilder => new CssClassBuilder();
    protected string CssClasses => CssClassBuilder
        .Add(ValidationErrors?.Any() == true ? "danger" : "")
        .ToString();
}
