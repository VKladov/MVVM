public class SimpleValueWidget : ViewModel<SimpleValue>
{
    public Property<string> Icon = new();
    public Property<string> Label = new();
    public Property<string> Value = new();

    public override void OnModelChange(SimpleValue model)
    {
        Icon.Value = model.Icon;
        Label.Value = model.Label;
        Value.Value = model.Value;
    }
}