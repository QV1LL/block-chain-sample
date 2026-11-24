using BlockChainSample.Models;

namespace BlockChainSample.Services;

public class BlockChain
{
  public List<Block> Chain { get; set; } = [];
  private int Difficulty { get; set; } = 1;
  private readonly HashingService _hashingService;
  private readonly MiningService _miningService;
  private readonly BalanceService _balanceService;

  private readonly MemPool _memPool;
  private readonly int _maxTransactionsPerBlock = 3;

  private readonly int _adjustmentInterval = 3;
  private readonly int _targetBlockTime = 1;

  private readonly int _rewardCount = 50;
  private readonly int _halvingInterval = 5;

  private readonly List<Transaction> _existingTransactions = [];

  public BlockChain(
      HashingService hashingService,
      MiningService miningService,
      BalanceService balanceService,
      MemPool memPool)
  {
    _hashingService = hashingService;
    _miningService = miningService;
    _balanceService = balanceService;
    _memPool = memPool;

    AddGenesysBlock();
  }

  public string? AddTransaction(Transaction transaction)
  {
    var result = ValidateTransaction(transaction, _memPool.Pending);

    if (result != null) return result;

    result = _memPool.AddTransaction(transaction);

    if (result != null) return result;

    return null;
  }

  public string? MinePending(string minerAddress)
  {
    var batch = _memPool.Peek(_maxTransactionsPerBlock);
    var errorMessage = ValidateTransactions(batch);

    if (errorMessage != null)
      return $"There is an error in transaction while adding block: {errorMessage}";

    var coinbaseTransaction = Transaction.CreateCoinbaseTransaction(minerAddress, GetCurrentReward());

    var lastBlock = Chain[^1];
    var newBlock = new Block
    {
      Index = lastBlock.Index + 1,
      TimeStamp = DateTime.UtcNow,
      Transactions = [coinbaseTransaction, .. batch],
      PreviousHash = lastBlock.Hash,
      DifficultyAtMining = Difficulty
    };

    newBlock.MerkleRoot = _hashingService.ComputeMerkleRoot(newBlock.Transactions);
    _miningService.MineBlock(newBlock, Difficulty);
    Chain.Add(newBlock);

    _existingTransactions.AddRange([.. batch]);
    _memPool.RemoveTransaction(batch);

    if (newBlock.Index % _adjustmentInterval == 0)
      AdjustDifficulty();

    return null;
  }

  public bool GetMerkleProof(Guid transactionId)
  {
    var result = FindTransactionById(transactionId);

    if (result == null) return false;

    var block = Chain.FirstOrDefault(b => b.Index == result.Value.blockIndex);

    if (block == null) return false;

    return _hashingService.ComputeMerkleRoot(block.Transactions) == block.MerkleRoot;
  }

  public decimal GetCurrentReward()
  {
    int halvings = Chain.Count / _halvingInterval;

    return (decimal)(_rewardCount * Math.Pow(0.5, halvings));
  }

  public bool IsChainValid()
  {
    for (int i = 1; i < Chain.Count; i++)
    {
      var currentBlock = Chain[i];
      var previousBlock = Chain[i - 1];

      if (currentBlock.Index == 0)
        continue;

      if (currentBlock.Hash != _hashingService.ComputeHash(currentBlock))
        return false;

      if (currentBlock.PreviousHash != previousBlock.Hash)
        return false;

      if (currentBlock.Hash[..currentBlock.DifficultyAtMining] != new string('0', currentBlock.DifficultyAtMining))
        return false;

      if (currentBlock.MerkleRoot != _hashingService.ComputeMerkleRoot(currentBlock.Transactions))
        return false;
    }
    return true;
  }

  public bool IsTransactionInChain(Guid id)
  {
    return _existingTransactions.Any(et => et.Id == id);
  }

  public (Block, string)? FindFirstInvalidBlock()
  {
    for (int i = 1; i < Chain.Count; i++)
    {
      var currentBlock = Chain[i];
      var previousBlock = Chain[i - 1];

      if (currentBlock.Transactions.Count != 0)
        continue;

      if (currentBlock.Hash != _hashingService.ComputeHash(currentBlock))
        return (currentBlock, "Invalid hash");

      if (currentBlock.PreviousHash != previousBlock.Hash)
        return (currentBlock, "Incorrect previous hash");

      if (currentBlock.Hash[..currentBlock.DifficultyAtMining] != new string('0', currentBlock.DifficultyAtMining))
        return (currentBlock, "Incorrect difficulty");
    }

    return null;
  }

