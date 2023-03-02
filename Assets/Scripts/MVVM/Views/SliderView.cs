
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderView : View<float>
{
    private Slider? _slider;
    
    public override void OnValueChanged(float newValue)
    {
        _slider ??= GetComponent<Slider>();
        _slider.value = newValue;
    }
}