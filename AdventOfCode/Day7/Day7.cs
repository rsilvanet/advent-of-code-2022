using System.Linq;

namespace AdventOfCode.Day7 {
    public static class Day7 {
        public static void Go() {
            var input = File.ReadLines("Day7/Input.txt");
            var rootDirectory = new AdventDirectory("/", parent: null);
            var currentDirectory = rootDirectory;

            foreach (var line in input) {
                if (line.StartsWith("$ cd")) {
                    var cdParameter = line.Split(' ')[2];

                    if (cdParameter == "/") {
                        currentDirectory = rootDirectory;
                    }
                    else if (cdParameter == "..") {
                        if (currentDirectory.Parent != null) {
                            currentDirectory = currentDirectory.Parent;
                        }
                    }
                    else if (currentDirectory.Directories.Any(x => x.Name == cdParameter)) {
                        currentDirectory = currentDirectory.Directories.Single(x => x.Name == cdParameter);
                    }
                }
                else if (line.StartsWith("$ ls")) {
                    continue;
                }
                else if (line.StartsWith("dir")) {
                    var directoryName = line.Split(' ')[1];
                    currentDirectory.Directories.Add(new AdventDirectory(directoryName, parent: currentDirectory));
                }
                else if (int.TryParse(line.Split(' ')[0], out int fileSize)) {
                    var fileName = line.Split(' ')[1];
                    currentDirectory.Files.Add(new AdventFile(fileName, fileSize));
                }
            }

            var requiredSpace = 30000000;
            var availableSpace = 70000000 - rootDirectory.Size;

            Console.WriteLine("Day 7, Star 1: {0}", rootDirectory.FlattenedDirectories.Where(x => x.Size <= 100000).Sum(x => x.Size));
            Console.WriteLine("Day 7, Star 2: {0}", rootDirectory.FlattenedDirectories.Where(x => x.Size + availableSpace >= requiredSpace).MinBy(x => x.Size)?.Size);
        }
    }

    public class AdventDirectory {
        public AdventDirectory(string name, AdventDirectory? parent) {
            Name = name;
            Parent = parent;
            Directories = new List<AdventDirectory>();
            Files = new List<AdventFile>();
        }

        public string Name { get; set; }
        public AdventDirectory? Parent { get; set; }
        public List<AdventDirectory> Directories { get; set; }
        public List<AdventFile> Files { get; set; }
        public int Size => Files.Sum(x => x.Size) + Directories.Sum(x => x.Size);
        public List<AdventDirectory> FlattenedDirectories => Flatten(Directories);

        private List<AdventDirectory> Flatten(List<AdventDirectory> directories) {
            return directories.SelectMany(x => Flatten(x.Directories)).Concat(directories).ToList();
        }
    }

    public class AdventFile {
        public AdventFile(string name, int size) {
            Name = name;
            Size = size;
        }

        public string Name { get; set; }
        public int Size { get; set; }
    }
}
