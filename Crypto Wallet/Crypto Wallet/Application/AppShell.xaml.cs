
using Xamarin.Forms;
using Autofac;
using Crypto_Wallet.Modules.AddTransactions;

namespace Crypto_Wallet
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<AppShellViewModel>();

            Routing.RegisterRoute("AddTransactionViewModel", typeof(AddTransactionView));
        }
    }
}
