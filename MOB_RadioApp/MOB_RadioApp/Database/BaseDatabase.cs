﻿using Polly;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MOB_RadioApp.Database
{
    // Not my code, taken from https://codetraveler.io/2019/11/26/efficiently-initializing-sqlite-database/
    public abstract class BaseDatabase
    {
        static readonly string _databasePath = Path.Combine(FileSystem.AppDataDirectory, "SqliteDatabase.db3");
        static readonly Lazy<SQLiteAsyncConnection> _databaseConnectionHolder = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(_databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));

        static SQLiteAsyncConnection DatabaseConnection => _databaseConnectionHolder.Value;

        protected static async ValueTask<SQLiteAsyncConnection> GetDatabaseConnection<T>()
        {
            if (!DatabaseConnection.TableMappings.Any(x => x.MappedType == typeof(T)))
            {
                // On sqlite-net v1.6.0+, enabling write-ahead logging allows for faster database execution
                await DatabaseConnection.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
                await DatabaseConnection.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
            }

            return DatabaseConnection;
        }

        protected static Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 10)
        {
            return Policy.Handle<SQLite.SQLiteException>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);
        }
        static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
    }
}
