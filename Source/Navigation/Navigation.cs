using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Salary_Control.Source.Navigation
{
    static class Navigation
    {
        private static Frame _frame;

        public static void Init(Frame frame)
        {
            _frame = frame;
        }

        public static void Navigate(Type pageType, object parameters = null)
        {
            if (_frame != null)
            {
                _frame.Navigate(pageType, parameters);
            } 
            else
            {
                throw new Exception("Навигация не инициализирована. Необходимо вызывать метод Navigation.Init(frame) до использования данной функции.");
            }
        }

        public static void GoBack()
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
        }
    }
}
