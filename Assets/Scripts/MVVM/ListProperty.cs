using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public abstract class ListPropertyBase
{
    public abstract event Action? ValuesChanged;
    public abstract object? GetValue(int index);
    public abstract int Count { get; }
}

public class ListProperty<TValue> : ListPropertyBase
{
    public override event Action? ValuesChanged;
    public override object? GetValue(int index) => _values[index];
    public override int Count => _values.Count;

    private List<TValue> _values = new();

    public void SetValues(List<TValue> values)
    {
        _values = values;
        ValuesChanged?.Invoke();
    }
}