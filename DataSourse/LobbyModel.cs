using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using MP4practic.ModelsDB;
using MP4practic.Views;

namespace MP4practic.DataSourse;

public class LobbyModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private ListProduct _listProduct;

    public ListProduct ListProduct
    {
        get
        {
            return _listProduct;
        }
        set
        {
            _listProduct = value;
            LoadEditPage();
            OnPropertyChanged();
        }
    }
    public void LoadEditPage()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desctop)
        {
            desctop.Windows.OfType<LobbyApp>().First().InputElement_OnTapped(_listProduct.Productarticlenumber);
        }
    }
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
}