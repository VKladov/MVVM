namespace DefaultNamespace
{
    public class ValueWidget : View<SimpleValue>
    {
        public Property<string> Label = new();
        public Property<string> Value = new();
        
        public override void OnValueChanged(SimpleValue? newValue)
        {
            Label.Value = newValue.Label;
            Value.Value = newValue.Value;
        }
    }
}