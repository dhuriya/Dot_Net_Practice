using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.Encapuslation
{
    public class BankAccount
    {
        private decimal balance;
        public decimal Balance
        {
            get { return balance; }
        }
        public BankAccount(decimal initialBalance)
        {
            if (initialBalance < 0)
            {
                throw new ArgumentException("Initial balance cannot be negative.");
            }
            balance = initialBalance;
        }
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive.");
            }
            balance += amount;
        }
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive.");
            }
            if (balance - amount < 0)
            {
                throw new InvalidOperationException("Insufficient funds.");
            }
            balance -= amount;
        }
    }
}