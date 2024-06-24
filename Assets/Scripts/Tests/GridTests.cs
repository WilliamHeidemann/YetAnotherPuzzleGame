using System;
using System.Collections.Generic;
using System.Linq;
using GameState;
using Model;
using NUnit.Framework;
using Type = Model.Type;

namespace Tests
{
    public class GridTests
    {
        private Grid grid;
        private List<Block> initialBlocks;

        [SetUp]
        public void Setup()
        {
            initialBlocks = new List<Block>
            {
                new Block(new Location(0, 0), Type.Cardinal),
                new Block(new Location(1, 1), Type.Cardinal)
            };
            grid = new Grid(3, 3, initialBlocks);
        }

        [Test]
        public void AddBlock_AddsBlockToGrid()
        {
            var block = new Block(new Location(2, 2), Type.Cardinal);
            grid.AddBlock(block);

            Assert.IsTrue(grid.ContainsBlock(block));
        }

        [Test]
        public void AddBlock_DoesNotAddDuplicateBlock()
        {
            var block = new Block(new Location(0, 0), Type.Cardinal);
            grid.AddBlock(block);

            Assert.AreEqual(2, grid.GetBlocks().Count());
        }

        [Test]
        public void IsMoveValid_ReturnsFalse_IfNoBlockAtPreviousLocation()
        {
            var move = new Move(new Location(2, 2), new Location(2, 1), Type.Cardinal);

            Assert.IsFalse(grid.IsMoveValid(move));
        }

        [Test]
        public void IsMoveValid_ReturnsFalse_IfNextLocationIsOccupied()
        {
            var move = new Move(new Location(0, 0), new Location(1, 1), Type.Cardinal);

            Assert.IsFalse(grid.IsMoveValid(move));
        }

        [Test]
        public void IsMoveValid_ReturnsTrue_IfMoveIsValid()
        {
            var move = new Move(new Location(0, 0), new Location(0, 1), Type.Cardinal);

            Assert.IsTrue(grid.IsMoveValid(move));
        }

        [Test]
        public void Move_ThrowsException_IfMoveIsInvalid()
        {
            var move = new Move(new Location(2, 2), new Location(2, 1), Type.Cardinal);

            Assert.Throws<Exception>(() => grid.Move(move));
        }

        [Test]
        public void Move_MovesBlockToNewLocation()
        {
            var move = new Move(new Location(0, 0), new Location(0, 1), Type.Cardinal);
            grid.Move(move);

            Assert.IsTrue(grid.ContainsBlock(new Block(new Location(0, 1), Type.Cardinal)));
            Assert.IsFalse(grid.ContainsBlock(new Block(new Location(0, 0), Type.Cardinal)));
        }

        [Test]
        public void IsAvailable_ReturnsFalse_IfLocationIsOutOfBounds()
        {
            Assert.IsFalse(grid.IsAvailable(new Location(-1, -1)));
            Assert.IsFalse(grid.IsAvailable(new Location(3, 3)));
        }

        [Test]
        public void IsAvailable_ReturnsFalse_IfLocationIsOccupied()
        {
            Assert.IsFalse(grid.IsAvailable(new Location(0, 0)));
        }

        [Test]
        public void IsAvailable_ReturnsTrue_IfLocationIsUnoccupiedAndWithinBounds()
        {
            Assert.IsTrue(grid.IsAvailable(new Location(2, 2)));
        }

        [Test]
        public void GetBlocks_ReturnsAllBlocks()
        {
            var blocks = grid.GetBlocks();

            Assert.AreEqual(2, blocks.Count());
            Assert.IsTrue(blocks.Any(b => b.location.x == 0 && b.location.y == 0));
            Assert.IsTrue(blocks.Any(b => b.location.x == 1 && b.location.y == 1));
        }
    }
}