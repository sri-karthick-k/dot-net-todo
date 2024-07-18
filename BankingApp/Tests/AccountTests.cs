using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Tests
{
    public class AccountTests
    {
        public static void Account_DepositBalance_ReturnsDecimal()
        {
            try
            {
                // Arrange (Get variables, classes and objects ready)
                decimal depositAmt = 100m;
                Account account = new Account();

                // Act (Execute the method)
                account.Deposit(depositAmt);
                decimal balance = account.GetBalance();

                // Assert
                if(depositAmt == balance)
                {
                    Console.WriteLine("PASSED: Account.GetBalance()");
                }
                else
                {
                    Console.WriteLine("FAILED: Account.GetBalance()");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
