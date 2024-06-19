using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using UnityEngine;
using UnityUtils;

namespace Presenter
{
    public class CommandManager : Singleton<CommandManager>
    {
        private Model.Block toMove;
        private readonly Dictionary<GameObject, Queue<Vector3>> animations = new();

        public void Hover(MovableBlock block)
        {
            toMove = block.model;
        }

        public void Select(Model.Block toLand)
        {
            Level.Instance.HideGhostBlocks();
            var command = new Command(toMove, toLand, toMove.type);
            Level.Instance.TryMove(command);
        }

        public void Do(Command command) => Move(command.previous, command.next);

        public void Undo(Command command) => Move(command.next, command.previous);

        private void Move(Model.Block from, Model.Block to)
        {
            var blockToMove = FindObjectsByType<MovableBlock>(FindObjectsSortMode.None)
                .First(block => block.model.location == from.location);
            blockToMove.model = to;
            Level.Instance.CheckCompletion();
            var targetLocation = to.location.asVector3;
            Tween(blockToMove.gameObject, targetLocation);
        }

        private void Tween(GameObject objectToMove, Vector3 targetLocation)
        {
            if (!animations.ContainsKey(objectToMove)) animations.Add(objectToMove, new Queue<Vector3>());
            if (animations[objectToMove].Count == 0) CreateTween(objectToMove, targetLocation);
            animations[objectToMove].Enqueue(targetLocation);
            
            void CreateTween(GameObject o, Vector3 position)
            {
                var tween = LeanTween.move(o, position, 1f).setEase(LeanTweenType.easeOutQuad);
                tween.setOnComplete(() => StartNext(o));
            }

            void StartNext(GameObject obj)
            {
                animations[obj].Dequeue();
                if (!animations[obj].TryPeek(out var next)) return;
                CreateTween(objectToMove, next);
            }
        }
    }
}