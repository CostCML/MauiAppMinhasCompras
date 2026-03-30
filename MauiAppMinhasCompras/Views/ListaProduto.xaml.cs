using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;
	}


	protected async override void OnAppearing()
	{

		try
		{

            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();
			tmp.ForEach(i => lista.Add(i));


		}

		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "Ok");
		}

	}

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "Ok");
		}
    }

    private async void txt_seach_TextChanged(object sender, TextChangedEventArgs e)
    {


		try
		{
			string q = e.NewTextValue;

			lst_produtos.IsRefreshing = true;

			lista.Clear();

			List<Produto> tmp = await App.Db.Search(q);

			tmp.ForEach(i => lista.Add(i));

		}
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
        finally
        {
            lst_produtos.IsRefreshing = false;

        }

    }

    /*private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {*/
        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                // 1. Mantém a função original: Soma total de todos os itens na lista 
                double somaTotalGeral = (double)lista.Sum(i => i.Total);

                // 2. ADICIONA a nova função do Desafio 1: Agrupamento por Categoria
                var relatorioAgrupado = lista
                    .GroupBy(p => string.IsNullOrEmpty(p.Categoria) ? "Sem Categoria" : p.Categoria)
                    .Select(grupo => new
                    {
                        NomeCategoria = grupo.Key,
                        SomaCategoria = grupo.Sum(p => p.Total)
                    })
                    .ToList();

                // 3. Monta a mensagem combinando as duas funções
                string mensagem = $"SOMA TOTAL: {somaTotalGeral:C}\n";
                mensagem += "\n--- DETALHAMENTO POR CATEGORIA ---\n";

                foreach (var item in relatorioAgrupado)
                {
                    mensagem += $"{item.NomeCategoria}: {item.SomaCategoria:C}\n";
                }

                // 4. Exibe o alerta com o Total Geral + Relatório por Categoria
                DisplayAlert("Relatório de Compras", mensagem, "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", "Erro ao processar: " + ex.Message, "OK");
            }
        }
    
    
        /*double soma = (double)lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");*

      // 1. Mantém a função original: Soma total de todos os itens na lista 
                double somaTotalGeral = (double)lista.Sum(i => i.Total);

                // 2. ADICIONA a nova função do Desafio 1: Agrupamento por Categoria
                var relatorioAgrupado = lista
                    .GroupBy(p => string.IsNullOrEmpty(p.Categoria) ? "Sem Categoria" : p.Categoria)
                    .Select(grupo => new
                    {
                        NomeCategoria = grupo.Key,
                        SomaCategoria = grupo.Sum(p => p.Total)
                    })
                    .ToList();

                // 3. Monta a mensagem combinando as duas funções
                string mensagem = $"SOMA TOTAL: {somaTotalGeral:C}\n";
                mensagem += "\n--- DETALHAMENTO POR CATEGORIA ---\n";

                foreach (var item in relatorioAgrupado)
                {
                    mensagem += $"{item.NomeCategoria}: {item.SomaCategoria:C}\n";
                }

                // 4. Exibe o alerta com o Total Geral + Relatório por Categoria
                DisplayAlert("Relatório de Compras", mensagem, "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", "Erro ao processar: " + ex.Message, "OK");
            }*/

    

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			MenuItem selecionado = sender as MenuItem;

			Produto p = selecionado.BindingContext as Produto;

			bool confirm = await DisplayAlert("Tem Certeza?", $"Remover {p.Descricao}", "Sim", "Não");

			if (confirm) {
				await App.Db.Delete(p.Id);
				lista.Remove(p);
			}
		}
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
		try
		{
			Produto p = e.SelectedItem as Produto;

			Navigation.PushAsync(new Views.EditarProduto()
			{
				BindingContext = p,
			});
		}

        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {

		try
		{
			lista.Clear();

			List<Produto> tmp = await App.Db.GetAll();

			tmp.ForEach(i => lista.Add(i));

		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "Ok");
		}

		finally
		{ 
			lst_produtos.IsRefreshing = false;
		
		}

    }

    private async void pck_filtro_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string selecionado = pck_filtro_categoria.SelectedItem.ToString();
            lista.Clear();

            List<Produto> tmp;

            if (selecionado == "Todos")
                tmp = await App.Db.GetAll();
            else
                tmp = await App.Db.SearchByCategoria(selecionado);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }
}