using BlockChainSample.Helpers.DI;
using BlockChainSample.Models;
using BlockChainSample.Services;
using BlockChainSample.Ui.Menu;
using BlockChainSample.Ui.Middlewares;
using BlockChainSample.Ui.Middlewares.Handlers;
using BlockChainSample.UI.Middlewares;
using BlockChainSample.UI.Middlewares.Handlers;

namespace BlockChainSample;

public class Program
{
  public static async Task Main()
  {
    var container = ConfigureServices();
    var memPool = container.Resolve<MemPool>();

    var menu = new Menu(container)
      .SetHeader(() => $"Pending transactions: {memPool.Count}")
      .UseGlobal<ExceptionHandlingMiddleware>()

      .AddItem("Add transaction", item => item
        .Use<PromptFromMiddleware>()
        .Use<PromptToMiddleware>()
        .Use<PromptAmountMiddleware>()
        .Use<ResolveOrCreateTransferWalletsMiddleware>()
        .Use<AddTransactionMiddleware>())
      .AddItem("Add invalid transaction", item => item
        .Use<PromptFromMiddleware>()
        .Use<PromptToMiddleware>()
        .Use<PromptAmountMiddleware>()
        .Use<ResolveOrCreateTransferWalletsMiddleware>()
        .Use<AddInvalidTransactionMiddleware>())
      .AddItem("Clear pending transactions", item => item
        .Use<ClearPendingTransactionsMiddleware>())
      .AddItem("Get transaction by id", item => item
        .Use<PromptTransactionIdMiddleware>()
        .Use<GetTransactionByIdMiddleware>())
      .AddItem("Modify transaction by id", item => item
        .Use<PromptTransactionIdMiddleware>()
        .Use<GetTransactionByIdMiddleware>()
        .Use<ModifyTransactionByIdMiddleware>())
      .AddItem("Try add existing transaction in block chain", item => item
        .Use<PromptTransactionIdMiddleware>()
        .Use<GetTransactionByIdMiddleware>()
        .Use<TryToAddExistingTransactionMiddleware>())
      .AddItem("Get pending transactions", item => item
        .Use<GetPendingTransactionsMiddleware>())
      .AddItem("Check transaction by id", item => item
        .Use<PromptTransactionIdMiddleware>()
        .Use<GetPendingTransactionByIdMiddleware>()
        .Use<CheckTransactionMiddleware>())
      .AddItem("Verify transaction by merkle root (transaction id required)", item => item
        .Use<PromptTransactionIdMiddleware>()
        .Use<GetTransactionByIdMiddleware>()
        .Use<VerifyTransactionByMerkleRootMiddleware>())
      .AddItem("Find corrupted transactions", item => item
        .Use<FindCorruptedTransactionsMiddleware>())
      .AddItem("Check pending transactions", item => item
        .Use<GetPendingTransactionsMiddleware>()
        .Use<PrintSummaryForPendingTransactionsMiddleware>())
      .AddItem("Get transactions by username", item => item
        .Use<PromptUserTransactionsFilterMiddleware>()
        .Use<GetTransactionsByUsernameMiddleware>())

      .AddItem("Add block", item => item
        .Use<PromptTargetWalletNameMiddleware>()
        .Use<ResolveOrCreateSingleWalletMiddleware>()
        .Use<AddBlockMiddleware>())
      .AddItem("Show blockchain", item => item
        .Use<ShowBlockchainMiddleware>())
      .AddItem("Check blockchain", item => item
        .Use<CheckBlockchainMiddleware>())
      .AddItem("Repair blockchain", item => item
        .Use<PromptRepairStartIndexMiddleware>()
        .Use<RepairBlockchainMiddleware>())
      .AddItem("Broke random block", item => item
        .Use<BreakRandomBlockMiddleware>())

      .AddItem("Find wallet by name", item => item
        .Use<PromptTargetWalletNameMiddleware>()
        .Use<FindWalletByNameMiddleware>())
      .AddItem("Find wallet by address", item => item
        .Use<PromptWalletAddressMiddleware>()
        .Use<FindWalletByAddressMiddleware>())
      .AddItem("Print all wallets", item => item
        .Use<PrintAllWalletsMiddleware>())
      .AddItem("Check wallet balance", item => item
        .Use<PromptTargetWalletNameMiddleware>()
        .Use<ResolveOrCreateSingleWalletMiddleware>()
        .Use<CheckWalletBalanceMiddleware>())
      .AddItem("Print transactions history", item => item
        .Use<PromptTargetWalletNameMiddleware>()
        .Use<ResolveOrCreateSingleWalletMiddleware>()
        .Use<PrintTransactionsHistoryMiddleware>())

      .AddItem("exit", "Type exit to exit", item => item
        .Use<ExitMenuMiddleware>());

    await menu.RunAsync();
  }

  private static ServiceContainer ConfigureServices()
  {
    var container = new ServiceContainer();

    container.RegisterTransient<HashingService, HashingService>();
    container.RegisterTransient<MiningService, MiningService>();
    container.RegisterTransient<BalanceService, BalanceService>();

    container.RegisterSingleton<WalletService, WalletService>();
    container.RegisterSingleton<BlockChain, BlockChain>();
    container.RegisterSingleton<MemPool, MemPool>();

    return container;
  }
}
