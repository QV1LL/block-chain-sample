
using System.Diagnostics;
using BlockChainSample.Models;
using BlockChainSample.Services;

public class Program
{
  public static void Main()
  {
    var transactionService = new TransactionService();
    var blockChain = new BlockChain();
    var displayService = new BlockChainDisplayService();

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
      Console.WriteLine("7. Exit");
      Console.Write("\nSelect an option: ");

      string choice = Console.ReadLine()?.Trim();

      switch (choice)
      {
        case "1":
          Console.Write("\nFrom: ");
          string from = Console.ReadLine();

          Console.Write("To: ");
          string to = Console.ReadLine();

          Console.Write("Amount: ");
          if (decimal.TryParse(Console.ReadLine(), out decimal ammount))
          {
            Console.WriteLine("Transaction added to pending list.");
          }
          else
          {
            Console.WriteLine("Invalid amount.");
          }
          var (errorMessage, transaction) = transactionService.CreateTransaction(from, to, ammount);

          if (errorMessage != null)
            Console.WriteLine($"Error while adding transaction: {errorMessage}");
          else
            pendingTransactions.Add(transaction!);

          Pause();
          break;

        case "2":
          if (pendingTransactions.Count == 0)
          {
            Console.WriteLine("\nNo pending transactions to add.");
          }
          else
          {
            blockChain.AddBlock(pendingTransactions);
            pendingTransactions.Clear();
            Console.WriteLine("\nBlock added and pending transactions cleared.");
          }
          Pause();
          break;

        case "3":
          Console.WriteLine("\n=== Blockchain ===");
          displayService.DisplayChain(blockChain);
          Pause();
          break;

        case "4":
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

        case "5":
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

        case "6":
          // I didn`t understand what count of tries in mean in task
          Console.WriteLine("Enter an index from where to repair block chain: ");
          var input = Console.ReadLine();

          if (!int.TryParse(input, out int startIndex))
          {
            Console.WriteLine($"Entered an incorrect index: {input}");
            Pause();
            break;
          }

          var stopwatch = new Stopwatch();
          var success = blockChain.RepairChain(startIndex);
          var timeTaken = stopwatch.Elapsed;

          if (success)
          {
            Console.WriteLine($"Chain was repaired, time taken: {timeTaken.Seconds}");
            Console.WriteLine($"Is chain valid: {blockChain.IsChainValid()}");
          }
          else
          {
            Console.WriteLine("Index was out of range");
          }

          Pause();
          break;

        case "7":
          Console.WriteLine("\nExiting...");
          return;

        default:
          Console.WriteLine("\nInvalid option. Please choose 1-4.");
          Pause();
          break;
      }
    }
  }

  private static void Pause()
  {
    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
  }
}
