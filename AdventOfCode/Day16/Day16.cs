namespace AdventOfCode.Day16 {
    public static class Day16 {
        private static readonly Dictionary<string, int> flowRates = new Dictionary<string, int>();
        private static readonly Dictionary<string, string[]> nextValves = new Dictionary<string, string[]>();

        public static void Go() {
            var input = File.ReadLines("Day16/Input.txt");

            foreach (var line in input) {
                var split = line.Split(',');
                var valve = split[0].Trim();

                flowRates.Add(valve, int.Parse(split[1]));
                nextValves.Add(valve, split.Skip(2).Select(x => x.Trim()).ToArray());
            }

            var bestScore = ExplorePaths(
                valve: "AA",
                minutesLeft: 30,
                openedValves: Array.Empty<string>(),
                currentScore: 0,
                bestScore: 0,
                memo: new Dictionary<(string, int, IEnumerable<string>, int, int), int>()
            );
            
            Console.WriteLine("Day 16, Star 1: {0}", bestScore);
            Console.WriteLine("Day 16, Star 2: {0}", "¯\\_(ツ)_/¯");
        }

        private static int ExplorePaths(
                string valve,
                int minutesLeft,
                IEnumerable<string> openedValves,
                int currentScore,
                int bestScore,
                Dictionary<(string, int, IEnumerable<string>, int, int), int> memo) {

            if (minutesLeft <= 2) {
                return Math.Max(bestScore, currentScore);
            }

            var memoKey = (valve, minutesLeft, openedValves, currentScore, bestScore);

            if (memo.TryGetValue(memoKey, out var memoScore)) {
                return memoScore;
            }

            if (flowRates[valve] > 0 && !openedValves.Contains(valve)) {
                bestScore = ExplorePaths(
                    valve,
                    minutesLeft - 1,
                    openedValves.Append(valve).ToArray(),
                    currentScore: currentScore + (flowRates[valve] * (minutesLeft - 1)),
                    bestScore,
                    memo
                );
            }

            foreach (var nextValve in nextValves[valve]) {
                bestScore = ExplorePaths(
                    nextValve,
                    minutesLeft - 1,
                    openedValves,
                    currentScore,
                    bestScore,
                    memo
                );
            }

            memo.Add(memoKey, bestScore);

            return bestScore;
        }
    }
}
