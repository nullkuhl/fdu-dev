﻿using System;
using System.Globalization;
using System.Windows.Data;
using DriverUtilites.Models;
using System.Windows;

namespace DriverUtilites.Routine
{
	public sealed class BackupStatusEqualsToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType,
							  object parameter, CultureInfo culture)
		{
			BackupStatus s1 = (BackupStatus)value;
			BackupStatus s2 = (BackupStatus)parameter;
			return s1 == s2 ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType,
								  object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
