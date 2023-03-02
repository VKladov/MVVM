using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageView : View<string>
{
    private Image? _image;
    public override void OnValueChanged(string newValue)
    {
        _image ??= GetComponent<Image>();
        _image.sprite = Resources.Load<Sprite>(newValue);
    }
}