using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public void ChangeColor(bool isSelected);

    public void SuscribeChangeColor(Action action);

    public void UnsuscribeChangeColor(Action action);
}