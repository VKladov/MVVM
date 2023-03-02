using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

[Serializable]
public class InfoModel
{
    public string Title;
    public string Subtitle;
    public string Description;
    public CardModel Card;
    public List<LineValue> LineValues;
    public List<SimpleValue> SimpleValues;
    public List<string> Buttons;
}

[Serializable]
public class LineValue
{
    public string Label;
    public string Icon;
    public string Color;
    public float Progress;
    public string Value;
}

[Serializable]
public class SimpleValue
{
    public string Icon;
    public string Label;
    public string Value;
}