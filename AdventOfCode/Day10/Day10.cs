using System.Linq;

namespace AdventOfCode.Day10 {
    public static class Day10 {
        public static void Go() {
            var input = File.ReadLines("Day10/Input.txt");
            var executions = new List<int>();

            foreach (var item in input) {
                if (item.StartsWith("addx")) {
                    executions.Add(0);
                    executions.Add(int.Parse(item.Split(' ').Last()));
                }
                else if (item == "noop") {
                    executions.Add(0);
                }
            }

            var cycle = 1;
            var registerX = 1;
            var signalStrength = 0;
            var crt = new char[executions.Count];
            var crtIndex = 0;

            foreach (var item in executions) {
                if ((crtIndex % 40) >= registerX - 1 && (crtIndex % 40) <= registerX + 1) {
                    crt[crtIndex] = '#';
                }
                else {
                    crt[crtIndex] = '.';
                }

                registerX += item;

                if (cycle > 1 && (cycle == 20 || (cycle - 20) % 40 == 0)) {
                    signalStrength += cycle * registerX;
                }

                cycle++;
                crtIndex++;
            }

            Console.WriteLine("Day 10, Star 1: {0}", signalStrength);

            foreach (var crtLine in crt.Chunk(40)) {
                Console.WriteLine("Day 10, Star 2: {0}", new string(crtLine));
            }
        }
    }
}
