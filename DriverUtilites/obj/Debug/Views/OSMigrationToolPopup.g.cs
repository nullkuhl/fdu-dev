﻿#pragma checksum "..\..\..\Views\OSMigrationToolPopup.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "204E1C98062D838DA612CB8362C3C11F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Extensions;


namespace FreemiumUtilites {
    
    
    /// <summary>
    /// OSMigrationToolPopup
    /// </summary>
    public partial class OSMigrationToolPopup : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\Views\OSMigrationToolPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal FreemiumUtilites.OSMigrationToolPopup AboutUs;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Views\OSMigrationToolPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Inner;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\..\Views\OSMigrationToolPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox restoreZipPath;
        
        #line default
        #line hidden
        
        
        #line 149 "..\..\..\Views\OSMigrationToolPopup.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button selectRestoreZipPath;
        
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
            System.Uri resourceLocater = new System.Uri("/DriverUtilites;component/views/osmigrationtoolpopup.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\OSMigrationToolPopup.xaml"
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
            this.AboutUs = ((FreemiumUtilites.OSMigrationToolPopup)(target));
            return;
            case 2:
            
            #line 55 "..\..\..\Views\OSMigrationToolPopup.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Close);
            
            #line default
            #line hidden
            return;
            case 3:
            this.Inner = ((System.Windows.Controls.Border)(target));
            return;
            case 4:
            
            #line 105 "..\..\..\Views\OSMigrationToolPopup.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RunOSMigrationTool);
            
            #line default
            #line hidden
            return;
            case 5:
            this.restoreZipPath = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.selectRestoreZipPath = ((System.Windows.Controls.Button)(target));
            
            #line 150 "..\..\..\Views\OSMigrationToolPopup.xaml"
            this.selectRestoreZipPath.Click += new System.Windows.RoutedEventHandler(this.selectRestoreZipPath_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 159 "..\..\..\Views\OSMigrationToolPopup.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.RunOSMigrationRestoreTool);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

