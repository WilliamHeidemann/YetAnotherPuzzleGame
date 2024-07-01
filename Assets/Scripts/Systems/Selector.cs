using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;

namespace Systems
{
    public class Selector : Singleton<Selector>
    {
        [SerializeField] private Image undo;
        private MovableBlock selected;

        public void Select(MovableBlock block)
        {
            if (selected != block && selected != null) 
                selected.GetComponent<Outline>().enabled = false;
            block.GetComponent<Outline>().enabled = true;
            selected = block;
            undo.gameObject.SetActive(true);
        }

        public void MoveTo(Location destination)
        {
            var move = new Move(selected.model.location, destination, selected.model.type);
            Controller.Instance.TryMove(move);
        }

        public void Undo()
        {
            Controller.Instance.TryUndo(selected.model);
        }
    }
}