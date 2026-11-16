using System;

namespace BankAccounts
{
    public class BankAccount : IComparable<BankAccount>
    {
        private string _owner = string.Empty;
        private double _balance;

        public BankAccount(string owner, double initialBalance)
        {
            Owner = owner;
            Balance = initialBalance;
        }

        public string Owner
        {
            get => _owner;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ім'я власника не може бути порожнім.");
                _owner = value.Trim();
            }
        }

        public double Balance
        {
            get => _balance;
            protected set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                    throw new ArgumentException("Баланс має бути скінченним числом.");
                _balance = value;
            }
        }

        public virtual string AccountType => "Базовий рахунок";

        public void Deposit(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума поповнення має бути більшою за нуль.");
            Balance += amount;
        }

        public virtual void Withdraw(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума зняття має бути більшою за нуль.");

            if (amount > Balance)
                throw new InsufficientFundsException(
                    $"Недостатньо коштів: баланс {Balance:F2}, спроба зняти {amount:F2}.");

            Balance -= amount;
        }

        public int CompareTo(BankAccount? other)
        {
            if (other is null) return 1;
            return Balance.CompareTo(other.Balance);
        }

        public virtual string GetInfo() =>
            $"[{AccountType}] Власник: {Owner,-15} Баланс: {Balance,12:F2} грн";
    }
}