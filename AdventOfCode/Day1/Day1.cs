namespace AdventOfCode.Day1 {
    public static class Day1 {
        public static void Go() {
            var input = File.ReadLines("Day1/Input.txt");
            var elves = new List<Elf>();
            var elf = new Elf();
            elves.Add(elf);

            foreach (var item in input) {
                if (string.IsNullOrWhiteSpace(item)) {
                    elf = new Elf();
                    elves.Add(elf);
                    continue;
                }

                elf.CalorieItems.Add(int.Parse(item));
            }

            Console.WriteLine("Day 1, Star 1: {0}", elves.OrderByDescending(x => x.TotalCalories).First().TotalCalories);
            Console.WriteLine("Day 1, Star 2: {0}", elves.OrderByDescending(x => x.TotalCalories).Take(3).Sum(x => x.TotalCalories));
        }
    }

    public class Elf {
        public Elf() {
            CalorieItems = new List<int>();
        }

        public List<int> CalorieItems { get; }
        public int TotalCalories => CalorieItems.Sum();
    }
}
