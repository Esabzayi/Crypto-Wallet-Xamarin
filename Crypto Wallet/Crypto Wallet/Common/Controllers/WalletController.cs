using Crypto_Wallet.Common.Database;
using Crypto_Wallet.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_Wallet.Common.Controllers
{

  

  
    public interface IWalletController
    {
        Task<List<Coin>> GetCoins(bool forceReload = false);
        Task<List<Transaction>> GetTransactions(bool forceReload = false);

    }

    public class WalletController : IWalletController
    {
        private IRepository<Transaction> _transactionRepository;

        private List<Coin> _defaultAssets = new List<Coin>
        {
                new Coin
                {
                    Name = "Bitcoin",
                    Amount = 1,
                    Symbol = "BTC",
                    DollarValue = 9500
                },
                new Coin
                {
                    Name = "Ethereum",
                    Amount = 2,
                    Symbol = "ETH",
                    DollarValue = 300
                },
                new Coin
                {
                    Name = "Litecoin",
                    Amount = 3,
                    Symbol = "LTC",
                    DollarValue = 150
                },
        };

        public WalletController(IRepository<Transaction> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public Task<List<Coin>> GetCoins(bool forceReload = false)
        {
            return Task.FromResult(_defaultAssets);
        }

        public async Task<List<Transaction>> GetTransactions(bool forceReload = false)
        {
            var transactions = await _transactionRepository.GetAllAsync();
            transactions = transactions.OrderByDescending(x => x.TransactionDate).ToList();
            transactions.ForEach(x =>
            {
                x.StatusImageSource = x.Status == Constants.TRANSACTION_DEPOSITED ? Constants.TRANSACTION_DEPOSITED_IMAGE : Constants.TRANSACTION_WITHDRAWN_IMAGE;
                x.DollarValue = x.Amount * 200;
            });
            return transactions;
        }
    }
}
