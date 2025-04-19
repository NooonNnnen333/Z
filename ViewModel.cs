using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Client = Supabase.Client;
using Constants = Supabase.Postgrest.Constants;

namespace AppForVision;

public partial class ViewModelZ : ObservableObject
{
    [ObservableProperty]
    public ObservableCollection<Time> zov;

    [ObservableProperty] public string testTaxt;
    
    public List<string> OptionsForPicker { get; } = new() // Для списка (Picker).
    {
        "Сегодня", 
        "Вчера", 
        "Позавчера"
    };

    [ObservableProperty] public string strokaForPicker;
    
    public DateTime ddd;
    
//---------------------Методы-------------------------------------------------------------------------------------------
    
    public ViewModelZ()
    {
        Zov = new ObservableCollection<Time>();   // Иницилизируем массив для отображения
        LoadBase(); // Иницилизируемся в БД
    }

    [RelayCommand]
    public async Task SelectBaseM()
    {
        if (StrokaForPicker != null)
        {
            TestTaxt = "Загрузка...";
            await SelectBase();
        }
        else if (StrokaForPicker == null)
        {
            await Shell.Current.DisplayAlert("Когда?", "Выберите дату\n" +
                                                       "из выпадающего списка (он снизу кнопки).", "Услышал");
        }
        
    }
    
//=====================Base=============================================================================================    

    private string url = "https://kqzirhgsubllbsynoyep.supabase.co";
    private string key =
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImtxemlyaGdzdWJsbGJzeW5veWVwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQ0NDQ1NzQsImV4cCI6MjA2MDAyMDU3NH0.HYNGd7PWI_e_9ntzefOG03UtyyPQbKfriot_5Us_v14";
    private SupabaseOptions options = new SupabaseOptions
    {
        AutoConnectRealtime = false
    };
    private Client supabase;
    private Time timeClass;
    
//----------------------------------------------------------------------------------------------------------------------    

    public async Task LoadBase() // Подключение к базе данных
    {
        // Присваиваем URL и ключ 
        supabase = new Client(url, key, options);
        supabase.InitializeAsync();

        Console.WriteLine("Инициализация прошла успешно!");
    }

    public async Task SelectBase()
    {
        DateTime data;
        switch (StrokaForPicker)
        {
            case "Сегодня":
                data = DateTime.UtcNow.Date;
                break;
            
            case "Вчера":
                data = DateTime.UtcNow.Date.AddDays(-1);
                break;
            
            case "Позавчера":
                data = DateTime.UtcNow.Date.AddDays(-2);
                break;
            
            default: return;
            
        }



        DateTime dddata = ddd.Date;
        
        var writedInf = await supabase.From<Time>()     // Запрос в БД на Селект
            .Where(x => x.bdate == dddata)
            .Select(x => new object[] { x.bdate, x.btime })
            .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Descending)
            .Limit(10)
            .Get();
        TestTaxt = $"{dddata.ToString()}"; // Для проверки
        
        List<Time> modelsTime = new List<Time>(writedInf.Models);
        
        List<Time> aZov = new List<Time>();
        foreach (var oneModelTime in modelsTime)
        {
            aZov.Add(new Time()
            {
                bdate = oneModelTime.bdate, btime = oneModelTime.btime
            }); // ZOV - массив данных, который отображается
        }

        Zov = new ObservableCollection<Time>(aZov);

    }
    
    
    
    
    
    
    
}

//=====================Classes==========================================================================================

[Table("Time")] // Название таблицы
public class Time : BaseModel // Наследуемся от BaseModel
{
    [PrimaryKey("id", false)]
    public long Id { get; set; }

    [Column("data")]
    public DateTime bdate { get ; set; }
    
    [Column("time")]
    public TimeSpan btime { get ; set;  }
}



public class Vivod()
{
    public string data{ get; set; }
    public string time{ get; set; }
}
