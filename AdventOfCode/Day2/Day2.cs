namespace AdventOfCode.Day1 {
    public static class Day2 {
        public static void Go() {
            var input = File.ReadLines("Day2/Input.txt");

            Console.WriteLine("Day 2, Star 1: {0}", input.Select(x => x.Split(" ")).Select(x => Game.FromHands(x.First(), x.Last())).Sum(x => x.Score));
            Console.WriteLine("Day 2, Star 2: {0}", input.Select(x => x.Split(" ")).Select(x => Game.FromResult(x.First(), x.Last())).Sum(x => x.Score));
        }
    }

    public class Game {
        public Game() { }
        public Hand OpponentHand { get; private set; }
        public Hand YourHand { get; private set; }
        public Result Result { get; private set; }
        public int Score => (int)Result + (int)YourHand;

        public static Game FromHands(string opponent, string you) {
            var game = new Game() {
                OpponentHand = opponent.ToHand(),
                YourHand = you.ToHand(),
            };

            game.CalculateResult();

            return game;
        }

        public static Game FromResult(string opponent, string result) {
            var game = new Game() {
                OpponentHand = opponent.ToHand(),
                Result = result.ToResult()
            };

            game.CalculateYourHand();

            return game;
        }

        private void CalculateResult() {
            if (OpponentHand.GetLosingHand() == YourHand) {
                Result = Result.Loss;
            }
            else if (OpponentHand.GetWinningHand() == YourHand) {
                Result = Result.Win;
            }
            else {
                Result = Result.Draw;
            }
        }

        private void CalculateYourHand() {
            if (Result == Result.Loss) {
                YourHand = OpponentHand.GetLosingHand();
            }
            else if (Result == Result.Win) {
                YourHand = OpponentHand.GetWinningHand();
            }
            else {
                YourHand = OpponentHand.GetDrawingHand();
            }
        }
    }

    public enum Hand {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    public enum Result {
        Loss = 0,
        Draw = 3,
        Win = 6
    }

    public static class EnumExtensions {

        public static Hand ToHand(this string hand) {
            switch (hand) {
                case "A":
                case "X":
                    return Hand.Rock;
                case "B":
                case "Y":
                    return Hand.Paper;
                case "C":
                case "Z":
                    return Hand.Scissors;
                default:
                    throw new NotImplementedException($"Invalid hand {hand}.");
            }
        }

        public static Result ToResult(this string result) {
            switch (result) {
                case "X":
                    return Result.Loss;
                case "Y":
                    return Result.Draw;
                case "Z":
                    return Result.Win;
                default:
                    throw new NotImplementedException($"Invalid result {result}.");
            }
        }

        public static Hand GetWinningHand(this Hand opponentHand) {
            switch (opponentHand) {
                case Hand.Rock:
                    return Hand.Paper;
                case Hand.Paper:
                    return Hand.Scissors;
                case Hand.Scissors:
                    return Hand.Rock;
                default:
                    throw new NotImplementedException($"Invalid hand to win from: {opponentHand}");
            }
        }

        public static Hand GetLosingHand(this Hand opponentHand) {
            switch (opponentHand) {
                case Hand.Rock:
                    return Hand.Scissors;
                case Hand.Paper:
                    return Hand.Rock;
                case Hand.Scissors:
                    return Hand.Paper;
                default:
                    throw new NotImplementedException($"Invalid hand to lose from: {opponentHand}");
            }
        }

        public static Hand GetDrawingHand(this Hand opponentHand) => opponentHand;
    }
}
