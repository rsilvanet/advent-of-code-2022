namespace AdventOfCode.Day5 {
    public static class Day5 {
        public static void Go() {
            var input = File.ReadLines("Day5/Input.txt");
            var stacksStar1 = ReadStacks(input);
            var stacksStar2 = stacksStar1.ToDictionary(x => x.Key, z => new Stack<char>(new Stack<char>(z.Value)));

            foreach (var instruction in ReadInstructions(input)) {
                for (int i = 0; i < instruction.quantity; i++) {
                    stacksStar1[instruction.destination].Push(stacksStar1[instruction.source].Pop());
                }

                var poppedBufferStar2 = new Stack<char>();

                for (int i = 0; i < instruction.quantity; i++) {
                    poppedBufferStar2.Push(stacksStar2[instruction.source].Pop());
                }

                while (poppedBufferStar2.Count > 0) {
                    stacksStar2[instruction.destination].Push(poppedBufferStar2.Pop());
                }
            }

            Console.WriteLine("Day 5, Star 1: {0}", new string(stacksStar1.Select(x => x.Value.Peek()).ToArray()));
            Console.WriteLine("Day 5, Star 2: {0}", new string(stacksStar2.Select(x => x.Value.Peek()).ToArray()));
        }

        private static Dictionary<int, Stack<char>> ReadStacks(IEnumerable<string> input) {
            var bottomIndex = -1;

            foreach (var line in input) {
                if (string.IsNullOrWhiteSpace(line)) {
                    break;
                }

                bottomIndex++;
            }

            var bottomLine = input.ElementAt(bottomIndex);
            var stackIndexes = new Dictionary<int, int>();

            foreach (var item in bottomLine) {
                if (char.IsNumber(item)) {
                    stackIndexes.Add(int.Parse(item.ToString()), bottomLine.IndexOf(item));
                }
            }

            var stacks = new Dictionary<int, Stack<char>>();

            foreach (var stackIndex in stackIndexes) {
                stacks.Add(stackIndex.Key, new Stack<char>());
            }

            for (int i = bottomIndex - 1; i >= 0; i--) {
                var currentLine = input.ElementAt(i);

                foreach (var stackIndex in stackIndexes) {
                    var letter = currentLine[stackIndex.Value];

                    if (char.IsLetter(letter)) {
                        stacks[stackIndex.Key].Push(letter);
                    }
                }
            }

            return stacks;
        }

        private static IEnumerable<(int quantity, int source, int destination)> ReadInstructions(IEnumerable<string> input) {
            foreach (var line in input) {
                if (!line.StartsWith("move")) {
                    continue;
                }

                var splitLine = line.Split(' ');
                var quantity = int.Parse(splitLine[1]);
                var source = int.Parse(splitLine[3]);
                var destination = int.Parse(splitLine[5]);

                yield return (quantity, source, destination);
            }
        }
    }
}
