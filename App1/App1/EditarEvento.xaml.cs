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
    public partial class EditarEvento : ContentPage
    {
        Evento evento;
        List<Categoria> categorias = new List<Categoria>();
        Categoria categoriaSelecionada = new Categoria();

        public EditarEvento(Evento evento)
        {
            InitializeComponent();
            this.evento = evento;
            GetData();

            txbNome.Text = evento.nome;
            txbValor.Text = evento.valor.ToString();
            txbData.Date = evento.data.Value;

        }

        public void GetData()
        {
            evento.CategoriaEvento.ToList().ForEach(x => categorias.Add(x.Categoria));
            cvCategorias.ItemsSource = categorias;
        }

        private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
        {
            Label label = (sender as Element).Parent as Label;
            categoriaSelecionada = categorias.Where(x => x.nome == label.Text).First();
        }

        private async void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
        {
            bool resposta = await DisplayAlert("Alerta", "Deseja mesmo realizar essa operação?", "Sim", "Não");

            if(resposta)
            {
                var categoriaEvento = evento.CategoriaEvento.Where(x => x.categoriaId == categoriaSelecionada.id).FirstOrDefault();

                await requester.delete($"categoriaEventoes/{categoriaEvento.id}");

                evento.CategoriaEvento.Remove(categoriaEvento);

                cvCategorias.ItemsSource = null;
                categorias.Clear();

                GetData();

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (txbNome.Text == "" || txbNome.Text == "")
            {
                await DisplayAlert("Alerta", "Todos os campos devem ser preenchidos", "Ok");
            }
            else
            {
                var eventoNome = await requester.get<List<Evento>>("eventoes");
                if(eventoNome.Where(x=>x.nome == txbNome.Text).FirstOrDefault() == null)
                {
                    evento.nome = txbNome.Text;
                    evento.data = txbData.Date;
                    evento.valor= Convert.ToDecimal(txbValor.Text);

                    await requester.put($"eventoes/{evento.id}", evento);

                    await Navigation.PushAsync(new InicialPage());
                }
                else
                {
                    await DisplayAlert("Alerta", "Já existe um evento com este nome", "Ok");
                }
            }
        }
    }
}