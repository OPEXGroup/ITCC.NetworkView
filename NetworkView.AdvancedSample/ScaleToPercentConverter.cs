// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Globalization;
using System.Windows.Data;

namespace NetworkView.AdvancedSample
{
    /// <summary>
    /// Used in MainWindow.xaml to converts a scale value to a percentage.
    /// It is used to display the 50%, 100%, etc that appears underneath the zoom and pan control.
    /// </summary>
    public class ScaleToPercentConverter : IValueConverter
    {
        /// <summary>
        /// Convert a fraction to a percentage.
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => (double)(int)((double)value * 100.0);

        /// <summary>
        /// Convert a percentage back to a fraction.
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => (double)value / 100.0;
    }
}
