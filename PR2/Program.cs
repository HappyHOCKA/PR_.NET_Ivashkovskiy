using System;
using System.Collections.Generic;
using System.Linq;

namespace BankAccounts
{
    internal class Program
    {
        private static readonly List<BankAccount> accounts = new()
        {
            new SavingsAccount("Олена Коваль", 5000, 1000),
            new CheckingAccount("Іван Петренко", 1200, 500),
            new SavingsAccount("Марія Шевченко", 2500, 500)
        };

        private static void Main()
        {
            bool running = true;

            while (running)
            {
                ShowMenu();
                int choice = ReadInt("Ваш вибір: ", 0, 5);

                try
                {
                    switch (choice)
                    {
                        case 1:
                            AddAccount();
                            break;
                        case 2:
                            Deposit();
                            break;
                        case 3:
                            Withdraw();
                            break;
                        case 4:
                            ShowAccounts();
                            break;
                        case 0:
                            running = false;
                            Console.WriteLine("\nДо побачення!");
                            break;
                    }
                }
                catch (InsufficientFundsException ex)
                {
                    Console.WriteLine($"\nПомилка операції: {ex.Message}");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\nНекоректні дані: {ex.Message}");
                }
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("БАНКІВСЬКІ РАХУНКИ");
            Console.WriteLine("1. Додати рахунок");
            Console.WriteLine("2. Поповнити рахунок");
            Console.WriteLine("3. Зняти кошти");
            Console.WriteLine("4. Показати всі рахунки");
            Console.WriteLine("0. Вихід");
        }

        private static void AddAccount()
        {
            Console.WriteLine("\nТип рахунку: 1 — ощадний, 2 — поточний");
            int type = ReadInt("Ваш вибір: ", 1, 2);

            string owner = ReadNonEmpty("Власник: ");
            double balance = ReadDouble("Початковий баланс: ", 0, double.MaxValue);

            BankAccount account = type == 1
                ? new SavingsAccount(owner, balance,
                    ReadDouble("Мінімальний залишок: ", 0, double.MaxValue))
                : new CheckingAccount(owner, balance,
                    ReadDouble("Ліміт овердрафту: ", 0, double.MaxValue));

            accounts.Add(account);
            Console.WriteLine("Рахунок додано.");
        }

        private static void Deposit()
        {
            BankAccount account = SelectAccount();
            double amount = ReadDouble("Сума поповнення: ", 0.01, double.MaxValue);
            account.Deposit(amount);
            Console.WriteLine($"Рахунок поповнено. Новий баланс: {account.Balance:F2} грн.");
        }

        private static void Withdraw()
        {
            BankAccount account = SelectAccount();
            double amount = ReadDouble("Сума зняття: ", 0.01, double.MaxValue);
            account.Withdraw(amount);
            Console.WriteLine($"Кошти знято. Новий баланс: {account.Balance:F2} грн.");
        }

        private static void ShowAccounts()
        {
            Console.WriteLine("\n--- Рахунки (за спаданням балансу) ---");

            if (accounts.Count == 0)
            {
                Console.WriteLine("Рахунків немає.");
                return;
            }

            foreach (BankAccount account in accounts.OrderByDescending(a => a))
            {
                Console.WriteLine(account.GetInfo());
            }
        }

        private static BankAccount SelectAccount()
        {
            if (accounts.Count == 0)
                throw new ArgumentException("Рахунків ще немає. Спочатку додайте рахунок.");

            Console.WriteLine();
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {accounts[i].GetInfo()}");
            }

            int index = ReadInt("Оберіть номер рахунку: ", 1, accounts.Count);
            return accounts[index - 1];
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Помилка: введіть ціле число від {min} до {max}.");
            }
        }

        private static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out double value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Помилка: введіть число від {min} до {max}.");
            }
        }

        private static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();
                Console.WriteLine("Помилка: значення не може бути порожнім.");
            }
        }
    }
}