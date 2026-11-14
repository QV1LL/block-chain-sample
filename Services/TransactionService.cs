
using BlockChainSample.Models;

namespace BlockChainSample.Services;

public class TransactionService
{
  public (string? errorMessage, Transaction? transaction) CreateTransaction(string from, string to, decimal ammount)
  {
    var transaction = new Transaction(from, ammount, to);

    var (isValid, errorMessage) = ValidateTransaction(transaction);

    if (!isValid) return (errorMessage, null);

    return (null, transaction);
  }

  public (bool isValid, string errorMessage) ValidateTransaction(Transaction tx)
  {
    if (string.IsNullOrEmpty(tx.From))
    {
      return (false, "Invalid sender address");
    }

    if (string.IsNullOrEmpty(tx.To))
    {
      return (false, "Invalid recipient address");
    }

    if (tx.Ammount <= 0)
    {
      return (false, "Invalid transaction amount");
    }

    return (true, string.Empty);
  }
}
