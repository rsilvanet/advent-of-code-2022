namespace AdventOfCode.Day14 {
    public static class Day14 {
        private const char AIR = '.';
        private const char ROCK = '#';
        private const char SAND = 'o';
        private const int FLOOR_SHIFT = 2;
        private const int COLUMN_SHIFT = 500;
        private static readonly (int row, int column) SandSource = (0, 500 + COLUMN_SHIFT);

        public static void Go() {
            var input = File.ReadLines("Day14/Input.txt");

            var countPart1 = 0;
            var cavePart1 = CreateCave(input);

            while (DropSand(cavePart1)) {
                countPart1++;
            }

            var countPart2 = 0;
            var cavePart2 = CreateCave(input, withFloor: true);

            while (DropSand(cavePart2)) {
                countPart2++;
            }

            Console.WriteLine("Day 14, Star 1: {0}", countPart1);
            Console.WriteLine("Day 14, Star 2: {0}", countPart2);
        }

        private static bool DropSand(char[,] cave) {
            var sandUnit = SandSource;

            for (int row = 0; row < cave.GetLength(0) - 1; row++) {
                var value = cave[row, sandUnit.column];

                sandUnit = (row, sandUnit.column);

                if (row == 0 && value == SAND && !CanSlide(cave, sandUnit)) {
                    return false;
                }

                if (value != AIR) {
                    var resting = false;

                    while (!resting) {
                        while (CanSlide(cave, sandUnit, out var direction)) {
                            if (direction == SlideDirection.LEFT) {
                                sandUnit = (sandUnit.row + 1, sandUnit.column - 1);
                            }
                            else if (direction == SlideDirection.RIGHT) {
                                sandUnit = (sandUnit.row + 1, sandUnit.column + 1);
                            }
                        }

                        if (!TryFindFloor(cave, sandUnit, out sandUnit.row)) {
                            return false;
                        }

                        resting = !CanSlide(cave, sandUnit);
                    }

                    cave[sandUnit.row, sandUnit.column] = SAND;
                    break;
                }
            }

            return true;
        }

        private static SlideDirection GetSlideDirection(char[,] cave, (int row, int column) sandUnit) {
            if (cave[sandUnit.row + 1, sandUnit.column] == AIR) {
                return SlideDirection.NONE;
            }
            else if (cave[sandUnit.row + 1, sandUnit.column - 1] == AIR) {
                return SlideDirection.LEFT;
            }
            else if (cave[sandUnit.row + 1, sandUnit.column + 1] == AIR) {
                return SlideDirection.RIGHT;
            }

            return SlideDirection.NONE;
        }

        private static bool CanSlide(char[,] cave, (int row, int column) sandUnit) {
            return GetSlideDirection(cave, sandUnit) != SlideDirection.NONE;
        }

        private static bool CanSlide(char[,] cave, (int row, int column) sandUnit, out SlideDirection slideDirection) {
            slideDirection = GetSlideDirection(cave, sandUnit);
            return slideDirection != SlideDirection.NONE;
        }

        private static bool TryFindFloor(char[,] cave, (int row, int column) sandUnit, out int floorRow) {
            floorRow = sandUnit.row;

            while (cave[floorRow + 1, sandUnit.column] == AIR) {
                floorRow++;

                if (floorRow + 1 >= cave.GetLength(0) || sandUnit.column + 1 >= cave.GetLength(1)) {
                    return false;
                }
            }

            return true;
        }

        private static char[,] CreateCave(IEnumerable<string> input, bool withFloor = false) {
            var paths = input.Select(p => p.Split(" -> ")
                .Select(c => {
                    var cSplit = c.Split(',');
                    var column = int.Parse(cSplit.First()) + COLUMN_SHIFT;
                    var row = int.Parse(cSplit.Last());
                    return (row, column);
                })
            );

            var rowMax = paths.Max(p => p.Max(c => c.row));
            var columnMax = paths.Max(p => p.Max(c => c.column));
            var cave = new char[rowMax + FLOOR_SHIFT + 1, columnMax + COLUMN_SHIFT];

            FillWithAir(cave, 0);
            MarkPaths(cave, paths);
            MarkSandSource(cave);

            if (withFloor) {
                AddFloorOnBottom(cave);
            }

            return cave;
        }

        private static void FillWithAir(char[,] cave, int columnMin) {
            for (int row = 0; row < cave.GetLength(0); row++) {
                for (int column = 0; column < cave.GetLength(1); column++) {
                    cave[row, column] = AIR;
                }
            }
        }

        private static void MarkPaths(char[,] cave, IEnumerable<IEnumerable<(int row, int column)>> paths) {
            foreach (var path in paths) {
                var previousPoint = path.First();

                foreach (var point in path) {
                    var rowMin = Math.Min(previousPoint.row, point.row);
                    var rowMax = Math.Max(previousPoint.row, point.row);
                    var rowRange = Enumerable.Range(rowMin, rowMax - rowMin + 1);

                    foreach (var row in rowRange) {
                        cave[row, point.column] = ROCK;
                    }

                    var columnMin = Math.Min(previousPoint.column, point.column);
                    var columnMax = Math.Max(previousPoint.column, point.column);
                    var columnRange = Enumerable.Range(columnMin, columnMax - columnMin + 1);

                    foreach (var column in columnRange) {
                        cave[point.row, column] = ROCK;
                    }

                    previousPoint = point;
                }
            }
        }

        private static void MarkSandSource(char[,] cave) {
            cave[SandSource.row, SandSource.column] = '+';
        }

        private static void AddFloorOnBottom(char[,] cave) {
            for (int column = 0; column < cave.GetLength(1); column++) {
                cave[cave.GetLength(0) - 1, column] = ROCK;
            }
        }

        private static void Print(char[,] cave) {
            for (int row = 0; row < cave.GetLength(0); row++) {
                for (int column = 0; column < cave.GetLength(1); column++) {
                    if (cave[row, column] != default) {
                        Console.Write(cave[row, column]);
                    }
                }

                Console.Write(Environment.NewLine);
            }
        }

        private enum SlideDirection { NONE, LEFT, RIGHT }
    }
}
