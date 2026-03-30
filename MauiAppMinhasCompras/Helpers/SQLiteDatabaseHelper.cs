using MauiAppMinhasCompras.Models;
using SQLite;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;
        public SQLiteDatabaseHelper(string path) 
        { 
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().Wait();
        }
        public Task <int> Insert(Produto p) 
        {
            return _conn.InsertAsync(p);
        }
        public Task<List<Produto>> Update(Produto p)
        {
            //string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=? WHERE Id=?";
            //return _conn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Id);

            string sql = "UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=?, DataCadastro=? WHERE Id=?";
            return _conn.QueryAsync<Produto>(sql, p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.DataCadastro, p.Id);

        }
        public Task<int> Delete(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }
        public Task<List<Produto>> GetAll()
        {
            return _conn.Table<Produto>().ToListAsync();
        }
        public Task<List<Produto>> Search(string q)
        {
            string sql = "SELECT * FROM Produto WHERE descricao LIKE '%" + q + "%'";
            return _conn.QueryAsync<Produto>(sql);
        }


        // NOVO: Filtro por Categoria
        public Task<List<Produto>> SearchByCategoria(string categoria)
        {
            return _conn.Table<Produto>().Where(i => i.Categoria == categoria).ToListAsync();
        }

        // NOVO: Filtro por Período
        public Task<List<Produto>> SearchByPeriodo(DateTime inicio, DateTime fim)
        {
            return _conn.Table<Produto>().Where(i => i.DataCadastro >= inicio && i.DataCadastro <= fim).ToListAsync();
        }
    }
}
