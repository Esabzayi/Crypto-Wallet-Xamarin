using Crypto_Wallet.Common.Base;
using Crypto_Wallet.Common.Controllers;
using Crypto_Wallet.Common.Models;
using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Crypto_Wallet.Modules.Wallet
{
    public class WalletViewModel : BaseViewModel
    {
        private IWalletController _walletController;

        public WalletViewModel(IWalletController walletController)
        {
            _walletController = walletController;
            Assets = new ObservableCollection<Coin>();
            LatestTransactions = new ObservableCollection<Transaction>();
        }

        public override async Task InitializeAsync(object parameter)
        {
            bool reload = (bool)parameter;
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;
            IsRefreshing = true;
            var transactions = await _walletController.GetTransactions();
            LatestTransactions = new ObservableCollection<Transaction>(transactions.Take(5));
            var assets = await _walletController.GetCoins();
            Assets = new ObservableCollection<Coin>(assets.Take(3));
            BuildChart(assets);
            PortfolioValue = assets.Sum(x => x.DollarValue);

            IsRefreshing = false;
            IsBusy = false;

        }

        private void BuildChart(List<Coin> assets)
        {
            var WhiteColor = SKColor.Parse("#ffffff");

            List<ChartEntry> entries = new List<ChartEntry>();

            var colors = Coin.GetAvailableAssets();

            foreach (var item in assets)
            {
                entries.Add(new ChartEntry((float)item.DollarValue)
                {
                    TextColor = WhiteColor,
                    ValueLabel = item.Name,
                    Color = SKColor.Parse(colors.First(x => x.Symbol == item.Symbol).HexColor)
                });
            }

            var chart = new DonutChart { Entries = entries };
            chart.BackgroundColor = WhiteColor;
            PortfolioView = chart;
        }
        private Chart _portfolioView;
        public Chart PortfolioView
        {
            get => _portfolioView;
            set { SetProperty(ref _portfolioView, value); }
        }


        private int _coinsHeight;
        public int CoinsHeight
        {
            get => _coinsHeight;
            set { SetProperty(ref _coinsHeight, value); }
        }

        private ObservableCollection<Coin> _assets;
        public ObservableCollection<Coin> Assets
        {
            get => _assets;
            set
            {

                SetProperty(ref _assets, value);

                if (_assets == null)
                {
                    return;
                }
                CoinsHeight = _assets.Count * 85;
            }
        }

        private int _transactionHeight;
        public int TransactionsHeight
        {
            get => _transactionHeight;
            set { SetProperty(ref _transactionHeight, value); }
        }


        private ObservableCollection<Transaction> _lastestTransactions;
        public ObservableCollection<Transaction> LatestTransactions
        {
            get => _lastestTransactions;
            set
            {

                SetProperty(ref _lastestTransactions, value);

                if (_lastestTransactions == null)
                {
                    return;
                }
                HasTransaction = _lastestTransactions.Count > 0;
                if (_lastestTransactions.Count == 0)
                {
                    TransactionsHeight = 430;
                    return;
                }
                TransactionsHeight = _lastestTransactions.Count * 85;
            }
        }

        public ICommand AddNewTransactionCommand { get => new Command(async () => await AddNewTransaction()); }
        private async Task AddNewTransaction()
        {
            await Shell.Current.DisplayAlert("Todo", "you have been logged out", "OK");
        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }

        private decimal _portfolioValue;
        public decimal PortfolioValue
        {
            get => _portfolioValue;
            set { SetProperty(ref _portfolioValue, value); }
        }

        public ICommand RefreshAssetsCommand { get => new Command(async () => await InitializeAsync(true)); }

        private bool _hasTransaction;
        public bool HasTransaction
        {
            get => _hasTransaction;
            set { SetProperty(ref _hasTransaction, value); }
        }
    }
}
