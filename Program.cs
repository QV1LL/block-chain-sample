using BlockChainSample;

var blockChain = new BlockChain();
var displayService = new BlockChainDisplayService();

blockChain.AddBlock("Bob went to concert");
blockChain.AddBlock("Charlie went to the cinema");
blockChain.AddBlock("Jessica went to concert");
blockChain.AddBlock("Someone went to the cinema");
blockChain.AddBlock("Charlie went to the cinema");
blockChain.AddBlock("Jessica went to concert");
blockChain.AddBlock("Someone went to the cinema");
blockChain.AddBlock("Bob went to concert");
blockChain.AddBlock("Charlie went to the cinema");
blockChain.AddBlock("Jessica went to concert");
blockChain.AddBlock("Someone went to the cinema");
blockChain.AddBlock("Charlie went to the cinema");
blockChain.AddBlock("Jessica went to concert");
blockChain.AddBlock("Someone went to the cinema");

displayService.DisplayDifficultyHistory(blockChain);


