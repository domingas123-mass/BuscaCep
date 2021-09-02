
using BuscaCep.Mobile.DTo;
using BuscaCep.Mobile.View_Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace BuscaCep.Mobile
{
    public partial class MainPage : ContentPage
    {
        BuscaCepViewModel viewModel { get => (BuscaCepViewModel)BindingContext;}
        public MainPage()
        {
            InitializeComponent();
            //BindingContext = new BuscaCepViewModel();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.Cep))
                {
                    return;
                }
                using (var cliente = new HttpClient())
                {
                    using (var response = await cliente.GetAsync($"https://viacep.com.br/ws/{viewModel.Cep}/json/"))
                    {
                        response.EnsureSuccessStatusCode();
                        var content = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(content))
                            throw new InvalidOperationException();

                        var ret = JsonConvert.DeserializeObject<BuscaDTO>(content);

                       if(ret.erro)
                            throw new InvalidOperationException();

                        txtLogradouro.Text = ret.logradouro;
                        txtcomplemento.Text = ret.complemento;
                        txtbairro.Text = ret.bairro;
                        txtLocalidade.Text = ret.localidade;
                        txtUF.Text = ret.uf;
                    }


                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("ooops", "nao deu certo", "Ok");
            }
        }

    }


  
}
