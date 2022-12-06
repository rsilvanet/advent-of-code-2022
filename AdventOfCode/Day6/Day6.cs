namespace AdventOfCode.Day6 {
    public static class Day6 {
        public static void Go() {
            var input = File.ReadAllText("Day6/Input.txt");

            Console.WriteLine("Day 6, Star 1: {0}", GetMarkerPosition(input, 4));
            Console.WriteLine("Day 6, Star 2: {0}", GetMarkerPosition(input, 14));
        }

        private static int GetMarkerPosition(string input, int distinctAmount) {
            for (int i = distinctAmount - 1; i < input.Length; i++) {
                if (input.Skip(i - distinctAmount).Take(distinctAmount).Distinct().Count() == distinctAmount) {
                    return i;
                }
            }

            return 0;
        }
    }
}
