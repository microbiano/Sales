namespace Sales.ViewModels
{

    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Sales.Helper;
    using Services;
    using Sales.Common.Models;

    public class AddProductViewModel : BaseViewModel
    {
        #region Attributes

        private ApiService apiService;

        private bool isRunning;
        private bool isEnabled;

        #endregion

        #region Properties

        public string Description { get; set; }

        public string Price { get; set; }

        public string Remarks { get; set; }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        #endregion

        #region Constructors

        public AddProductViewModel()
        {
            this.apiService = new ApiService();
            isEnabled = true;

        }


        #endregion


        #region Command

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DescriptionError,
                    Languages.Accept);
                return;
            }
            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            var price = decimal.Parse(this.Price);
            if (price < 0)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PriceError,
                    Languages.Accept);
                return;
            }

            this.isRunning = true;
            this.isEnabled = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.isRunning = false;
                this.isEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        connection.Message,
                        Languages.Accept);
                return;
            }

            var product = new Product
            {

                Description = this.Description,
                Price = price,
                Remarks = this.Remarks,

            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["UrlPrefix"].ToString();
            var controller = Application.Current.Resources["UrlProductsController"].ToString();

            var response = await this.apiService.Post<Product>(url, prefix, controller, product);
            if (!response.IsSuccess)
            {
                this.isRunning = false;
                this.isEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                Languages.Error,
                response.Message,
                Languages.Accept);
                return;
            }

            this.isRunning = false;
            this.isEnabled = true;
            await Application.Current.MainPage.Navigation.PopAsync();

            //Tod:Tutorial 56 - Parte 24 - Post desde la ViewModel
        }



        #endregion


    }

    //Todo:Tutorial 56 - Parte 20 - Products Page
    //Todo:Tutorial 56 - Parte 21 - Product Page
}
