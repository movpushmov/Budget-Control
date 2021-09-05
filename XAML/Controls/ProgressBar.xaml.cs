using Microsoft.Toolkit.Uwp.Helpers;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;


namespace Budget_Control.XAML.Controls
{
    public sealed partial class ProgressBar : UserControl
    {
        public int Balance
        {
            get { return (int)GetValue(BalanceProperty); }
            set { SetValue(BalanceProperty, value); }
        }

        public static readonly DependencyProperty BalanceProperty =
            DependencyProperty.Register("Balance", typeof(int), typeof(ProgressBar), new PropertyMetadata(0));


        public int Needed
        {
            get { return (int)GetValue(NeededProperty); }
            set { SetValue(NeededProperty, value); }
        }

        public static readonly DependencyProperty NeededProperty =
            DependencyProperty.Register("Needed", typeof(int), typeof(ProgressBar), new PropertyMetadata(1));

        private static readonly int progressBarWidth = 374; // in px

        public ProgressBar()
        {
            InitializeComponent();
        }

        public double GetProgressWidth(int balance, int needed)
        {
            if (balance <= 0)
            {
                return 0;
            }

            if (needed <= 0)
            {
                throw new Exception("Needed property must be greater then 0!");
            }

            double progressPercentage = decimal.ToDouble(decimal.Divide(balance, needed));

            if (progressPercentage >= 1)
            {
                balanceProgress.Background = new SolidColorBrush(ColorHelper.ToColor("#38E272"));
                return progressBarWidth;
            }
            else
            {
                balanceProgress.Background = new SolidColorBrush(ColorHelper.ToColor("#6b6b6b"));
                return progressBarWidth * progressPercentage;
            }
        }
    }
}
