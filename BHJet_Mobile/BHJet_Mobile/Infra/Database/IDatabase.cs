using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BHJet_Mobile.Infra.Database
{
    public interface IDatabase
    {
        SQLiteAsyncConnection database { get; }

        Task<List<T>> BuscaItems<T>(string query) where T : new();

        Task ExecuteQuery(string query);

        Task<List<T>> BuscaItems<T>() where T : new();

        Task<T> BuscaPrimeiroItem<T>(string query) where T : new();

        Task<int> InsereItem<T>(T item);

        Task<int> AlteraItem<T>(T item);

        Task<int> DeletaItem<T>(T item);

        Task<int> DropTable<T>(T item) where T : new();

        Task<CreateTableResult> CriaTabela<T>() where T : new();

        Task<bool> ExisteTabela(string nomeTabela);
    }
}
