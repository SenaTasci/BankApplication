using BankApplication.Domain;
using BankApplication.Domain.Entities;

namespace BankApplication
{
    class Program
    {
        static AccountService accountService = new AccountService();
        static TransferMoneyService transferService = new TransferMoneyService(accountService);
        static Account currentAccount = null;

        static void Main(string[] args)
        {
            bool running = true;

            while (running)
            {
                if (currentAccount == null)
                {
                    Console.WriteLine("Bankacılık Uygulamasına Hoş Geldiniz!");
                    Console.WriteLine(" ");
                    Console.WriteLine("Lütfen Yapmak İstediğiniz İşlemi Giriniz: ");
                    Console.WriteLine("1. Hesap Oluştur");
                    Console.WriteLine("2. Hesaba Giriş");
                    Console.WriteLine("3. Çıkış");
                    Console.Write("Lütfen bir seçenek girin: ");
                    string initialChoice = Console.ReadLine();

                    if (initialChoice == "1")
                    {
                        CreateAccount();
                    }
                    else if (initialChoice == "2")
                    {
                        Login();
                    }
                    else if (initialChoice == "3")
                    {
                        running = false;
                        Console.WriteLine("Çıkış yapılıyor...");
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz seçenek, lütfen tekrar deneyin.");
                    }
                }
                else
                {
                    Console.WriteLine(" ");
                    Console.WriteLine("Lütfen Yapmak İstediğiniz İşlemi Giriniz: ");
                    Console.WriteLine("1. Bakiye Sorgulama");
                    Console.WriteLine("2. Para Yatırma");
                    Console.WriteLine("3. Para Çekme");
                    Console.WriteLine("4. Para Gönderme");
                    Console.WriteLine("5. Çıkış");
                    Console.Write("Lütfen bir seçenek girin: ");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        CheckBalance();
                    }
                    else if (choice == "2")
                    {
                        DepositMoney();
                    }
                    else if (choice == "3")
                    {
                        WithdrawMoney();
                    }
                    else if (choice == "4")
                    {
                        TransferMoney();
                    }
                    else if (choice == "5")
                    {
                        currentAccount = null;
                        Console.WriteLine("Çıkış yapılıyor...");
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz seçenek, lütfen tekrar deneyin.");
                    }
                }

                Console.WriteLine();
            }
        }

        static void CreateAccount()
        {
            Console.Write("Adınızı girin: ");
            string name = Console.ReadLine();

            Console.Write("Soyadınızı girin: ");
            string surname = Console.ReadLine();

            Console.Write("E-posta adresinizi girin: ");
            string email = Console.ReadLine();

            Console.Write("Şifrenizi oluşturun: ");
            string password = Console.ReadLine();
            Console.WriteLine("Şifreniz başarıyla oluşturuldu!");

            Console.Write("Telefon numaranızı girin: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Kimlik numaranızı girin: ");
            string identityNumber = Console.ReadLine();

            Account newAccount = accountService.CreateAccount(name, surname, email, password, phoneNumber, identityNumber);
            Console.WriteLine($"Hesap başarıyla oluşturuldu! Hesap Numarası: {newAccount.AccountNumber}");
        }

        static void Login()
        {
            Console.Write("E-posta adresinizi girin: ");
            string email = Console.ReadLine();

            Console.Write("Şifrenizi girin: ");
            string password = Console.ReadLine();

            currentAccount = accountService.Login(email, password);
            if (currentAccount != null)
            {
                Console.WriteLine($"Giriş başarılı! Hoş geldiniz, {currentAccount.Name} {currentAccount.Surname}");
            }
            else
            {
                Console.WriteLine("Geçersiz e-posta veya şifre.");
            }
        }

        static void CheckBalance()
        {
            if (currentAccount != null)
            {
                Console.WriteLine($"Hesap Numarası: {currentAccount.AccountNumber}, Bakiye: {currentAccount.Balance} TL");
            }
            else
            {
                Console.WriteLine("Giriş yapmanız gerekmektedir.");
            }
        }

        static void DepositMoney()
        {
            if (currentAccount != null)
            {
                Console.Write("Yatırmak istediğiniz miktarı girin: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    if (accountService.DepositMoney(amount))
                    {
                        Console.WriteLine($"Yeni bakiye: {currentAccount.Balance} TL");
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz miktar.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz miktar.");
                }
            }
            else
            {
                Console.WriteLine("Giriş yapmanız gerekmektedir.");
            }
        }

        static void WithdrawMoney()
        {
            if (currentAccount != null)
            {
                Console.Write("Çekmek istediğiniz miktarı girin: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    if (accountService.WithdrawMoney(currentAccount, amount))
                    {
                        Console.WriteLine($"Yeni bakiye: {currentAccount.Balance} TL");
                    }
                    else
                    {
                        Console.WriteLine("Yetersiz bakiye.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz miktar.");
                }
            }
            else
            {
                Console.WriteLine("Giriş yapmanız gerekmektedir.");
            }
        }

        static void TransferMoney()
        {
            if (currentAccount != null)
            {
                Console.Write("Alıcı hesap numarasını girin: ");
                string receiverAccountNumber = Console.ReadLine();



                Console.Write("Transfer etmek istediğiniz miktarı girin: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    if (transferService.TransferMoney(currentAccount.AccountNumber, receiverAccountNumber, amount))
                    {
                        Console.WriteLine($"Transfer başarılı! Gönderen yeni bakiye: {currentAccount.Balance} TL");
                    }
                    else
                    {
                        Console.WriteLine("Yetersiz bakiye veya geçersiz alıcı hesap numarası.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz miktar.");
                }
            }
            else
            {
                Console.WriteLine("Giriş yapmanız gerekmektedir.");
            }

        }
    }
}
