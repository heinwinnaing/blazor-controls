using Microsoft.AspNetCore.Components;

namespace BlazorControls.Components;
public partial class Checkbox
    : ComponentBase
{
    private string Id { get; set; } = null!;
    [Parameter, EditorRequired] public string FieldName { get; set; } = null!;
    [Parameter] public string Value { get; set; } = null!;
    [Parameter] public string? Label { get; set; }
    [Parameter] public bool Disabled { get; set; } = false;
    [Parameter] public bool Checked { get; set; }
    [Parameter] public bool IsSwitch { get; set; } = false;
    [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
    [Parameter] public EventCallback<bool> OnChanged { get; set; }
    protected override void OnInitialized()
    {
        Id = Guid.NewGuid().ToString();
        base.OnInitialized();
    }
    private async Task OnCheckedChanged(ChangeEventArgs e)
    {
        Checked = !Checked;
        await CheckedChanged.InvokeAsync(Checked);
        await OnChanged.InvokeAsync(Checked);
    }

    protected CssClassBuilder CssClassBuilder => new CssClassBuilder();
    protected string CssClasses => CssClassBuilder
        .Add(IsSwitch ? "switch-box" : "input-check")
        .Add(Disabled ? "disabled" : "")
        .ToString();
}
