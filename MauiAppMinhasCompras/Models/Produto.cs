using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {

        string _descricao;

        decimal _preco;

        [PrimaryKey, AutoIncrement]

        public int Id { get; set; }
        public string Descricao { get => _descricao;
            set
            {
                if (value == null) {
                    throw new Exception("Por favor, preencha a descrição");
                
                }
                _descricao = value;
            }
        
        }
        public int Quantidade { get; set; }
        public decimal Preco
        {
            get => _preco;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("O preço deve ser maior que zero");
                }

                _preco = value;
            }
        }

        public decimal Total => Quantidade * Preco;
    }
}
