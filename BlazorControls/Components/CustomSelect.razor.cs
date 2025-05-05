using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace BlazorControls.Components;

public record SelectOption(string Value, string Label);
public partial class CustomSelect
    : ComponentBase
{
    [Parameter, EditorRequired]
    public List<SelectOption> Options { get; set; } = new();

    [Parameter]
    public string Placeholder { get; set; } = "Select";

    [Parameter]
    public bool IsSearchable { get; set; } = false;

    [Parameter]
    public bool Disabled { get; set; } = false;

    [Parameter]
    public string? Value { get; set; }

    private string DisplayText { get; set; } = null!;

    [Parameter]
    public EventCallback<string> OnChanged { get; set; }
    [Parameter] public Expression<Func<string>>? ValidationFor { get; set; }

    private bool isActive = false;
    private void ToggleSelect() => isActive = !isActive;
    private string? txtSearch { get; set; }

    protected override void OnInitialized()
    {
        var lblCurrent = Options
            .FirstOrDefault(o => o.Value == Value)?.Label;
        if (!IsSearchable)
        {
            DisplayText = lblCurrent ?? Placeholder;
        }
        base.OnInitialized();
    }

    private async Task OnSelectedOption(SelectOption option)
    {
        DisplayText = option?.Label ?? Placeholder;
        txtSearch = null;
        await OnChanged.InvokeAsync(option?.Value);
    }

    private void OnSearching(ChangeEventArgs e)
    {
        txtSearch = (string?)e.Value;
    }

    protected CssClassBuilder CssClassBuilder => new CssClassBuilder();
    protected string CssClasses => CssClassBuilder
        .Add("select-wrapper")
        .Add(isActive ? "active" : "")
        .Add(Disabled ? "disabled": "")
        .ToString();
}
