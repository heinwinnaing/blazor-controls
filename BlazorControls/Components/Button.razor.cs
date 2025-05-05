using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorControls.Components;
public enum ButtonType
{
    Button,
    Submit
}
public enum ButtonSize
{
    Default,
    Large,
    Small,
}
public enum ButtonStyle
{
    Default,
    Success,
    Warning,
    Info,
    Danger,    
}
public enum ButtonShape
{
    Default,
    Pill,
    Square
}
public partial class Button
    : ComponentBase
{
    [Parameter] public ButtonType Type { get; set; } = ButtonType.Button;
    [Parameter] public ButtonSize Size { get; set; } = ButtonSize.Default;
    [Parameter] public ButtonStyle Style { get; set; } = ButtonStyle.Default;
    [Parameter] public ButtonShape Shape { get; set; } = ButtonShape.Default;
    [Parameter] public string Text { get; set; } = "Button";
    [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
    [Parameter] public bool Disabled { get; set; } = false;
    protected CssClassBuilder CssClassBuilder => new CssClassBuilder();
    protected string ClassName => CssClassBuilder
        .Add("btn")
        .Add(Style == ButtonStyle.Success ? "btn-success" 
            : Style == ButtonStyle.Info ? "btn-info"
            : Style == ButtonStyle.Warning ? "btn-warning"
            : Style == ButtonStyle.Danger ? "btn-danger"
            : "")
        .Add(Shape == ButtonShape.Pill ? "btn-pill"
            : Shape == ButtonShape.Square ? "btn-square"
            : "")
        .Add(Disabled ? "disabled" : "")
        .ToString();
}
