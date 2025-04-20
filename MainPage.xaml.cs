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
        if (e != null)
        {
            vm.CalendDate = e.NewDate;
            vm.SelectBase();
        }
        else
        {
            vm.CalendDate = DateTime.UtcNow;
            vm.SelectBase();
        }
    }


    
}