using MOB_RadioApp.Database;
using MOB_RadioApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MOB_RadioApp.Services
{
    public class SqlLiteService : BaseDatabase
    {
        //static SQLiteAsyncConnection _db;
        //static async Task InitAsync()
        //{
        //    if (_db != null)
        //        return;
        //    string databasePath = Path.Combine(FileSystem.AppDataDirectory, "RadioApp.db3");
        //    _db = new SQLiteAsyncConnection(databasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        //    if (!_db.TableMappings.Any(x => x.MappedType == typeof(SqlStation)))
        //    {
        //        await _db.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
        //        await _db.CreateTablesAsync(CreateFlags.None, typeof(SqlStation)).ConfigureAwait(false);

        //    }
        //}

        public static async Task AddFavourite(Station s)
        {
            var databaseConnection = await GetDatabaseConnection<SqlStation>().ConfigureAwait(false);
            SqlStation stat = new SqlStation(s.StationId);
            await databaseConnection.InsertOrReplaceAsync(stat).ConfigureAwait(false);
            
        }
        public static async Task RemoveFavourite(Station s )
        {
            var databaseConnection = await GetDatabaseConnection<SqlStation>().ConfigureAwait(false);
            
            SqlStation stat = new SqlStation(s.StationId);
            await databaseConnection.DeleteAsync(stat);
        }
        public static async Task<List<string>> GetFavourites()
        {
            
            var databaseConnection = await GetDatabaseConnection<SqlStation>().ConfigureAwait(false);
            var listStations = await AttemptAndRetry(() => databaseConnection.Table<SqlStation>().ToListAsync()).ConfigureAwait(false);
            //var list = await _db.Table<SqlStation>().ToListAsync();
            List<string> favourites = new List<string>();
            foreach (var stat in listStations)
            {
                favourites.Add(stat.Id);
            }
            return favourites;

        }
    }
}
