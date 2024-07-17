using System;
using UnityEngine;
using UtilityToolkit.Runtime;
using Animator = Animation.Animator;

public class TutorialSystem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer handPrefab;
    [SerializeField] private Vector3 startBlockClick;
    [SerializeField] private Vector3 endBlockClick;
    [SerializeField] private Sprite hand;
    [SerializeField] private Sprite click;
    [SerializeField] private Sprite hold;

    [SerializeField] private AnimationCurve curve;
    private void Start()
    {
        StartClickBlockTutorial();
    }

    public void StartClickBlockTutorial()
    {
        var data = new TutorialData(handPrefab, hand, click, startBlockClick, endBlockClick, curve);
        For.Range(50, () => Animator.ClickTutorial(handPrefab.gameObject, data));
    }
    
    public void StartClickTileTutorial()
    {
        
    }
    
    public class TutorialData
    {
        public readonly SpriteRenderer SpriteRenderer;
        public readonly Sprite Sprite1;
        public readonly Sprite Sprite2;
        public readonly Vector3 Start;
        public readonly Vector3 End;
        public readonly AnimationCurve PopupCurve;

        public TutorialData(SpriteRenderer spriteRenderer, Sprite sprite1, Sprite sprite2, Vector3 start, Vector3 end, AnimationCurve popupCurve)
        {
            SpriteRenderer = spriteRenderer;
            Sprite1 = sprite1;
            Sprite2 = sprite2;
            Start = start;
            End = end;
            PopupCurve = popupCurve;
        }
    }
}
