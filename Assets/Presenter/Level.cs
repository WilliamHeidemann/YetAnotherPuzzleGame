using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using UnityEngine;
using Model;
using ScriptableObjects;
using UnityUtils;
using UtilityToolkit.Runtime;
using Grid = Model.Grid;

namespace Presenter
{
    public class Level : Singleton<Level>
    {
        [SerializeField] private BlockLayout blockLayout;
        [SerializeField] private Block cardinalPrefab;
        [SerializeField] private Block diagonalPrefab;
        [SerializeField] private GhostBlock ghostPrefab;
        private Grid grid;
        private History history;
        public MoveData moveData { get; private set; }
        private readonly List<GhostBlock> ghostBlocks = new();
        
        protected override void Awake()
        {
            base.Awake();
            
            if (blockLayout == null) throw new Exception("Block Layout Scriptable Object has not been set.");
            grid = new Grid(blockLayout.width);
            history = new History();
            moveData = new MoveData(blockLayout.maxMoves);
            foreach (var block in blockLayout.list)
            {
                grid.AddBlock(block);
                InstantiateBlock(block);
            }
        }
        
        private void InstantiateBlock(Model.Block block)
        {
            var prefab = block.type switch
            {
                Model.Block.Type.Cardinal => cardinalPrefab,
                Model.Block.Type.Diagonal => diagonalPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };

            var position = block.location.asVector3;
            var sceneBlock = Instantiate(prefab, position, Quaternion.identity);
            sceneBlock.model = block;
        }

        public bool HasMoves() => moveData.used.Value < moveData.max;

        public void ShowGhostBlocks(Model.Block hover)
        {
            HideGhostBlocks();
            
            var middle = hover.location;
            var neighbors = hover.neighbors;
            
            foreach (var neighbor in neighbors)
            {
                if (!grid.IsAvailable(neighbor)) continue;
                var ghost = Instantiate(ghostPrefab, neighbor.asVector3, Quaternion.identity);
                ghost.model = new Model.Block(neighbor, hover.type);
                ghostBlocks.Add(ghost);
            }
        }

        public void HideGhostBlocks()
        {
            foreach (var ghostBlock in ghostBlocks)
            {
                Destroy(ghostBlock.gameObject);
            }
            ghostBlocks.Clear();
        }

        public void TryMove(Command command)
        {
            if (!grid.IsMoveValid(command)) return;
            grid.Move(command);
            history.Add(command);
            CommandManager.Instance.Do(command);
            moveData.IncrementCount();
        }

        
        public void RedoCardinalCommand() => RedoCommand(Model.Block.Type.Cardinal);
        public void RedoDiagonalCommand() => RedoCommand(Model.Block.Type.Diagonal);
        private void RedoCommand(Model.Block.Type type)
        {
            if (!history.HasRedo(type, out var command)) return;
            if (!grid.IsMoveValid(command)) return;
            HideGhostBlocks();
            history.Redo(type);
            grid.Move(command);
            CommandManager.Instance.Do(command);
            moveData.IncrementCount();
        }

        public void UndoCardinalCommand() => UndoCommand(Model.Block.Type.Cardinal);
        public void UndoDiagonalCommand() => UndoCommand(Model.Block.Type.Diagonal);
        private void UndoCommand(Model.Block.Type type)
        {
            if (!history.HasUndo(type, out var command)) return;
            if (!grid.IsUndoMoveValid(command)) return;
            HideGhostBlocks();
            history.Undo(type);
            grid.Move(command, true);
            CommandManager.Instance.Undo(command);
            moveData.DecrementCount();
        }

        public class MoveData
        {
            public readonly Observable<int> used;
            public readonly int max;

            public MoveData(int max)
            {
                this.max = max;
                used = new Observable<int>(0);
            }

            public void IncrementCount() => used.Value++;
            public void DecrementCount() => used.Value--;
        }
    }
}