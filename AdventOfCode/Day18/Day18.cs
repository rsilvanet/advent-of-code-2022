using System.Numerics;

namespace AdventOfCode.Day18 {
    public static class Day18 {
        public static void Go() {
            var input = File.ReadLines("Day18/Input.txt");
            var cubes = input.Select(l => {
                var lineSplit = l.Split(',');

                return new Cube(
                    x: int.Parse(lineSplit[0]),
                    y: int.Parse(lineSplit[1]),
                    z: int.Parse(lineSplit[2])
                );
            }).ToList();

            var cubeVectors = cubes.Select(c => c.Vector).ToList();
            var minBoundary = new Vector3(cubes.Min(c => c.X), cubes.Min(c => c.Y), cubes.Min(c => c.Z));
            var maxBoundary = new Vector3(cubes.Max(c => c.X), cubes.Max(c => c.Y), cubes.Max(c => c.Z));

            var resultPart1 = cubes.Sum(x => x.CountUnconnectedSurfaces(cubes));

            var resultPart2 = cubes
                .SelectMany(c => c.Vector.Sides().Where(s => !cubeVectors.Contains(s)))
                .Where(side => CanReachExterior(side, minBoundary, maxBoundary, new List<Vector3>(), cubeVectors))
                .Count();

            Console.WriteLine("Day 18, Star 1: {0}", resultPart1);
            Console.WriteLine("Day 18, Star 2: {0}", resultPart2);
        }

        private static bool CanReachExterior(Vector3 position, Vector3 minBoundary, Vector3 maxBoundary, List<Vector3> visited, List<Vector3> cubes) {
            visited.Add(position);

            if (position.X < minBoundary.X || position.Y < minBoundary.Y || position.Z < minBoundary.Y) {
                return true;
            }

            if (position.X > maxBoundary.X || position.Y > maxBoundary.Y || position.Z > maxBoundary.Z) {
                return true;
            }

            foreach (var side in position.Sides().Where(s => !cubes.Contains(s)).Except(visited)) {
                return CanReachExterior(side, minBoundary, maxBoundary, visited, cubes);
            }

            return false;
        }
    }

    public class Cube {
        public Cube(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public Vector3 Vector => new Vector3(X, Y, Z);

        public int CountUnconnectedSurfaces(IEnumerable<Cube> cubes) => 6 - cubes.Count(c => Vector.Sides().Contains(c.Vector));

        public override string ToString() => $"X={X}, Y={Y}, Z={Z}";
    }

    public static class VectorExtensions {
        public static Vector3 Front(this Vector3 vector) => new Vector3(vector.X, vector.Y, vector.Z + 1);
        public static Vector3 Back(this Vector3 vector) => new Vector3(vector.X, vector.Y, vector.Z - 1);
        public static Vector3 Up(this Vector3 vector) => new Vector3(vector.X, vector.Y + 1, vector.Z);
        public static Vector3 Down(this Vector3 vector) => new Vector3(vector.X, vector.Y - 1, vector.Z);
        public static Vector3 Right(this Vector3 vector) => new Vector3(vector.X + 1, vector.Y, vector.Z);
        public static Vector3 Left(this Vector3 vector) => new Vector3(vector.X - 1, vector.Y, vector.Z);
        public static Vector3[] Sides(this Vector3 v) => new[] { v.Front(), v.Back(), v.Up(), v.Down(), v.Right(), v.Left() };
    }
}
