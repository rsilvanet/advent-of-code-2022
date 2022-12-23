using System.Diagnostics;

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

            var tasks = new List<Task<(int blueprint, int bestResult)>>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var blueprint in blueprints) {
                var task = Task.Run(() => (blueprint.Id, Collect(
                    blueprint: blueprint,
                    minutesLeft: 24,
                    robots,
                    minerals,
                    0,
                    memo: new Dictionary<(Blueprint, int, RockDictionary, RockDictionary, int), int>()
                )));

                tasks.Add(task);
            }

            var taskResults = Task.WhenAll(tasks).Result;

            Console.WriteLine("Day 19, Elapsed: {0}ms", stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Day 19, Star 1: {0}", taskResults.Sum(x => x.blueprint * x.bestResult));
            Console.WriteLine("Day 19, Star 2: {0}", "¯\\_(\"/)_/¯");
        }

        private static bool ShouldBuildRobot(Blueprint blueprint, RockDictionary robots, RockDictionary minerals, RockType robotType) {
            if (!blueprint.CanBuild(robotType, minerals)) {
                return false;
            }

            if (robotType == RockType.Geode) {
                return true;
            }

            return robots[robotType] < blueprint.GetMaxNeeded(robotType);
        }

        private static int Collect(
                Blueprint blueprint,
                int minutesLeft,
                RockDictionary robots,
                RockDictionary minerals,
                int bestResult,
                Dictionary<(Blueprint, int, RockDictionary, RockDictionary, int), int> memo) {

            if (minutesLeft == 0) {
                return Math.Max(bestResult, minerals[RockType.Geode]);
            }

            if (minutesLeft < 17 && robots[RockType.Clay] == 0) {
                return bestResult;
            }

            if (minutesLeft < 10 && robots[RockType.Obsidian] == 0) {
                return bestResult;
            }

            if (minutesLeft < 2 && robots[RockType.Geode] == 0) {
                return bestResult;
            }

            var memoKey = (blueprint, minutesLeft, robots, minerals, bestResult);

            if (memo.TryGetValue(memoKey, out var memoResult)) {
                return memoResult;
            }

            var rockTypes = new[] { RockType.Geode, RockType.Obsidian, RockType.Clay, RockType.Ore };

            foreach (var rockType in rockTypes) {
                if (ShouldBuildRobot(blueprint, robots, minerals, rockType)) {
                    var robotsPlusNew = new RockDictionary(robots);
                    var mineralsMinusSpent = new RockDictionary(minerals);

                    robotsPlusNew[rockType]++;
                    mineralsMinusSpent[RockType.Ore] += robots[RockType.Ore] - blueprint.Costs[(rockType, RockType.Ore)];
                    mineralsMinusSpent[RockType.Clay] += robots[RockType.Clay] - blueprint.Costs[(rockType, RockType.Clay)];
                    mineralsMinusSpent[RockType.Obsidian] += robots[RockType.Obsidian] - blueprint.Costs[(rockType, RockType.Obsidian)];
                    mineralsMinusSpent[RockType.Geode] += robots[RockType.Geode];

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
            mineralsPlusNew[RockType.Ore] += robots[RockType.Ore];
            mineralsPlusNew[RockType.Clay] += robots[RockType.Clay];
            mineralsPlusNew[RockType.Obsidian] += robots[RockType.Obsidian];
            mineralsPlusNew[RockType.Geode] += robots[RockType.Geode];

            bestResult = Collect(
                blueprint,
                minutesLeft - 1,
                new RockDictionary(robots),
                mineralsPlusNew,
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

        public bool CanBuild(RockType robotType, RockDictionary minerals) => robotType switch {
            RockType.Ore => minerals[RockType.Ore] >= Costs[(RockType.Ore, RockType.Ore)],
            RockType.Clay => minerals[RockType.Ore] >= Costs[(RockType.Clay, RockType.Ore)],
            RockType.Obsidian => minerals[RockType.Ore] >= Costs[(RockType.Obsidian, RockType.Ore)] && minerals[RockType.Clay] >= Costs[(RockType.Obsidian, RockType.Clay)],
            RockType.Geode => minerals[RockType.Ore] >= Costs[(RockType.Geode, RockType.Ore)] && minerals[RockType.Obsidian] >= Costs[(RockType.Geode, RockType.Obsidian)],
            _ => throw new NotImplementedException()
        };
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
