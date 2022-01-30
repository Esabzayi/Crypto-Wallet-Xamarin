using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Crypto_Wallet
{
    public class AppShellViewModel
    {
        public ICommand SignOutCommand { get => new Command(async () => await SignOut()); }
         
        private async Task SignOut()
        {
            await Shell.Current.DisplayAlert("Todo", "you have been logged out","OK");
        }
    }
   
}
