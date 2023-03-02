using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextView : View<string>
{
    private TMP_Text? _text;
    public override void OnValueChanged(string newValue)
    {
        _text ??= GetComponent<TMP_Text>();
        _text.text = newValue;
    }
}