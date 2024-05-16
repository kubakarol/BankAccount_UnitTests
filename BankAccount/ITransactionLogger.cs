public interface ITransactionLogger
{
    void LogTransaction(string transactionType, decimal amount, decimal balance);
}
