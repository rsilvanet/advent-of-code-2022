using System.Numerics;

namespace AdventOfCode.Day24 {
    public static class Day24 {
        private static List<Blizzard> blizzards = new List<Blizzard>();
        private static Dictionary<int, List<Blizzard>> blizzardsCache = new Dictionary<int, List<Blizzard>>();
        private static Dictionary<(int row, int column), char> map = new Dictionary<(int row, int column), char>();
        private static (int row, int column) start;
        private static (int row, int column) finish;

        public static void Go() {
            var input = File.ReadLines("Day24/Input.txt");
            var arrows = new[] { '>', 'v', '<', '^' };

            map = ReadMap(input);
            start = (row: map.Min(x => x.Key.row), column: map.Min(x => x.Key.column));
            finish = (row: map.Max(x => x.Key.row), column: map.Max(x => x.Key.column));

            blizzards = map.Where(x => arrows.Contains(x.Value))
                .Select(x => new Blizzard() {
                    Row = x.Key.row,
                    Column = x.Key.column,
                    Direction = x.Value
                })
                .ToList();

            var memo = new Dictionary<((int, int), (int, int), int, int), int>();
            var bestTimeToFinish = Move(start, finish, 0, 300, memo);
            var bestTimeBackToStart = Move(finish, start, bestTimeToFinish, 600, memo);
            var bestTimeToFinishAgain = Move(start, finish, bestTimeBackToStart, 900, memo);

            Console.WriteLine("Day 24, Star 1: {0}", bestTimeToFinish);
            Console.WriteLine("Day 24, Star 2: {0}", bestTimeToFinishAgain);
        }

        private static int Move(
                (int row, int column) expedition,
                (int row, int column) finish,
                int minute,
                int bestTime,
                Dictionary<((int, int), (int, int), int, int), int> memo) {

            if (expedition == finish) {
                return Math.Min(bestTime, minute);
            }

            if (minute >= bestTime) {
                return bestTime;
            }

            var memoKey = (expedition, finish, minute, bestTime);

            if (memo.TryGetValue(memoKey, out var memoScore)) {
                return memoScore;
            }

            var expeditionUp = (expedition.row - 1, expedition.column);
            var expeditionDown = (expedition.row + 1, expedition.column);
            var expeditionLeft = (expedition.row, expedition.column - 1);
            var expeditionRight = (expedition.row, expedition.column + 1);
            var blizzardsAtNextMinute = GetBlizzardsAtMinute(minute + 1);

            if (!blizzardsAtNextMinute.Any(x => x.Position == expedition)) {
                bestTime = Move(expedition, finish, minute + 1, bestTime, memo);
            }

            if (map.ContainsKey(expeditionUp) && !blizzardsAtNextMinute.Any(x => x.Position == expeditionUp)) {
                bestTime = Move(expeditionUp, finish, minute + 1, bestTime, memo);
            }

            if (map.ContainsKey(expeditionDown) && !blizzardsAtNextMinute.Any(x => x.Position == expeditionDown)) {
                bestTime = Move(expeditionDown, finish, minute + 1, bestTime, memo);
            }

            if (map.ContainsKey(expeditionLeft) && !blizzardsAtNextMinute.Any(x => x.Position == expeditionLeft)) {
                bestTime = Move(expeditionLeft, finish, minute + 1, bestTime, memo);
            }

            if (map.ContainsKey(expeditionRight) && !blizzardsAtNextMinute.Any(x => x.Position == expeditionRight)) {
                bestTime = Move(expeditionRight, finish, minute + 1, bestTime, memo);
            }

            memo.Add(memoKey, bestTime);

            return bestTime;
        }

        private static List<Blizzard> GetBlizzardsAtMinute(int minute) {
            var minRow = start.row + 1;
            var maxRow = finish.row - 1;
            var minColumn = start.column;
            var maxColumn = finish.column;
            var newBlizzards = new List<Blizzard>();
            var lcm = maxRow * maxColumn / (int)BigInteger.GreatestCommonDivisor(maxRow, maxColumn);

            minute = minute % lcm;

            if (blizzardsCache.TryGetValue(minute, out var cached)) {
                return cached;
            }

            foreach (var blizzard in blizzards) {
                var newBlizzard = new Blizzard() {
                    Row = blizzard.Row,
                    Column = blizzard.Column,
                    Direction = blizzard.Direction
                };

                newBlizzards.Add(newBlizzard);

                if (blizzard.Direction == '^') {
                    for (int i = 0; i < minute; i++) {
                        newBlizzard.Row = newBlizzard.Row - 1 < minRow ? maxRow : newBlizzard.Row - 1;
                    }
                }
                else if (blizzard.Direction == 'v') {
                    for (int i = 0; i < minute; i++) {
                        newBlizzard.Row = newBlizzard.Row + 1 > maxRow ? minRow : newBlizzard.Row + 1;
                    }
                }
                else if (blizzard.Direction == '<') {
                    for (int i = 0; i < minute; i++) {
                        newBlizzard.Column = newBlizzard.Column - 1 < minColumn ? maxColumn : newBlizzard.Column - 1;
                    }
                }
                else if (blizzard.Direction == '>') {
                    for (int i = 0; i < minute; i++) {
                        newBlizzard.Column = newBlizzard.Column + 1 > maxColumn ? minColumn : newBlizzard.Column + 1;
                    }
                }
            }

            blizzardsCache.Add(minute, newBlizzards);

            return newBlizzards;
        }

        private static Dictionary<(int row, int column), char> ReadMap(IEnumerable<string> mapLines) {
            var map = new Dictionary<(int row, int column), char>();

            for (int row = 0; row < mapLines.Count(); row++) {
                var line = mapLines.ElementAt(row);

                for (int column = 0; column < line.Length; column++) {
                    if (line.ElementAt(column) != '#') {
                        map.Add((row, column), line.ElementAt(column));
                    }
                }
            }

            return map;
        }
    }

    public class Blizzard {
        public int Row { get; set; }
        public int Column { get; set; }
        public char Direction { get; set; }
        public (int, int) Position => (Row, Column);
    }
}
