﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DriverUtilites.Infrastructure;
using DriverUtilites.Models;
using DriverUtilites.ViewModels;
using Microsoft.Win32;
using sysUtils;
using WPFLocalizeExtension.Engine;
using System.IO;
using Ookii.Dialogs.Wpf;

namespace DriverUtilites.Views
{
	/// <summary>
	/// Interaction logic for PanelPreferences.xaml
	/// </summary>
	public partial class PanelPreferences
	{
		// The path to the key where Windows looks for startup applications
		readonly RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
		const string RegistryAppName = "DriverUtilites";
		readonly string appPath = Assembly.GetExecutingAssembly().Location;
		readonly string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
		readonly Dictionary<string, int> langIndex = new Dictionary<string, int> { { "en", 0 }, { "de", 1 } };

		public PanelPreferences()
		{
			InitializeComponent();
			LanguagesList.SelectedIndex = langIndex[CfgFile.Get("Lang")];
			MinToTray.IsChecked = CfgFile.Get("MinimizeToTray") == "1";
			StartUpAction.IsChecked = rkApp.GetValue(RegistryAppName) != null;
			driverDownloadsFolder.Text = Uri.UnescapeDataString(CfgFile.Get("DriverDownloadsFolder"));
			backupsFolder.Text = Uri.UnescapeDataString(CfgFile.Get("BackupsFolder"));
		}

		private void selectDriverDownloadsFolder_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new VistaFolderBrowserDialog
							{
								Description = WPFLocalizeExtensionHelpers.GetUIString("SelectFolderForDownloads"),
								UseDescriptionForTitle = true
							};
			// This applies to the Vista style dialog only, not the old dialog.
			var showDialog = dialog.ShowDialog();
			if (showDialog != null && (bool)showDialog)
			{
				if (!String.IsNullOrEmpty(dialog.SelectedPath))
				{
					driverDownloadsFolder.Text = dialog.SelectedPath;
				}
			}
		}

		private void selectBackupsFolder_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new VistaFolderBrowserDialog
							{
								Description = WPFLocalizeExtensionHelpers.GetUIString("SelectFolderForBackups"),
								UseDescriptionForTitle = true
							};
			// This applies to the Vista style dialog only, not the old dialog.
			var showDialog = dialog.ShowDialog();
			if (showDialog != null && (bool)showDialog)
			{
				if (!String.IsNullOrEmpty(dialog.SelectedPath))
				{
					backupsFolder.Text = dialog.SelectedPath;
				}
			}
		}

		private void driverDownloadsFolder_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (Directory.Exists(driverDownloadsFolder.Text))
			{
				CfgFile.Set("DriverDownloadsFolder", Uri.EscapeUriString(driverDownloadsFolder.Text));
			}
		}

		private void backupsFolder_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (Directory.Exists(driverDownloadsFolder.Text))
			{
				CfgFile.Set("BackupsFolder", Uri.EscapeUriString(backupsFolder.Text));
			}
		}

		private void ShowScanPanel(object sender, RoutedEventArgs e)
		{
			((MainWindow)Application.Current.MainWindow).ShowPanelScan(null, null);
		}

		private void MinToTrayOnClose(object sender, RoutedEventArgs e)
		{
			CfgFile.Set("MinimizeToTray", "1");
		}

		private void ShutdownOnClose(object sender, RoutedEventArgs e)
		{
			CfgFile.Set("MinimizeToTray", "0");
		}

		private void LanguageChanged(object sender, SelectionChangedEventArgs e)
		{
			if (LanguagesList.SelectedIndex == 0)
			{
				LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("en");
				CfgFile.Set("Lang", "en");
			}
			if (LanguagesList.SelectedIndex == 1)
			{
				LocalizeDictionary.Instance.Culture = CultureInfo.GetCultureInfo("de");
				CfgFile.Set("Lang", "de");
			}

			var mainViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
			if (mainViewModel != null)
			{
				mainViewModel.SetBackupTypes();
			}
		}

		private void LaunchAtStartup(object sender, RoutedEventArgs e)
		{
			// Add the value in the registry so that the application runs at startup
			rkApp.SetValue(RegistryAppName, appPath);
		}

		private void DoNotLaunchAtStartup(object sender, RoutedEventArgs e)
		{
			// Remove the value from the registry so that the application doesn't start
			rkApp.DeleteValue(RegistryAppName, false);
		}
	}
}
