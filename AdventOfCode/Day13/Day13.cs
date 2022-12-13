using AdventOfCode.Day2;

namespace AdventOfCode.Day13 {
    public static class Day13 {
        private static readonly CustomComparer Comparer = new CustomComparer();

        public static void Go() {
            var input = File.ReadLines("Day13/Input.txt");
            var index = 1;
            var counter = 0;

            foreach (var pair in input.Chunk(3)) {
                if (IsPairInOrder(pair[0].Trim(), pair[1].Trim())) {
                    counter += index;
                }

                index++;
            }

            var sorted = input.Where(x => !string.IsNullOrEmpty(x))
                .Append(string.Empty)
                .Append("[[2]]")
                .Append("[[6]]")
                .ToList();

            sorted.Sort(Comparer);

            Console.WriteLine("Day 13, Star 1: {0}", counter);
            Console.WriteLine("Day 13, Star 2: {0}", sorted.IndexOf("[[2]]") * sorted.IndexOf("[[6]]"));
        }

        public static bool IsPairInOrder(string left, string right) => Comparer.Compare(left, right) == -1;
    }

    public class CustomComparer : IComparer<string> {
        public int Compare(string? left, string? right) => Compare(Parse(left ?? "[]"), Parse(right ?? "[]"));

        private int Compare(List<dynamic> left, List<dynamic> right) {
            for (int i = 0; i < Math.Max(left.Count, right.Count); i++) {
                if (i >= left.Count) {
                    return -1;
                }
                else if (i >= right.Count) {
                    return 1;
                }

                var leftItem = left[i];
                var rightItem = right[i];
                var result = 0;

                if (leftItem is int leftInteger && rightItem is int rightInteger) {
                    result = leftInteger.CompareTo(rightInteger);
                }
                else if (leftItem is List<dynamic> leftList && rightItem is List<dynamic> rightList) {
                    result = Compare(leftList, rightList);
                }
                else if (leftItem is int leftInteger2 && rightItem is List<dynamic> rightList2) {
                    result = Compare(new List<dynamic> { leftInteger2 }, rightList2);
                }
                else if (leftItem is List<dynamic> leftList2 && rightItem is int rightInteger2) {
                    result = Compare(leftList2, new List<dynamic> { rightInteger2 });
                }

                if (result != 0) {
                    return result;
                }
            }

            return 0;
        }

        private List<dynamic> Parse(string value) {
            var stack = new Stack<List<dynamic>>();

            stack.Push(new List<dynamic>());

            for (int i = 0; i < value.Length; i++) {
                switch (value[i]) {
                    case ',':
                        break;
                    case ']':
                        stack.Pop();
                        break;
                    case '[':
                        var innerList = new List<dynamic>();
                        stack.Peek().Add(innerList);
                        stack.Push(innerList);
                        break;
                    default:
                        var possibleEndOfNumberIndexes = new List<int>() {
                            value.IndexOf(',', i),
                            value.IndexOf(']', i)
                        };

                        var endOfNumberIndex = possibleEndOfNumberIndexes.Where(x => x > 0).Min(x => x);
                        var number = int.Parse(value.Substring(i, endOfNumberIndex - i));
                        stack.Peek().Add(number);
                        i = endOfNumberIndex - 1;
                        break;
                }
            }

            return stack.Pop();
        }
    }
}
