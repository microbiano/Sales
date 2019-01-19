﻿namespace Sales.ViewModels
{
   
   
    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Command;
    using Sales.Views;

    public class MainViewModel
    {

        public ProductsViewModel Products { get; set; }

        public AddProductViewModel AddProduct { get; set; }

        public MainViewModel() {
           this.Products = new ProductsViewModel();
        }

    public ICommand AddProductCommand {

            get { return new RelayCommand(goToAddProduct); }
     }

        private async void goToAddProduct()
        {
            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
    }
}
