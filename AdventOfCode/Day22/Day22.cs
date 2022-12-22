namespace AdventOfCode.Day22 {
    public static class Day22 {
        public static void Go() {
            var input = File.ReadLines("Day22/Input.txt");
            var mapLines = input.TakeWhile(l => !string.IsNullOrWhiteSpace(l));
            var commandLine = input.Last();
            var commands = new Queue<string>(ReadCommands(commandLine));
            var (map, start) = ReadMap(mapLines);

            Console.WriteLine("Day 22, Star 1: {0}", Run1(map, start, new Queue<string>(commands)));
            Console.WriteLine("Day 22, Star 2: {0}", Run2(map, start, new Queue<string>(commands)));

        }

        private static int Run1(IDictionary<(int row, int column), (char value, int side)> map, (int row, int column) start, Queue<string> commands) {
            var currentDirection = 'R';
            var currentPosition = start;

            while (commands.Any()) {
                var command = commands.Dequeue();

                if (command == "L") {
                    currentDirection = TurnCounterClockwise(currentDirection);
                }
                else if (command == "R") {
                    currentDirection = TurnClockwise(currentDirection);
                }
                else {
                    var steps = int.Parse(command);

                    if (currentDirection == 'R') {
                        var currentRow = map.Where(x => x.Key.row == currentPosition.row);
                        var firstColumn = currentRow.Min(x => x.Key.column);
                        var lastColumn = currentRow.Max(x => x.Key.column);

                        for (int i = 0; i < steps; i++) {
                            var nextColumn = currentPosition.column + 1 > lastColumn ? firstColumn : currentPosition.column + 1;

                            if (map[(currentPosition.row, nextColumn)].value == '#') {
                                break;
                            }

                            currentPosition = (currentPosition.row, nextColumn);
                        }
                    }
                    else if (currentDirection == 'L') {
                        var currentRow = map.Where(x => x.Key.row == currentPosition.row);
                        var firstColumn = currentRow.Min(x => x.Key.column);
                        var lastColumn = currentRow.Max(x => x.Key.column);

                        for (int i = 0; i < steps; i++) {
                            var nextColumn = currentPosition.column - 1 < firstColumn ? lastColumn : currentPosition.column - 1;

                            if (map[(currentPosition.row, nextColumn)].value == '#') {
                                break;
                            }

                            currentPosition = (currentPosition.row, nextColumn);
                        }
                    }
                    else if (currentDirection == 'D') {
                        var currentColumn = map.Where(x => x.Key.column == currentPosition.column);
                        var firstRow = currentColumn.Min(x => x.Key.row);
                        var lastRow = currentColumn.Max(x => x.Key.row);

                        for (int i = 0; i < steps; i++) {
                            var nextRow = currentPosition.row + 1 > lastRow ? firstRow : currentPosition.row + 1;

                            if (map[(nextRow, currentPosition.column)].value == '#') {
                                break;
                            }

                            currentPosition = (nextRow, currentPosition.column);
                        }
                    }
                    else if (currentDirection == 'U') {
                        var currentColumn = map.Where(x => x.Key.column == currentPosition.column);
                        var firstRow = currentColumn.Min(x => x.Key.row);
                        var lastRow = currentColumn.Max(x => x.Key.row);

                        for (int i = 0; i < steps; i++) {
                            var nextRow = currentPosition.row - 1 < firstRow ? lastRow : currentPosition.row - 1;

                            if (map[(nextRow, currentPosition.column)].value == '#') {
                                break;
                            }

                            currentPosition = (nextRow, currentPosition.column);
                        }
                    }
                }
            }

            return 1000 * currentPosition.row + 4 * currentPosition.column + GetPoints(currentDirection);
        }

        private static int Run2(IDictionary<(int row, int column), (char value, int side)> map, (int row, int column) start, Queue<string> commands) {
            var currentDirection = 'R';
            var currentPosition = start;

            while (commands.Any()) {
                var command = commands.Dequeue();

                if (command == "L") {
                    currentDirection = TurnCounterClockwise(currentDirection);
                }
                else if (command == "R") {
                    currentDirection = TurnClockwise(currentDirection);
                }
                else {
                    var steps = int.Parse(command);

                    var side12 = map.Where(x => x.Value.side == 12);
                    var side12MinRow = side12.Min(x => x.Key.row);
                    var side12MaxRow = side12.Max(x => x.Key.row);
                    var side12MinColumn = side12.Min(x => x.Key.column);
                    var side12MaxColumn = side12.Max(x => x.Key.column);

                    var side13 = map.Where(x => x.Value.side == 13);
                    var side13MinRow = side13.Min(x => x.Key.row);
                    var side13MaxRow = side13.Max(x => x.Key.row);
                    var side13MinColumn = side13.Min(x => x.Key.column);
                    var side13MaxColumn = side13.Max(x => x.Key.column);

                    var side22 = map.Where(x => x.Value.side == 22);
                    var side22MinRow = side22.Min(x => x.Key.row);
                    var side22MaxRow = side22.Max(x => x.Key.row);
                    var side22MinColumn = side22.Min(x => x.Key.column);
                    var side22MaxColumn = side22.Max(x => x.Key.column);

                    var side31 = map.Where(x => x.Value.side == 31);
                    var side31MinRow = side31.Min(x => x.Key.row);
                    var side31MaxRow = side31.Max(x => x.Key.row);
                    var side31MinColumn = side31.Min(x => x.Key.column);
                    var side31MaxColumn = side31.Max(x => x.Key.column);

                    var side32 = map.Where(x => x.Value.side == 32);
                    var side32MinRow = side32.Min(x => x.Key.row);
                    var side32MaxRow = side32.Max(x => x.Key.row);
                    var side32MinColumn = side32.Min(x => x.Key.column);
                    var side32MaxColumn = side32.Max(x => x.Key.column);

                    var side41 = map.Where(x => x.Value.side == 41);
                    var side41MinRow = side41.Min(x => x.Key.row);
                    var side41MaxRow = side41.Max(x => x.Key.row);
                    var side41MinColumn = side41.Min(x => x.Key.column);
                    var side41MaxColumn = side41.Max(x => x.Key.column);

                    for (int i = 0; i < steps; i++) {
                        var currentSide = map[currentPosition].side;

                        if (currentDirection == 'R') {
                            var currentRow = map.Where(x => x.Key.row == currentPosition.row && x.Value.side == currentSide);
                            var firstColumn = currentRow.Min(x => x.Key.column);
                            var lastColumn = currentRow.Max(x => x.Key.column);
                            var nextPosition = (row: currentPosition.row, column: currentPosition.column + 1);
                            var nextDirection = currentDirection;

                            if (nextPosition.column > lastColumn) {
                                if (currentSide == 13) {
                                    var side13RowDelta = currentPosition.row - side13MinRow;
                                    nextPosition = (side32MaxRow - side13RowDelta, side32MaxColumn);
                                    nextDirection = 'L';
                                }
                                else if (currentSide == 22) {
                                    var side22RowDelta = currentPosition.row - side22MinRow;
                                    nextPosition = (side13MaxRow, side13MinColumn + side22RowDelta);
                                    nextDirection = 'U';
                                }
                                else if (currentSide == 32) {
                                    var side32RowDelta = currentPosition.row - side32MinRow;
                                    nextPosition = (side13MaxRow - side32RowDelta, side13MaxColumn);
                                    nextDirection = 'L';
                                }
                                else if (currentSide == 41) {
                                    var side41RowDelta = currentPosition.row - side41MinRow;
                                    nextPosition = (side32MaxRow, side32MinColumn + side41RowDelta);
                                    nextDirection = 'U';
                                }
                            }

                            if (map[nextPosition].value == '#') {
                                break;
                            }

                            currentPosition = nextPosition;
                            currentDirection = nextDirection;
                        }
                        else if (currentDirection == 'L') {
                            var currentRow = map.Where(x => x.Key.row == currentPosition.row && x.Value.side == currentSide);
                            var firstColumn = currentRow.Min(x => x.Key.column);
                            var lastColumn = currentRow.Max(x => x.Key.column);
                            var nextPosition = (row: currentPosition.row, column: currentPosition.column - 1);
                            var nextDirection = currentDirection;

                            if (nextPosition.column < firstColumn) {
                                if (currentSide == 12) {
                                    var side12RowDelta = currentPosition.row - side12MinRow;
                                    nextPosition = (side31MaxRow - side12RowDelta, side31MinColumn);
                                    nextDirection = 'R';
                                }
                                else if (currentSide == 22) {
                                    var side22RowDelta = currentPosition.row - side22MinRow;
                                    nextPosition = (side31MinRow, side31MinColumn + side22RowDelta);
                                    nextDirection = 'D';
                                }
                                else if (currentSide == 31) {
                                    var side31RowDelta = currentPosition.row - side31MinRow;
                                    nextPosition = (side12MaxRow - side31RowDelta, side12MinColumn);
                                    nextDirection = 'R';
                                }
                                else if (currentSide == 41) {
                                    var side41RowDelta = currentPosition.row - side41MinRow;
                                    nextPosition = (side12MinRow, side12MinColumn + side41RowDelta);
                                    nextDirection = 'D';
                                }
                            }

                            if (map[nextPosition].value == '#') {
                                break;
                            }

                            currentPosition = nextPosition;
                            currentDirection = nextDirection;
                        }
                        else if (currentDirection == 'D') {
                            var currentColumn = map.Where(x => x.Key.column == currentPosition.column && x.Value.side == currentSide);
                            var firstRow = currentColumn.Min(x => x.Key.row);
                            var lastRow = currentColumn.Max(x => x.Key.row);
                            var nextPosition = (row: currentPosition.row + 1, column: currentPosition.column);
                            var nextDirection = currentDirection;

                            if (nextPosition.row > lastRow) {
                                if (currentSide == 13) {
                                    var side13ColumnDelta = currentPosition.column - side13MinColumn;
                                    nextPosition = (side22MinRow + side13ColumnDelta, side22MaxColumn);
                                    nextDirection = 'L';
                                }
                                else if (currentSide == 32) {
                                    var side32ColumnDelta = currentPosition.column - side32MinColumn;
                                    nextPosition = (side41MinRow + side32ColumnDelta, side41MaxColumn);
                                    nextDirection = 'L';
                                }
                                else if (currentSide == 41) {
                                    var side41ColumnDelta = currentPosition.column - side41MinColumn;
                                    nextPosition = (side13MinRow, side13MinColumn + side41ColumnDelta);
                                    nextDirection = 'D';
                                }
                            }

                            if (map[nextPosition].value == '#') {
                                break;
                            }

                            currentPosition = nextPosition;
                            currentDirection = nextDirection;
                        }
                        else if (currentDirection == 'U') {
                            var currentColumn = map.Where(x => x.Key.column == currentPosition.column && x.Value.side == currentSide);
                            var firstRow = currentColumn.Min(x => x.Key.row);
                            var lastRow = currentColumn.Max(x => x.Key.row);
                            var nextPosition = (row: currentPosition.row - 1, column: currentPosition.column);
                            var nextDirection = currentDirection;

                            if (nextPosition.row < firstRow) {
                                if (currentSide == 12) {
                                    var side12ColumnDelta = currentPosition.column - side12MinColumn;
                                    nextPosition = (side41MinRow + side12ColumnDelta, side41MinColumn);
                                    nextDirection = 'R';
                                }
                                else if (currentSide == 13) {
                                    var side13ColumnDelta = currentPosition.column - side13MinColumn;
                                    nextPosition = (side41MaxRow, side41MinColumn + side13ColumnDelta);
                                    nextDirection = 'U';
                                }
                                else if (currentSide == 31) {
                                    var side31ColumnDelta = currentPosition.column - side31MinColumn;
                                    nextPosition = (side22MinRow + side31ColumnDelta, side22MinColumn);
                                    nextDirection = 'R';
                                }
                            }

                            if (map[nextPosition].value == '#') {
                                break;
                            }

                            currentPosition = nextPosition;
                            currentDirection = nextDirection;
                        }
                    }
                }
            }

            return 1000 * currentPosition.row + 4 * currentPosition.column + GetPoints(currentDirection);
        }

        private static int GetPoints(char currentDirection) => currentDirection switch {
            'R' => 0,
            'D' => 1,
            'L' => 2,
            'U' => 3,
            _ => throw new NotImplementedException()
        };

        private static char TurnClockwise(char currentDirection) => currentDirection switch {
            'U' => 'R',
            'R' => 'D',
            'D' => 'L',
            'L' => 'U',
            _ => throw new NotImplementedException()
        };

        private static char TurnCounterClockwise(char currentDirection) => currentDirection switch {
            'U' => 'L',
            'L' => 'D',
            'D' => 'R',
            'R' => 'U',
            _ => throw new NotImplementedException()
        };

        private static IEnumerable<string> ReadCommands(string commandLine) {
            while (!string.IsNullOrWhiteSpace(commandLine)) {
                int indexOfNextL = commandLine.IndexOf('L');
                int indexOfNextR = commandLine.IndexOf('R');
                int indexOfNextDirection;

                if (indexOfNextL > -1 && indexOfNextR > -1) {
                    indexOfNextDirection = Math.Min(indexOfNextL, indexOfNextR);
                }
                else if (indexOfNextL > -1) {
                    indexOfNextDirection = indexOfNextL;
                }
                else if (indexOfNextR > -1) {
                    indexOfNextDirection = indexOfNextR;
                }
                else {
                    yield return commandLine;
                    break;
                }

                yield return commandLine[0..indexOfNextDirection];
                yield return commandLine[indexOfNextDirection].ToString();
                commandLine = commandLine[(indexOfNextDirection + 1)..];
            }
        }

        private static (IDictionary<(int row, int column), (char value, int side)> map, (int row, int column) start) ReadMap(IEnumerable<string> mapLines) {
            var map = new Dictionary<(int row, int column), (char value, int side)>();
            var sideSize = mapLines.First().Length / 3;

            for (int row = 1; row <= mapLines.Count(); row++) {
                var line = mapLines.ElementAt(row - 1);

                for (int column = 1; column <= line.Length; column++) {
                    var value = line.ElementAt(column - 1);
                    var side1 = (int)Math.Ceiling((decimal)row / (decimal)sideSize) * 10;
                    var side2 = (int)Math.Ceiling((decimal)column / (decimal)sideSize);

                    if (value == '.' || value == '#') {
                        map.Add((row, column), (value, side1 + side2));
                    }
                }
            }

            return (map, map.First(x => x.Value.value == '.').Key);
        }

        private static void Print(IDictionary<(int row, int column), char> map) {
            for (int row = 1; row <= map.Keys.Max(x => x.row); row++) {
                for (int column = 1; column <= map.Keys.Max(x => x.column); column++) {
                    if (map.TryGetValue((row, column), out var value)) {
                        Console.Write(value);
                    }
                    else {
                        Console.Write(' ');
                    }
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}
