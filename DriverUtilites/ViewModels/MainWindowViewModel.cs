﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Serialization;
using DriverUtilites.Infrastructure;
using DriverUtilites.Models;
using DriverUtilites.Routine;
using DriverUtilites.Utils;
using MessageBoxUtils;
using Microsoft.Win32;
using sysUtils;
using System.Runtime.InteropServices;
using DriverUtilites.Engine;
using System.Text;
using FreemiumUtilites;
using System.Windows.Media.Animation;

namespace DriverUtilites.ViewModels
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constructors

        public MainWindowViewModel()
        {
            CurrentDispatcher = Dispatcher.CurrentDispatcher;

            #region Commands initialization

            scanCommand = new SimpleCommand
            {
                ExecuteDelegate = x => RunScan()
            };

            cancelScanCommand = new SimpleCommand
            {
                ExecuteDelegate = x => CancelScan()
            };

            showScanCommand = new SimpleCommand
            {
                ExecuteDelegate = x => ShowScan()
            };

            checkDevicesForUpdateCommand = new SimpleCommand
            {
                ExecuteDelegate = x => CheckDevicesForUpdate()
            };

            checkDeviceForUpdateCommand = new SimpleCommand
            {
                ExecuteDelegate = CheckDeviceForUpdate
            };

            updateCommand = new SimpleCommand
            {
                ExecuteDelegate = x => RunUpdate()
            };

            cancelUpdateCommand = new SimpleCommand
            {
                ExecuteDelegate = x => CancelUpdate()
            };

            selectBackupTypeCommand = new SimpleCommand
            {
                ExecuteDelegate = SelectBackupType
            };

            backupSelectedDriversCommand = new SimpleCommand
            {
                ExecuteDelegate = x => BackupSelectedDrivers()
            };

            backupActionFinishedCommand = new SimpleCommand
            {
                ExecuteDelegate = x => BackupActionFinished()
            };

            checkDevicesGroupCommand = new SimpleCommand
            {
                ExecuteDelegate = CheckDevicesGroup
            };

            checkDeviceCommand = new SimpleCommand
            {
                ExecuteDelegate = CheckDevice
            };

            selectDriversToRestoreCommand = new SimpleCommand
            {
                ExecuteDelegate = SelectDriversToRestore
            };

            checkDriverRestoreGroupCommand = new SimpleCommand
            {
                ExecuteDelegate = CheckDriverRestoreGroup
            };

            checkDriverRestoreCommand = new SimpleCommand
            {
                ExecuteDelegate = CheckDriverRestore
            };

            restoreDriversCommand = new SimpleCommand
            {
                ExecuteDelegate = x => RestoreDrivers()
            };

            excludeDeviceCommand = new SimpleCommand
            {
                ExecuteDelegate = ExcludeDevice
            };

            showDriverInfoCommand = new SimpleCommand
            {
                ExecuteDelegate = ShowDriverInfo
            };

            checkDevicesForIncludeCommand = new SimpleCommand
            {
                ExecuteDelegate = x => CheckDevicesForInclude()
            };

            checkDeviceForIncludeCommand = new SimpleCommand
            {
                ExecuteDelegate = CheckDeviceForInclude
            };

            includeDevicesCommand = new SimpleCommand
            {
                ExecuteDelegate = x => IncludeDevices()
            };

            #endregion

            SetBackupTypes();

            initBackgroundWorker = new BackgroundWorker();
            initBackgroundWorker.DoWork += InitBackgroundWorkerDoWork;
            initBackgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                // Fill grouped devices collection with a values from XML
                UpdateGroupedDevices();

                InitialScanFinished = true;
            };
            initBackgroundWorker.RunWorkerAsync();

        }

        /// <summary>
        /// Sets backup types
        /// </summary>
        public void SetBackupTypes()
        {
            BackupTypes = new Dictionary<BackupType, string>
				                            	{
				                            		{
				                            			BackupType.ManualFull,
														WPFLocalizeExtensionHelpers.GetUIString("ManualBackupFull")
				                            		},
				                            		{
				                            			BackupType.ManualSelected,
				                            			WPFLocalizeExtensionHelpers.GetUIString("ManualBackupSelected")
				                            		}
				                            	};
            var backupTypesList = UIUtils.FindChild<ListView>(Application.Current.MainWindow, "BackupTypesList");
            if (backupTypesList != null)
            {
                backupTypesList.ItemsSource = BackupTypes;
            }
        }

        #endregion

        #region Commands

        #region Driver scan related commands

        readonly ICommand scanCommand;
        public ICommand ScanCommand
        {
            get { return scanCommand; }
        }

        readonly ICommand cancelScanCommand;
        public ICommand CancelScanCommand
        {
            get { return cancelScanCommand; }
        }

        void CancelScan()
        {
            RunCancelScan();
        }

        readonly ICommand showScanCommand;
        public ICommand ShowScanCommand
        {
            get { return showScanCommand; }
        }

        void ShowScan()
        {
            RunShowScan();
        }

        readonly ICommand checkDevicesForUpdateCommand;
        public ICommand CheckDevicesForUpdateCommand
        {
            get { return checkDevicesForUpdateCommand; }
        }
        void CheckDevicesForUpdate()
        {
            foreach (DeviceInfo item in DevicesThatNeedsUpdate)
            {
                item.SelectedForUpdate = AllDevicesSelectedForUpdate;
            }
        }

        readonly ICommand checkDeviceForUpdateCommand;
        public ICommand CheckDeviceForUpdateCommand
        {
            get { return checkDeviceForUpdateCommand; }
        }
        void CheckDeviceForUpdate(object selectedDevice)
        {
            if (!AllDevices.Any(d => d.NeedsUpdate && d.SelectedForUpdate))
            {
                AllDevicesSelectedForUpdate = false;
            }
            else
            {
                if (AllDevices.Count(d => d.NeedsUpdate && d.SelectedForUpdate == false) == 0)
                {
                    AllDevicesSelectedForUpdate = true;
                }
            }
        }

        readonly ICommand updateCommand;
        public ICommand UpdateCommand
        {
            get { return updateCommand; }
        }

        readonly ICommand cancelUpdateCommand;
        public ICommand CancelUpdateCommand
        {
            get { return cancelUpdateCommand; }
        }
        void CancelUpdate()
        {
            RunCancelUpdate();
        }

        #endregion

        #region Driver backup related commands

        readonly ICommand selectBackupTypeCommand;
        public ICommand SelectBackupTypeCommand
        {
            get { return selectBackupTypeCommand; }
        }
        void SelectBackupType(object selectedBackupType)
        {
            if (selectedBackupType != null)
            {
                var backupType = ((KeyValuePair<BackupType, string>)selectedBackupType).Key;
                switch (backupType)
                {
                    case BackupType.ManualFull:
                        ThreadPool.QueueUserWorkItem(x => RunBackup(GroupedDevices, BackupType.ManualFull));
                        break;

                    case BackupType.ManualSelected:
                        BackupStatus = BackupStatus.BackupTargetsSelection;
                        break;
                }
            }
            else
            {
                WPFMessageBox.Show(WPFLocalizeExtensionHelpers.GetUIString("SelectBackupTypeText"), WPFLocalizeExtensionHelpers.GetUIString("SelectBackupType"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        readonly ICommand backupSelectedDriversCommand;
        public ICommand BackupSelectedDriversCommand
        {
            get { return backupSelectedDriversCommand; }
        }
        void BackupSelectedDrivers()
        {
            var devicesToBackup = new ObservableCollection<DevicesGroup>();
            int i = 0;
            foreach (DevicesGroup group in GroupedDevices)
            {
                if (@group.Devices.Count(d => d.SelectedForBackup) > 0)
                {
                    devicesToBackup.Add(group);
                    devicesToBackup[i].Devices.RemoveAll(d => d.SelectedForBackup == false);
                    i++;
                }
            }
            ThreadPool.QueueUserWorkItem(x => RunBackup(devicesToBackup, BackupType.ManualSelected));
        }

        readonly ICommand backupActionFinishedCommand;
        public ICommand BackupActionFinishedCommand
        {
            get { return backupActionFinishedCommand; }
        }
        void BackupActionFinished()
        {
            BackupStatus = BackupStatus.NotStarted;
            BackupFinishTitle = "";
        }

        readonly ICommand checkDevicesGroupCommand;
        public ICommand CheckDevicesGroupCommand
        {
            get { return checkDevicesGroupCommand; }
        }
        void CheckDevicesGroup(object selectedDevicesGroup)
        {
            DevicesGroup devicesGroup = GroupedDevices.FirstOrDefault(d => d.DeviceClass == (string)selectedDevicesGroup);
            if (devicesGroup != null)
            {
                foreach (DeviceInfo item in devicesGroup.Devices)
                {
                    item.SelectedForBackup = devicesGroup.GroupChecked;
                }
            }
        }

        readonly ICommand checkDeviceCommand;
        public ICommand CheckDeviceCommand
        {
            get { return checkDeviceCommand; }
        }
        void CheckDevice(object selectedDevice)
        {
            DeviceInfo device = AllDevices.FirstOrDefault(d => d.Id == (string)selectedDevice);
            if (device != null)
            {
                DevicesGroup devicesGroup = GroupedDevices.FirstOrDefault(d => d.DeviceClass == device.DeviceClass);
                if (devicesGroup != null && devicesGroup.Devices.Count(d => d.SelectedForBackup) == 0)
                {
                    devicesGroup.GroupChecked = false;
                }
                else
                {
                    if (devicesGroup != null && devicesGroup.Devices.Count(d => d.SelectedForBackup == false) == 0)
                    {
                        devicesGroup.GroupChecked = true;
                    }
                }
            }
        }

        readonly ICommand selectDriversToRestoreCommand;
        public ICommand SelectDriversToRestoreCommand
        {
            get { return selectDriversToRestoreCommand; }
        }
        void SelectDriversToRestore(object backupItem)
        {
            if (backupItem != null)
            {
                RunSelectDriversToRestore((BackupItem)backupItem);
            }
        }

        readonly ICommand checkDriverRestoreGroupCommand;
        public ICommand CheckDriverRestoreGroupCommand
        {
            get { return checkDriverRestoreGroupCommand; }
        }
        void CheckDriverRestoreGroup(object selectedDevicesGroup)
        {
            DevicesGroup devicesGroup = currentBackupItem.GroupedDrivers.FirstOrDefault(d => d.DeviceClass == (string)selectedDevicesGroup);
            if (devicesGroup != null)
            {
                foreach (DeviceInfo item in devicesGroup.Devices)
                {
                    item.SelectedForRestore = devicesGroup.GroupChecked;
                }
            }
        }

        readonly ICommand checkDriverRestoreCommand;
        public ICommand CheckDriverRestoreCommand
        {
            get { return checkDriverRestoreCommand; }
        }
        void CheckDriverRestore(object selectedDevice)
        {
            DeviceInfo device = AllDevices.FirstOrDefault(d => d.Id == (string)selectedDevice);
            if (device != null)
            {
                DevicesGroup devicesGroup = currentBackupItem.GroupedDrivers.FirstOrDefault(d => d.DeviceClass == device.DeviceClass);
                if (devicesGroup != null && devicesGroup.Devices.Count(d => d.SelectedForRestore) == 0)
                {
                    devicesGroup.GroupChecked = false;
                }
                else
                {
                    if (devicesGroup != null && devicesGroup.Devices.Count(d => d.SelectedForRestore == false) == 0)
                    {
                        devicesGroup.GroupChecked = true;
                    }
                }
            }
        }

        readonly ICommand restoreDriversCommand;
        public ICommand RestoreDriversCommand
        {
            get { return restoreDriversCommand; }
        }
        void RestoreDrivers()
        {
            ThreadPool.QueueUserWorkItem(x => RunRestore());
        }

        #endregion

        #region Devices exclusion related commands

        readonly ICommand excludeDeviceCommand;
        public ICommand ExcludeDeviceCommand
        {
            get { return excludeDeviceCommand; }
        }
        void ExcludeDevice(object id)
        {
            DeviceInfo device = AllDevices.FirstOrDefault(d => d.Id == (string)id);
            if (device != null)
            {
                MessageBoxResult result = WPFMessageBox.Show(Application.Current.MainWindow, String.Format(WPFLocalizeExtensionHelpers.GetUIString("Exclude") + " {0} " + WPFLocalizeExtensionHelpers.GetUIString("FromScans"), device.DeviceName), WPFLocalizeExtensionHelpers.GetUIString("ExcludeDevice"), MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var item = DevicesForScanning.FirstOrDefault(d => d.Id == device.Id);
                    if (item != null)
                    {
                        DevicesForScanning.Remove(item);
                    }

                    device.IsExcluded = true;
                    ExcludedDevices.Add(device);

                    SaveExcludedDevicesToXML();
                    WPFMessageBox.Show(Application.Current.MainWindow, String.Format("{0} excluded from scans", device.DeviceName), "Device excluded", MessageBoxButton.OK, MessageBoxImage.Information);

                    ScanFinishTitle = String.Format("{0} " + WPFLocalizeExtensionHelpers.GetUIString("OutdatedDriversFound"), DevicesForScanning.Count(d => d.NeedsUpdate));

                    if (DevicesThatNeedsUpdate.IsEmpty)
                    {
                        RunShowScan();
                    }
                }
            }
        }

        readonly ICommand checkDevicesForIncludeCommand;
        public ICommand CheckDevicesForIncludeCommand
        {
            get { return checkDevicesForIncludeCommand; }
        }
        void CheckDevicesForInclude()
        {
            foreach (DeviceInfo item in ExcludedDevices.Where(d => d.IsExcluded))
            {
                item.SelectedForExclude = AllDevicesSelectedForInclude;
            }
        }

        readonly ICommand checkDeviceForIncludeCommand;
        public ICommand CheckDeviceForIncludeCommand
        {
            get { return checkDeviceForIncludeCommand; }
        }
        void CheckDeviceForInclude(object selectedDevice)
        {
            if (ExcludedDevices.Count(d => d.IsExcluded && d.SelectedForExclude) == 0)
            {
                AllDevicesSelectedForInclude = false;
            }
            else
            {
                if (ExcludedDevices.Count(d => d.IsExcluded && d.SelectedForExclude == false) == 0)
                {
                    AllDevicesSelectedForInclude = true;
                }
            }
        }

        readonly ICommand includeDevicesCommand;
        public ICommand IncludeDevicesCommand
        {
            get { return includeDevicesCommand; }
        }
        void IncludeDevices()
        {
            var devicesSelectedForInclude = ExcludedDevices.Where(d => d.SelectedForExclude).ToList();
            if (devicesSelectedForInclude.Any())
            {
                foreach (DeviceInfo item in devicesSelectedForInclude)
                {
                    item.IsExcluded = false;
                    item.SelectedForExclude = false;
                    ExcludedDevices.Remove(item);
                }

                SaveExcludedDevicesToXML();
                AllDevicesSelectedForInclude = false;
            }
            else
            {
                WPFMessageBox.Show(WPFLocalizeExtensionHelpers.GetUIString("SelectDevicesToRemoveFromExclude"), WPFLocalizeExtensionHelpers.GetUIString("SelectDevices"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region Driver Info related commands
        readonly ICommand showDriverInfoCommand;
        public ICommand ShowDriverInfoCommand
        {
            get { return showDriverInfoCommand; }
        }
        void ShowDriverInfo(object id)
        {
            Window mainWnd = Application.Current.MainWindow;
            DriverDetailInfoPopup driverInfoDetailPopup = new DriverDetailInfoPopup { Owner = mainWnd };


            DeviceInfo device = AllDevices.FirstOrDefault(d => d.Id == (string)id);
            if (device != null)
            {
                driverInfoDetailPopup.DriverName.Text = device.DeviceName;
                driverInfoDetailPopup.DetailCurrentDriverDate.Text = device.InstalledDriverDate.ToString();
                driverInfoDetailPopup.DetailNewDriverDate.Text = device.NewDriverDate.ToString();
            }

            driverInfoDetailPopup.Left = mainWnd.Left + (mainWnd.Width / 2 - driverInfoDetailPopup.Width / 2);
            var regToolsHeight = (int)driverInfoDetailPopup.Height;
            driverInfoDetailPopup.Height = 0;
            int topStart = (int)(mainWnd.Top + mainWnd.Height) + 30;
            driverInfoDetailPopup.Top = topStart;
            var topFinal = (int)(mainWnd.Top + (mainWnd.Height / 2 - regToolsHeight / 2));

            const int fullAnimationDuration = 300;
            int heightAnimationDuration = (fullAnimationDuration * regToolsHeight / (topStart - topFinal));

            var slideUp = new DoubleAnimation
             {
                 From = topStart,
                 To = topFinal,
                 Duration = new Duration(TimeSpan.FromMilliseconds(fullAnimationDuration))
             };
            driverInfoDetailPopup.BeginAnimation(Window.TopProperty, slideUp);

            var scaleUp = new DoubleAnimation
            {
                From = 0,
                To = regToolsHeight,
                Duration = new Duration(TimeSpan.FromMilliseconds(heightAnimationDuration))
            };
            driverInfoDetailPopup.BeginAnimation(Window.HeightProperty, scaleUp);

            driverInfoDetailPopup.AnimateInnerBox();

            driverInfoDetailPopup.ShowDialog();
        }
        #endregion

        #endregion

        #region Properties

        readonly BackgroundWorker initBackgroundWorker;
        const uint dwScanFlag = (uint)DUSDKHandler.SCAN_FLAGS.SCAN_DEVICES_PRESENT;
        const string ExcludedDevicesXMLFilePath = "excludedDevices.xml";
        const string BackupItemsXMLFilePath = "backupItems.xml";
        public static readonly int DaysFromLastScanMax = 31;
        public static readonly string FreemiumDriverScanTaskName = "FreemiumDriverScan";
        readonly RegistryKey deviceClasses = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Class\", true);

        readonly Dispatcher CurrentDispatcher;
        public delegate void MethodInvoker();
        readonly DriverUtils driverUtils = new DriverUtils();

        Dictionary<int, DriverData> UserUpdates;
        DriverData[] driverData = null;
        int nTotalDrivers = 0;
        bool silentUpdate = false;

        BackgroundWorker bgScan;
        BackgroundWorker bgUpdate;

        List<DeviceInfo> devicesForUpdate;
        BackupItem currentBackupItem;

        bool xmlItemsLoaded;
        public bool XMLItemsLoaded
        {
            get
            {
                return xmlItemsLoaded;
            }
            set
            {
                xmlItemsLoaded = value;
                OnPropertyChanged("XMLItemsLoaded");
            }
        }

        bool initialScanFinished;
        public bool InitialScanFinished
        {
            get
            {
                return initialScanFinished;
            }
            set
            {
                initialScanFinished = value;
                OnPropertyChanged("InitialScanFinished");
            }
        }

        bool driverInfoPopupIsOpen;
        public bool DriverInfoPopupIsOpen
        {
            get
            {
                return driverInfoPopupIsOpen;
            }
            set
            {
                driverInfoPopupIsOpen = value;
                OnPropertyChanged("DriverInfoPopupIsOpen");
            }
        }

        Dictionary<BackupType, String> backupTypes = new Dictionary<BackupType, string>();
        public Dictionary<BackupType, String> BackupTypes
        {
            get
            {
                return backupTypes;
            }
            set
            {
                backupTypes = value;
                OnPropertyChanged("BackupTypes");
            }
        }

        ScanStatus status = ScanStatus.NotStarted;
        public ScanStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        BackupStatus backupStatus = BackupStatus.NotStarted;
        public BackupStatus BackupStatus
        {
            get
            {
                return backupStatus;
            }
            set
            {
                backupStatus = value;
                OnPropertyChanged("BackupStatus");
            }
        }

        string panelScanHeader = WPFLocalizeExtensionHelpers.GetUIString("ScanAndUpdateDrivers");
        public string PanelScanHeader
        {
            get
            {
                return panelScanHeader;
            }
            set
            {
                panelScanHeader = value;
                OnPropertyChanged("PanelScanHeader");
            }
        }

        string scanStatusTitle;
        public string ScanStatusTitle
        {
            get
            {
                return scanStatusTitle;
            }
            set
            {
                scanStatusTitle = value;
                OnPropertyChanged("ScanStatusTitle");
            }
        }

        string backupFinishTitle;
        public string BackupFinishTitle
        {
            get
            {
                return backupFinishTitle;
            }
            set
            {
                backupFinishTitle = value;
                OnPropertyChanged("BackupFinishTitle");
            }
        }

        string scanStatusText;
        public string ScanStatusText
        {
            get
            {
                return scanStatusText;
            }
            set
            {
                scanStatusText = value;
                OnPropertyChanged("ScanStatusText");
            }
        }

        string driverInfoPopupText;
        public string DriverInfoPopupText
        {
            get
            {
                return driverInfoPopupText;
            }
            set
            {
                driverInfoPopupText = value;
                OnPropertyChanged("DriverInfoPopupText");
            }
        }

        string scanFinishTitle;
        public string ScanFinishTitle
        {
            get
            {
                return scanFinishTitle;
            }
            set
            {
                scanFinishTitle = value;
                OnPropertyChanged("ScanFinishTitle");
            }
        }

        int progress;
        public int Progress
        {
            get
            {
                return progress;
            }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        bool allDevicesSelectedForUpdate = true;
        public bool AllDevicesSelectedForUpdate
        {
            get
            {
                return allDevicesSelectedForUpdate;
            }
            set
            {
                allDevicesSelectedForUpdate = value;
                OnPropertyChanged("AllDevicesSelectedForUpdate");
            }
        }

        bool allDevicesSelectedForInclude;
        public bool AllDevicesSelectedForInclude
        {
            get
            {
                return allDevicesSelectedForInclude;
            }
            set
            {
                allDevicesSelectedForInclude = value;
                OnPropertyChanged("AllDevicesSelectedForInclude");
            }
        }

        ObservableCollection<DeviceInfo> excludedDevices = new ObservableCollection<DeviceInfo>();
        public ObservableCollection<DeviceInfo> ExcludedDevices
        {
            get
            {
                return excludedDevices;
            }
            set
            {
                excludedDevices = value;
                OnPropertyChanged("ExcludedDevices");
            }
        }

        ObservableCollection<DeviceInfo> allDevices = new ObservableCollection<DeviceInfo>();
        public ObservableCollection<DeviceInfo> AllDevices
        {
            get
            {
                return allDevices;
            }
            set
            {
                allDevices = value;
                OnPropertyChanged("AllDevices");
            }
        }

        ObservableCollection<DeviceInfo> devicesForScanning;
        public ObservableCollection<DeviceInfo> DevicesForScanning
        {
            get
            {
                return devicesForScanning;
            }
            set
            {
                devicesForScanning = value;
                OnPropertyChanged("DevicesForScanning");
            }
        }

        ObservableCollection<DownloadingDriverModel> downloadedDrivers = new ObservableCollection<DownloadingDriverModel>();
        public ObservableCollection<DownloadingDriverModel> DownloadedDrivers
        {
            get
            {
                return downloadedDrivers;
            }
            set
            {
                downloadedDrivers = value;
                OnPropertyChanged("DownloadedDrivers");
            }
        }

        ICollectionView devicesThatNeedsUpdate;
        public ICollectionView DevicesThatNeedsUpdate
        {
            get
            {
                return devicesThatNeedsUpdate;
            }
            set
            {
                devicesThatNeedsUpdate = value;
                OnPropertyChanged("DevicesThatNeedsUpdate");
            }
        }

        ObservableCollection<BackupItem> backupItems = new ObservableCollection<BackupItem>();
        public ObservableCollection<BackupItem> BackupItems
        {
            get
            {
                return backupItems;
            }
            set
            {
                backupItems = value;
                OnPropertyChanged("BackupItems");
            }
        }

        ObservableCollection<DevicesGroup> groupedDevices = new ObservableCollection<DevicesGroup>();
        public ObservableCollection<DevicesGroup> GroupedDevices
        {
            get
            {
                return groupedDevices;
            }
            set
            {
                groupedDevices = value;
                OnPropertyChanged("GroupedDevices");
            }
        }

        ICollectionView orderedDeviceGroups;
        public ICollectionView OrderedDeviceGroups
        {
            get
            {
                return orderedDeviceGroups;
            }
            set
            {
                orderedDeviceGroups = value;
                OnPropertyChanged("OrderedDeviceGroups");
            }
        }

        ICollectionView orderedDriverRestoreGroups;
        public ICollectionView OrderedDriverRestoreGroups
        {
            get
            {
                return orderedDriverRestoreGroups;
            }
            set
            {
                orderedDriverRestoreGroups = value;
                OnPropertyChanged("OrderedDriverRestoreGroups");
            }
        }

        #endregion


        #region Global Variables

        StringBuilder szProductKey = new StringBuilder().Append("18546-16122-13463");
        StringBuilder szAppDataLoc;
        StringBuilder szTempLoc;
        StringBuilder szRegistryLoc = new StringBuilder().Append("Software\\DriverUtilities");
        StringBuilder szRestorePointName = new StringBuilder().Append("DriverUtilities");

        bool bScanningIsGoingOn = false;
        bool bDriverUpdateIsGoingOn = false;
        bool bIsStopped = false;

        #endregion

        #region Private Methods

        private void InitBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            /*
             * Schedule 
             */
            if (TaskManager.IsTaskScheduled(FreemiumDriverScanTaskName))
            {
                Microsoft.Win32.TaskScheduler.Task task = TaskManager.GetTaskByName(FreemiumDriverScanTaskName);
            }
            else
            {
                TaskManager.CreateDefaultTask(FreemiumDriverScanTaskName, false);
            }
            string taskDescription = TaskManager.GetTaskDescription(FreemiumDriverScanTaskName);

            //Fill BackupItems from XML
            if (File.Exists(BackupItemsXMLFilePath))
            {
                try
                {
                    var xs = new XmlSerializer(typeof(ObservableCollection<BackupItem>));
                    using (var rd = new StreamReader(BackupItemsXMLFilePath))
                    {
                        BackupItems = xs.Deserialize(rd) as ObservableCollection<BackupItem>;
                    }
                }
                catch { }
            }

            //Fill ExcludedDevices from XML
            if (File.Exists(ExcludedDevicesXMLFilePath))
            {
                try
                {
                    var xs = new XmlSerializer(typeof(ObservableCollection<DeviceInfo>));
                    using (var rd = new StreamReader(ExcludedDevicesXMLFilePath))
                    {
                        ExcludedDevices = xs.Deserialize(rd) as ObservableCollection<DeviceInfo>;
                    }
                }
                catch { }
            }

            XMLItemsLoaded = true;

            // For testing only
            //Thread.Sleep(20000);

            // Fill AllDevices models
            RunInitialScan();
        }

        private void StartScan(object sender, DoWorkEventArgs e)
        {
            bScanningIsGoingOn = true;
            bIsStopped = false;

            DriverUtils.SaveDir = Uri.UnescapeDataString(CfgFile.Get("DriverDownloadsFolder"));
            if (!String.IsNullOrEmpty(DriverUtils.SaveDir) && Directory.Exists(DriverUtils.SaveDir))
            {
                szTempLoc = new StringBuilder().Append(DriverUtils.SaveDir);
                szAppDataLoc = new StringBuilder().Append(DriverUtils.SaveDir);
            }

            DUSDKHandler.DUSDK_scanDeviceDriversForUpdates(
                progressCallback,
                szProductKey,
                szAppDataLoc,
                szTempLoc,
                szRegistryLoc,
                dwScanFlag);

            //bScanningIsGoingOn = false;
            //bIsStopped = false;
        }

        void RunInitialScan()
        {
            var devices = driverUtils.ScanDevices();
            AllDevices = new ObservableCollection<DeviceInfo>();
            object ClassGuid;
            foreach (ManagementObject device in devices)
            {
                if (DriverUtils.RestrictedClasses.Contains(device.GetPropertyValue("DeviceClass")))
                    continue;

                //Uses device friendly name instead of device name when it's possible
                string deviceName = "";
                if (device.GetPropertyValue("FriendlyName") != null)
                {
                    deviceName = (string)device.GetPropertyValue("FriendlyName");
                }
                else
                {
                    deviceName = (string)(device.GetPropertyValue("DeviceName"));
                }

                //Uses device friendly name instead of class name when it's possible
                string deviceClassName = "";

                if (device.GetPropertyValue("ClassGuid") != null)
                {
                    var openSubKey = deviceClasses.OpenSubKey((string)device.GetPropertyValue("ClassGuid"));
                    if (openSubKey != null)
                    {
                        ClassGuid = openSubKey.GetValue(null);
                        if (ClassGuid != null)
                        {
                            deviceClassName = ClassGuid.ToString();
                        }
                    }
                }
                else
                {
                    deviceClassName = (string)(device.GetPropertyValue("DeviceClass"));
                }

                var item = new DeviceInfo(
                    (string)(device.GetPropertyValue("DeviceClass")),
                    deviceClassName,
                    deviceName,
                    (string)(device.GetPropertyValue("InfName")),
                    (string)(device.GetPropertyValue("DriverVersion")),
                    (string)(device.GetPropertyValue("DeviceId")),
                    (string)(device.GetPropertyValue("HardwareID")),
                    (string)(device.GetPropertyValue("CompatID"))
                    );
                AllDevices.Add(item);
            }
        }

        void RunScan(bool silentUpdate = false)
        {
            try
            {
                if (Status == ScanStatus.NotStarted)
                {
                    // Update device collections in the UI thread
                    if (CurrentDispatcher.Thread != null)
                    {
                        CurrentDispatcher.BeginInvoke((Action)(() =>
                        {
                            AllDevices = new ObservableCollection<DeviceInfo>();
                            DevicesForScanning = new ObservableCollection<DeviceInfo>();

                            PanelScanHeader = WPFLocalizeExtensionHelpers.GetUIString("DriverScan");
                            ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("NowScanning");
                            Progress = 0;
                            Status = ScanStatus.ScanStarted;

                            ScanStatusText = string.Empty;
                        }));
                    }

                    bgScan = new BackgroundWorker();
                    bgScan.DoWork += StartScan;
                    bgScan.WorkerSupportsCancellation = true;
                    bgScan.RunWorkerCompleted += ScanCompleted;
                    bgScan.RunWorkerAsync();
                }
                else
                {
                    PanelScanHeader = WPFLocalizeExtensionHelpers.GetUIString("ScanAndUpdateDrivers");
                    Status = ScanStatus.NotStarted;
                }
            }
            catch { }
        }

        void ScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bool isCancelled = bIsStopped;

            bScanningIsGoingOn = false;
            bIsStopped = false;

            if (CurrentDispatcher.Thread != null)
            {
                CurrentDispatcher.Invoke((MethodInvoker)delegate
                {
                    if (isCancelled)
                    {
                        ShowScan();
                        return;
                    }

                    ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("ScanCompleted");
                    ScanStatusText = string.Empty;

                    DevicesThatNeedsUpdate = CollectionViewSource.GetDefaultView(DevicesForScanning);
                    DevicesThatNeedsUpdate.Filter = i => ((DeviceInfo)i).NeedsUpdate;

                    UpdateGroupedDevices();

                    if (DevicesThatNeedsUpdate.IsEmpty)
                    {
                        PanelScanHeader = WPFLocalizeExtensionHelpers.GetUIString("DriverUpdate");
                        ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("AllDriversAreUpToDate");
                        ScanStatusText = string.Empty;
                        ScanFinishTitle = WPFLocalizeExtensionHelpers.GetUIString("AllDriversAreUpToDate");
                        Status = ScanStatus.ScanFinishedOK;
                    }
                    else
                    {
                        if (isCancelled)
                        {
                            ShowScan();
                            return;
                        }

                        if (!silentUpdate)
                        {
                            PanelScanHeader = WPFLocalizeExtensionHelpers.GetUIString("DriverScanResults");
                            Status = ScanStatus.ScanFinishedError;
                            ScanFinishTitle = String.Format("{0} " + WPFLocalizeExtensionHelpers.GetUIString("OutdatedDriversFound"), AllDevices.Count(d => d.NeedsUpdate));
                        }
                        else
                        {
                            RunUpdate();
                        }
                    }
                }
                , null);
            }
            SaveExcludedDevicesToXML();
        }

        /// <summary>
        /// receives the progress of driver scan
        /// </summary>
        /// <param name="progressType"></param>
        /// <param name="data"></param>
        /// <param name="currentItemPos"></param>
        /// <param name="nTotalDriversToScan"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public bool progressCallback(DUSDKHandler.PROGRESS_TYPE progressType, IntPtr data, int currentItemPos, int nTotalDriversToScan, int progress)
        {
            // here we will get the progress of driver scan.

            DriverData? dd = null;
            string driverName = string.Empty;
            string category = string.Empty;
            string VersionInstalled = string.Empty;
            string VersionUpdated = string.Empty;
            DateTime dtInstalled;

            if (data != IntPtr.Zero)
            {
                dd = (DriverData)Marshal.PtrToStructure(
                            (IntPtr)(data.ToInt64() + currentItemPos * Marshal.SizeOf(typeof(DriverData))),
                            typeof(DriverData)
                            );

            }

            switch (progressType)
            {
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_SIZE_SCAN_DATA:
                    nTotalDrivers = nTotalDriversToScan;
                    driverData = new DriverData[nTotalDriversToScan];
                    UserUpdates = new Dictionary<int, DriverData>();
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_SCANNING:
                    if (data != IntPtr.Zero && !bIsStopped)
                    {
                        driverData[currentItemPos] = (DriverData)dd;
                        DeviceInfo item = new DeviceInfo(
                        null,
                            //driverData[currentItemPos].nDbCategoryID.ToString(),
                        driverData[currentItemPos].category,
                        driverData[currentItemPos].driverName,
                        driverData[currentItemPos].installInf,
                        driverData[currentItemPos].version,
                        currentItemPos.ToString(),
                        driverData[currentItemPos].hardwareId,
                        driverData[currentItemPos].CompatibleIdIndex.ToString()
                        );

                        dtInstalled = DateTime.FromFileTime(driverData[currentItemPos].ulDateTimeQuadPart);
                        item.InstalledDriverDate = dtInstalled.ToShortDateString();

                        if (CurrentDispatcher.Thread != null)
                        {
                            CurrentDispatcher.Invoke((MethodInvoker)delegate
                            {
                                ScanStatusText = item.DeviceName;

                                var excludedDevice = ExcludedDevices.FirstOrDefault(d => d.Id == item.Id);
                                if (excludedDevice != null)
                                {
                                    item.IsExcluded = excludedDevice.IsExcluded;
                                }
                                if (!item.IsExcluded)
                                {
                                    DevicesForScanning.Add(item);
                                }
                                AllDevices.Add(item);
                            }
                            , null);
                        }
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_RETRIEVING_UPDATES_DATA:
                    if (CurrentDispatcher.Thread != null)
                    {
                        CurrentDispatcher.Invoke((MethodInvoker)delegate
                        {
                            ScanStatusText = string.Empty;
                            ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("ContactingServer");
                        }, null);
                    }
                    break;
                //case DUSDKHandler.PROGRESS_TYPE.PROGRESS_RETRIEVING_UPDATES_FAILED_INET:
                //    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_FILTERING_UPDATES:
                    //        this.UIThread(() => label3.Text = "Getting driver updates ... ");                    
                    if (data != IntPtr.Zero && !bIsStopped)
                    {
                        // get driver update information                        
                        DriverData DriverUpdate = (DriverData)Marshal.PtrToStructure(
                              (IntPtr)(data.ToInt64() + currentItemPos * Marshal.SizeOf(typeof(DriverData))),
                              typeof(DriverData)
                              );

                        if (CurrentDispatcher.Thread != null)
                        {
                            CurrentDispatcher.Invoke((MethodInvoker)delegate
                            {
                                DeviceInfo deviceInfo = DevicesForScanning.Where(wh => wh.Id == currentItemPos.ToString()).FirstOrDefault();
                                if (deviceInfo != null)
                                {
                                    deviceInfo.NeedsUpdate = true;
                                    deviceInfo.SelectedForUpdate = true;
                                    deviceInfo.NewDriverDate = DateTime.FromFileTime(DriverUpdate.ulDateTimeQuadPart).ToShortDateString();
                                    deviceInfo.DownloadLink = DriverUpdate.libURL;
                                    deviceInfo.InstallCommand = DriverUpdate.SetupLaunchParam;
                                }

                                UserUpdates.Add(currentItemPos, DriverUpdate);
                            }
                            , null);
                        }
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_SCANNED:
                    //if (CurrentDispatcher.Thread != null)
                    //{
                    //    CurrentDispatcher.BeginInvoke((Action)(() =>
                    //    {
                    //        ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("ScanCompleted");
                    //        ScanStatusText = "";
                    //    }));
                    //}                   
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_UPDATE_STARTED_FOR_SINGLE:
                    {
                        if (data != IntPtr.Zero && dd.HasValue)
                        {
                            if (CurrentDispatcher.Thread != null)
                            {
                                CurrentDispatcher.BeginInvoke((Action)(() =>
                                {
                                    DownloadedDrivers.Add(new DownloadingDriverModel(WPFLocalizeExtensionHelpers.GetUIString("InstallingDriver"), devicesForUpdate[currentItemPos]));
                                    ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("InstallingDriver");
                                    //Progress = 0;
                                }));
                            }
                        }
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_UPDATE_END_FOR_SINGLE:
                    {
                        if (data != IntPtr.Zero && dd.HasValue)
                        {
                            if (CurrentDispatcher.Thread != null)
                            {
                                CurrentDispatcher.BeginInvoke((Action)(() =>
                                {
                                    //ScanStatusTitle = 
                                    ScanStatusText = WPFLocalizeExtensionHelpers.GetUIString("UpdateCompleted") + " " + ((DriverData)dd).driverName;
                                }));
                            }
                        }
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_UPDATE_SUCCESSFUL:
                    {
                        if (data != IntPtr.Zero && dd.HasValue)
                        {
                            if (CurrentDispatcher.Thread != null)
                            {
                                CurrentDispatcher.BeginInvoke((Action)(() =>
                                {
                                    ScanFinishTitle = String.Format("{0} " + WPFLocalizeExtensionHelpers.GetUIString("OutdatedDriversUpdated"), devicesForUpdate.Count());
                                    Status = ScanStatus.UpdateFinished;
                                    ScanStatusTitle = ScanFinishTitle;
                                    ScanStatusText = string.Empty;
                                }));
                            }
                        }
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_UPDATE_FAILED:
                    {
                        if (data != IntPtr.Zero && dd.HasValue)
                        {
                            if (CurrentDispatcher.Thread != null)
                            {
                                CurrentDispatcher.BeginInvoke((Action)(() =>
                                {
                                    ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("UpdateFailed") + " " + ((DriverData)dd).driverName;
                                    ScanStatusText = string.Empty;
                                }));
                            }
                        }
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_DOWNLOAD_STARTED_FOR_SINGLE:
                    {
                        if (data != IntPtr.Zero && dd.HasValue)
                        {
                            if (CurrentDispatcher.Thread != null)
                            {
                                CurrentDispatcher.BeginInvoke((Action)(() =>
                                {
                                    DownloadedDrivers.Add(new DownloadingDriverModel(WPFLocalizeExtensionHelpers.GetUIString("DownloadingDriver"), devicesForUpdate[currentItemPos]));
                                    ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("DownloadingDriver");
                                    ScanStatusText = ((DriverData)dd).driverName;
                                }));
                            }
                        }
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_DOWNLOAD_END_FOR_SINGLE:
                    {
                        if (data != IntPtr.Zero && dd.HasValue)
                        {
                            if (CurrentDispatcher.Thread != null)
                            {
                                CurrentDispatcher.BeginInvoke((Action)(() =>
                                {
                                    ScanStatusText = WPFLocalizeExtensionHelpers.GetUIString("DownloadComplete") + " " + ((DriverData)dd).driverName;
                                }));
                            }
                        }
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_DOWNLOAD_END_FOR_SINGLE_UNREG:
                    {

                        // download failed as user is unregistered
                    }
                    break;
                case DUSDKHandler.PROGRESS_TYPE.PROGRESS_DOWNLOAD_END_FOR_SINGLE_INET_ERROR:
                    {
                        // download failed because of Internet Error
                    }
                    break;
                default:
                    break;
            }

            if (progress > 0)
            {
                if (CurrentDispatcher.Thread != null)
                {
                    CurrentDispatcher.BeginInvoke((Action)(() =>
                    {
                        Progress = progress;
                    }));
                }
            }

            return true;
        }

        void RunCancelScan()
        {
            bIsStopped = true;
            if (bgScan.IsBusy)
                bgScan.CancelAsync();
            CancelOperation();
        }

        void RunShowScan()
        {
            PanelScanHeader = WPFLocalizeExtensionHelpers.GetUIString("ScanAndUpdateDrivers");
            Status = ScanStatus.NotStarted;
        }

        void RunUpdate()
        {
            devicesForUpdate = DevicesForScanning.Where(d => d.NeedsUpdate && d.SelectedForUpdate).ToList();
            if (devicesForUpdate.Any())
            {
                DriverUtils.SaveDir = Uri.UnescapeDataString(CfgFile.Get("DriverDownloadsFolder"));
                if (!String.IsNullOrEmpty(DriverUtils.SaveDir) && Directory.Exists(DriverUtils.SaveDir))
                {
                    bIsStopped = false;
                    PanelScanHeader = WPFLocalizeExtensionHelpers.GetUIString("DriverDownloadingAndInstallation");
                    Status = ScanStatus.UpdateStarted;
                    Progress = 0;
                    DownloadedDrivers.Clear();

                    szTempLoc = new StringBuilder().Append(DriverUtils.SaveDir);
                    szAppDataLoc = new StringBuilder().Append(DriverUtils.SaveDir);

                    bgUpdate = new BackgroundWorker();
                    bgUpdate.DoWork += DriverUpdate;
                    bgUpdate.WorkerSupportsCancellation = true;
                    bgUpdate.RunWorkerAsync();
                }
                else
                {
                    WPFMessageBox.Show(WPFLocalizeExtensionHelpers.GetUIString("CheckDownloadFolder"), WPFLocalizeExtensionHelpers.GetUIString("CheckPreferences"), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                WPFMessageBox.Show(WPFLocalizeExtensionHelpers.GetUIString("SelectDriversToUpdate"), WPFLocalizeExtensionHelpers.GetUIString("SelectDrivers"), MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        void DriverUpdate(object sender, DoWorkEventArgs e)
        {
            bDriverUpdateIsGoingOn = true;
            devicesForUpdate = DevicesForScanning.Where(d => d.NeedsUpdate && d.SelectedForUpdate).ToList();

            DriverData[] scanData = new DriverData[devicesForUpdate.Count];
            DriverData[] updateData = new DriverData[devicesForUpdate.Count];

            int i = 0;
            int id;

            foreach (DeviceInfo di in devicesForUpdate)
            {
                if (Int32.TryParse(di.Id, out id))
                {
                    scanData[i] = driverData[id];
                    updateData[i] = UserUpdates[id];
                    i++;
                }
            }

            UpdateDrivers(scanData, updateData);

            scanData = null;
            updateData = null;
        }

        /// <summary>
        /// call to SDK for driver update function
        /// </summary>
        /// <param name="arrUserDrivers"></param>
        /// <param name="arrUserUpdates"></param>
        public int UpdateDrivers(DriverData[] arrUserDrivers, DriverData[] arrUserUpdates)
        {
            //bDriverUpdateIsGoingOn = true;
            int retval = 0;
            IntPtr pUnmanagedUserDrivers = IntPtr.Zero;
            IntPtr pUnmanagedUserUpdates = IntPtr.Zero;

            try
            {
                // marshal input
                pUnmanagedUserDrivers = MarshalHelper.MarshalArray(ref arrUserDrivers);
                pUnmanagedUserUpdates = MarshalHelper.MarshalArray(ref arrUserUpdates);

                // call driver update function
                retval = DUSDKHandler.updateDeviceDriversEx(
                    progressCallback,
                    szProductKey,
                    szAppDataLoc,
                    szTempLoc,
                    szRegistryLoc,
                    downloadProgressCallback,
                    pUnmanagedUserDrivers,
                    pUnmanagedUserUpdates,
                    arrUserDrivers.Length,
                    szRestorePointName
                    );


                if (retval == DUSDKHandler.DefineConstants.SUCCESS)
                {
                    //this.UIThread(() => this.label3.Text = "Drivers Updated Successfully!!");
                }
                else if (retval == DUSDKHandler.DefineConstants.CANCEL_INSTALL)
                {
                    //this.UIThread(() => this.label3.Text = "Update Stopped Successfully!!");
                }
                else if (retval == DUSDKHandler.DefineConstants.FAIL)
                {
                    ScanStatusTitle = WPFLocalizeExtensionHelpers.GetUIString("UpdateFailed");
                    ScanStatusText = string.Empty;
                }
                else
                {
                    ; // check for error code
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // free memory
                if (pUnmanagedUserDrivers != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pUnmanagedUserDrivers);
                }

                if (pUnmanagedUserUpdates != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pUnmanagedUserUpdates);
                }

                pUnmanagedUserDrivers = IntPtr.Zero;
                pUnmanagedUserUpdates = IntPtr.Zero;

                bDriverUpdateIsGoingOn = false;
            }

            return retval;
        }

        /// <summary>
        /// Driver download progress
        /// </summary>
        /// <param name="progressType"></param>
        /// <param name="iTotalDownloaded"></param>
        /// <param name="iTotalSize"></param>
        /// <param name="iRetCode"></param>
        /// <param name="iPercentageCompleted"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public bool downloadProgressCallback(
           DUSDKHandler.PROGRESS_TYPE progressType,
           long iTotalDownloaded,
           long iTotalSize,
           int iRetCode,
           int iPercentageCompleted,
           int progress
           )
        {
            if (iTotalDownloaded >= 0 && iTotalSize > 0)
            {
                if (CurrentDispatcher.Thread != null)
                {
                    CurrentDispatcher.BeginInvoke((Action)(() =>
                    {
                        Progress = progress;
                    }));
                }
            }

            return true;
        }

        void RunCancelUpdate()
        {
            bIsStopped = true;
            if (bgUpdate.IsBusy)
                bgUpdate.CancelAsync();
            CancelOperation();
            PanelScanHeader = WPFLocalizeExtensionHelpers.GetUIString("DriverScanResults");
            Status = ScanStatus.ScanFinishedError;
            ScanFinishTitle = String.Format("{0} " + WPFLocalizeExtensionHelpers.GetUIString("OutdatedDriversFound"), DevicesForScanning.Count(d => d.NeedsUpdate));
        }

        void RunBackup(ObservableCollection<DevicesGroup> driversToBackup, BackupType backupType)
        {
            string backupDir = Uri.UnescapeDataString(CfgFile.Get("BackupsFolder"));
            if (!String.IsNullOrEmpty(backupDir) && new DirectoryInfo(backupDir).Exists)
            {
                int driversCount = 0;
                foreach (DevicesGroup group in driversToBackup)
                {
                    foreach (DeviceInfo item in group.Devices)
                    {
                        item.SelectedForRestore = true;
                        driverUtils.BackupDriver(item.DeviceName, item.InfName, backupDir);
                        driversCount++;
                    }
                }
                if (CurrentDispatcher.Thread != null)
                {
                    CurrentDispatcher.BeginInvoke((Action)(() =>
                    {
                        BackupItems.Add(
                            new BackupItem(
                                backupDir,
                                DateTime.Now,
                                BackupTypes[backupType],
                                driversToBackup
                            )
                        );
                        SaveBackupItemsToXML();

                        BackupFinishTitle = String.Format("{0} " + WPFLocalizeExtensionHelpers.GetUIString("DriversBackupedSuccesfully"), driversCount);
                        BackupStatus = BackupStatus.BackupFinished;
                    }));
                }
            }
            else
            {
                if (CurrentDispatcher.Thread != null)
                {
                    CurrentDispatcher.BeginInvoke((Action)(() => WPFMessageBox.Show(WPFLocalizeExtensionHelpers.GetUIString("CheckBackupsFolder"), WPFLocalizeExtensionHelpers.GetUIString("CheckPreferences"), MessageBoxButton.OK, MessageBoxImage.Error)));
                }
            }
        }

        void RunSelectDriversToRestore(BackupItem backupItem)
        {
            currentBackupItem = backupItem;
            OrderedDriverRestoreGroups = CollectionViewSource.GetDefaultView(backupItem.GroupedDrivers);
            OrderedDriverRestoreGroups.SortDescriptions.Clear();
            OrderedDriverRestoreGroups.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
            OrderedDriverRestoreGroups.Refresh();
            BackupStatus = BackupStatus.RestoreTargetsSelection;
        }

        void RunRestore()
        {
            string backupDir = currentBackupItem.Path;
            DirectoryInfo[] subDirs = new DirectoryInfo(backupDir).GetDirectories();
            int restoredDriversCount = 0;
            foreach (DevicesGroup group in currentBackupItem.GroupedDrivers)
            {
                foreach (DeviceInfo item in group.Devices)
                {
                    if (item.SelectedForRestore)
                    {
                        foreach (DirectoryInfo dirInfo in subDirs)
                        {
                            // see if dirInfo.Name == item.DeviceName
                            if (dirInfo.Exists && dirInfo.Name.Trim() == item.DeviceName.Trim())
                            {
                                driverUtils.RestoreDriver(dirInfo.Name, backupDir);
                                restoredDriversCount++;
                            }
                        }
                    }
                }
            }

            if (CurrentDispatcher.Thread != null)
            {
                CurrentDispatcher.BeginInvoke((Action)(() =>
                {
                    if (restoredDriversCount != 0)
                    {
                        BackupFinishTitle = String.Format("{0} " + WPFLocalizeExtensionHelpers.GetUIString("DriversRestoredSuccesfully"), restoredDriversCount);
                        BackupStatus = BackupStatus.RestoreFinished;
                    }
                    else
                    {
                        WPFMessageBox.Show(WPFLocalizeExtensionHelpers.GetUIString("SelectDriversForRestoreText"), WPFLocalizeExtensionHelpers.GetUIString("SelectDriversForRestoreCaption"), MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }));
            }
        }

        /// <summary>
        /// Will call to cancel driver scan or driver update function
        /// </summary>
        /// <returns></returns>
        public bool CancelOperation()
        {
            bool retval = false;
            if (bScanningIsGoingOn || bDriverUpdateIsGoingOn)
            {
                //Thread th = new Thread(delegate()
                //{
                //this.UIThread(() => btnCancel.Enabled = false);

                int CancelFn = DUSDKHandler.DefineConstants.DEFAULT;
                if (bScanningIsGoingOn)
                {
                    CancelFn = DUSDKHandler.DefineConstants.CANCEL_SCAN;
                }
                else if (bDriverUpdateIsGoingOn)
                {
                    CancelFn = DUSDKHandler.DefineConstants.CANCEL_INSTALL;
                }

                retval = DUSDKHandler.DUSDK_cancelOperation(CancelFn);
                //this.UIThread(() => btnCancel.Enabled = true);
                //});
                //th.Start();
                //th.Join();
            }

            return retval;
        }


        void UpdateGroupedDevices()
        {
            GroupedDevices = new ObservableCollection<DevicesGroup>();
            foreach (DeviceInfo device in AllDevices)
            {
                //device.InfName == "null" means that there is no installed *.inf for that device, so nothing to backup for it
                if (device.InfName != "null")
                {
                    DevicesGroup devicesGroup = GroupedDevices.FirstOrDefault(g => g.DeviceClass == device.DeviceClass);
                    if (devicesGroup == null)
                    {
                        GroupedDevices.Add(new DevicesGroup(device.DeviceClass, device.DeviceClassName, device.DeviceClassImageSmall, new List<DeviceInfo> { device }));
                    }
                    else
                    {
                        devicesGroup.Devices.Add(device);
                    }
                }
            }

            OrderedDeviceGroups = CollectionViewSource.GetDefaultView(GroupedDevices);
            OrderedDeviceGroups.SortDescriptions.Clear();
            OrderedDeviceGroups.SortDescriptions.Add(new SortDescription("Order", ListSortDirection.Ascending));
            OrderedDeviceGroups.Refresh();
        }

        void SaveExcludedDevicesToXML()
        {
            try
            {
                var xs = new XmlSerializer(typeof(ObservableCollection<DeviceInfo>));
                using (var wr = new StreamWriter(ExcludedDevicesXMLFilePath))
                {
                    xs.Serialize(wr, ExcludedDevices);
                }
            }
            catch { }
        }

        void SaveBackupItemsToXML()
        {
            try
            {
                var xs = new XmlSerializer(typeof(ObservableCollection<BackupItem>));
                using (var wr = new StreamWriter(BackupItemsXMLFilePath))
                {
                    xs.Serialize(wr, BackupItems);
                }
            }
            catch { }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects all apps and runs Scan on it
        /// </summary>
        public void SelectAllAndScan(bool updateAfterScan)
        {
            ThreadPool.QueueUserWorkItem(x => RunScan(updateAfterScan));
        }

        #endregion

        #region INotifyPropertyChanged

        void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
