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
    public partial class CadastroEvento : ContentPage
    {
        public CadastroEvento()
        {
            InitializeComponent();

            txbNome.Text = "";
            txbValor.Text = "";
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (txbNome.Text == "" || txbValor.Text == "")
            {
                await DisplayAlert("Alerta", "Todos os campos devem ser preenchidos", "Ok");
            }
            else
            {
                var eventoNome = await requester.get<List<Evento>>("eventoes");
                if (eventoNome.Where(x => x.nome.ToLower() == txbNome.Text.ToLower()).FirstOrDefault() == null)
                {
                    var evento = new Evento();
                    evento.nome = txbNome.Text;
                    evento.data = txbData.Date;
                    evento.valor = Convert.ToDecimal(txbValor.Text);

                    await requester.post($"eventoes", evento);

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