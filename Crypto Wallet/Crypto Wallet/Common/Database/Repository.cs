﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crypto_Wallet.Common.Extensions;
using System.Linq;

namespace Crypto_Wallet.Common.Database
{
    public interface IDatabaseItem
    {
        int Id { get; set; }
    }

    public abstract class BaseDatabaseItem : IDatabaseItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }

    public interface IRepository<T> where T : IDatabaseItem, new()
    {
        Task<T> GetById(int id);
        Task<int> DeleteAsync(T item);
        Task<List<T>> GetAllAsync();
        Task<int> SaveAsync(T item);
    }

    public class Repository<T> : IRepository<T> where T : IDatabaseItem, new()
    {
        readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DatabaseConstant.DatabasePath, DatabaseConstant.Flags);
        });

        private SQLiteAsyncConnection Database => lazyInitializer.Value;

        public Repository()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
            {
                await Database.CreateTableAsync(typeof(T)).ConfigureAwait(false);
            }
        }

        public Task<T> GetById(int id)
        {
            return Database.Table<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> DeleteAsync(T item)
        {
            return Database.DeleteAsync(item);
        }

        public Task<List<T>> GetAllAsync()
        {
            return Database.Table<T>().ToListAsync();
        }

        public Task<int> SaveAsync(T item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }
    }
}
