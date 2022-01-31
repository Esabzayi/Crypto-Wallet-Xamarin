using Crypto_Wallet.Common.Base;
using Crypto_Wallet.Common.Controllers;
using Crypto_Wallet.Common.Models;
using Crypto_Wallet.Common.Navigation;
using Crypto_Wallet.Modules.AddTransactions;
using Crypto_Wallet.Modules.Wallet;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Crypto_Wallet.Modules.Transactions
{
    public class TransactionViewModel : BaseViewModel
    {
        private IWalletController _walletController;
        private INavigationService _navigationService;
        private string _filter = string.Empty;

        public TransactionViewModel(IWalletController walletController , INavigationService navigationService)
        {
            _walletController = walletController;
            _navigationService = navigationService;
            Transaction = new ObservableCollection<Transaction>();
        }

        public override async Task InitializeAsync(object parameter)
        {
            _filter = parameter.ToString();
            await GetTransactions();
        }

        private async Task GetTransactions()
        {
            IsRefreshing = true;
            var tranactions = await _walletController.GetTransactions();
            if (!string.IsNullOrEmpty(_filter))
            {
                tranactions = tranactions.Where(x => x.Status == _filter).ToList();
            }
            Transaction = new ObservableCollection<Transaction>(tranactions);
            IsRefreshing = false;
        }

        private ObservableCollection<Transaction> _transactions;
        public ObservableCollection<Transaction> Transaction
        {
            get => _transactions;
            set { SetProperty(ref _transactions, value); }
        }

        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => _selectedTransaction;
            set { SetProperty(ref _selectedTransaction, value); }
        }


        public ICommand RefreshTransactionsCommand { get => new Command(async () => await RefreshTransactions()); }
        private async Task RefreshTransactions()
        {
            await GetTransactions();
        }

        public ICommand TransactionSelectedCommand { get => new Command(async () => await TransactionSelected()); }
        private async Task TransactionSelected()
        {

        }

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ICommand TradeCommand { get => new Command(async () => await PerformNavigation()); }
        private async Task PerformNavigation()
        {
            await _navigationService.PushAsync<AddTransactionViewModel>();
        }
    }
}
