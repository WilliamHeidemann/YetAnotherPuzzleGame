using System.Collections.Generic;
using NUnit.Framework;
using GameState;
using Model;
using Type = Model.Type;

namespace Tests
{
    [TestFixture]
    public class RewindHistoryTests
    {
        private History history;
        private Type typeCardinal;
        private Type typeDiagonal;
        private Move move1;
        private Move move2;
        private Move moveUndo1;
        private Move moveUndo2;

        [SetUp]
        public void SetUp()
        {
            history = new History();
            typeCardinal = Type.Cardinal;
            typeDiagonal = Type.Diagonal;

            move1 = new Move(new Location(0, 0), new Location(1, 1), typeCardinal);
            move2 = new Move(new Location(1, 1), new Location(0, 0), typeCardinal);
            moveUndo1 = new Move(new Location(1, 1), new Location(0, 0), typeCardinal, true);
            moveUndo2 = new Move(new Location(0, 0), new Location(1, 1), typeCardinal, true);
        }

        [Test]
        public void TestRewindAvoidsInfiniteLoop()
        {
            history.Add(move1);
            history.Add(move2);
            history.Add(moveUndo1);
            history.Add(moveUndo2);

            var count = 0;
            while (history.Count(typeCardinal) > 0)
            {
                var moveOption = history.GetLastMove(typeCardinal);
                if (moveOption.IsSome(out var move))
                {
                    history.Undo(move);
                    count++;
                    if (count == 6)
                    {
                        Assert.Fail("Infinite loop detected during rewind.");
                    }
                }
            }

            Assert.AreEqual(0, history.Count(typeCardinal));
        }

        [Test]
        public void TestRewindWithMixedMoves()
        {
            history.Add(move1);
            history.Add(move2);
            history.Add(new Move(new Location(2, 2), new Location(3, 3), typeDiagonal));

            var count = 0;
            while (history.Count(typeCardinal) > 0)
            {
                var moveOption = history.GetLastMove(typeCardinal);
                if (moveOption.IsSome(out var move))
                {
                    history.Undo(move);
                    count++;
                    if (count == 6)
                    {
                        Assert.Fail("Infinite loop detected during rewind.");
                    }
                }
            }

            Assert.AreEqual(0, history.Count(typeCardinal));
        }

        [Test]
        public void TestRewindHandlesNoUndoMoves()
        {
            history.Add(move1);
            history.Add(move2);

            var count = 0;
            while (history.Count(typeCardinal) > 0)
            {
                var moveOption = history.GetLastMove(typeCardinal);
                if (moveOption.IsSome(out var move))
                {
                    history.Undo(move);
                    count++;
                    if (count == 6)
                    {
                        Assert.Fail("Infinite loop detected during rewind.");
                    }
                }
            }

            Assert.AreEqual(0, history.Count(typeCardinal));
        }

        [Test]
        public void TestRewindHandlesMixedUndoAndMoves()
        {
            history.Add(move1);
            history.Add(moveUndo1);
            history.Add(move2);
            history.Add(moveUndo2);

            var count = 0;
            while (history.Count(typeCardinal) > 0)
            {
                var moveOption = history.GetLastMove(typeCardinal);
                if (moveOption.IsSome(out var move))
                {
                    history.Undo(move);
                    count++;
                    if (count == 6)
                    {
                        Assert.Fail("Infinite loop detected during rewind.");
                    }
                }
            }

            Assert.AreEqual(0, history.Count(typeCardinal));
        }
    }
}