using System;
using System.Collections.Generic;
using System.Text;

namespace Crypto_Wallet.Common.Validations
{
    class NonNegativeRule : IValidationRule<decimal>
    {
        public string ValidationMessage { get; set; }

        public bool Check(decimal value)
        {
            return value > 0;
        }
    }
}
