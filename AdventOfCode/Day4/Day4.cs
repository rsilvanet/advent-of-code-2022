namespace AdventOfCode.Day4 {
    public static class Day4 {
        public static void Go() {
            var input = File.ReadLines("Day4/Input.txt");
            var assignmentPairs = input.Select(x => new AssignmentPair(x));

            Console.WriteLine("Day 4, Star 1: {0}", assignmentPairs.Count(x => x.HasFullOverlap()));
            Console.WriteLine("Day 4, Star 2: {0}", assignmentPairs.Count(x => x.HasAnyOverlap()));
        }
    }

    public class AssignmentPair {
        public AssignmentPair(string line) {
            var pairs = line.Split(',');
            var firstPair = pairs[0].Split('-').Select(x => int.Parse(x));
            var secondPair = pairs[1].Split('-').Select(x => int.Parse(x));

            SectionsFirstPair = Enumerable.Range(firstPair.First(), firstPair.Last() - firstPair.First() + 1);
            SectionsSecondPair = Enumerable.Range(secondPair.First(), secondPair.Last() - secondPair.First() + 1);
        }

        public IEnumerable<int> SectionsFirstPair { get; }
        public IEnumerable<int> SectionsSecondPair { get; }

        public bool HasFullOverlap() {
            return SectionsFirstPair.All(x => SectionsSecondPair.Contains(x)) || SectionsSecondPair.All(x => SectionsFirstPair.Contains(x));
        }

        public bool HasAnyOverlap() {
            return SectionsFirstPair.Any(x => SectionsSecondPair.Contains(x)) || SectionsSecondPair.Any(x => SectionsFirstPair.Contains(x));
        }
    }
}
