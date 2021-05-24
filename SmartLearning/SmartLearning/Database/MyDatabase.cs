using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace SmartLearning
{
    public class MyDatabase
    {
        readonly SQLiteAsyncConnection database;

        public MyDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Result>().Wait();

        }

        public Task SaveItemAsync(Result result)
        {

            if (result.ID != 0)
            {
                return database.UpdateAsync(result);
            }
            else
            {
                return database.InsertAsync(result);

            }
        }

        public Task<List<Result>> getIetmsAsync()
        {

            return database.Table<Result>().ToListAsync();

        }
        public Task UpdateRow( Result result)
        {
            var a= database.QueryAsync<Result>("update Result where topicName = ?", result.topicName);
            Debug.Write(a.Result, "Update status");
            return a;
        }
        public Task DeleteRow(Result result)
        {
            return database.DeleteAsync(result);
        }

        public Task DeleteItemAsync()
        {
            return database.ExecuteAsync("DELETE FROM Result");

        }

    }

}
