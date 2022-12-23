namespace AdventOfCode.Day19 {
    public static class Day19 {
        public static void Go() {
            var input = File.ReadLines("Day19/Input.txt");
            var blueprints = input.Select(x => new Blueprint(x)).ToList();
            var tasksStar1 = new List<Task<(int blueprint, int bestResult)>>();
            var tasksStar2 = new List<Task<(int blueprint, int bestResult)>>();

            foreach (var blueprint in blueprints) {
                var task = Task.Run(() => (blueprint.Id, Collect(
                    blueprint: blueprint,
                    minutesLeft: 24,
                    oreRobots: 1, clayRobots: 0, obsidianRobots: 0, geodeRobots: 0,
                    oreCount: 0, clayCount: 0, obsidianCount: 0, geodeCount: 0,
                    bestResult: 0,
                    memo: new Dictionary<(Blueprint, int, int, int, int, int, int, int, int, int, int), int>()
                )));

                tasksStar1.Add(task);
            }

            Console.WriteLine("Day 19, Star 1: {0}", Task.WhenAll(tasksStar1).Result.Sum(x => x.blueprint * x.bestResult));

            foreach (var blueprint in blueprints.Take(3)) {
                var task = Task.Run(() => (blueprint.Id, Collect(
                    blueprint: blueprint,
                    minutesLeft: 32,
                    oreRobots: 1, clayRobots: 0, obsidianRobots: 0, geodeRobots: 0,
                    oreCount: 0, clayCount: 0, obsidianCount: 0, geodeCount: 0,
                    bestResult: 0,
                    memo: new Dictionary<(Blueprint, int, int, int, int, int, int, int, int, int, int), int>()
                )));

                tasksStar2.Add(task);
            }

            Console.WriteLine("Day 19, Star 2: {0}", Task.WhenAll(tasksStar2).Result.Select(x => x.bestResult).Aggregate((x, y) => x * y));
        }

        private static bool ShouldBuildRobot(
                Blueprint blueprint,
                int oreRobots,
                int clayRobots,
                int obsidianRobots,
                int geodeRobots,
                int oreCount,
                int clayCount,
                int obsidianCount,
                int geodeCount,
                RockType robotType) {

            if (!blueprint.CanBuild(robotType, oreCount, clayCount, obsidianCount, geodeCount)) {
                return false;
            }

            if (robotType == RockType.Geode) {
                return true;
            }

            return robotType switch {
                RockType.Ore => oreRobots < blueprint.GetMaxNeeded(RockType.Ore),
                RockType.Clay => clayRobots < blueprint.GetMaxNeeded(RockType.Clay),
                RockType.Obsidian => obsidianRobots < blueprint.GetMaxNeeded(RockType.Obsidian),
                RockType.Geode => geodeRobots < blueprint.GetMaxNeeded(RockType.Geode),
                _ => throw new NotImplementedException(),
            };
        }

        private static int Collect(
                Blueprint blueprint,
                int minutesLeft,
                int oreRobots,
                int clayRobots,
                int obsidianRobots,
                int geodeRobots,
                int oreCount,
                int clayCount,
                int obsidianCount,
                int geodeCount,
                int bestResult,
                Dictionary<(Blueprint, int, int, int, int, int, int, int, int, int, int), int> memo) {

            if (minutesLeft == 0) {
                return Math.Max(bestResult, geodeCount);
            }

            if (minutesLeft < 17 && clayRobots == 0) {
                return bestResult;
            }

            if (minutesLeft < 10 && obsidianRobots == 0) {
                return bestResult;
            }

            if (minutesLeft < 2 && geodeRobots == 0) {
                return bestResult;
            }

            var memoKey = (blueprint, minutesLeft, oreRobots, clayRobots, obsidianRobots, geodeRobots, oreCount, clayCount, obsidianCount, geodeCount, bestResult);

            if (memo.TryGetValue(memoKey, out var memoResult)) {
                return memoResult;
            }

            var rockTypes = new[] { RockType.Geode, RockType.Obsidian, RockType.Clay, RockType.Ore };

            foreach (var robotType in rockTypes) {
                if (ShouldBuildRobot(blueprint, oreRobots, clayRobots, obsidianRobots, geodeRobots, oreCount, clayCount, obsidianCount, geodeCount, robotType)) {
                    bestResult = Collect(
                        blueprint,
                        minutesLeft - 1,
                        oreRobots: robotType == RockType.Ore ? oreRobots + 1 : oreRobots,
                        clayRobots: robotType == RockType.Clay ? clayRobots + 1 : clayRobots,
                        obsidianRobots: robotType == RockType.Obsidian ? obsidianRobots + 1 : obsidianRobots,
                        geodeRobots: robotType == RockType.Geode ? geodeRobots + 1 : geodeRobots,
                        oreCount: oreCount + oreRobots - blueprint.Costs[(robotType, RockType.Ore)],
                        clayCount: clayCount + clayRobots - blueprint.Costs[(robotType, RockType.Clay)],
                        obsidianCount: obsidianCount + obsidianRobots - blueprint.Costs[(robotType, RockType.Obsidian)],
                        geodeCount: geodeCount + geodeRobots,
                        bestResult,
                        memo
                    );

                    if (robotType == RockType.Geode || robotType == RockType.Obsidian) {
                        break;
                    }
                }
            }

            bestResult = Collect(
                blueprint,
                minutesLeft - 1,
                oreRobots,
                clayRobots,
                obsidianRobots,
                geodeRobots,
                oreCount: oreCount + oreRobots,
                clayCount: clayCount + clayRobots,
                obsidianCount: obsidianCount + obsidianRobots,
                geodeCount: geodeCount + geodeRobots,
                bestResult,
                memo
            );

            memo.Add(memoKey, bestResult);

            return bestResult;
        }
    }

    public class Blueprint {
        public Blueprint(string line) {
            var lineSplit = line.Split(',');

            Id = int.Parse(lineSplit[0].Trim());
            Costs = new Dictionary<(RockType robotType, RockType resourceNeeded), int>();

            Costs.Add((robotType: RockType.Ore, resourceNeeded: RockType.Ore), int.Parse(lineSplit[1].Trim()));
            Costs.Add((robotType: RockType.Ore, resourceNeeded: RockType.Clay), 0);
            Costs.Add((robotType: RockType.Ore, resourceNeeded: RockType.Obsidian), 0);
            Costs.Add((robotType: RockType.Ore, resourceNeeded: RockType.Geode), 0);

            Costs.Add((robotType: RockType.Clay, resourceNeeded: RockType.Ore), int.Parse(lineSplit[2].Trim()));
            Costs.Add((robotType: RockType.Clay, resourceNeeded: RockType.Clay), 0);
            Costs.Add((robotType: RockType.Clay, resourceNeeded: RockType.Obsidian), 0);
            Costs.Add((robotType: RockType.Clay, resourceNeeded: RockType.Geode), 0);


            Costs.Add((robotType: RockType.Obsidian, resourceNeeded: RockType.Ore), int.Parse(lineSplit[3].Trim()));
            Costs.Add((robotType: RockType.Obsidian, resourceNeeded: RockType.Clay), int.Parse(lineSplit[4].Trim()));
            Costs.Add((robotType: RockType.Obsidian, resourceNeeded: RockType.Obsidian), 0);
            Costs.Add((robotType: RockType.Obsidian, resourceNeeded: RockType.Geode), 0);


            Costs.Add((robotType: RockType.Geode, resourceNeeded: RockType.Ore), int.Parse(lineSplit[5].Trim()));
            Costs.Add((robotType: RockType.Geode, resourceNeeded: RockType.Clay), 0);
            Costs.Add((robotType: RockType.Geode, resourceNeeded: RockType.Obsidian), int.Parse(lineSplit[6].Trim()));
            Costs.Add((robotType: RockType.Geode, resourceNeeded: RockType.Geode), 0);
        }

        public int Id { get; set; }
        public Dictionary<(RockType robotType, RockType resourceNeeded), int> Costs { get; set; }

        public int GetMaxNeeded(RockType rockType) => Costs.Where(x => x.Key.resourceNeeded == rockType).Max(x => x.Value);

        public bool CanBuild(RockType robotType, int ore, int clay, int obsidian, int geode) => robotType switch {
            RockType.Ore => ore >= Costs[(RockType.Ore, RockType.Ore)],
            RockType.Clay => ore >= Costs[(RockType.Clay, RockType.Ore)],
            RockType.Obsidian => ore >= Costs[(RockType.Obsidian, RockType.Ore)] && clay >= Costs[(RockType.Obsidian, RockType.Clay)],
            RockType.Geode => ore >= Costs[(RockType.Geode, RockType.Ore)] && obsidian >= Costs[(RockType.Geode, RockType.Obsidian)],
            _ => throw new NotImplementedException()
        };
    }

    public enum RockType {
        Ore,
        Clay,
        Obsidian,
        Geode
    }
}
