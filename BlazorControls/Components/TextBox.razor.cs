using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using System.Linq.Expressions;

namespace BlazorControls.Components;
public enum InputType
{
    Text,
    Email,
    Password,
    Number
}
public partial class TextBox<TValue>
    : ComponentBase
{
    [CascadingParameter(Name = "Theme")]
    public string? Theme { get; set; }

    private string? currentValue;
    [Parameter] public InputType Type { get; set; } = InputType.Text;
    [Parameter, EditorRequired] public string FieldName { get; set; } = null!;
    [Parameter] public string? Placeholder { get; set; }
    [Parameter] public bool ReadyOnly { get; set; } = false;
    [Parameter] public TValue Value { get; set; }
    [Parameter] public EventCallback<TValue> ValueChanged { get; set; }
    [Parameter] public Expression<Func<TValue>>? ValidationFor { get; set; }
    public bool IsValid { get; set; } = true;
    [CascadingParameter] private EditContext? EditContext { get; set; }
    public IEnumerable<string>? ValidationErrors { get; set; }
    private string? CurrentValue
    {
        get => currentValue;

        set
        {
            currentValue = value;
            TValue newValue;
            if (typeof(TValue) == typeof(Guid))
            {
                IsValid = Guid.TryParse(value, out var guidValue);
                newValue = (TValue)(object)guidValue;
            }
            else
            {
                IsValid = BindConverter.TryConvertTo<TValue>(value, CultureInfo.CurrentCulture, out newValue);
            }


            if (IsValid)
            {
                ValueChanged.InvokeAsync(newValue);
            }

            FieldIdentifier? fid = EditContext?.Field(FieldName);
            if (fid is not null)
            {
                //EditContext?.Validate();
                EditContext?.NotifyFieldChanged(fid.Value);
                ValidationErrors = EditContext?.GetValidationMessages(fid.Value);
            }
        }
    }

    protected override void OnParametersSet()
    {
        currentValue = Value?.ToString() ?? "";
        base.OnParametersSet();
    }

    protected override void OnInitialized()
    {
        if(EditContext is not null
            && ValidationFor is not null)
        {
            EditContext.OnValidationRequested += HandleValidationRequested;
        }
        base.OnInitialized();
    }

    private void HandleValidationRequested(object? sender,
        ValidationRequestedEventArgs args)
    {
        FieldIdentifier? fid = EditContext?.Field(FieldName);
        if (fid is not null)
        {
            EditContext?.NotifyFieldChanged(fid.Value);
            ValidationErrors = EditContext?.GetValidationMessages(fid.Value);
        }
    }

    protected CssClassBuilder CssClassBuilder => new CssClassBuilder();
    protected string CssClasses => CssClassBuilder
        .Add(Theme ?? "")
        .Add(ValidationErrors?.Any() == true ? "danger"
        : ValidationErrors?.Any() == false ? "success" : "")
        .ToString();

}
