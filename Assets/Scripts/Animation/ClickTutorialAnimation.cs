using System;
using UnityEngine;

namespace Animation
{
    public class ClickTutorialAnimation : AnimationData
    {
        private readonly TutorialSystem.TutorialData data;
        private readonly Quaternion originalRotation;
        private readonly Vector3 originalScale;

        public ClickTutorialAnimation(GameObject handPrefabGameObject, TutorialSystem.TutorialData data) : base(handPrefabGameObject)
        {
            this.data = data;
            originalRotation = gameObject.transform.rotation;
            originalScale = gameObject.transform.lossyScale;
        }

        public override LTSeq Tween()
        {
            var sequence = LeanTween.sequence();

            var bounce = LeanTween.scale(gameObject, originalScale, 0.2f).setEase(data.PopupCurve);
            var move = LeanTween.move(gameObject, data.End, 0.5f);
            var click = LeanTween.scale(gameObject, originalScale, 0.5f).setEase(LeanTweenType.easeInOutBack);

            sequence.append(Setup);
            sequence.append(bounce);
            sequence.append(move);
            sequence.append(ChangeSprite);
            sequence.append(click);
            sequence.append(Reset);

            return sequence;

            void Setup()
            {
                data.SpriteRenderer.sprite = data.Sprite1;
                gameObject.transform.position = data.Start;
            }
            
            void ChangeSprite()
            {
                data.SpriteRenderer.sprite = data.Sprite2;
                gameObject.transform.Rotate(Vector3.forward, 60f);
            }
            
            void Reset()
            {
                data.SpriteRenderer.sprite = data.Sprite1;
                gameObject.transform.rotation = originalRotation;
                gameObject.transform.localScale = originalScale;
                gameObject.transform.position = data.Start;
            } 
        }
    }
}

// Feedback: Blokke skal være mindre,
// dobbelt klik i stedet for hold.
// At gå tilbage hvor man stod før skal tælle som en undo
// Ud over klik blok + klik tile for at move, skal man også kunne swipe

// indikator på hvor den ville komme hen med undo ville være nice
// hvis man klikke på den uden at den kan move eller undo, så skal den shake

// sindsygt god overall feedback. Smooth og sjovt
// forvirring over hvilke blocke ikke står hvor de startede

// når man kommer ind i "puzzle mindsettet" husker man bare hvor blockede stod -play tester

// hvis man klikker så hurtigt, at block 1 ikke når på sin plads, før man rykker blok 2, er det
// blok 1 som går hen hvor blok 2 skulle hen. 

// den svære bane i world 1 skaber god pacing. Kan godt lide når banerne er 6-7 brikker

// alle klik skal give instant feedback. Man vil ikke vente.