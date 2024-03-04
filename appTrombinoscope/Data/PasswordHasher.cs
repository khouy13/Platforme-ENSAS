using appTrombinoscope.Context;
using appTrombinoscope.Models;
using System.Security.Cryptography;
using System.Text;

public class PasswordHasher
{
    readonly AppDbContext dbContext;

    public PasswordHasher(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void CreateDefaultUser()
    {
        Account account = new Account
        {
            Email = "user@uca.ac.ma",
            UserName = "user",
            Password = "P@ssword123",
            Role = "user"
        };
        if (!dbContext.Accounts.Any(e => e.Email == account.Email))
        {
            Register(account);
        }
    }

    public void CreateDefaultAdmin()
    {
        Account account = new Account
        {
            Email = "admin@uca.ac.ma",
            UserName = "admin",
            Password = "P@ssword123",
            Role = "admin"
        };
        if (!dbContext.Accounts.Any(e => e.Email == account.Email))
        {
            Register(account);
        }
    }

    public bool Register(Account account)
    {
        // Generate a random salt for each user
        string salt = GenerateSalt();

        // Combine the password and salt and then hash
        string hashedPassword = HashPassword(account.Password, salt);

        account.Password = hashedPassword;
        account.Salt = salt;

        try
        {
            dbContext.Accounts.Add(account);
            dbContext.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool Login(Account account)
    {
        try
        {
            var user = dbContext.Accounts.SingleOrDefault(u => u.Email == account.Email);

            if (user != null)
            {
                string hashedPasswordFromDatabase = user.Password;
                string salt = user.Salt;

                // Hash the entered password with the retrieved salt
                string enteredPasswordHash = HashPassword(account.Password, salt);

                // Compare the hashed passwords
                return string.Equals(hashedPasswordFromDatabase, enteredPasswordHash);
            }
            else
            {
                // User not found, login failed
                return false;
            }
        }
        catch (Exception ex)
        {
            // Handle the exception (log, show user-friendly error, etc.)
            return false;
        }
    }

    protected string GenerateSalt()
    {
        // Generate a random salt (you can use a cryptographically secure random number generator)
        // For simplicity, we are using a simple random string generator here
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var saltChars = new char[16];
        for (int i = 0; i < saltChars.Length; i++)
        {
            saltChars[i] = chars[random.Next(chars.Length)];
        }
        return new string(saltChars);
    }

    protected string HashPassword(string password, string salt)
    {
        string combinedPassword = password + salt;

        using (var sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(combinedPassword);

            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("x2"));
            }
            return result.ToString();
        }
    }
}