using Salary_Control.Source.API.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salary_Control.Source.API.XAML_Bridges
{
    public class EventsList : INotifyPropertyChanged
    {
        ObservableCollection<Event> _events;
        public ObservableCollection<Event> Events
        {
            get => _events;
            set
            {
                _events = value;
                // This will make it so `myData.Previews = ...` will fire a changed notification
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Events)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
