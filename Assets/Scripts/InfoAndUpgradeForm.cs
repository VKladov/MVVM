using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InfoAndUpgradeForm : ViewModel<InfoModel>
{
    public Property<string> Title = new();
    public Property<string> Subtitle = new();
    public ListProperty<LineValue> Lines = new();
    public ListProperty<SimpleValue> SimpleValues = new();
    public ListProperty<string> Buttons = new();
    public Property<string> Description = new();
    public Property<CardModel> Card = new();

    public override void OnModelChange(InfoModel model)
    {
        Title.Value = model.Title;
        Subtitle.Value = model.Subtitle;
        Lines.SetValues(model.LineValues);
        SimpleValues.SetValues(model.SimpleValues);
        Buttons.SetValues(model.Buttons);
        Description.Value = model.Description;
        Card.Value = model.Card;
    }
}