namespace AdventOfCode.Day17 {
    public static class Day17 {
        public static void Go() {
            var input = File.ReadAllText("Day17/Input.txt");
            var jets = new CircularList<char>(input.ToCharArray());
            var rocks = Enumerable.Empty<Rock>();

            while (rocks.Count() < 2022) {
                var rock = GenerateNewRock(rocks);
                
                while (true) {
                    if (jets.Current() == '<' && rock.CanMoveLeft(rocks)) {
                        rock.MoveLeft();
                    }
                    else if (jets.Current() == '>' && rock.CanMoveRight(rocks)) {
                        rock.MoveRight();
                    }

                    jets.Next();

                    if (rock.CanMoveDown(rocks)) {
                        rock.MoveDown();
                    }
                    else {
                        break;
                    }
                }

                rocks = rocks.Append(rock);
            }

            Console.WriteLine("Day 17, Star 1: {0}", rocks.Max(x => x.Points.Max(y => y.Row)) + 1);
            Console.WriteLine("Day 17, Star 2: {0}", "¯\\_(ツ)_/¯");
        }

        private static Rock GenerateNewRock(IEnumerable<Rock> rocks) {
            var highestRow = rocks.Any() ? rocks.Max(x => x.Points.Max(y => y.Row)) : -1;
            var nextRockType = GetNextRockType(rocks.Any() ? rocks.Last().Type : null);

            return GenerateNewRock(highestRow, nextRockType);
        }

        private static RockType GetNextRockType(RockType? lastRockType) => lastRockType switch {
            RockType.Horizontal => RockType.Cross,
            RockType.Cross => RockType.L,
            RockType.L => RockType.Vertical,
            RockType.Vertical => RockType.Square,
            RockType.Square => RockType.Horizontal,
            _ => RockType.Horizontal
        };

        private static Rock GenerateNewRock(long highestRow, RockType type) => type switch {
            RockType.Horizontal => GenerateHorizontalRock(highestRow),
            RockType.Cross => GenerateCrossRock(highestRow),
            RockType.L => GenerateLRock(highestRow),
            RockType.Vertical => GenerateVerticalRock(highestRow),
            RockType.Square => GenerateSquareRock(highestRow),
            _ => throw new NotImplementedException()
        };

        private static Rock GenerateHorizontalRock(long highestRow) {
            return new Rock(RockType.Horizontal, new[] {
                (highestRow + 4, 2),
                (highestRow + 4, 3),
                (highestRow + 4, 4),
                (highestRow + 4, 5)
            });
        }

        private static Rock GenerateCrossRock(long highestRow) {
            return new Rock(RockType.Cross, new[] {
                (highestRow + 4, 3),
                (highestRow + 5, 2),
                (highestRow + 5, 3),
                (highestRow + 5, 4),
                (highestRow + 6, 3)
            });
        }

        private static Rock GenerateLRock(long highestRow) {
            return new Rock(RockType.L, new[] {
                (highestRow + 4, 2),
                (highestRow + 4, 3),
                (highestRow + 4, 4),
                (highestRow + 5, 4),
                (highestRow + 6, 4)
            });
        }

        private static Rock GenerateVerticalRock(long highestRow) {
            return new Rock(RockType.Vertical, new[] {
                (highestRow + 4, 2),
                (highestRow + 5, 2),
                (highestRow + 6, 2),
                (highestRow + 7, 2)
            });
        }

        private static Rock GenerateSquareRock(long highestRow) {
            return new Rock(RockType.Square, new[] {
                (highestRow + 4, 2),
                (highestRow + 4, 3),
                (highestRow + 5, 2),
                (highestRow + 5, 3)
            });
        }

        private static void Print(IEnumerable<Rock> rocks) {
            for (long row = rocks.Max(x => x.Points.Max(y => y.Row)); row >= 0; row--) {
                for (int column = 0; column < 7; column++) {
                    if (rocks.Any(x => x.Points.Any(y => y.Row == row && y.Column == column))) {
                        Console.Write('#');
                    }
                    else {
                        Console.Write('.');
                    }
                }
                Console.Write(Environment.NewLine);
            }
            Console.Write(Environment.NewLine);
        }
    }

    public class Rock {
        public Rock(RockType type, (long Row, int Column)[] points) {
            Type = type;
            Points = points;
        }

        public RockType Type { get; }
        public (long Row, int Column)[] Points { get; }

        public void MoveDown() {
            for (long i = 0; i < Points.Length; i++) {
                Points[i].Row--;
            }
        }

        public void MoveLeft() {
            for (long i = 0; i < Points.Length; i++) {
                Points[i].Column--;
            }
        }

        public void MoveRight() {
            for (long i = 0; i < Points.Length; i++) {
                Points[i].Column++;
            }
        }

        public bool CanMoveDown(IEnumerable<Rock> rocks) {
            if (Points.Min(x => x.Row) == 0) {
                return false;
            }

            var downSimulation = Points.Select(x => (x.Row - 1, x.Column));

            if (rocks.Any(x => x.Points.Intersect(downSimulation).Any())) {
                return false;
            }

            return true;
        }

        public bool CanMoveLeft(IEnumerable<Rock> rocks) {
            if (Points.Min(x => x.Column) == 0) {
                return false;
            }

            var leftSimulation = Points.Select(x => (x.Row, x.Column - 1));

            if (rocks.Any(x => x.Points.Intersect(leftSimulation).Any())) {
                return false;
            }

            return true;
        }

        public bool CanMoveRight(IEnumerable<Rock> rocks) {
            if (Points.Max(x => x.Column) == 6) {
                return false;
            }

            var rightSimulation = Points.Select(x => (x.Row, x.Column + 1));

            if (rocks.Any(x => x.Points.Intersect(rightSimulation).Any())) {
                return false;
            }

            return true;
        }
    }

    public enum RockType {
        Horizontal,
        Cross,
        L,
        Vertical,
        Square
    }

    public class CircularList<T> : List<T> {
        private int Index;

        public CircularList() : this(0) { }

        public CircularList(IEnumerable<T> source) : this(0) {
            foreach (var item in source) {
                Add(item);
            }
        }

        public CircularList(int index) {
            Index = index;
        }

        public T Current() {
            return this[Index];
        }

        public T Next() {
            Index++;
            Index %= Count;

            return this[Index];
        }

        public T Previous() {
            Index--;

            if (Index < 0) {
                Index = Count - 1;
            }

            return this[Index];
        }

        public void Reset() {
            Index = 0;
        }

        public void MoveToEnd() {
            Index = Count - 1;
        }
    }
}
