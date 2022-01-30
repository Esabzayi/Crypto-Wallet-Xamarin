using Autofac;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Crypto_Wallet.Modules.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionsView : ContentPage
    {
        public TransactionsView()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<TransactionViewModel>();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
           await (BindingContext as TransactionViewModel).InitializeAsync("");
        }
    }
}