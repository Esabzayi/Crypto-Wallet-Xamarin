
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Reflection;
using Crypto_Wallet;
using Autofac;

namespace Crypto_Wallet
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = App.Container.Resolve<AppShellViewModel>();
        }
    }
}
