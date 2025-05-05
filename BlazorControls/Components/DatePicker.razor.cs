using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorControls.Components;

public partial class DatePicker
    : ComponentBase
{
    [Parameter]
    public DateTime? Value { get; set; }

    [Parameter]
    public bool Disabled { get; set; } = false;

    private List<DateTime> dateList { get; set; } = new();

    [Parameter]
    public EventCallback<DateTime?> ValueChanged { get; set; }
    private DateTime dtNow { get; set; }
    private bool isActive = false;
    private void ToggleSelect() => isActive = !isActive;

    protected override void OnInitialized()
    {
        dtNow = Value ?? DateTime.Now;
        LoadCalendar();
    }

    private void LoadCalendar()
    {
        int daysInMonth = DateTime.DaysInMonth(dtNow.Year, dtNow.Month);
        dateList = Enumerable.Range(1, daysInMonth)
            .Select(d => new DateTime(year: dtNow.Year, month: dtNow.Month, day: d)).ToList();
    }

    private async Task OnSelectedDate(MouseEventArgs e, DateTime dt)
    {
        Value = dt;
        isActive = false;
        await ValueChanged.InvokeAsync(dt);
    }

    private void CloseOnFocusOut(FocusEventArgs e)
    {
        isActive = false;
    }

    private void OnNextMonth(MouseEventArgs e)
    {
        dtNow = dtNow.AddMonths(-1);
        LoadCalendar();
    }

    private void OnPrevMonth(MouseEventArgs e)
    {
        dtNow = dtNow.AddMonths(1);
        LoadCalendar();
    }

    protected CssClassBuilder CssClassBuilder => new CssClassBuilder();
    protected string CssClasses => CssClassBuilder
        .Add("calendar")
        .Add(isActive ? "show" : "")
        .ToString();
}
