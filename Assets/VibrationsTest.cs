using System;
using UnityEngine;

public class VibrationsTest : MonoBehaviour
{
    private void Awake()
    {
        Vibration.Init();
    }

    public void Vibrate()
    {
        Vibration.Vibrate();
    }
    public void VibrateNope()
    {
        Vibration.VibrateNope();
    }
    public void VibratePeek()
    {
        Vibration.VibratePeek();
    }
    public void VibratePop()
    {
        Vibration.VibratePop();
    }
}
