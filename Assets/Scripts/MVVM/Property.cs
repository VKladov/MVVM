using System;

public class Property<TValue>
{
    private TValue? _value;
        
    public TValue? Value
    {
        get => _value;
        set
        {
            if (_value != null && _value.Equals(value))
            {
                return;
            }
            
            _value = value;
            Changed?.Invoke(_value);
        }
    }

    public event Action<TValue?>? Changed;
}