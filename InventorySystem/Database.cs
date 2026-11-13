using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;

namespace InventorySystem;

internal static class Database
{
    private static readonly string Folder =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "InventorySystem");
    private static readonly string DbPath = Path.Combine(Folder, "inventory.db");
    private static string ConnectionString => $"Data Source={DbPath}";

    public static SqliteConnection Open()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    public static void Initialize()
    {
        Directory.CreateDirectory(Folder);
        using var con = Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ProductName TEXT NOT NULL,
                Description TEXT,
                UnitPrice REAL NOT NULL,
                Quantity INTEGER NOT NULL,
                ReorderLevel INTEGER NOT NULL
            );";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "SELECT COUNT(*) FROM Users;";
        long users = (long)cmd.ExecuteScalar()!;
        if (users == 0)
        {
            cmd.CommandText = "INSERT INTO Users (Username, PasswordHash) VALUES ($u, $p);";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("$u", "admin");
            cmd.Parameters.AddWithValue("$p", Hash("admin123"));
            cmd.ExecuteNonQuery();
        }
    }

    public static string Hash(string value)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));
    }

    public static bool Login(string username, string password)
    {
        using var con = Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username=$u AND PasswordHash=$p;";
        cmd.Parameters.AddWithValue("$u", username);
        cmd.Parameters.AddWithValue("$p", Hash(password));
        return Convert.ToInt64(cmd.ExecuteScalar()) > 0;
    }

    public static bool Register(string username, string password)
    {
        try
        {
            using var con = Open();
            using var cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO Users (Username, PasswordHash) VALUES ($u, $p);";
            cmd.Parameters.AddWithValue("$u", username);
            cmd.Parameters.AddWithValue("$p", Hash(password));
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (SqliteException)
        {
            return false;
        }
    }
}
