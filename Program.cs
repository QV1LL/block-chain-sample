using BlockChainSample.Helpers.DI;
using BlockChainSample.Services;
using BlockChainSample.Ui.Menu;
using BlockChainSample.Ui.Middlewares;
using BlockChainSample.Ui.Middlewares.Handlers;
using BlockChainSample.UI.Middlewares;

namespace BlockChainSample;

public class Program
{
  public static async Task Main()
  {
    var container = ConfigureServices();
    var transactionService = container.Resolve<TransactionService>();

    var menu = new Menu(container)
      .SetHeader(() => $"Pending transactions: {transactionService.GetPendingTransactions().Count}")
      .UseGlobal<ExceptionHandlingMiddleware>()
      .AddItem("1", "Add transaction", item => item
        .Use<PromptFromMiddleware>()
        .Use<PromptToMiddleware>()
        .Use<PromptAmountMiddleware>()
        .Use<ResolveOrCreateTransferWalletsMiddleware>()
        .Use<AddTransactionMiddleware>())
      .AddItem("2", "Add block", item => item
        .Use<PromptTargetWalletNameMiddleware>()
        .Use<ResolveOrCreateSingleWalletMiddleware>()
        .Use<AddBlockMiddleware>())
      .AddItem("3", "Show blockchain", item => item
        .Use<ShowBlockchainMiddleware>())
      .AddItem("4", "Get transaction by id", item => item
        .Use<PromptTransactionIdMiddleware>()
        .Use<GetTransactionByIdMiddleware>())
      .AddItem("5", "Get transactions by username", item => item
        .Use<PromptUserTransactionsFilterMiddleware>()
        .Use<GetTransactionsByUsernameMiddleware>())
      .AddItem("6", "Repair blockchain", item => item
        .Use<PromptRepairStartIndexMiddleware>()
        .Use<RepairBlockchainMiddleware>())
      .AddItem("7", "Broke random block", item => item
        .Use<BreakRandomBlockMiddleware>())
      .AddItem("8", "Check blockchain", item => item
        .Use<CheckBlockchainMiddleware>())
      .AddItem("9", "Find wallet by name", item => item
        .Use<PromptTargetWalletNameMiddleware>()
        .Use<FindWalletByNameMiddleware>())
      .AddItem("10", "Find wallet by address", item => item
        .Use<PromptWalletAddressMiddleware>()
        .Use<FindWalletByAddressMiddleware>())
      .AddItem("11", "Print all wallets", item => item
        .Use<PrintAllWalletsMiddleware>())
      .AddItem("12", "Add invalid transaction", item => item
        .Use<AddInvalidTransactionMiddleware>())
      .AddItem("13", "Clear pending transactions", item => item
        .Use<ClearPendingTransactionsMiddleware>())
      .AddItem("14", "Check wallet balance", item => item
        .Use<PromptTargetWalletNameMiddleware>()
        .Use<ResolveOrCreateSingleWalletMiddleware>()
        .Use<CheckWalletBalanceMiddleware>())
      .AddItem("15", "Print transactions history", item => item
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

    container.RegisterSingleton<TransactionService, TransactionService>();
    container.RegisterSingleton<WalletService, WalletService>();
    container.RegisterSingleton<BlockChain, BlockChain>();

    return container;
  }
}
