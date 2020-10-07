using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using tthk_app.Models;

namespace tthk_app
{
    public class ChangeDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public ChangeDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Change).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Change)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task ClearTable()
        {
            return Database.QueryAsync<Change>("DELETE FROM [Change]");
        }

        public Task<List<Change>> GetItemsAsync()
        {
            return Database.Table<Change>().ToListAsync();
        }

        public Task<List<Change>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<Change>("SELECT * FROM [Change] WHERE [Done] = 0");
        }

        public Task<Change> GetItemAsync(int id)
        {
            return Database.Table<Change>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Change item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Change item)
        {
            return Database.DeleteAsync(item);
        }
    }
}