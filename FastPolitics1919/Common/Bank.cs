using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class Bank : MapObject
    {
        public static int Index = 0;
        
        //- Constructor
        public Bank(string name)
        {
            Index++;
            ID = Index;

            Name = name;
        }

        //- Money
        public double Worth
        {
            get
            {
                double local = GoldWorth;

                foreach (BankAccount account in EveryBankAccount)
                    local += account.Money;

                return local;
            }
        }
        private double GoldWorth => GoldQuanity * 250; //-  Change to Market later
        public int GoldQuanity { get; set; }

        //- Accounts
        public List<BankAccount> EveryBankAccount = new List<BankAccount>();
    }
}
