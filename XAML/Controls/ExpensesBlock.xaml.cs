using Microsoft.Toolkit.Uwp.Helpers;
using Salary_Control.Source.API.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пользовательский элемент управления" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234236

namespace Salary_Control.XAML.Controls
{
    public sealed partial class ExpensesBlock : UserControl
    {
        private readonly decimal _width = 420 - 32;

        public ExpensesBlock()
        {
            this.InitializeComponent();

            DateTimeFormatInfo info = CultureInfo.GetCultureInfo("ru-RU").DateTimeFormat;
            monthName.Text = info.MonthNames[DateTime.Now.Month - 1].ToLower();
        }

        public void UpdateExpenses(Dictionary<int, EventCategory> expenseses, Dictionary<int, int> totalMinus)
        {
            if (expenseses.Count == 0 || totalMinus.Count  == 0)
            {
                totalMinusCount.Text = "0";
                return;
            }

            var minus = -totalMinus.Sum(x => x.Value);
            totalMinusCount.Text = minus.ToString();

            foreach (var expense in expenseses)
            {
                var e = -totalMinus[expense.Key];
                var expensePercentage = decimal.Divide(e, minus);

                var grid = new Grid
                {
                    Width = (double)(_width * expensePercentage),
                    Background = new SolidColorBrush(ColorHelper.ToColor(expense.Value.Color))
                };

                var tooltip = new ToolTip()
                {
                    Content = expense.Value.Name,
                };

                ToolTipService.SetToolTip(grid, tooltip);

                expensesBlock.Children.Add(grid);
            }

            (expensesBlock.Children[expensesBlock.Children.Count - 1] as Grid).Width -= 2;
        }
    }
}
