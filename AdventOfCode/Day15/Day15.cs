namespace AdventOfCode.Day15 {
    public static class Day15 {
        const int ROW_PART_1 = 2_000_000;
        const int ROW_LIMIT_PART_2 = 4_000_000;

        public static void Go() {
            var input = File.ReadLines("Day15/Input.txt");

            Console.Write("Day 15, Parsing input: ");

            var sensors = input.Select((line, i) => {
                var numbers = line.Split(',').Select(int.Parse).ToArray();
                var sensor = (column: numbers[0], row: numbers[1]);
                var closestBeacon = (column: numbers[2], row: numbers[3]);
                var closestBeaconDistance = GetManhattanDistance(sensor, closestBeacon);

                Console.Write("{0} ", i + 1);

                return GetManhattanRadius(sensor, closestBeaconDistance)
                    .GroupBy(x => x.row)
                    .ToDictionary(x => x.Key, z => {
                        var columns = z.Where(y => y.row == z.Key).Select(x => x.column).ToArray();

                        if (columns.Length == 2) {
                            return (start: columns.Min(), end: columns.Max());
                        }

                        return (start: columns[0], end: columns[0]);
                    });
            })
            .ToList();

            Console.Write(Environment.NewLine);

            var sensorsInRow = sensors.Where(x => x.ContainsKey(ROW_PART_1));
            var missingColumnsPart1 = sensorsInRow.Select(x => x[ROW_PART_1]).FindMissingNumbers();
            var columnStart = sensorsInRow.Select(x => x[ROW_PART_1]).Min(x => x.start);
            var columnEnd = sensorsInRow.Select(x => x[ROW_PART_1]).Max(x => x.end);
            var columnCount = Math.Abs(columnStart - columnEnd);

            Console.WriteLine("Day 15, Star 1: {0}", columnCount - missingColumnsPart1.Count());

            for (int row = 0; row < ROW_LIMIT_PART_2; row++) {
                var missingColumnsPart2 = sensors
                    .Where(x => x.ContainsKey(row))
                    .Select(x => x[row])
                    .FindMissingNumbers();

                if (missingColumnsPart2.Any()) {
                    var column = missingColumnsPart2.Single();
                    Console.WriteLine("Day 15, Star 1: {0}", (column * 4_000_000L) + row);
                    break;
                }
            }
        }

        private static int GetManhattanDistance((int column, int row) source, (int column, int row) destination) {
            int columnDelta = source.column - destination.column;
            int rowDelta = source.row - destination.row;

            return Math.Abs(columnDelta) + Math.Abs(rowDelta);
        }

        private static IEnumerable<(int column, int row)> GetManhattanRadius((int column, int row) source, int distance) {
            yield return (source.column + distance, source.row);
            yield return (source.column - distance, source.row);
            yield return (source.column, source.row + distance);
            yield return (source.column, source.row - distance);

            for (var columnDelta = 1; columnDelta < distance; columnDelta++) {
                var rowDelta = distance - columnDelta;

                yield return (source.column + columnDelta, source.row + rowDelta);
                yield return (source.column + columnDelta, source.row - rowDelta);
                yield return (source.column - columnDelta, source.row + rowDelta);
                yield return (source.column - columnDelta, source.row - rowDelta);
            }
        }

        private static IEnumerable<int> FindMissingNumbers(this IEnumerable<(int start, int end)> ranges) {
            var sortedRanges = ranges
                .Where(x => ranges.Count(y => y.start <= x.start && y.end >= x.end) == 1)
                .OrderBy(x => x.start)
                .ThenBy(x => x.end)
                .ToArray();

            for (var i = 1; i < sortedRanges.Count(); i++) {
                var thisRange = sortedRanges[i];
                var previousRange = sortedRanges[i - 1];

                if (previousRange.end < thisRange.start - 1) {
                    yield return previousRange.end + 1;
                }
            }
        }
    }
}
