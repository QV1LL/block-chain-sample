using System.Diagnostics;

namespace BlockChainSample;

public class MiningService(HashingService hashingService)
{
  private readonly HashingService _hashingService = hashingService;
  private readonly Lock _lock = new();

  public void MineBlock(Block block, int difficulty)
  {
    var target = new string('0', difficulty);
    var stopwatch = Stopwatch.StartNew();
    int workerCount = Environment.ProcessorCount;

    var cts = new CancellationTokenSource();
    var options = new ParallelOptions
    {
      MaxDegreeOfParallelism = Environment.ProcessorCount,
      CancellationToken = cts.Token
    };

    try
    {
      Parallel.For(0, workerCount, options, workerId =>
        {
          int nonce = workerId;

          while (!options.CancellationToken.IsCancellationRequested)
          {
            var possibleHash = _hashingService.ComputeHash(block, nonce);

            if (possibleHash.StartsWith(target))
            {
              lock (_lock)
              {
                if (!cts.IsCancellationRequested)
                {
                  stopwatch.Stop();

                  block.Nonce = nonce;
                  block.Hash = possibleHash;
                  block.MiningDuration = stopwatch.Elapsed.TotalSeconds;

                  cts.Cancel();
                }
              }

              return;
            }

            nonce += workerCount;
          }
        });
    }
    catch (OperationCanceledException)
    {
      Console.WriteLine("Parallel loop was canceled.");
    }
  }
}
