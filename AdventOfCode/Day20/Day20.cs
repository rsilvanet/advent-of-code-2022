namespace AdventOfCode.Day20 {
    public static class Day20 {
        public static void Go() {
            var input = File.ReadLines("Day20/Input.txt");

            var numbersPart1 = input.Select((x, i) => new IndexedNumber(int.Parse(x), i)).ToList();
            var numbersPart2 = input.Select((x, i) => new IndexedNumber(long.Parse(x) * 811589153L, i)).ToList();

            Mix(numbersPart1, 1);
            Mix(numbersPart2, 10);

            Console.WriteLine("Day 20, Star 1: {0}", GetGroveCoordinates(numbersPart1).Sum(x => x.Value));
            Console.WriteLine("Day 20, Star 2: {0}", GetGroveCoordinates(numbersPart2).Sum(x => x.Value));
        }

        private static void Mix(List<IndexedNumber> numbers, int iterations) {
            for (int i = 0; i < iterations; i++) {
                for (int n = 0; n < numbers.Count; n++) {
                    var number = numbers.Single(x => x.OriginalIndex == n);
                    var indexOfNumber = numbers.IndexOf(number);
                    numbers.Remove(number);

                    var newIndex = (int)((indexOfNumber + number.Value) % numbers.Count);

                    if (newIndex >= 0) {
                        numbers.Insert(newIndex, number);
                    }
                    else {
                        numbers.Insert(numbers.Count + newIndex, number);
                    }
                }
            }
        }

        private static IEnumerable<IndexedNumber> GetGroveCoordinates(List<IndexedNumber> numbers) {
            var zero = numbers.Single(x => x.Value == 0);
            var indexOfZero = numbers.IndexOf(zero);

            yield return numbers[(indexOfZero + 1000) % numbers.Count];
            yield return numbers[(indexOfZero + 2000) % numbers.Count];
            yield return numbers[(indexOfZero + 3000) % numbers.Count];
        }
    }

    public class IndexedNumber {
        public IndexedNumber(long value, int index) {
            Value = value;
            OriginalIndex = index;
        }

        public long Value { get; private set; }
        public int OriginalIndex { get; private set; }
    }
}
