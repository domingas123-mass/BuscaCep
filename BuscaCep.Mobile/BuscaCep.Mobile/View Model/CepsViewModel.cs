using BuscaCep.Mobile.DTo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BuscaCep.Mobile.View_Model
{
    class CepsViewModel : ViewModelBase
    {
        private string _cep;

        public bool HasCep { get => !(_buscaDTO is null); }
        public bool Isen { get => !isNotBusy; }


        private BuscaDTO _buscaDTO=null;
        public string Cep
        {
            get => _cep;
            set
            {
                _cep = value;
                OnPropertyChanged();
                BuscarComando.ChangeCanExecute();
            }
        }
        //public string Lograd{ get => _buscaDTO?.logradouro;}
        //public string Complem { get => _buscaDTO?.complemento; }
        //public string Bairro { get => _buscaDTO?.bairro; }

        //public string Locali { get => _buscaDTO?.localidade; }
        //public string Uf { get => _buscaDTO?.uf; }


        private Command _buscarComand;

        public Command BuscarComando
        {
            get
            {
                if (_buscarComand is null)
                    _buscarComand = new Command(async()=> await BuscarComangoExecute(), 
                       () => BuscarComangoCanExecute());
                return _buscarComand;
            }
        }

        private bool BuscarComangoCanExecute()
     => !string.IsNullOrWhiteSpace(Cep)
            && Cep.Length >= 8 && isNotBusy;
               

        private async Task BuscarComangoExecute()
        {
            try
            {
                
                IsVsy = true;

                 if( MobileDatabaseservice.Current.Get<BuscaDTO>(x=>x.cep.Equals(Regex.Replace(Cep, @"[^\d]", string.Empty))).Any())
                {
                    await App.Current.MainPage.DisplayAlert("ooops", "nao deu certo", "Ok");
                    return;
                   
                }
                BuscarComando.ChangeCanExecute();
                using (var cliente = new HttpClient())
                {
                    using (var response = await cliente.GetAsync($"https://viacep.com.br/ws/{Cep}/json/"))
                    {
                        response.EnsureSuccessStatusCode();
                        var content = await response.Content.ReadAsStringAsync();

                        if (string.IsNullOrWhiteSpace(content))
                            throw new InvalidOperationException();

                      _buscaDTO = JsonConvert.DeserializeObject<BuscaDTO>(content);

                        if (_buscaDTO.erro)
                            throw new InvalidOperationException();

                        MobileDatabaseservice.Current.Save(_buscaDTO);

                        RefreshComando.Execute(true);
                      
                       
                    }


                }
            }
            catch (Exception ex)
            {
                _buscaDTO = null;
                await App.Current.MainPage.DisplayAlert("ooops", "nao deu certo", "Ok");
            }
            finally
            {
              
                IsVsy = false;
                BuscarComando.ChangeCanExecute();
            }
        }

        public ObservableCollection<BuscaDTO> Ceps { get; private set; } = new ObservableCollection<BuscaDTO>();

        private Command _RefreshComand;

        public Command RefreshComando
       
           
            => _RefreshComand 
                   ??( _RefreshComand = new Command<bool>
            (async (args) => await RefreshComangoExecute(args),
                       (args) => RefreshComangoCanExecute()));
               
          private bool RefreshComangoCanExecute()
     =>  isNotBusy;


        private async Task RefreshComangoExecute(bool force=false)
        {
            try
            {
                if (!force && IsVsy)
                    return;
                IsVsy = true;
                BuscarComando.ChangeCanExecute();
                await Task.Factory.StartNew(() =>
                {
                    foreach (var cep in MobileDatabaseservice.Current.Get<BuscaDTO>())
                    {
                        Ceps.Add(cep);
                    }


                });

              
            }
            catch (Exception ex)
            {
                _buscaDTO = null;
                await App.Current.MainPage.DisplayAlert("ooops", "nao deu certo", "Ok");
            }
            finally
            {

                IsVsy = false;
                BuscarComando.ChangeCanExecute();
            }
        }
    }
}
