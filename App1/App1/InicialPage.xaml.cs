using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicialPage : ContentPage
    {
        List<Evento> eventos = new List<Evento>();
        Categoria categoriaSelecionada = new Categoria();
        Evento eventoSelecionado = new Evento();
        List<Categoria> categorias = new List<Categoria>();
        List<CategoriaEvento> categoriaEventos = new List<CategoriaEvento>();
        public InicialPage()
        {
            InitializeComponent();
            GetData();
        }

        public async void GetData()
        {

            categorias = await requester.get<List<Categoria>>("categorias");
            eventos = await requester.get<List<Evento>>("eventoes");
            categoriaEventos = await requester.get<List<CategoriaEvento>>("categoriaeventoes");

            cvCategorias.ItemsSource = categorias;
            cvEventos.ItemsSource = eventos;


        }

        private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
        {
            Label label = (sender as Element).Parent as Label;
            categoriaSelecionada = categorias.Where(x => x.nome == label.Text).First();
        }

        private async void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
        {

            StackLayout sl = (sender as Element).Parent as StackLayout;
            Label label = sl.Children[0] as Label;

            eventoSelecionado = eventos.Where(x => x.nome == label.Text).First();
            var ligacao = eventoSelecionado.CategoriaEvento.Where(x => x.categoriaId == categoriaSelecionada.id).FirstOrDefault();

            if (ligacao == null)
            {
                var categoriaEvento = new CategoriaEvento();

                categoriaEvento.categoriaId = categoriaSelecionada.id;
                categoriaEvento.eventoId = eventoSelecionado.id;

                await requester.post("categoriaeventoes", categoriaEvento);

                await DisplayAlert("Alerta", "Categoria Associada com sucesso!", "OK");

            }
            else
            {
                await DisplayAlert("Alerta", "Categoria já Associada com esse evento", "OK");
            }

            GetData();

        }

        private void cvEventos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Evento evento = e.CurrentSelection[0] as Evento;

            Navigation.PushAsync(new EditarEvento(evento));
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CadastroEvento());
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var lista = eventos.Where(x => x.nome.ToLower().Contains(txbFiltro.Text.ToLower())).ToList();

            cvEventos.ItemsSource = lista;
        }

        private async void DropGestureRecognizer_Drop_1(object sender, DropEventArgs e)
        {
            

        }

        private void DragGestureRecognizer_DragStarting_1(object sender, DragStartingEventArgs e)
        {
            StackLayout sl = (sender as Element).Parent as StackLayout;

            Label label = sl.Children[0] as Label;

            eventoSelecionado = eventos.Where(x => x.nome == label.Text).FirstOrDefault();
        }

        private async void DropGestureRecognizer_Drop_2(object sender, DropEventArgs e)
        {
            bool res = await DisplayAlert("Alerta", "Deseja mesmo realizar essa operação?", "Sim", "Não");
            if (res)
            {
                await requester.delete($"eventoes/{eventoSelecionado.id}");
                GetData();
            }
        }
    }
}