namespace AdventOfCode.Day19 {
    public static class Day19 {
        public static void Go() {
            var input = File.ReadLines("Day19/Input.txt");
            var blueprints = input.Select(x => new Blueprint(x)).ToList();

            var robots = new RockDictionary() {
                { RockType.Ore, 1 },
                { RockType.Clay, 0 },
                { RockType.Obsidian, 0 },
                { RockType.Geode, 0 }
            };

            var minerals = new RockDictionary() {
                { RockType.Ore, 0 },
                { RockType.Clay, 0 },
                { RockType.Obsidian, 0 },
                { RockType.Geode, 0 }
            };

            var bestResult = new RockDictionary() {
                { RockType.Ore, 0 },
                { RockType.Clay, 0 },
                { RockType.Obsidian, 0 },
                { RockType.Geode, 0 }
            };

            var tasks = new List<Task<(int blueprint, RockDictionary bestResult)>>();

            Console.Write("Day 19, Processing: ");

            foreach (var blueprint in blueprints) {
                var task = Task.Run(() => (blueprint.Id, Collect(
                    blueprint: blueprint,
                    minutesLeft: 24,
                    robots,
                    minerals,
                    bestResult,
                    memo: new Dictionary<(Blueprint, int, RockDictionary, RockDictionary, RockDictionary), RockDictionary>()
                )));

                tasks.Add(task);
            }

            var taskResults = Task.WhenAll(tasks).Result;

            Console.Write(Environment.NewLine);
            Console.WriteLine("Day 19, Star 1: {0}", taskResults.Sum(x => x.blueprint * x.bestResult[RockType.Geode]));
            Console.WriteLine("Day 19, Star 2: {0}", "¯\\_(\"/)_/¯");
        }

        private static bool CanBuildRobot(Blueprint blueprint, RockDictionary robots, RockDictionary minerals, RockType robotType) {
            var hasEnoughMaterialToBuild = true;
            var hasEnoughRobotsCollecting = robotType != RockType.Geode;

            foreach (var cost in blueprint.Costs.Where(x => x.Key.robotType == robotType)) {
                if (minerals[cost.Key.resourceNeeded] < cost.Value) {
                    hasEnoughMaterialToBuild = false;
                }
            }

            foreach (var cost in blueprint.Costs.Where(x => x.Key.resourceNeeded == robotType)) {
                if (robots[robotType] < cost.Value) {
                    hasEnoughRobotsCollecting = false;
                }
            }

            return hasEnoughMaterialToBuild && !hasEnoughRobotsCollecting;
        }

        private static RockDictionary Collect(
                Blueprint blueprint,
                int minutesLeft,
                RockDictionary robots,
                RockDictionary minerals,
                RockDictionary bestResult,
                Dictionary<(Blueprint, int, RockDictionary, RockDictionary, RockDictionary), RockDictionary> memo) {

            if (minutesLeft == 0) {
                if (minerals[RockType.Geode] > bestResult[RockType.Geode]) {
                    return minerals;
                }

                return bestResult;
            }

            var memoKey = (blueprint, minutesLeft, robots, minerals, bestResult);

            if (memo.TryGetValue(memoKey, out var memoResult)) {
                return memoResult;
            }

            var rockTypesEnabled = new[] { RockType.Geode, RockType.Obsidian, RockType.Clay, RockType.Ore };

            foreach (var rockType in rockTypesEnabled) {
                if (CanBuildRobot(blueprint, robots, minerals, rockType)) {
                    var robotsPlusNew = new RockDictionary(robots);
                    var mineralsMinusSpent = new RockDictionary(minerals);

                    robotsPlusNew[rockType]++;

                    foreach (var cost in blueprint.Costs.Where(x => x.Key.robotType == rockType)) {
                        mineralsMinusSpent[cost.Key.resourceNeeded] -= cost.Value;
                    }

                    foreach (var item in rockTypesEnabled) {
                        mineralsMinusSpent[item] += robots[item];
                    }

                    bestResult = Collect(
                        blueprint,
                        minutesLeft - 1,
                        robotsPlusNew,
                        mineralsMinusSpent,
                        bestResult,
                        memo
                    );

                    if (rockType == RockType.Geode || rockType == RockType.Obsidian) {
                        break;
                    }
                }
            }

            var mineralsPlusNew = new RockDictionary(minerals);

            foreach (var rockType in rockTypesEnabled) {
                mineralsPlusNew[rockType] += robots[rockType];
            }

            bestResult = Collect(
                blueprint,
                minutesLeft - 1,
                new RockDictionary(robots),
                mineralsPlusNew,
                bestResult,
                memo
            );

            memo.Add(memoKey, bestResult);

            if (minutesLeft == 24) {
                Console.Write($"{blueprint.Id} ");
            }

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
    }

    public class RockDictionary : Dictionary<RockType, int>, IEquatable<RockDictionary?> {
        public RockDictionary() { }

        public RockDictionary(RockDictionary rockDictionary) : base(rockDictionary) { }

        public override bool Equals(object? obj) {
            if (obj is RockDictionary customDictionary) {
                return Equals(customDictionary);
            }

            return false;
        }

        public bool Equals(RockDictionary? other) {
            if (other == null) {
                return false;
            }

            return this[RockType.Ore] == other[RockType.Ore]
                && this[RockType.Clay] == other[RockType.Clay]
                && this[RockType.Obsidian] == other[RockType.Obsidian]
                && this[RockType.Geode] == other[RockType.Geode];
        }

        public override int GetHashCode() {
            return HashCode.Combine(
                this[RockType.Ore],
                this[RockType.Clay],
                this[RockType.Obsidian],
                this[RockType.Geode]
            );
        }
    }

    public enum RockType {
        Ore,
        Clay,
        Obsidian,
        Geode
    }
}
