namespace AdventOfCode.Day16 {
    public static class Day16 {
        private static Dictionary<string, int> flowRates = new Dictionary<string, int>();
        private static Dictionary<string, string[]> nextValves = new Dictionary<string, string[]>();

        public static void Go() {
            var input = File.ReadLines("Day16/Sample.txt");

            foreach (var line in input) {
                var split = line.Split(',');
                var valve = split[0].Trim();

                flowRates.Add(valve, int.Parse(split[1]));
                nextValves.Add(valve, split.Skip(2).Select(x => x.Trim()).ToArray());
            }

            flowRates = flowRates.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, y => y.Value);

            var bestScore = ExplorePaths(
                valve: "AA",
                minutesLeft: 30,
                openedValves: new List<string>(),
                currentScore: 0,
                bestScore: 0,
                memo: new Dictionary<(string, int, List<string>, int, int), int>()
            );

            Console.WriteLine("Day 16, Star 1: {0}", bestScore);

            var bestScoreWithElephant = ExplorePathsWithElephant(
                human: "AA",
                elephant: "AA",
                minutesLeft: 26,
                openedValves: new List<string>(),
                currentScore: 0,
                bestScore: 0,
                memo: new Dictionary<(string, string, int, List<string>, int, int), int>()
            );

            Console.WriteLine("Day 16, Star 2: {0}", bestScoreWithElephant);
        }

        private static int ExplorePaths(
                string valve,
                int minutesLeft,
                List<string> openedValves,
                int currentScore,
                int bestScore,
                Dictionary<(string, int, List<string>, int, int), int> memo) {

            if (minutesLeft == 0) {
                return Math.Max(bestScore, currentScore);
            }

            var memoKey = (valve, minutesLeft, openedValves, currentScore, bestScore);

            if (memo.TryGetValue(memoKey, out var memoScore)) {
                return memoScore;
            }

            if (flowRates[valve] > 0 && !openedValves.Contains(valve)) {
                openedValves.Add(valve);

                bestScore = ExplorePaths(
                    valve,
                    minutesLeft - 1,
                    openedValves,
                    currentScore: currentScore + (flowRates[valve] * (minutesLeft - 1)),
                    bestScore,
                    memo
                );

                openedValves.Remove(valve);
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

        private static int ExplorePathsWithElephant(
                string human,
                string elephant,
                int minutesLeft,
                List<string> openedValves,
                int currentScore,
                int bestScore,
                Dictionary<(string, string, int, List<string>, int, int), int> memo) {

            if (minutesLeft == 0) {
                return Math.Max(bestScore, currentScore);
            }

            if (minutesLeft < 20 && openedValves.Count == 0) {
                return bestScore;
            }

            var unopenedValves = flowRates.Where(x => !openedValves.Contains(x.Key)).Take(minutesLeft);

            if (bestScore > currentScore + unopenedValves.Sum(x => x.Value * (minutesLeft - 1))) {
                return bestScore;
            }

            var memoKey = (human, elephant, minutesLeft, openedValves, currentScore, bestScore);
            var reversedMemoKey = (elephant, human, minutesLeft, openedValves, currentScore, bestScore);

            if (memo.TryGetValue(memoKey, out var memoScore) || memo.TryGetValue(reversedMemoKey, out memoScore)) {
                return memoScore;
            }

            bestScore = OpenValves(human, elephant, minutesLeft, openedValves, currentScore, bestScore, memo);
            bestScore = OpenValves(elephant, human, minutesLeft, openedValves, currentScore, bestScore, memo);

            foreach (var nextValve in nextValves[human]) {
                foreach (var nextElephantValve in nextValves[elephant]) {
                    bestScore = ExplorePathsWithElephant(nextValve, nextElephantValve, minutesLeft - 1, openedValves, currentScore, bestScore, memo);
                }
            }

            memo.Add(memoKey, bestScore);

            return bestScore;
        }

        private static int OpenValves(
                string valve1, 
                string valve2, 
                int minutesLeft, 
                List<string> openedValves, 
                int currentScore, 
                int bestScore, 
                Dictionary<(string, string, int, List<string>, int, int), int> memo) {

            if (flowRates[valve1] > 0 && !openedValves.Contains(valve1)) {
                openedValves.Add(valve1);

                bestScore = ExplorePathsWithElephant(
                    valve1,
                    valve2,
                    minutesLeft - 1,
                    openedValves,
                    currentScore + (flowRates[valve1] * (minutesLeft - 1)),
                    bestScore,
                    memo
                );

                if (valve1 != valve2 && flowRates[valve2] > 0 && !openedValves.Contains(valve2)) {
                    openedValves.Add(valve2);

                    bestScore = ExplorePathsWithElephant(
                        valve1,
                        valve2,
                        minutesLeft - 1,
                        openedValves,
                        currentScore + (flowRates[valve1] * (minutesLeft - 1)) + (flowRates[valve2] * (minutesLeft - 1)),
                        bestScore,
                        memo
                    );

                    openedValves.Remove(valve2);
                }
                else {
                    foreach (var nextValve2 in nextValves[valve2]) {
                        bestScore = ExplorePathsWithElephant(
                            valve1,
                            nextValve2,
                            minutesLeft - 1,
                            openedValves,
                            currentScore + (flowRates[valve1] * (minutesLeft - 1)),
                            bestScore,
                            memo
                        );
                    }
                }

                openedValves.Remove(valve1);
            }

            return bestScore;
        }
    }
}
