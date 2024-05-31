namespace BankApplication.Domain;

using global::BankApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;



public class AccountService
{
    private List<Account> accounts = new List<Account>();
    private Account loggedInAccount = null;

    public Account CreateAccount(string name, string surname, string email, string password, string phoneNumber, string identityNumber)
    {
        Account newAccount = new Account
        {
            Id = Guid.NewGuid(),
            Created = DateTime.Now,
            Updated = DateTime.Now,
            Name = name,
            Surname = surname,
            Email = email,
            Password = password,
            PhoneNumber = phoneNumber,
            IdentityNumber = identityNumber,
            Balance = "0",
            AccountNumber = GenerateAccountNumber()
        };

        accounts.Add(newAccount);
        return newAccount;
    }

    public Account Login(string email, string password)
    {
        return accounts.SingleOrDefault(a => a.Email == email && a.Password == password);
    }

    public Account GetAccountByNumber(string accountNumber)
    {
        return accounts.SingleOrDefault(a => a.AccountNumber == accountNumber);
    }

    public bool DepositMoney(decimal amount)
    {
        if (amount > 0)
        {
            decimal currentBalance = decimal.Parse(loggedInAccount.Balance);
            loggedInAccount.Balance = (currentBalance + amount).ToString();
            loggedInAccount.Updated = DateTime.Now;
            return true;
        }
        return false;
    }

    public bool WithdrawMoney(Account account, decimal amount)
    {

        if (amount > 0)
        {
            decimal currentBalance = decimal.Parse(account.Balance);
            if (currentBalance >= amount)
            {
                account.Balance = (currentBalance - amount).ToString();
                account.Updated = DateTime.Now;
                return true;
            }
        }
        return false;
    }

    public void Transfer(string recipientAccountNumber, decimal amount)
    {
        Account receiverAccount = accounts.Find(acc => acc.AccountNumber == recipientAccountNumber);
        if (receiverAccount == null)
        {
            Console.WriteLine("Recipient account not found.");
            return;
        }

        // Para transferi yap
        if (WithdrawMoney(receiverAccount, amount))
        {
            DepositMoney(amount);
            Console.WriteLine($"Transferred {amount:C} to {receiverAccount.Name}'s account ({receiverAccount.AccountNumber}).");
        }
        else
        {
            Console.WriteLine("Transfer failed.");
        }

        
    }

    public Account GetAccountByNumbers(string accountNumber)
    {
        foreach (Account account in accounts)
        {
            if (account.AccountNumber == accountNumber)
            {
                return account;
            }
        }
        return null;
    }

    public string GenerateAccountNumber()
    {
        // Hesap numarası oluşturma işlemi
        return Guid.NewGuid().ToString().Substring(0, 10); // Örnek hesap numarası
    }
}


