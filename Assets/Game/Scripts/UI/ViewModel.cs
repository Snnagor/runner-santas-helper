using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ViewModel : MonoBehaviour, INotifyPropertyChanged
{    
    private string ducks = "0";
    
    public event PropertyChangedEventHandler PropertyChanged;   

    [Binding]
    public string Ducks
    { 
        get => ducks;
        set 
        {
            if (ducks.Equals(value)) return;

            ducks = value;
            OnPropertyChanged("Ducks");
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
