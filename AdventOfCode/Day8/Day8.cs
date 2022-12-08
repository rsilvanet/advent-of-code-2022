using System.Diagnostics;
using System;

namespace AdventOfCode.Day8 {
    public static class Day8 {
        public static void Go() {
            var input = File.ReadLines("Day8/Input.txt");
            var forest = Array.Empty<Tree>();

            for (int x = 0; x < input.Count(); x++) {
                var row = input.ElementAt(x);
                for (int y = 0; y < row.Count(); y++) {
                    var height = (int)char.GetNumericValue(row.ElementAt(y));
                    forest = forest.Append(new Tree(x, y, height)).ToArray();
                }
            }

            Console.WriteLine("Day 8, Star 1: {0}", forest.Count(x => x.IsVisible(forest)));
            Console.WriteLine("Day 8, Star 2: {0}", forest.Max(t => t.GetScore(forest)));
        }
    }

    public class Tree {
        public Tree(int x, int y, int height) {
            X = x;
            Y = y;
            Height = height;
        }

        public int X { get; }
        public int Y { get; }
        public int Height { get; }

        public bool IsOnEdge(IEnumerable<Tree> forest) => X == 0 || Y == 0
            || X == forest.Max(t => t.X)
            || Y == forest.Max(t => t.Y);

        public bool IsVisible(IEnumerable<Tree> forest) => IsOnEdge(forest)
            || LookUp(forest).All(t => t.Height < Height)
            || LookDown(forest).All(t => t.Height < Height)
            || LookLeft(forest).All(t => t.Height < Height)
            || LookRight(forest).All(t => t.Height < Height);

        public int GetScore(IEnumerable<Tree> forest) => LookUp(forest).StopWhen(t => t.Height < Height).Count()
            * LookDown(forest).StopWhen(t => t.Height < Height).Count()
            * LookLeft(forest).StopWhen(t => t.Height < Height).Count()
            * LookRight(forest).StopWhen(t => t.Height < Height).Count();

        private IEnumerable<Tree> LookUp(IEnumerable<Tree> forest) => forest
            .Where(t => t.Y == Y && t.X < X)
            .OrderByDescending(t => t.X);

        private IEnumerable<Tree> LookDown(IEnumerable<Tree> forest) => forest
            .Where(t => t.Y == Y && t.X > X)
            .OrderBy(t => t.X);

        private IEnumerable<Tree> LookLeft(IEnumerable<Tree> forest) => forest
            .Where(t => t.X == X && t.Y < Y)
            .OrderByDescending(t => t.Y);

        private IEnumerable<Tree> LookRight(IEnumerable<Tree> forest) => forest
            .Where(t => t.X == X && t.Y > Y)
            .OrderBy(t => t.Y);
    }

    public static class EnumerableExtensions {
        public static Tree GetTree(this IEnumerable<Tree> forest, (int x, int y) position) {
            return forest.Single(t => t.X == position.x && t.Y == position.y);
        }

        public static IEnumerable<Tree> StopWhen(this IEnumerable<Tree> trees, Func<Tree, bool> condition) {
            foreach (var tree in trees) {
                if (condition(tree)) {
                    yield return tree;
                }
                else {
                    yield return tree;
                    yield break;
                }
            }
        }
    }
}
