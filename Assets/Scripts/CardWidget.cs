public class CardWidget : ViewModel<CardModel>
{
    public Property<string> Icon = new();
    public Property<string> Fragment1 = new();
    public Property<string> Fragment2 = new();
    public Property<string> Fragment3 = new();
    public Property<string> Fragment4 = new();
    public Property<string> Fragment5 = new();

    public override void OnModelChange(CardModel model)
    {
        Icon.Value = model.IconName;
        Fragment1.Value = model.Fragment1;
        Fragment2.Value = model.Fragment2;
        Fragment3.Value = model.Fragment3;
        Fragment4.Value = model.Fragment4;
        Fragment5.Value = model.Fragment5;
    }

    public void OnSelect()
    {
        
    }

    public void OnInfoClick()
    {
        
    }
}