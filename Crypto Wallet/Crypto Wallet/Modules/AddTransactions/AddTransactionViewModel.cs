using Crypto_Wallet.Common.Base;
using Crypto_Wallet.Common.Database;
using Crypto_Wallet.Common.Dialog;
using Crypto_Wallet.Common.Models;
using Crypto_Wallet.Common.Validations;
using Crypto_Wallet.Common.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Crypto_Wallet.Modules.AddTransactions
{
    [QueryProperty("Id","id")]
   public class AddTransactionViewModel : BaseViewModel
    {
        private IRepository<Transaction> _repository;
        private IDialogMessage  _dialogMessage;
        private INavigationService _navigationService;

        public AddTransactionViewModel(IRepository<Transaction> repository , IDialogMessage dialogMessage , INavigationService navigationService)
        {
            _repository = repository;
            _dialogMessage = dialogMessage;
            _navigationService = navigationService;
            AvailableAssets = new ObservableCollection<Coin>(Coin.GetAvailableAssets());
            IsDeposit = true;
            TransactionDate = DateTime.Now;
            _amount = new ValidatableObject<decimal>();
            _amount.Validations.Add(new NonNegativeRule { ValidationMessage = "Please Enter Amount Greater Than Zero" });
        }
        private bool _isDeposit;
        public bool IsDeposit
        {
            get => _isDeposit;
            set { SetProperty(ref _isDeposit, value); }
        }

        private string _id;
        public string Id
        {
            set
            {
                _id = Uri.UnescapeDataString(value);
            }
        }

        private ObservableCollection<Coin> _availableAssets;
        public ObservableCollection<Coin> AvailableAssets
        {
            get => _availableAssets;
            set { SetProperty(ref _availableAssets, value); }
        }

        private Coin _selectedCoin;
        public Coin SelectedCoin
        {
            get => _selectedCoin;
            set { SetProperty(ref _selectedCoin, value); }
        }

        private DateTime _transactionDate;
        public DateTime TransactionDate
        {
            get => _transactionDate;
            set { SetProperty(ref _transactionDate, value); }
        }

        private ValidatableObject<decimal> _amount;
        public ValidatableObject<decimal> Amount
        {
            get => _amount;
            set { SetProperty(ref _amount, value); }
        }

        public ICommand AddTransactionCommand { get => new Command(async () => await AddTransaction(), () => IsNotBusy); }
        private async Task AddTransaction()
        {
            _amount.Validate();
            if (!_amount.IsValid)
            {
                return;
            }

            if (SelectedCoin == null)
            {
                await _dialogMessage.DisplayAlert("Error", "Please Select a Coin", "OK");
                return;
            }
            IsBusy = true;

            Transaction transaction = SaveNewTransaction();
            await _repository.SaveAsync(transaction);
            await _navigationService.PopAsync();
            IsBusy = false;
        }

        private Transaction SaveNewTransaction()
        {
            return new Transaction
            {
                Amount = Amount.Value,
                TransactionDate = TransactionDate,
                Symbol = SelectedCoin.Symbol,
                Status = IsDeposit == true ? Constants.TRANSACTION_DEPOSITED : Constants.TRANSACTION_WITHDRAWN
            };
        }
    }
}
