using System;
using Components;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;
using Type = Model.Type;

public class UndoButton : Singleton<UndoButton>
{
    [SerializeField] private Color green;
    [SerializeField] private Color blue;
    [SerializeField] private Color red;
    [SerializeField] private Color grey;
    [SerializeField] private Image spinner;
    [SerializeField] private Spinner rotator;

    public void Enable(Type type)
    {
        var color = type switch
        {
            Type.Cardinal => blue,
            Type.Diagonal => red,
            Type.Frog => green,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        spinner.color = color;
        rotator.enabled = true;
        // spinner.CrossFadeColor(color, 0.5f, true, false);
    }

    public void Disable()
    {
        spinner.color = grey;
        rotator.enabled = false;
    }
}
