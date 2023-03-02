public class VisibilityByStringView : View<string>
{
    public override void OnValueChanged(string newValue)
    {
        gameObject.SetActive(!string.IsNullOrEmpty(newValue));
    }
}