using MauiAppMinhasCompras.Models;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{

			Produto p = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = (int)Convert.ToDecimal(txt_quantidade.Text),
				Preco = Convert.ToDecimal(txt_preco.Text)
			};

			await App.Db.Insert(p);
			await DisplayAlert("Sucesso!", "Registro Inserido", "OK!");
			await Navigation.PopAsync();


		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}

    }
}