namespace BlazorControls;

public sealed class CssClassBuilder
{
    public List<string> ClassNames = new();
    public CssClassBuilder(string? className = null)
    {
        if (!string.IsNullOrWhiteSpace(className))
            Add(className);
    }

    public CssClassBuilder Add(string className)
    {
        if (!string.IsNullOrWhiteSpace(className))
        {
            className
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList()
                .ForEach(c => {
                    if (!string.IsNullOrWhiteSpace(c))
                    {
                        ClassNames.Add(c);
                    }
                });
        }
        return this;
    }

    public CssClassBuilder Remove(string className)
    {
        ClassNames
            .RemoveAll(c => c.Equals(className, StringComparison.InvariantCultureIgnoreCase));
        return this;
    }

    public override string ToString()
    {
        return string.Join(' ',
            ClassNames
            .Distinct()
            .Where(c => !string.IsNullOrWhiteSpace(c)));
    }
}
