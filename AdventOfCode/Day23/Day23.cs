namespace AdventOfCode.Day23 {
    public static class Day23 {
        public static void Go() {
            var input = File.ReadLines("Day23/Sample.txt");
            var map = ReadMap(input);
            var directions = new List<char>() { 'N', 'S', 'W', 'E' };

            var mapStar1 = new Dictionary<(int row, int column), char>(map);
            var directionsStar1 = new List<char>(directions);

            for (int round = 1; round <= 10; round++) {
                Move(mapStar1, directionsStar1);
            }

            Console.WriteLine("Day 23, Star 1: {0}", CountEmpty(mapStar1));

            var mapStar2 = new Dictionary<(int row, int column), char>(map);
            var directionsStar2 = new List<char>(directions);

            for (int round = 1; round <= 10_000; round++) {
                if (!Move(mapStar2, directionsStar2)) {
                    Console.WriteLine("Day 23, Star 2: {0}", round);
                    break;
                }
            }
        }

        private static bool Move(IDictionary<(int row, int column), char> map, List<char> directionOrder) {
            var proposedMoves = new Dictionary<(int row, int column), (int row, int column)>();

            ExpandMap(map);

            foreach (var elf in map.Where(x => x.Value == '#')) {
                var positionN = (elf.Key.row - 1, elf.Key.column);
                var positionNE = (elf.Key.row - 1, elf.Key.column + 1);
                var positionE = (elf.Key.row, elf.Key.column + 1);
                var positionSE = (elf.Key.row + 1, elf.Key.column + 1);
                var positionS = (elf.Key.row + 1, elf.Key.column);
                var positionSW = (elf.Key.row + 1, elf.Key.column - 1);
                var positionW = (elf.Key.row, elf.Key.column - 1);
                var positionNW = (elf.Key.row - 1, elf.Key.column - 1);
                var allPositions = new[] { positionN, positionNE, positionE, positionSE, positionS, positionSW, positionW, positionNW };

                if (allPositions.All(p => map[p] == '.')) {
                    continue;
                }

                foreach (var direction in directionOrder) {
                    if (direction == 'N') {
                        if (map[positionN] == '.' && map[positionNE] == '.' && map[positionNW] == '.') {
                            proposedMoves.Add(elf.Key, positionN);
                            break;
                        }
                    }
                    else if (direction == 'S') {
                        if (map[positionS] == '.' && map[positionSE] == '.' && map[positionSW] == '.') {
                            proposedMoves.Add(elf.Key, positionS);
                            break;
                        }
                    }
                    else if (direction == 'W') {
                        if (map[positionW] == '.' && map[positionNW] == '.' && map[positionSW] == '.') {
                            proposedMoves.Add(elf.Key, positionW);
                            break;
                        }
                    }
                    else if (direction == 'E') {
                        if (map[positionE] == '.' && map[positionNE] == '.' && map[positionSE] == '.') {
                            proposedMoves.Add(elf.Key, positionE);
                            break;
                        }
                    }
                }
            }

            if (proposedMoves.Count == 0) {
                return false;
            }

            foreach (var move in proposedMoves) {
                if (proposedMoves.Where(x => x.Value == move.Value).Count() > 1) {
                    continue;
                }

                map[move.Key] = '.';
                map[move.Value] = '#';
            }

            directionOrder.Add(directionOrder.First());
            directionOrder.RemoveAt(0);

            return true;
        }

        private static int CountEmpty(IDictionary<(int row, int column), char> map) {
            var rowMin = map.Where(x => x.Value == '#').Min(x => x.Key.row);
            var rowMax = map.Where(x => x.Value == '#').Max(x => x.Key.row);
            var columnMin = map.Where(x => x.Value == '#').Min(x => x.Key.column);
            var columnMax = map.Where(x => x.Value == '#').Max(x => x.Key.column);

            return map.Where(x => x.Key.row >= rowMin && x.Key.row <= rowMax)
                .Where(x => x.Key.column >= columnMin && x.Key.column <= columnMax)
                .Where(x => x.Value == '.')
                .Count();
        }

        private static IDictionary<(int row, int column), char> ReadMap(IEnumerable<string> mapLines) {
            var map = new Dictionary<(int row, int column), char>();

            for (int row = 1; row <= mapLines.Count(); row++) {
                var line = mapLines.ElementAt(row - 1);

                for (int column = 1; column <= line.Length; column++) {
                    map.Add((row + 10, column + 10), line.ElementAt(column - 1));
                }
            }

            return map;
        }

        private static void ExpandMap(IDictionary<(int row, int column), char> map) {
            var rowMin = map.Where(x => x.Value == '#').Min(x => x.Key.row) - 1;
            var rowMax = map.Where(x => x.Value == '#').Max(x => x.Key.row) + 1;
            var columnMin = map.Where(x => x.Value == '#').Min(x => x.Key.column) - 1;
            var columnMax = map.Where(x => x.Value == '#').Max(x => x.Key.column) + 1;

            for (int row = rowMin; row <= rowMax; row++) {
                for (int column = columnMin; column <= columnMax; column++) {
                    if (row == rowMin || row == rowMax || column == columnMin || column == columnMax) {
                        if (!map.ContainsKey((row, column))) {
                            map.Add((row, column), '.');
                        }
                    }
                }
            }
        }

        private static void Print(IDictionary<(int row, int column), char> map) {
            for (int row = map.Keys.Min(x => x.row); row <= map.Keys.Max(x => x.row); row++) {
                for (int column = map.Keys.Min(x => x.column); column <= map.Keys.Max(x => x.column); column++) {
                    if (map.TryGetValue((row, column), out var value)) {
                        Console.Write(value);
                    }
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}
