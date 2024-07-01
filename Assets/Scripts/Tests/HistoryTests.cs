// using System.Collections.Generic;
// using System.Linq;
// using NUnit.Framework;
// using GameState;
// using Model;
// using Type = Model.Type;
//
// namespace Tests
// {
//     [TestFixture]
//     public class HistoryTests
//     {
//         private History history;
//         private Type typeCardinal;
//         private Type typeDiagonal;
//         private Move move1;
//         private Move move2;
//         private Move moveUndo;
//
//         [SetUp]
//         public void SetUp()
//         {
//             history = new History();
//             typeCardinal = Type.Cardinal;
//             typeDiagonal = Type.Diagonal;
//
//             move1 = new Move(new Location(0, 0), new Location(1, 1), typeCardinal);
//             move2 = new Move(new Location(1, 1), new Location(2, 2), typeDiagonal);
//             moveUndo = new Move(new Location(0, 0), new Location(1, 1), typeCardinal, true);
//         }
//
//         [Test]
//         public void TestAddMove()
//         {
//             history.Add(move1);
//             Assert.AreEqual(1, history.Count(typeCardinal));
//             Assert.AreEqual(1, history.count);
//         }
//
//         [Test]
//         public void TestAddMoveWithUndo()
//         {
//             history.Add(moveUndo);
//             Assert.AreEqual(0, history.Count(typeCardinal));
//             Assert.AreEqual(0, history.count);
//         }
//
//         [Test]
//         public void TestGetLastMove()
//         {
//             history.Add(move1);
//             var lastMove = history.GetLastMove(typeCardinal);
//             Assert.IsTrue(lastMove.IsSome(out var move));
//             Assert.AreEqual(move1, move);
//         }
//
//         [Test]
//         public void TestGetLastMoveWhenNone()
//         {
//             var lastMove = history.GetLastMove(typeCardinal);
//             Assert.IsFalse(lastMove.IsSome(out _));
//         }
//
//         [Test]
//         public void TestUndoMove()
//         {
//             history.Add(move1);
//             history.Undo(move1);
//             Assert.AreEqual(0, history.Count(typeCardinal));
//             Assert.AreEqual(0, history.count);
//         }
//
//         [Test]
//         public void TestGetMoveByLocation()
//         {
//             history.Add(move1);
//             var foundMove = history.GetMove(move1.next);
//             Assert.IsTrue(foundMove.IsSome(out var move));
//             Assert.AreEqual(move1, move);
//         }
//
//         [Test]
//         public void TestGetMoveByLocationWhenNone()
//         {
//             var location = new Location(3, 3);
//             var foundMove = history.GetMove(location);
//             Assert.IsFalse(foundMove.IsSome(out _));
//         }
//
//         [Test]
//         public void TestCountByType()
//         {
//             history.Add(move1);
//             history.Add(move2);
//             Assert.AreEqual(1, history.Count(typeCardinal));
//             Assert.AreEqual(1, history.Count(typeDiagonal));
//         }
//
//         [Test]
//         public void TestOverallCount()
//         {
//             history.Add(move1);
//             history.Add(move2);
//             Assert.AreEqual(2, history.count);
//         }
//     }
// }