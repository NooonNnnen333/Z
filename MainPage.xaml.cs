using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppForVision;

public partial class MainPage : ContentPage
{

    public ViewModelZ vm = new ViewModelZ();
    public MainPage()
    {
        InitializeComponent();
        BindingContext = vm;
        MyDatePicker.DateSelected += OnDateSelected;
    }

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        vm.ddd = e.NewDate;
    }


    
}