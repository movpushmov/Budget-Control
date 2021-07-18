using Salary_Control.Source.API.Entities;
using Salary_Control.Source.API.XAML_Bridges;
using System;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Salary_Control.XAML.SubPages.Categories
{
    public sealed partial class ListOfCategories : UserControl
    {
        public CategoriesList Categories
        {
            get { return (CategoriesList)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Categories.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(CategoriesList), typeof(ListOfCategories), new PropertyMetadata(
                new CategoriesList() { Categories = new ObservableCollection<EventCategory>() }
                ));

        public event TypedEventHandler<ListOfCategories, EventCategory> RemoveAction;
        public event TypedEventHandler<ListOfCategories, EventCategory> EditAction;

        public ListOfCategories()
        {
            this.InitializeComponent();
        }

        private void RemoveCategory(object sender, RoutedEventArgs e)
        {
            if (RemoveAction != null)
            {
                RemoveAction(this, (sender as Button).Tag as EventCategory);
            }
        }

        private void EditCategory(object sender, RoutedEventArgs e)
        {
            if (EditAction != null)
            {
                EditAction(this, (sender as Button).Tag as EventCategory);
            }
        }
    }
}
