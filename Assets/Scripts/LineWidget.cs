public class LineWidget : ViewModel<LineValue>
{
    public Property<string> Label = new();
    public Property<string> Value = new();
    public Property<float> Progress = new();
    public Property<string> Icon = new();
    public Property<string> Color = new();

    public override void OnModelChange(LineValue model)
    {
        Label.Value = model.Label;
        Value.Value = model.Value;
        Progress.Value = model.Progress;
        Icon.Value = model.Icon;
        Color.Value = model.Color;
    }
}