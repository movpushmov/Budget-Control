using Salary_Control.Source.API.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Salary_Control.Source.API.XAML_Bridges
{
    public class CategoriesList : INotifyPropertyChanged
    {
        ObservableCollection<EventCategory> _categories;
        public ObservableCollection<EventCategory> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                // This will make it so `myData.Previews = ...` will fire a changed notification
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Categories)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
