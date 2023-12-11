using System;

namespace Guess_game
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set up parameters for the game
            Parameters gameParameters = new Parameters();
            gameParameters.ChooseGameMode();
            gameParameters.ChooseGameLevel();
            gameParameters.SetUpLimit();

            // Create an instance of the appropriate game class based on the value of Mode property
            Game selectedGame;

            switch (gameParameters.Mode)
            {
                case "T":
                    selectedGame = new TimeLimitedGame(gameParameters.Limit);
                    break;
                case "N":
                    selectedGame = new GuessesLimitGame(gameParameters.Limit);
                    break;
                default:
                    selectedGame = new GuessTheNumberGame();
                    break;
            }

            // Start the game
            selectedGame.StartGame();
        }
        public class Parameters
        {
            private int timeLimitHardLevel;
            private int timeLimitMediumLevel;
            private int guessesLimitHardLevel;
            private int guessesLimitMediumLevel;
            public int Level { get; set; }
            public string Mode { get; set; }
            public int Limit { get; set; }
            public Parameters()
            //constructor
            {
                string timeLimitHardLevelStr = Environment.GetEnvironmentVariable("TIME_LIMIT_HARD_LEVEL");
                string timeLimitMediumLevelStr = Environment.GetEnvironmentVariable("TIME_LIMIT_MEDIUM_LEVEL");
                string guessesLimitHardLevelStr = Environment.GetEnvironmentVariable("GUESSES_LIMIT_HARD_LEVEL");
                string guessesLimitMediumLevelStr = Environment.GetEnvironmentVariable("GUESSES_LIMIT_MEDIUM_LEVEL");

                //initialize private fields with default values
                timeLimitHardLevel = 1;
                timeLimitMediumLevel = 3;
                guessesLimitHardLevel = 10;
                guessesLimitMediumLevel = 30;

                if (!string.IsNullOrEmpty(timeLimitHardLevelStr) && int.TryParse(timeLimitHardLevelStr, out int timeLimitHardLevelValue))
                {
                    timeLimitHardLevel = timeLimitHardLevelValue;
                }
            }
            public void ChooseGameLevel()
            {
                Console.WriteLine("Choose a game level (1-3):");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out int chosenLevel) && chosenLevel >= 1 && chosenLevel <= 3)
                    {
                        Level = chosenLevel;
                        Console.WriteLine($"You have Chose level {Level}.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 3");
                    }
                }
            }
            public void ChooseGameMode()
            {
                Console.WriteLine("Choose the game mode (T for time, N for number of guesses:");
                while (true)
                {
                    string input = Console.ReadLine().ToUpper();
                    if (input == "T" || input == "N")
                    {
                        Mode = input;
                        Console.WriteLine($"you have chosen mode {Mode}.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter T or N.");
                    }
                }
            }
            public void SetUpLimit()
            {
                if (Mode == "T" && Level == 3)
                {
                    Limit = timeLimitHardLevel;
                }
                else if (Mode == "N" && Level == 2)
                {
                    Limit = guessesLimitMediumLevel;
                }
                else
                {
                    Limit = 0;
                }
            }
        }

        public abstract class Game
        {
            protected int targetNumber;

            public Game()
            {
                targetNumber = new Random().Next(0, 101);
            }

            public abstract void StartGame();

            protected bool MakeGuess()
            {
                Console.WriteLine("Enter your guess:");
                if (int.TryParse(Console.ReadLine(), out int guess))
                {
                    if (guess == targetNumber)
                    {
                        Console.WriteLine($"Congratulations! You guessed the number {targetNumber}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(guess < targetNumber ? "The target number is more" : "The target number is less");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please entera valid number");
                }
                return false;
            }

        }
        public class GuessTheNumberGame : Game
        {
            public override void StartGame()
            {
                Console.WriteLine("Welcome to Guess the Number Game!");
                Console.WriteLine("Try to guess the randomly generated by me!!!");
                    while (true)
                {
                    if (MakeGuess())
                    {
                        break;

                    }
                }
            }
        }
        public class TimeLimitedGame : Game
        {
            private readonly int timeLimit;
            public TimeLimitedGame(int limit)
            {
                timeLimit = limit;
            }
            public override void StartGame()
            {
                if (timeLimit <= 0)
                {
                    Console.WriteLine("Time limit is not set");
                    return;
                }
                DateTime startTime = DateTime.Now;
                DateTime endTime = startTime.AddMinutes(timeLimit);
                Console.WriteLine($"Welcome to time limited game! you have {timeLimit} minutes to guess the number");

                while (DateTime.Now < endTime)
                {
                    if (MakeGuess())
                    {
                        return;
                    }
                }
                Console.WriteLine("Time is up! Game over!");
            }
        }
        public class GuessesLimitGame : Game
        {
            private readonly int guessLimit;
            protected int guessCount;

            public GuessesLimitGame(int limit)
            {
                guessLimit = limit;
                guessCount = 0;
            }
            public override void StartGame()
            {
                if (guessLimit <= 0)
                {
                    Console.WriteLine("Guess limit is not set");
                        return;
                }
                Console.WriteLine($"Welcome to the guesses limited game! you have {guessLimit} guesses to guess the number");
                while (guessCount < guessLimit)
                {
                    if (MakeGuess())
                    {
                        return;
                    }
                    guessCount++;
                }
                Console.WriteLine("Out of guesses, Game over!");
            }
        }
    }
}

        

