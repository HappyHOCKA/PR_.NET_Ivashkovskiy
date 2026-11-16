using System;
using System.Collections.Generic;
using System.Linq;

namespace GuessNumber
{
    internal class Program
    {
        private const int MinNumber = 1;
        private const int MaxNumber = 1000;
        private const int MaxAttempts = 10;

        private static readonly Random random = new Random();
        private static readonly List<int> gameResults = new List<int>();

        private static void Main()
        {
            bool running = true;

            while (running)
            {
                ShowMenu();
                int choice = ReadInt("Ваш вибір: ", 0, 2);

                switch (choice)
                {
                    case 1:
                        PlayGame();
                        break;
                    case 2:
                        ShowStatistics();
                        break;
                    case 0:
                        running = false;
                        Console.WriteLine("\nДо побачення!");
                        break;
                }
            }
        }


        private static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("ГРА «ВГАДАЙ ЧИСЛО»");
            Console.WriteLine("1. Нова гра");
            Console.WriteLine("2. Статистика за сеанс");
            Console.WriteLine("0. Вихід");
        }

        private static void PlayGame()
        {
            int secret = random.Next(MinNumber, MaxNumber + 1);
            int attemptsUsed = PlayRound(secret, MaxAttempts);

            if (attemptsUsed > 0 && IsWon(secret, attemptsUsed))
            {
                Console.WriteLine($"\nВітаємо! Ви вгадали число {secret} за {attemptsUsed} спроб(у).");
                gameResults.Add(attemptsUsed);
            }
            else
            {
                Console.WriteLine($"\nСпроби вичерпано. Загадане число було: {secret}.");
            }
        }

        private static int PlayRound(int secret, int maxAttempts)
        {
            Console.WriteLine($"\nЯ загадав число від {MinNumber} до {MaxNumber}. " +
                              $"У вас є {maxAttempts} спроб.");

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                int guess = ReadInt($"Спроба {attempt}: ", MinNumber, MaxNumber);

                if (guess == secret)
                {
                    lastRoundWon = true;
                    return attempt;
                }

                lastRoundWon = false;
                int left = maxAttempts - attempt;

                string hint = guess < secret ? "більше" : "менше";
                Console.WriteLine($"Загадане число {hint}. Залишилось спроб: {left}.");
            }

            return maxAttempts;
        }

        // Прапорець результату останньої партії (для читабельності логіки PlayGame)
        private static bool lastRoundWon;

        private static bool IsWon(int secret, int attemptsUsed) => lastRoundWon;

        private static void ShowStatistics()
        {
            Console.WriteLine("\nСтатистика");

            if (gameResults.Count == 0)
            {
                Console.WriteLine("Поки що немає жодної перемоги. Зіграйте першу гру!");
                return;
            }

            Console.WriteLine($"Зіграно перемог: {gameResults.Count}");
            Console.WriteLine($"Найкращий результат: {GetBest(gameResults)} спроб(и)");
            Console.WriteLine($"Середня кількість спроб: {GetAverage(gameResults):F2}");
        }

        private static int GetBest(IEnumerable<int> results) => results.Min();

        private static double GetAverage(IEnumerable<int> results) => results.Average();

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Помилка: введіть ціле число від {min} до {max}.");
            }
        }
    }
}