namespace BankAccounts
{
    public class CheckingAccount : BankAccount
    {
        public CheckingAccount(string owner, double initialBalance, double overdraftLimit)
            : base(owner, initialBalance)
        {
            if (overdraftLimit < 0)
                throw new ArgumentException("Ліміт овердрафту не може бути від'ємним.");
            OverdraftLimit = overdraftLimit;
        }

        public double OverdraftLimit { get; }

        public override string AccountType => "Поточний рахунок";

        public override void Withdraw(double amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сума зняття має бути більшою за нуль.");

            if (Balance - amount < -OverdraftLimit)
                throw new InsufficientFundsException(
                    $"Перевищено ліміт овердрафту {OverdraftLimit:F2}. " +
                    $"Максимально можна зняти: {Balance + OverdraftLimit:F2}.");

            Balance -= amount;
        }

        public override string GetInfo() =>
            base.GetInfo() + $" Овердрафт: {OverdraftLimit:F2}";
    }
}