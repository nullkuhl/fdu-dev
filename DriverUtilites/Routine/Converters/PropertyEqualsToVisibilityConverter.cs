﻿using System;
using System.Globalization;
using System.Windows.Data;
using DriverUtilites.Models;
using System.Windows;

namespace DriverUtilites.Routine
{
	public sealed class PropertyEqualsToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
							  object parameter, CultureInfo culture)
		{
			ScanStatus s1 = (ScanStatus)value;
			ScanStatus s2 = (ScanStatus)parameter;
			return s1 == s2 ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType,
								  object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
