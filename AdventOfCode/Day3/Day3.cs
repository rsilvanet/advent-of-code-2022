namespace AdventOfCode.Day3 {
    public static class Day3 {
        public static void Go() {
            var input = File.ReadLines("Day3/Input.txt");
            var scoreStar1 = 0;

            foreach (var item in input) {
                var half = item.Length / 2;
                var first = item.Substring(0, half);
                var second = item.Substring(half);

                scoreStar1 += CalculatePriority(first, second);
            }

            var scoreStar2 = 0;

            for (int i = 0; i < input.Count(); i += 3) {
                var first = input.ElementAt(i);
                var second = input.ElementAt(i + 1);
                var third = input.ElementAt(i + 2);

                scoreStar2 += CalculatePriority(first, second, third);
            }

            Console.WriteLine("Day 3, Star 1: {0}", scoreStar1);
            Console.WriteLine("Day 3, Star 2: {0}", scoreStar2);
        }
        
        private const string PRIORITY_MAP = "*abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static int GetPriority(char item) => PRIORITY_MAP.IndexOf(item);

        private static int CalculatePriority(params IEnumerable<char>[] items) => GetPriority(items.Aggregate((x, y) => x.Intersect(y)).First());
    }
}
