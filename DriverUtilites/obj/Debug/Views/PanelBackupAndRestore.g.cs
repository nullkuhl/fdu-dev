﻿#pragma checksum "..\..\..\Views\PanelBackupAndRestore.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E211D54147B7CAACDE31FFAF61D134AF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DriverUtilites.Routine;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFLocalizeExtension.Extensions;


namespace DriverUtilites.Views {
    
    
    /// <summary>
    /// PanelBackupAndRestore
    /// </summary>
    public partial class PanelBackupAndRestore : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\..\Views\PanelBackupAndRestore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton ActionBackup;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Views\PanelBackupAndRestore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView BackupTypesList;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\Views\PanelBackupAndRestore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid BackupItemsList;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\Views\PanelBackupAndRestore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView BackupItems;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\..\Views\PanelBackupAndRestore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BackupButton;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\..\Views\PanelBackupAndRestore.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RestoreButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DriverUtilites;component/views/panelbackupandrestore.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\PanelBackupAndRestore.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ActionBackup = ((System.Windows.Controls.RadioButton)(target));
            
            #line 33 "..\..\..\Views\PanelBackupAndRestore.xaml"
            this.ActionBackup.Checked += new System.Windows.RoutedEventHandler(this.BackupActionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 40 "..\..\..\Views\PanelBackupAndRestore.xaml"
            ((System.Windows.Controls.RadioButton)(target)).Checked += new System.Windows.RoutedEventHandler(this.BackupActionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.BackupTypesList = ((System.Windows.Controls.ListView)(target));
            return;
            case 4:
            this.BackupItemsList = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.BackupItems = ((System.Windows.Controls.ListView)(target));
            return;
            case 6:
            this.BackupButton = ((System.Windows.Controls.Button)(target));
            return;
            case 7:
            this.RestoreButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

