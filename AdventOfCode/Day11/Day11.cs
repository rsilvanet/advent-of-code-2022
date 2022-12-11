namespace AdventOfCode.Day11 {
    public static class Day11 {
        public static void Go() {
            var input = File.ReadLines("Day11/Input.txt").ToList();
            var monkeysPart1 = input.Chunk(7).Select(x => new Monkey(x)).ToList();

            for (int round = 0; round < 20; round++) {
                foreach (var monkey in monkeysPart1) {
                    monkey.Throw(monkeysPart1, (worryLevel) => worryLevel / 3);
                }
            }

            var monkeysPart2 = input.Chunk(7).Select(x => new Monkey(x)).ToList();
            var modulus = monkeysPart2.Select(x => x.TestDivisor).Aggregate((x, y) => x * y);

            for (int round = 0; round < 10_000; round++) {
                foreach (var monkey in monkeysPart2) {
                    monkey.Throw(monkeysPart2, (worryLevel) => worryLevel % modulus);
                }
            }

            Console.WriteLine("Day 11, Star 1: {0}", monkeysPart1
                .Select(x => x.InspectionCounter)
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((x, y) => x * y));

            Console.WriteLine("Day 11, Star 2: {0}", monkeysPart2
                .Select(x => x.InspectionCounter)
                .OrderByDescending(x => x)
                .Take(2)
                .Aggregate((x, y) => x * y));
        }
    }

    public class Monkey {
        public Monkey(string[] lines) {
            Id = int.Parse(lines[0].Split(' ').Last().Trim(':'));
            Items = new Queue<long>(lines[1].Split(':').Last().Split(',').Select(x => long.Parse(x.Trim())));
            Operation = lines[2].Split(':').Last().Trim().Split(' ').Skip(2).Take(3).ToArray();
            TestDivisor = int.Parse(lines[3].Split(' ').Last());
            TestSuccessMonkeyId = int.Parse(lines[4].Split(' ').Last());
            TestFailMonkeyId = int.Parse(lines[5].Split(' ').Last());
        }

        public int Id { get; }
        public Queue<long> Items { get; }
        public string[] Operation { get; }
        public int TestDivisor { get; }
        public int TestSuccessMonkeyId { get; }
        public int TestFailMonkeyId { get; }
        public long InspectionCounter { get; private set; }

        public long ChangeWorryLevel(long item) {
            var leftHand = Operation[0] == "old" ? item : int.Parse(Operation[0]);
            var rightHand = Operation[2] == "old" ? item : int.Parse(Operation[2]);

            InspectionCounter++;

            return Operation[1] switch {
                "+" => leftHand + rightHand,
                "*" => leftHand * rightHand,
                _ => throw new NotImplementedException()
            };
        }

        public long SelectMonkeyToThrowAt(long item) {
            if (item % TestDivisor == 0) {
                return TestSuccessMonkeyId;
            }

            return TestFailMonkeyId;
        }

        public void Throw(IEnumerable<Monkey> monkeys, Func<long, long> reduceWorryLevel) {
            while (Items.TryDequeue(out var item)) {
                var newWorryLevel = ChangeWorryLevel(item);
                var reducedWorryLevel = reduceWorryLevel(newWorryLevel);
                var monkeyToThrowAt = monkeys.Single(x => x.Id == SelectMonkeyToThrowAt(reducedWorryLevel));

                monkeyToThrowAt.Catch(reducedWorryLevel);
            }
        }

        public void Catch(long item) => Items.Enqueue(item);
    }
}
