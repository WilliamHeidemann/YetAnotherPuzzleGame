using GameState;
using Model;
using NUnit.Framework;

namespace Tests
{
    public class HistoryTests
    {
        private History history;
        private Move cardinalMove1;
        private Move cardinalMove2;
        private Move diagonalMove1;
        private Move diagonalMove2;

        [SetUp]
        public void Setup()
        {
            history = new History();

            cardinalMove1 = new Move(new Location(0, 0), new Location(0, 1), Type.Cardinal);
            cardinalMove2 = new Move(new Location(1, 0), new Location(1, 1), Type.Cardinal);

            diagonalMove1 = new Move(new Location(0, 0), new Location(1, 1), Type.Diagonal);
            diagonalMove2 = new Move(new Location(1, 1), new Location(2, 2), Type.Diagonal);
        }

        [Test]
        public void Add_AddsCardinalMove()
        {
            history.Add(cardinalMove1);

            Assert.AreEqual(1, history.cardinalCount);
            Assert.AreEqual(1, history.count);
        }

        [Test]
        public void Add_AddsDiagonalMove()
        {
            history.Add(diagonalMove1);

            Assert.AreEqual(1, history.diagonalCount);
            Assert.AreEqual(1, history.count);
        }

        [Test]
        public void Add_AddsMultipleMoves()
        {
            history.Add(cardinalMove1);
            history.Add(diagonalMove1);
            history.Add(cardinalMove2);
            history.Add(diagonalMove2);

            Assert.AreEqual(2, history.cardinalCount);
            Assert.AreEqual(2, history.diagonalCount);
            Assert.AreEqual(4, history.count);
        }

        [Test]
        public void HasUndo_ReturnsFalse_WhenNoCardinalMove()
        {
            var result = history.HasUndo(Type.Cardinal, out var move);

            Assert.IsFalse(result);
            Assert.AreEqual(default(Move), move);
        }

        [Test]
        public void HasUndo_ReturnsFalse_WhenNoDiagonalMove()
        {
            var result = history.HasUndo(Type.Diagonal, out var move);

            Assert.IsFalse(result);
            Assert.AreEqual(default(Move), move);
        }

        [Test]
        public void HasUndo_ReturnsTrue_WhenCardinalMoveExists()
        {
            history.Add(cardinalMove1);

            var result = history.HasUndo(Type.Cardinal, out var move);

            Assert.IsTrue(result);
            Assert.AreEqual(cardinalMove1, move);
        }

        [Test]
        public void HasUndo_ReturnsTrue_WhenDiagonalMoveExists()
        {
            history.Add(diagonalMove1);

            var result = history.HasUndo(Type.Diagonal, out var move);

            Assert.IsTrue(result);
            Assert.AreEqual(diagonalMove1, move);
        }

        [Test]
        public void Undo_RemovesCardinalMove()
        {
            history.Add(cardinalMove1);
            history.Undo(Type.Cardinal);

            Assert.AreEqual(0, history.cardinalCount);
            Assert.AreEqual(0, history.count);
        }

        [Test]
        public void Undo_RemovesDiagonalMove()
        {
            history.Add(diagonalMove1);
            history.Undo(Type.Diagonal);

            Assert.AreEqual(0, history.diagonalCount);
            Assert.AreEqual(0, history.count);
        }

        [Test]
        public void Undo_DoesNothing_WhenNoCardinalMove()
        {
            history.Undo(Type.Cardinal);

            Assert.AreEqual(0, history.cardinalCount);
            Assert.AreEqual(0, history.count);
        }

        [Test]
        public void Undo_DoesNothing_WhenNoDiagonalMove()
        {
            history.Undo(Type.Diagonal);

            Assert.AreEqual(0, history.diagonalCount);
            Assert.AreEqual(0, history.count);
        }
    }
}
