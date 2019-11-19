﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CarTracker.Models;
using Xamarin.Forms;
using System.Reflection;
using System.Globalization;
using SQLite;
using System.Linq;
using Color = Xamarin.Forms.Color;

//using CustomCodeAttributes;

namespace CarTracker {
    public partial class YourCarsPage : ContentPage {

        public static ObservableCollection<Car> Cars = new ObservableCollection<Car>();
        public static Dictionary<string, string> SortingAttributes = new Dictionary<string, string>() {
            {"Sort by license plate", "plate"},
            {"Sort by make", "make"},
            {"Sort by model", "model"},
            {"Sort by VIN", "vin"},
            {"Sort by nickname", "name"}
        };

        public YourCarsPage() {
            InitializeComponent();
            PopulateSortingPicker();
            PopulateColorPicker();
            //yourCarsList.ItemsSource = Cars;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using(SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Car>();
                var carsList = conn.Table<Car>().ToList();

                yourCarsList.ItemsSource = carsList;
            }
        }
        //*****For populating from database*****
        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    yourCarsList.ItemsSource = await App.CarDatabase.GetCarAsync();
        //}

        //**********************

        private void PopulateYourCarsList() {

        }

        private void PopulateSortingPicker() {
            var PickerSortingOptions = new List<string>(SortingAttributes.Keys);
            sortPicker.ItemsSource = PickerSortingOptions;
            sortPicker.SelectedItem = PickerSortingOptions[0];
        }

        private void PopulateColorPicker() {
            var colorList = new List<string>(Car.nameToColor.Keys);
            colorPicker.ItemsSource = colorList;
            colorPicker.SelectedItem = colorList[0];
        }

        private void AddNewCarClicked(object sender, System.EventArgs e) {
            popupLoginView.IsVisible = true;
        }
        
        private void ConfirmNewCar(object sender, System.EventArgs e) {

            Car car = new Car()
            {
                plate = plateEntry.Text,
                make = makeEntry.Text,
                model = modelEntry.Text,
                v_color = Car.nameToColor[colorPicker.SelectedItem.ToString()],
                vin = vinEntry.Text,
                name = nameEntry.Text
            };

            using(SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Car>();
                conn.Insert(car);
                var carsList = conn.Table<Car>().ToList();

                yourCarsList.ItemsSource = carsList;
            }

            //using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            //{
            //    conn.CreateTable<Car>();
                
            //}
            popupLoginView.IsVisible = false;
            //Car newCar = new Car(plate.Text, make.Text, model.Text, Car.nameToColor[colorPicker.SelectedItem.ToString()], vin.Text, name.Text);
            //Cars.Add(newCar);
            //popupLoginView.IsVisible = false;
            //ClearEntryFields();
        }

        //*******Storing to DB
        /*
        async void ConfirmNewCar(object sender, System.EventArgs e)
        {
            //Car newCar = new Car(plate.Text, make.Text, model.Text, Car.nameToColor[colorPicker.SelectedItem.ToString()], vin.Text, name.Text);
            //Cars.Add(newCar);
            //popupLoginView.IsVisible = false;
            //testLabel.Text = "Total Cars: " + Cars.Count.ToString();
            //ClearEntryFields();
            if (!string.IsNullOrWhiteSpace(plate.Text))
            {
                await App.CarDatabase.SaveCarAsync(new StoredCarsModel
                {
                    Plate = plate.Text,
                    Make = make.Text,
                    Model = model.Text,
                    V_Color = Car.nameToColor[colorPicker.SelectedItem.ToString()],
                    Vin = vin.Text,
                    Name = name.Text
                });
            }
            popupLoginView.IsVisible = false;
            ClearEntryFields();
            yourCarsList.ItemsSource = await App.CarDatabase.GetCarAsync();
        }*/
        //************


        private void CancelNewCar(object sender, System.EventArgs e) {
            popupLoginView.IsVisible = false;
        }

        private void ClearEntryFields() {
            plateEntry.Text = null;
            makeEntry.Text = null;
            modelEntry.Text = null;
            vinEntry.Text = null;
            nameEntry.Text = null;
        }

        private void SortByOption(object sender, System.EventArgs e) {
            List<Car> tempList = new List<Car>(Cars);
            int minIndex = 0;

            for (int i = 0; i < tempList.Count; i++) {
                minIndex = i;
                for (int unsort = i + 1; unsort < tempList.Count; unsort++) {
                    if (string.Compare(tempList[unsort].GetAttribute(SortingAttributes[sortPicker.SelectedItem.ToString()]), tempList[minIndex].GetAttribute(SortingAttributes[sortPicker.SelectedItem.ToString()])) == -1) {
                        minIndex = unsort;
                    }
                }
                Car tempCar = tempList[minIndex];
                tempList[minIndex] = tempList[i];
                tempList[i] = tempCar;
            }
            Cars = new ObservableCollection<Car>(tempList);
            yourCarsList.ItemsSource = Cars;

        }

    }


    public class PickerBehavior : Behavior<Picker>
    {
        static readonly BindableProperty cellcolor = BindableProperty.Create(nameof(black), typeof(string[]), typeof(PickerBehavior));

        public string[] black = { "black" };
        public string[] blue = { "blue" };
        public string[] gray = { "gray" };
        public string[] aqua = { "aqua" };
        public string[] fucshia = { "fucshia" };
        public string[] green = { "green" };
        public string[] lime = { "lime" };
        public string[] maroon = { "maroon" };
        public string[] olive = { "olive" };
        public string[] purple = { "purple" };
        public string[] red = { "red" };
        public string[] silver = { "silver" };
        public string[] teal = { "teal" };
        public string[] white = { "white" };
        public string[] yellow = { "yellow" };

        protected override void OnAttachedTo(Picker bindable)
        {
            bindable.SelectedIndexChanged += Bindable_SelectedIndexChanged;
        }
        protected override void OnDetachingFrom(Picker bindable)
        {
            bindable.SelectedIndexChanged -= Bindable_SelectedIndexChanged;
        }

        protected void Bindable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (black.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Black;
                ((Picker)sender).TextColor = Color.White;
            }
            else if (blue.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Blue;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (gray.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Gray;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (aqua.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Aqua;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (fucshia.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Fuchsia;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (green.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Green;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (lime.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Lime;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (maroon.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Maroon;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (olive.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Olive;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (purple.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Purple;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (red.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Red;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (silver.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Silver;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (teal.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Teal;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (white.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.White;
                ((Picker)sender).TextColor = Color.Black;
            }
            else if (yellow.Contains(((Picker)sender).SelectedItem.ToString().ToLower()))
            {
                ((Picker)sender).BackgroundColor = Color.Yellow;
                ((Picker)sender).TextColor = Color.Black;
            }
            else
            {
                ((Picker)sender).BackgroundColor = Color.Default;
                ((Picker)sender).TextColor = Color.Default;
            }
        }
    }
}
