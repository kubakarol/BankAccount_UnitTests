using NUnit.Framework;
using Moq;


[TestFixture]
public class BankAccountTests
{
    private BankAccount bankAccount;
    private Mock<ITransactionLogger> mockLogger;

    [SetUp]
    public void Setup()
    {
        mockLogger = new Mock<ITransactionLogger>();
        bankAccount = new BankAccount(mockLogger.Object);
    }

    [Test]
    public void Deposit_ValidAmount_IncreasesBalance()
    {
        bankAccount.Deposit(100);
        Assert.AreEqual(100, bankAccount.GetBalance());
    }

    [Test]
    public void Deposit_NegativeAmount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => bankAccount.Deposit(-50));
    }

    [Test]
    public void Withdraw_ValidAmount_DecreasesBalance()
    {
        bankAccount.Deposit(100);
        bankAccount.Withdraw(50);
        Assert.AreEqual(50, bankAccount.GetBalance());
    }

    [Test]
    public void Withdraw_AmountGreaterThanBalance_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => bankAccount.Withdraw(50));
    }

    [Test]
    public void Withdraw_NegativeAmount_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => bankAccount.Withdraw(-50));
    }

    [Test]
    public void GetBalance_InitialBalance_ReturnsZero()
    {
        Assert.AreEqual(0, bankAccount.GetBalance());
    }

    [Test]
    public void Deposit_ValidAmount_CallsLogTransaction()
    {
        bankAccount.Deposit(100);
        mockLogger.Verify(logger => logger.LogTransaction("Deposit", 100, 100), Times.Once);
    }

    [Test]
    public void Withdraw_ValidAmount_CallsLogTransaction()
    {
        bankAccount.Deposit(100);
        bankAccount.Withdraw(50);
        mockLogger.Verify(logger => logger.LogTransaction("Withdraw", 50, 50), Times.Once);
    }

    [Test]
    public void GetBalance_CallsLogTransaction()
    {
        bankAccount.GetBalance();
        mockLogger.Verify(logger => logger.LogTransaction("Check Balance", 0, 0), Times.Once);
    }

    [Test]
    public void MultipleTransactions_LogTransactionCalledCorrectNumberOfTimes()
    {
        bankAccount.Deposit(100);
        bankAccount.Withdraw(30);
        bankAccount.GetBalance();
        mockLogger.Verify(logger => logger.LogTransaction(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>()), Times.Exactly(3));
    }

    [Test]
    public void Withdraw_ValidAmount_LogsCorrectBalance()
    {
        bankAccount.Deposit(100);
        bankAccount.Withdraw(40);
        mockLogger.Verify(logger => logger.LogTransaction("Withdraw", 40, 60), Times.Once);
    }
}
