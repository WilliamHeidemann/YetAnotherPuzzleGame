using System.Collections.Generic;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

public class MoveCounterComponent : Singleton<MoveCounterComponent>
{
    [SerializeField] private List<GameObject> circles;
    [SerializeField] private List<GameObject> fillers;

    public void SetCircles(int limit)
    {
        For.Range(circles.Count, i =>
        {
            circles[i].SetActive(i < limit);
            fillers[i].SetActive(false);
        });
    }

    public void FillCircles(int filled)
    {
        For.Range(fillers.Count, i =>
        {
            fillers[i].SetActive(i < filled);
        });
    }
}
