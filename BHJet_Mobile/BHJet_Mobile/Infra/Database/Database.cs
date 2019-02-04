using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BHJet_Mobile.Infra.Database
{
    public class Database : IDatabase, IDisposable
    {
        public SQLiteAsyncConnection database { get; private set; }
        string diretorioDatabase = string.Empty;

        public Database()
        {
            database = new SQLiteAsyncConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                                            "todosqlib4j3t.db3"));
        }

        public void Dispose()
        {
            database.CloseAsync();
            GC.SuppressFinalize(this);
        }

        public Task<List<T>> BuscaItems<T>(string query) where T : new()
        {
            return database.QueryAsync<T>(query);
        }

        public async Task<T> BuscaPrimeiroItem<T>(string query) where T : new()
        {
            var result = await database.QueryAsync<T>(query);
            return result.FirstOrDefault();
        }

        public Task ExecuteQuery(string query)
        {
            return database.ExecuteAsync(query);
        }

        public Task<List<T>> BuscaItems<T>() where T : new()
        {
            return database.Table<T>().ToListAsync();
        }

        public Task<int> InsereItem<T>(T item)
        {
            return database.InsertAsync(item);
        }

        public Task<int> AlteraItem<T>(T item)
        {
            return database.UpdateAsync(item);
        }

        public Task<int> DeletaItem<T>(T item)
        {
            return database.DeleteAsync(item);
        }

        public Task<int> DropTable<T>(T item) where T : new()
        {
            return database.DropTableAsync<T>();
        }

        public Task<CreateTableResult> CriaTabela<T>() where T : new()
        {
            return database.CreateTableAsync<T>();
        }

        public async Task LimpaTabela(string nome)
        {
            await database.ExecuteAsync($"delete from {nome}");
        }

        public async Task<bool> ExisteTabela<T>() where T : new()
        {
            try
            {
                var item = await BuscaItems<T>();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
