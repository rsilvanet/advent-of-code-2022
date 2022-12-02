namespace AdventOfCode.Day2 {
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
        public static Hand ToHand(this string hand) => hand switch {
            "A" => Hand.Rock,
            "X" => Hand.Rock,
            "B" => Hand.Paper,
            "Y" => Hand.Paper,
            "C" => Hand.Scissors,
            "Z" => Hand.Scissors,
            _ => throw new NotImplementedException($"Invalid hand {hand}.")
        };

        public static Result ToResult(this string result) => result switch {
            "X" => Result.Loss,
            "Y" => Result.Draw,
            "Z" => Result.Win,
            _ => throw new NotImplementedException($"Invalid result {result}.")
        };

        public static Hand GetWinningHand(this Hand opponentHand) => opponentHand switch {
            Hand.Rock => Hand.Paper,
            Hand.Paper => Hand.Scissors,
            Hand.Scissors => Hand.Rock,
            _ => throw new NotImplementedException($"Invalid hand to win from: {opponentHand}")
        };

        public static Hand GetLosingHand(this Hand opponentHand) => opponentHand switch {
            Hand.Rock => Hand.Scissors,
            Hand.Paper => Hand.Rock,
            Hand.Scissors => Hand.Paper,
            _ => throw new NotImplementedException($"Invalid hand to lose from: {opponentHand}")
        };

        public static Hand GetDrawingHand(this Hand opponentHand) => opponentHand;
    }
}
