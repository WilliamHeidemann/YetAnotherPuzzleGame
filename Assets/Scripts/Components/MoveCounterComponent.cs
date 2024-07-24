using System.Collections.Generic;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Components
{
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

                LeanTween.scale(circles[i], Vector3.one * 2, 0.5f).setEase(LeanTweenType.easeInQuad)
                    .setOnComplete(() => LeanTween.scale(circles[i], Vector3.one, 0.5f).setEase(LeanTweenType.easeOutQuad));
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
}
