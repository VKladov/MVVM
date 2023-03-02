using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Image))]
    public class ImageColorView : View<string>
    {
        private Image? _image;
        public override void OnValueChanged(string newValue)
        {
            _image ??= GetComponent<Image>();
            if (ColorUtility.TryParseHtmlString(newValue, out var color))
            {
                _image.color = color;
            }
        }
    }
}