  public (bool isSuccess, int? countOfTries, int? counfOfRepainedBlocks) RepairChain(int startIndex)
  {
    if (startIndex >= Chain.Count || startIndex < 0) return (false, null, null);

    var chainToRepair = Chain[startIndex..];
    string? previousHash = null;
    int countOfRepaired = 0;
    int countOfTries = 0;

    foreach (var block in chainToRepair)
    {
      previousHash ??= startIndex == 0 ? Chain[0].PreviousHash : Chain[startIndex - 1].Hash;
      block.PreviousHash = previousHash;

      _miningService.MineBlock(block, block.DifficultyAtMining);
      previousHash = block.Hash;
      countOfTries += block.Nonce;
      countOfRepaired++;
    }

    return (true, countOfTries, countOfRepaired);
  }

  public Block? FindBlockByIndex(int index)
  {
    if (index < 0 || Chain.Count <= index)
    {
      return null;
    }

    return Chain[index];
  }

  public (Transaction transaction, int blockIndex)? FindTransactionById(Guid id)
  {
    foreach (var block in Chain)
      foreach (var transaction in block.Transactions)
        if (transaction.Id == id) return (transaction, block.Index);

    return null;
  }

  public List<Transaction> GetAllUserTransactions(string user, string? filterBy = "")
  {
    var userTransactions = new List<Transaction>();

    foreach (var block in Chain)
    {
      foreach (var transaction in block.Transactions)
      {
        bool isFromMatch = transaction.From == user && (filterBy == "" || filterBy == "from");
        bool isToMatch = transaction.To == user && (filterBy == "" || filterBy == "to");

        if (isFromMatch || isToMatch)
        {
          userTransactions.Add(transaction);
        }
      }
    }

    return userTransactions;
  }

  public Block? FindByEvent(string text)
  {
    foreach (Block block in Chain)
      if (block.Transactions.Any(t => t.From.Contains(text) || t.To.Contains(text)))
        return block;

    return null;
  }

  public string? ValidateTransactions(IEnumerable<Transaction> transactions)
  {
    foreach (var transaction in transactions)
    {
      var errorMessage = ValidateTransaction(transaction, transactions);

      if (errorMessage != null) return errorMessage;
    }

    return null;
  }

  public string? ValidateTransaction(Transaction transaction, IEnumerable<Transaction> transactions)
  {
    var (isValid, errorMessage) = TransactionService.ValidateTransaction(transaction);

    if (!isValid)
      return errorMessage;

    if (IsTransactionInChain(transaction.Id))
      return $"Transaction with id {transaction.Id} is already exists";

    if (_existingTransactions.Contains(transaction))
      return "Transaction is already in block chain";

    if (!_balanceService.HasSufficientBalance(
          transaction.From,
          transaction.Ammount,
          Chain,
          transactions.Where(t => t.Id != transaction.Id)))
      return "Transaction have not enough ammount of coin";

    return null;
  }

  private void AdjustDifficulty()
  {
    var recent = Chain.Where(b => b.Index > 0).TakeLast(_adjustmentInterval).ToList();

    if (recent.Count == 0) return;

    var avgTime = recent.Average(b => b.MiningDuration);

    if (avgTime < _targetBlockTime)
    {
      Difficulty++;
    }
    else if (avgTime > _targetBlockTime)
    {
      Difficulty = Math.Max(1, Difficulty - 1);
    }
  }

  private void AddGenesysBlock()
  {
    var genesysBlock = new Block
    {
      Index = 0,
      TimeStamp = DateTime.UtcNow,
      Transactions = [],
      PreviousHash = "0",
      DifficultyAtMining = Difficulty
    };

    genesysBlock.MerkleRoot = _hashingService.ComputeMerkleRoot(genesysBlock.Transactions);
    _miningService.MineBlock(genesysBlock, Difficulty);
    Chain.Add(genesysBlock);
  }
}
