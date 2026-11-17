using System.Diagnostics;
using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Services.Display;

namespace BlockChainSample;

public class Program
{
  public static void Main()
  {
    var hashingService = new HashingService();
    var miningService = new MiningService(hashingService);
    var transactionService = new TransactionService();
    var blockChainDisplayService = new BlockChainDisplayService();
    var walletDisplayService = new WalletsDisplayService();
    var walletService = new WalletService();

    var blockChain = new BlockChain(
        hashingService,
        miningService,
        transactionService,
        blockChainDisplayService
    );

    var wallets = new List<Wallet>();
    var pendingTransactions = new List<Transaction>();

    while (true)
    {
      Console.Clear();
      Console.WriteLine("=== Blockchain Menu ===");
      Console.WriteLine($"Pending transactions: {pendingTransactions.Count}");
      Console.WriteLine("1. Add transaction");
      Console.WriteLine("2. Add block");
      Console.WriteLine("3. Show blockchain");
      Console.WriteLine("4. Get transaction by id");
      Console.WriteLine("5. Get transactions by username");
      Console.WriteLine("6. Repair blockchain");
      Console.WriteLine("7. Broke random block");
      Console.WriteLine("8. Check blockchain");
      Console.WriteLine("9. Find wallet by name");
      Console.WriteLine("10. Find wallet by address");
      Console.WriteLine("11. Print all wallets");
      Console.WriteLine("12. Add invalid transaction");
      Console.WriteLine("--- Type exit to exit");
      Console.Write("\nSelect an option: ");

      string choice = Console.ReadLine()?.Trim();

      switch (choice)
      {
        case "1":
          {
            Console.Write("\nFrom: ");
            string fromName = Console.ReadLine() ?? string.Empty;

            Console.Write("To: ");
            string toName = Console.ReadLine() ?? string.Empty;

            if (fromName == string.Empty || toName == string.Empty)
            {
              Console.WriteLine("Names cannot be empty");
              Pause();
              break;
            }

            var fromWallet = GetOrCreateWallet(fromName, walletService);
            var toWallet = GetOrCreateWallet(toName, walletService);

            Console.Write("Amount: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal ammount))
            {
              Console.WriteLine("Transaction added to pending list.");
            }
            else
            {
              Console.WriteLine("Invalid amount.");
              Pause();
              break;
            }
            var (errorMessage, transaction) = transactionService.CreateTransaction(
                fromWallet,
                fromWallet.Address,
                toWallet.Address.ToString(),
                ammount
            );

            if (errorMessage != null)
              Console.WriteLine($"Error while adding transaction: {errorMessage}");
            else
              pendingTransactions.Add(transaction!);

            Pause();
            break;
          }

        case "2":
          {
            if (pendingTransactions.Count == 0)
            {
              Console.WriteLine("\nNo pending transactions to add.");
            }
            else
            {
              var message = blockChain.AddBlock(pendingTransactions);

              if (message != null)
              {
                Console.WriteLine(message);
                Pause();
                break;
              }

              pendingTransactions.Clear();
              Console.WriteLine("\nBlock added and pending transactions cleared.");
            }

            Pause();
            break;
          }

        case "3":
          {
            Console.WriteLine("\n=== Blockchain ===");
            blockChainDisplayService.DisplayChain(blockChain);

            Pause();
            break;
          }

        case "4":
          {
            Console.WriteLine("Enter id of transaction: ");

            if (!Guid.TryParse(Console.ReadLine(), out Guid id))
            {
              Console.WriteLine("Incorrect format of guid");
            }
            else
            {
              var result = blockChain.FindTransactionById(id);

              if (result == null)
              {
                Console.WriteLine($"Didn`t find transaction with id: {id}");
              }
              else
              {
                Console.WriteLine($"Found transaction in block {result.Value.blockIndex}: ");
                Console.WriteLine(result.Value.transaction);
              }
            }

            Pause();
            break;
          }

        case "5":
          {
            Console.WriteLine("Enter username: ");
            var user = Console.ReadLine();

            Console.WriteLine("Enter filter (from, to or leave empty): ");
            var filterBy = Console.ReadLine();

            if (!new List<string>() { "from", "to", "" }.Contains(filterBy))
            {
              Console.WriteLine($"Incorrect filter by value: {filterBy}");
            }
            else
            {
              var userTransactions = blockChain.GetAllUserTransactions(user, filterBy);

              var filterByString = filterBy == "" ? string.Empty : $" (Filter by {filterBy}) ";
              Console.WriteLine($"User transactions{filterByString}:");

              foreach (var tx in userTransactions)
              {
                Console.WriteLine(tx);
              }
            }

            Pause();
            break;
          }

        case "6":
          {
            Console.WriteLine("Enter an index from where to repair block chain: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int startIndex))
            {
              Console.WriteLine($"Entered an incorrect index: {input}");
              Pause();
              break;
            }

            var stopwatch = new Stopwatch();
            var (success, conntOfTries, conntOfRepaired) = blockChain.RepairChain(startIndex);
            var timeTaken = stopwatch.Elapsed;

            if (success)
            {
              Console.WriteLine($"Chain repaired {conntOfRepaired} blocks with {conntOfTries} tries to mine blocks, time taken: {timeTaken.Milliseconds}ms");
              Console.WriteLine($"Is chain valid: {blockChain.IsChainValid()}");
            }
            else
            {
              Console.WriteLine("Index was out of range");
            }

            Pause();
            break;
          }

        case "7":
          {
            var transactions = blockChain.Chain.SelectMany(b => b.Transactions).ToList();
            var index = Random.Shared.Next(transactions.Count);
            var randomTransaction = transactions[index];
            randomTransaction.Id = Guid.NewGuid();

            break;
          }

        case "8":
          {
            Console.WriteLine($"Is chain valid: {blockChain.IsChainValid()}");

            Pause();
            break;
          }

        case "9":
          {
            var name = Console.ReadLine() ?? string.Empty;
            var wallet = walletService.FindByName(name);

            if (wallet != null) walletDisplayService.DisplayWallet(wallet);
            else Console.WriteLine($"No wallet found with name: {name}");

            Pause();
            break;
          }

        case "10":
          {
            var address = Console.ReadLine() ?? string.Empty;
            var wallet = walletService.FindByAddress(address);

            if (wallet != null) walletDisplayService.DisplayWallet(wallet);
            else Console.WriteLine($"No wallet found with address: {address}");

            Pause();
            break;
          }

        case "11":
          {
            walletDisplayService.DisplayWallets(walletService.GetWallets());

            Pause();
            break;
          }

        case "12":
          {
            var transaction = new Transaction("me", 999999, "you");
            pendingTransactions.Add(transaction);

            Console.WriteLine("Incorrect transaction added to pendings: ");
            blockChainDisplayService.DisplayTransaction(transaction);

            Pause();
            break;
          }

        case "exit":
          Console.WriteLine("\nExiting...");
          return;

        default:
          Console.WriteLine("\nInvalid option. Please choose 1-4.");
          Pause();
          break;
      }
    }
  }

  private static Wallet GetOrCreateWallet(string name, WalletService service)
  {
    var wallet = service.FindByName(name);

    return wallet ?? service.CreateWallet(name)!;
  }

  private static void Pause()
  {
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
  }
}
