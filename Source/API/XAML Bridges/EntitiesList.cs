﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Control.Source.API.XAML_Bridges
{
    public class EntitiesList<T>: INotifyPropertyChanged
    {
        ObservableCollection<T> _entities;
        public ObservableCollection<T> Entities
        {
            get => _entities;
            set
            {
                _entities = value;
                // This will make it so `myData.Previews = ...` will fire a changed notification
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(T)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
