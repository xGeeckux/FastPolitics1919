using FastPolitics1919.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastPolitics1919.Common
{
    public class BankAccount
    {
        public int Identification { get; set; }

        public Bank Bank { get; set; }

        public GameObject Owner { get; set; }

        public List<Person> RegisterdPersons { get; set; }

        public BankAccount(Bank bank, GameObject owner, int identification)
        {
            Identification = identification;
            Bank = bank;
            Owner = owner;

            RegisterdPersons = new List<Person>();
            if (owner is Person person)
                RegisterdPersons.Add(person);
        }

        public double Money { get; set; }

        //- Methodes
        public void Deposit(double money)
        {
            if (money < 0)
                return;
            Money += money;
        }
        public double Withdraw(double money)
        {
            if (money < 0)
                return 0;
            if (Money < 0)
                return 0;
            if (Money >= money)
            {
                Money -= money;
                return money;
            }
            else
            {
                money = Money;
                Money = 0;
                return money;
            }
        }
        public void TransferTo(int acc_id, double money)
        {
            foreach (BankAccount account in Bank.EveryBankAccount)
            {
                if (account.Identification == acc_id)
                {
                    account.Deposit(Withdraw(money));
                    return;
                }
            }
        }
    }
}
