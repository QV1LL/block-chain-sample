using BlockChainSample.Services.Display;

namespace BlockChainSample.Models;

public class MemPool
{
  public int Count => _pending.Count;
  public IReadOnlyCollection<Transaction> Pending => _pending.AsReadOnly();
  private readonly List<Transaction> _pending = [];
  private readonly int _maxTransactionLifetimeInMinutes = 2;

  public Transaction? GetTransactionById(Guid id)
  {
    return _pending.FirstOrDefault(p => p.Id == id);
  }

  public string? AddTransaction(Transaction transaction)
  {
    if (_pending.Any(p => p.Id == transaction.Id))
      return "Transaction is already exist in mem pool";

    if (_pending.Count(t => t.From == transaction.From) >= 3)
      return "Cannot add more than 3 transactions from 1 user";

    transaction.AddedAt = DateTime.Now;
    _pending.Add(transaction);

    return null;
  }

  public IEnumerable<Transaction> Peek(int maxCount)
  {
    var overdueTransactions = _pending.Where(p => DateTime.Now - p.AddedAt > TimeSpan.FromMinutes(_maxTransactionLifetimeInMinutes));

    if (overdueTransactions.Any())
    {
      BlockChainDisplayService.DisplayDeletedOverdueTransactions(overdueTransactions);
      RemoveTransaction(overdueTransactions);
    }

    return _pending.Take(maxCount);
  }

  public void RemoveTransaction(IEnumerable<Transaction> transactions)
  {
    var ids = transactions.Select(p => p.Id).ToHashSet();
    _pending.RemoveAll(t => ids.Contains(t.Id));
  }

  public override string ToString()
  {
    return $"""
    Transactions count: {Count}
    {string.Join('\n', _pending)}
    """;
  }
}
