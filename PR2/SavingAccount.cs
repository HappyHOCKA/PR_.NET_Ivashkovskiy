namespace BankAccounts
{
    public class SavingsAccount : BankAccount
    {
        public SavingsAccount(string owner, double initialBalance, double minimumBalance)
            : base(owner, initialBalance)
        {
            MinimumBalance = minimumBalance;
            if (initialBalance < MinimumBalance)
                throw new ArgumentException("Початковий баланс не може бути нижчим за мінімальний залишок.");
        }

        public double MinimumBalance { get; }

        public override string AccountType => "Ощадний рахунок";

        public override void Withdraw(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума зняття має бути більшою за нуль.");

            double limit = Balance - MinimumBalance;
            if (amount > limit)
                throw new InsufficientFundsException(
                    $"Перевищено мінімальний залишок {MinimumBalance:F2}. " +
                    $"Доступно для зняття: {Math.Max(limit, 0):F2}.");

            Balance -= amount;
        }

        public override string GetInfo() =>
            base.GetInfo() + $" Мін. залишок: {MinimumBalance:F2}";
    }
}