namespace AdventOfCode.Day21 {
    public static class Day21 {
        public static void Go() {
            var input = File.ReadLines("Day21/Input.txt").ToList();
            var results = new Dictionary<string, decimal>();
            var operations = new Dictionary<string, (string left, string operation, string right)>();

            foreach (var line in input) {
                var split = line.Split(' ');
                var monkeyName = split[0].TrimEnd(':');

                if (split.Length == 2) {
                    results.Add(monkeyName, int.Parse(split[1]));
                }
                else {
                    operations.Add(monkeyName, (split[1], split[2], split[3]));
                }
            }

            Console.WriteLine("Day 21, Star 1: {0}", CalculateRoot(Copy(results), Copy(operations)));

            var equalityForHuman0 = CheckRootEquality(Copy(results, human: 0), Copy(operations));
            var equalityForHuman1 = CheckRootEquality(Copy(results, human: 1), Copy(operations));
            var leftDelta = equalityForHuman0.left - equalityForHuman1.left;

            Console.WriteLine("Day 21, Star 2: {0}", Math.Round(equalityForHuman0.left / leftDelta - equalityForHuman0.right / leftDelta));
        }

        private static Dictionary<string, T> Copy<T>(Dictionary<string, T> dictionary, decimal? human = null) {
            var copy = new Dictionary<string, T>(dictionary);

            if (human.HasValue && typeof(T) == typeof(decimal)) {
                copy["humn"] = (T)Convert.ChangeType(human.Value, typeof(T));
            }

            return copy;
        }

        private static decimal CalculateRoot(Dictionary<string, decimal> results, Dictionary<string, (string left, string operation, string right)> operations) {
            while (true) {
                foreach (var operation in operations.Where(x => !results.ContainsKey(x.Key))) {
                    if (results.TryGetValue(operation.Value.left, out var leftValue) && results.TryGetValue(operation.Value.right, out var rightValue)) {
                        results.Add(operation.Key, Operate(leftValue, operation.Value.operation, rightValue));

                        if (operation.Key == "root") {
                            return results["root"];
                        }
                    }
                }
            }
        }

        private static (bool result, decimal left, decimal right) CheckRootEquality(Dictionary<string, decimal> results, Dictionary<string, (string left, string operation, string right)> operations) {
            while (true) {
                foreach (var operation in operations.Where(x => !results.ContainsKey(x.Key))) {
                    if (results.TryGetValue(operation.Value.left, out var leftValue) && results.TryGetValue(operation.Value.right, out var rightValue)) {
                        if (operation.Key == "root") {
                            return (leftValue == rightValue, leftValue, rightValue);
                        }
                        else {
                            results.Add(operation.Key, Operate(leftValue, operation.Value.operation, rightValue));
                        }
                    }
                }
            }
        }

        private static decimal Operate(decimal left, string operation, decimal right) => operation switch {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            _ => throw new NotImplementedException()
        };
    }
}
