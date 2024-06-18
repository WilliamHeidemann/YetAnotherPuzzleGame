using System.Linq;
using Commands;
using UnityEngine;
using UnityUtils;

namespace Presenter
{
    public class CommandManager : Singleton<CommandManager>
    {
        private Model.Block toMove;
        private LTDescr activeTween;

        public void Hover(Block block)
        {
            toMove = block.model;
        }

        public void Select(Model.Block toLand)
        {
            Level.Instance.HideGhostBlocks();
            var command = new Command(toMove, toLand, toMove.type);
            Level.Instance.TryMove(command);
        }

        public void Do(Command command)
        {
            var blockToMove = FindObjectsByType<Block>(FindObjectsSortMode.None)
                .First(block => block.model.location == command.previous.location);
            blockToMove.model = command.next;
            var targetLocation = command.next.location.asVector3;
            Tween(blockToMove.gameObject, targetLocation);
        }
        
        public void Undo(Command command)
        {
            var blockToMove = FindObjectsByType<Block>(FindObjectsSortMode.None)
                .First(block => block.model.location == command.next.location);
            blockToMove.model = command.previous;
            var targetLocation = command.previous.location.asVector3;
            Tween(blockToMove.gameObject, targetLocation);
        }
        
        private void Tween(GameObject objectToMove, Vector3 targetLocation)
        {
            if (activeTween != null)
            {
                LeanTween.cancel(activeTween.id);
            }
            activeTween = LeanTween.move(objectToMove, targetLocation, 2f).setEase(LeanTweenType.easeOutExpo);
        }
    }
}