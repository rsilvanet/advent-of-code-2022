namespace AdventOfCode.Day3 {
    public static class Day3 {
        public static void Go() {
            var input = File.ReadLines("Day3/Input.txt");
            var rucksacks = input.Select(x => new IntersectionPrioritizer(x));
            var score1 = 0;

            foreach (var item in input) {
                var half = item.Length / 2;
                var first = item.Substring(0, half);
                var second = item.Substring(half);

                score1 += new IntersectionPrioritizer(first, second).CalculatePriority();

            }

            var score2 = 0;

            for (int i = 0; i < input.Count(); i += 3) {
                var first = input.ElementAt(i);
                var second = input.ElementAt(i + 1);
                var third = input.ElementAt(i + 2);

                score2 += new IntersectionPrioritizer(first, second, third).CalculatePriority();
            }

            Console.WriteLine("Day 3, Star 1: {0}", score1);
            Console.WriteLine("Day 3, Star 2: {0}", score2);
            
        }
    }

    public class IntersectionPrioritizer {
        private const string PRIORITY_MAP = "*abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public IntersectionPrioritizer(params string[] items) {
            Items = items;
        }

        public IEnumerable<IEnumerable<char>> Items { get; }

        private int GetPriority(char item) => PRIORITY_MAP.IndexOf(item);

        public int CalculatePriority() => GetPriority(Items.Aggregate((x, y) => x.Intersect(y)).First());
    }
}
