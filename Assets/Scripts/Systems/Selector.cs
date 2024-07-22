using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Systems
{
    public class Selector : Singleton<Selector>
    {
        private Option<MovableBlock> selected = Option<MovableBlock>.None;

        public void Select(MovableBlock block)
        {
            if (selected.IsSome(out var previous) && block != previous)
                previous.GetComponent<Outline>().enabled = false;

            if (previous == block)
                return; 
            
            block.GetComponent<Outline>().enabled = true;
            selected = Option<MovableBlock>.Some(block);
        }

        public void TryMoveTo(Location destination)
        {
            if (!selected.IsSome(out var movable))
                return;
            
            var move = new Move(movable.model.location, destination, movable.model.type);
            Controller.Instance.TryMove(move);
        }

        public void Deselect()
        {
            if (!selected.IsSome(out var movable))
                return;
            movable.GetComponent<Outline>().enabled = false;
            selected = Option<MovableBlock>.None;
            Spawner.Instance.HideHighlights();
        }
    }
}