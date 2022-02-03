using System;
using System.IO;


namespace Crypto_Wallet.Common.Database
{
    public static class DatabaseConstant
    {
        public const string DatabaseFilename = "AppSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            // Open Database in Read Write Mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // Create The Database if it doesn't Exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
