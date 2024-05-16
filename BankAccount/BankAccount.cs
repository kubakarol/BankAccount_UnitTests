
public class BankAccount
{
    private decimal balance;
    private readonly ITransactionLogger transactionLogger;

    public BankAccount(ITransactionLogger logger)
    {
        balance = 0;
        transactionLogger = logger;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.");

        balance += amount;
        transactionLogger.LogTransaction("Deposit", amount, balance);
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.");
        if (amount > balance)
            throw new InvalidOperationException("Insufficient funds.");

        balance -= amount;
        transactionLogger.LogTransaction("Withdraw", amount, balance);
    }

    public decimal GetBalance()
    {
        transactionLogger.LogTransaction("Check Balance", 0, balance);
        return balance;
    }
}
