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
        [SerializeField] private List<BlockLayout> levels;
        [SerializeField] private MovableBlock cardinalPrefab;
        [SerializeField] private MovableBlock diagonalPrefab;
        [SerializeField] private GhostBlock ghostPrefab;
        [SerializeField] private GameObject groundPrefab;
        [SerializeField] private Material blue;
        [SerializeField] private Material red;

        private Grid grid;
        private History history;
        private MoveData moveData;
        private readonly List<MovableBlock> movableBlocks = new();
        private readonly List<GhostBlock> ghostBlocks = new();
        private readonly List<GameObject> groundBlocks = new();
        private bool isLevelComplete;
        private BlockLayout currentLevel;
        private int currentLevelIndex;

        private IEnumerable<GameObject> AllBlocks()
        {
            var movables = movableBlocks.Select(b => b.gameObject);
            var ghosts = ghostBlocks.Select(b => b.gameObject);
            var ground = groundBlocks.Select(b => b.gameObject);
            return movables.Concat(ghosts).Concat(ground);
        }

        protected override void Awake()
        {
            base.Awake();
            ClearLevel();
            CreateLevel(levels[0]);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) ClearLevel();
        }

        private void ClearLevel()
        {
            LevelAnimator.BlocksOut(AllBlocks());
            isLevelComplete = false;
        }
        
        private void CreateLevel(BlockLayout level)
        {
            if (level == null) throw new Exception("Block Layout Scriptable Object has not been set.");
            currentLevel = level;
            currentLevelIndex++;
            grid = new Grid(level.width, level.height);
            history = new History();
            moveData = new MoveData(level.maxMoves);
            MoveCounter.Instance.Subscribe(moveData);
            
            For.NestedRange(level.height, level.width, InstantiateGroundBlock);

            foreach (var block in level.startingConfiguration)
            {
                grid.AddBlock(block);
                InstantiateBlock(block);
            }
            
            LevelAnimator.BlocksIn(AllBlocks());
        }

        private void InstantiateGroundBlock(int i, int j)
        {
            var position = new Vector3(j, -1, i);
            var groundBlock = Instantiate(groundPrefab, position, Quaternion.identity);
            groundBlocks.Add(groundBlock);
            var location = new Location(j, i);
            if (currentLevel.targetConfiguration.Any(block => block.location == location))
            {
                var block = currentLevel.targetConfiguration.First(block => block.location == location);
                groundBlock.GetComponent<MeshRenderer>().material =
                    block.type == Model.Block.Type.Cardinal ? blue : red;
            }
        }

        private void InstantiateBlock(Block block)
        {
            var prefab = block.type switch
            {
                Block.Type.Cardinal => cardinalPrefab,
                Block.Type.Diagonal => diagonalPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };

            var position = block.location.asVector3;
            var movableBlock = Instantiate(prefab, position, Quaternion.identity);
            movableBlock.model = block;
            movableBlocks.Add(movableBlock);
        }

        public bool HasMoves() => moveData.used.Value < moveData.max && isLevelComplete == false;

        public void ShowGhostBlocks(Model.Block hover)
        {
            HideGhostBlocks();

            var middle = hover.location;
            var neighbors = hover.neighbors;

            foreach (var neighbor in neighbors)
            {
                if (!grid.IsAvailable(neighbor)) continue;
                var ghost = Instantiate(ghostPrefab, middle.asVector3, Quaternion.identity);
                ghost.model = new Model.Block(neighbor, hover.type);
                ghost.origin = hover;
                ghost.GetComponent<MeshRenderer>().material = hover.type == Model.Block.Type.Cardinal ? blue : red;
                ghostBlocks.Add(ghost);
            }
        }

        public void HideGhostBlocks()
        {
            foreach (var ghostBlock in ghostBlocks)
            {
                // LeanTween.move(ghostBlock.gameObject, ghostBlock.origin.location.asVector3, 1f)
                //     .setEase(LeanTweenType.easeOutExpo);
                //     .setOnComplete(() => Destroy(ghostBlock.gameObject));
                Destroy(ghostBlock.gameObject);
            }

            ghostBlocks.Clear();
        }

        public void TryMove(Command command)
        {
            if (!grid.IsMoveValid(command)) return;
            history.Add(command);
            grid.Move(command);
            CommandManager.Instance.Do(command);
            moveData.IncrementCount();
            MovableBlock.NullifyHovered();
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
            MovableBlock.NullifyHovered();
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
            MovableBlock.NullifyHovered();
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

        public void CheckCompletion()
        {
            var board = grid.GetBlocks();
            var goal = currentLevel.targetConfiguration;
            isLevelComplete = board.All(block => goal.Contains(block));
            if (isLevelComplete)
            {
                NextLevel();
            }
        }

        private async void NextLevel()
        {
            await Awaitable.WaitForSecondsAsync(1);
            ClearLevel();
            await Awaitable.WaitForSecondsAsync(2);
            CreateLevel(levels[currentLevelIndex]);
        }
    }
}