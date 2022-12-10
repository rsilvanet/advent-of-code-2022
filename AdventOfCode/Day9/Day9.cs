namespace AdventOfCode.Day9 {
    public static class Day9 {
        public static void Go() {
            var input = File.ReadLines("Day9/Input.txt");

            var movements = input.Select(x => {
                var split = x.Split(' ');
                return (direction: split[0], steps: int.Parse(split[1]));
            });

            var snake = Enumerable.Repeat((0, 0), 10).ToArray();
            var visitationsPart1 = new List<(int, int)>();
            var visitationsPart2 = new List<(int, int)>();

            foreach (var movement in movements) {
                for (int i = 0; i < movement.steps; i++) {
                    snake[0] = MoveHead(snake[0], movement.direction);

                    for (int j = 1; j < snake.Count(); j++) {
                        snake[j] = MoveTail(snake[j - 1], snake[j]);
                    }

                    visitationsPart1.Add(snake.ElementAt(1));
                    visitationsPart2.Add(snake.Last());
                }
            }

            Console.WriteLine("Day 9, Star 1: {0}", visitationsPart1.Distinct().Count());
            Console.WriteLine("Day 9, Star 2: {0}", visitationsPart2.Distinct().Count());
        }

        private static (int row, int column) MoveHead((int row, int column) head, string direction) {
            switch (direction) {
                case "U":
                    return (head.row + 1, head.column);
                case "D":
                    return (head.row - 1, head.column);
                case "R":
                    return (head.row, head.column + 1);
                case "L":
                    return (head.row, head.column - 1);
                default:
                    throw new NotImplementedException();
            }
        }

        private static (int row, int column) MoveTail((int row, int column) head, (int row, int column) tail) {
            var rowDelta = head.row - tail.row;
            var columnDelta = head.column - tail.column;

            if (Math.Abs(rowDelta) <= 1 && Math.Abs(columnDelta) <= 1) {
                return tail;
            }

            tail.row += Math.Sign(rowDelta);
            tail.column += Math.Sign(columnDelta);

            return tail;
        }
    }
}
