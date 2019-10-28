﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using CarTracker.Models;

namespace CarTracker
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CarServicePage : ContentPage
    {
        public static ObservableCollection<Service> Services = new ObservableCollection<Service>();

        public CarServicePage()
        {
            InitializeComponent();
            yourCarsList.ItemsSource = Services;
        }

        private void AddNewCarClicked(object sender, System.EventArgs e)
        {
            ServiceView.IsVisible = true;
        }

        private void ConfirmNewName(object sender, System.EventArgs e)
        {
            Service newService = new Service(date.Text, Convert.ToInt32(millage.Text), location.Text, description.Text, car.Text);
            Services.Add(newService);
            ServiceView.IsVisible = false;
            testLabel.Text = "Total Service: " + Services.Count.ToString();
            ClearEntryFields();
        }

        private void ClearEntryFields()
        {
            date.Text = null;
            millage.Text = null;
            location.Text = null;
            description.Text = null;
            car.Text = null;
        }

        private void OnSortClicked(object sender, System.EventArgs e)
        {
          
        }

    }

   

}