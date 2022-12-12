namespace AdventOfCode.Day12 {
    public static class Day12 {
        public static void Go() {
            var input = File.ReadLines("Day12/Input.txt");
            var gridPart1 = new Node[input.Count(), input.First().Length];
            var gridPart2 = new Node[input.Count(), input.First().Length];

            for (int row = 0; row < input.Count(); row++) {
                for (int column = 0; column < input.First().Length; column++) {
                    gridPart1[row, column] = new Node(row, column, input.ElementAt(row)[column]);
                    gridPart2[row, column] = new Node(row, column, input.ElementAt(row)[column]);
                }
            }

            Console.WriteLine("Day 12, Star 1: {0}", Dijkstra(gridPart1, 'S', 'E').Distance);
            Console.WriteLine("Day 12, Star 2: {0}", Dijkstra(gridPart2, 'E', 'a', backwards: true).Distance);
        }

        private static Node Dijkstra(Node[,] grid, char sourceValue, char destinationValue, bool backwards = false) {
            var source = default(Node);
            var destination = default(Node);
            var remainingNodes = new List<Node>();

            for (int row = 0; row < grid.GetLength(0); row++) {
                for (int column = 0; column < grid.GetLength(1); column++) {
                    var node = grid[row, column];

                    remainingNodes.Add(node);

                    if (grid[row, column].Value == sourceValue) {
                        source = node;
                        source.UpdateDistance(0);
                    }
                    else if (grid[row, column].Value == destinationValue) {
                        destination = node;
                    }
                }
            }

            while (remainingNodes.Any()) {
                var node = remainingNodes.OrderBy(x => x.Distance).First();

                if (node.Value == destinationValue) {
                    return node;
                }

                remainingNodes.Remove(node);

                var reacheableNodes = GetReacheableNodes(node, grid, backwards).Where(x => remainingNodes.Contains(x));

                foreach (var item in reacheableNodes) {
                    if (node.Distance <= item.Distance) {
                        item.UpdateDistance(node.Distance + 1);
                    }
                }
            }

            throw new InvalidOperationException();
        }

        public static IEnumerable<Node> GetReacheableNodes(Node node, Node[,] grid, bool backwards = false) {
            if (backwards) {
                foreach (var item in GetBackwardReacheableNodes(node, grid)) {
                    yield return item;
                }

                yield break;
            }

            var upperRow = node.Row - 1;
            var lowerRow = node.Row + 1;
            var rightColumn = node.Column + 1;
            var leftColumn = node.Column - 1;

            if (upperRow >= 0) {
                var heightDelta = grid[upperRow, node.Column].Value.ToHeight() - node.Height;

                if (heightDelta <= 1) {
                    yield return grid[upperRow, node.Column];
                }
            }

            if (lowerRow <= grid.GetLength(0) - 1) {
                var heightDelta = grid[lowerRow, node.Column].Value.ToHeight() - node.Height;

                if (heightDelta <= 1) {
                    yield return grid[lowerRow, node.Column];
                }
            }

            if (rightColumn <= grid.GetLength(1) - 1) {
                var heightDelta = grid[node.Row, rightColumn].Value.ToHeight() - node.Height;

                if (heightDelta <= 1) {
                    yield return grid[node.Row, rightColumn];
                }
            }

            if (leftColumn >= 0) {
                var heightDelta = grid[node.Row, leftColumn].Value.ToHeight() - node.Height;

                if (heightDelta <= 1) {
                    yield return grid[node.Row, leftColumn];
                }
            }
        }

        public static IEnumerable<Node> GetBackwardReacheableNodes(Node node, Node[,] grid) {
            var upperRow = node.Row - 1;
            var lowerRow = node.Row + 1;
            var rightColumn = node.Column + 1;
            var leftColumn = node.Column - 1;

            if (upperRow >= 0) {
                var heightDelta = grid[upperRow, node.Column].Value.ToHeight() - node.Height;

                if (heightDelta >= -1) {
                    yield return grid[upperRow, node.Column];
                }
            }

            if (lowerRow <= grid.GetLength(0) - 1) {
                var heightDelta = grid[lowerRow, node.Column].Value.ToHeight() - node.Height;

                if (heightDelta >= -1) {
                    yield return grid[lowerRow, node.Column];
                }
            }

            if (rightColumn <= grid.GetLength(1) - 1) {
                var heightDelta = grid[node.Row, rightColumn].Value.ToHeight() - node.Height;

                if (heightDelta >= -1) {
                    yield return grid[node.Row, rightColumn];
                }
            }

            if (leftColumn >= 0) {
                var heightDelta = grid[node.Row, leftColumn].Value.ToHeight() - node.Height;

                if (heightDelta >= -1) {
                    yield return grid[node.Row, leftColumn];
                }
            }
        }
    }

    public class Node {
        public Node(int row, int column, char value) {
            Row = row;
            Column = column;
            Value = value;
            Distance = 1_000_000;
        }

        public int Row { get; }
        public int Column { get; }
        public char Value { get; }
        public int Distance { get; private set; }
        public int Height => Value.ToHeight();
        public void UpdateDistance(int value) => Distance = value;
    }

    public static class CharExtensions {
        public static int ToHeight(this char value) => value == 'S' ? 'a' : (value == 'E' ? 'z' : value);
    }
}
