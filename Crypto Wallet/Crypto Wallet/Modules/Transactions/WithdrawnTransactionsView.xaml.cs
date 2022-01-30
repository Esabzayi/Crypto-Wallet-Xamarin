using Autofac;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crypto_Wallet.Modules.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WithdrawnTransactionsView : ContentPage
    {
        public WithdrawnTransactionsView()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<TransactionViewModel>();
        }
        protected override async void OnAppearing()
        {
            await (BindingContext as TransactionViewModel).InitializeAsync(Constants.TRANSACTION_WITHDRAWN);
        }
    }
}