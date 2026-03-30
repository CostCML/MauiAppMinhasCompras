using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	public EditarProduto()
	{
       InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Produto p)
        {
           dtp_data.Date = p.DataCadastro;
            
            if (!string.IsNullOrEmpty(p.Categoria))
            {
                pck_categoria.SelectedItem = p.Categoria;
            }
        }
    }


    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {

            Produto produto_anexado = BindingContext as Produto;

            Produto p = new Produto
            {
                Id = produto_anexado.Id,
                Descricao = txt_descricao.Text,
                Quantidade = (int)Convert.ToDecimal(txt_quantidade.Text),
                Preco = Convert.ToDecimal(txt_preco.Text),

                // --- NOVAS LINHAS ADICIONADAS ABAIXO ---
               
                Categoria = pck_categoria.SelectedItem?.ToString(),
                DataCadastro = dtp_data.Date
            };

            await App.Db.Update(p);
            await DisplayAlert("Sucesso!", "Registro Atualizado", "OK!");
            await Navigation.PopAsync();


        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}