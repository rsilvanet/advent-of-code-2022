namespace AdventOfCode.Day25 {
    public static class Day25 {
        private static readonly Dictionary<char, long> map = new Dictionary<char, long>() {
            { '2',  2 },
            { '1',  1 },
            { '0',  0 },
            { '-', -1 },
            { '=', -2 }
        };

        public static void Go() {
            var input = File.ReadLines("Day25/Input.txt");
            var result = GetSnafu(input.Sum(x => ReadNumber(x)));

            Console.WriteLine("Day 25, Star 1: {0}", result);
        }

        private static long ReadNumber(string number) {
            var value = 0L;
            var multiplier = 1L;

            foreach (var character in number.Reverse()) {
                value += map[character] * multiplier;
                multiplier *= 5;
            }

            return value;
        }

        private static string GetSnafu(long value) {
            var chars = new List<char>();

            while (value > 0) {
                var remainder = value % 5;

                switch (remainder) {
                    case 3:
                    case 4:
                        chars.Add(map.Single(x => x.Value == remainder - 5).Key);
                        value += 5;
                        break;
                    default:
                        chars.Add((char)('0' + remainder));
                        break;
                }

                value /= 5;
            }

            chars.Reverse();

            return new string(chars.ToArray());
        }
    }
}
