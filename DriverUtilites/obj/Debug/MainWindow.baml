   M S B A M L   `   `   `  ����I  EDriverUtilites, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:DriverUtilites.RoutineDriverUtilites.Routine  5      ;"clr-namespace:DriverUtilites.ViewsDriverUtilites.Views  5      =#clr-namespace:DriverUtilites.ModelsDriverUtilites.Models  5      O KWPFLocalizeExtension, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulloKclr-namespace:WPFLocalizeExtension.Extensions;assembly=WPFLocalizeExtensionWPFLocalizeExtension.Extensions 5          DriverUtilites.MainWindow   5   	   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    8x,http://schemas.microsoft.com/winfx/2006/xaml    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5      6routine$clr-namespace:DriverUtilites.Routine    5      2views"clr-namespace:DriverUtilites.Views    5      4models#clr-namespace:DriverUtilites.Models    5      YlexKclr-namespace:WPFLocalizeExtension.Extensions;assembly=WPFLocalizeExtension    5      -   5        $��AppMainWindow��5         MainWindowStyle#��[  5        � Title$  Freemium Utilites��5       � WindowStartupLocation$ CenterScreen=�5      $	��716��5      $	��576��5       � SizeToContent$ WidthAndHeight=�5       � 	Resources 5        RightClickSystemTray �� 	Placement@   :DriverUtilites.Routine.PropertyEqualsToVisibilityConverter ' #PropertyEqualsToVisibilityConverterD   >DriverUtilites.Routine.BackupStatusEqualsToVisibilityConverter + 'BackupStatusEqualsToVisibilityConverterC   =DriverUtilites.Routine.AnyPropertyEqualsToVisibilityConverter * &AnyPropertyEqualsToVisibilityConverter��%�   &	       &	 m     &	 �     &	 �     �� 5      $ 
MousePoint=�5      .@�5      m� -   5        $
��Exit��5      5      5        5      60     5      64     5      63   5      .��5       �� -   5          WindowBorder#��[ 5!      ._�5"      ��   WindowInnerBorder#��[ 6   ._�5$      �  � RowDefinitions 5%      �� 5&      $��94 �6   �� 5'      $��67 �6   �� 5(      $	��326 �6   �� 5)      $��42 �6   �� 5*      $��31 �6   .+�55      ��  �� CornerRadius$ 6,6,0,0��6     ContentBackGround#��[ 56      ._�57      ]� $��Stretch=�6   $	��Top=�58      .+�59      �� $��	12,11,0,0q�6   $��Images/bg-logo.png��5:      $
��None=�5;      $
��Left=�5<      $	��Top=�5=      �� 5>      $@��:/DriverUtilites.Infrastructure;component/Fonts/#Myriad Pro)�6   $��SemiBold"�5?      $
��30.5(�5@      $��
-62,39,0,0q�5A      ���	#FF4E4E4E J� Effect 5C      3  -System.Windows.Media.Effects.DropShadowEffect  5D   	      
BlurRadius$ 1Q�6   	   Color$	 	#FFFFFFFF��5E      
   	Direction$	
 -90Q�5F         ShadowDepth$ 1Q�5G         Opacity$ 1Q�5H         RenderingBias$ Performance=�5I      Driver Utilities �� 5K      $��Light"�6   .�60   Professional6>   6D   ]� 5L      $	��Top=�6   $��	8,10,10,0q�5M      $��Right=�5N      $��Right=�5O      .+�5P   	   �� -   5          
HelpButton#��[ 5Q   
   $J��D/DriverUtilites.Infrastructure;component/Fonts/#Helvetica Neue CE 55)�5R   
   .��5S   
   �� $��i��6   $��Segoe UI)�5T      $��12(�5U      $
��Bold"�5V       � ToolTip 5W   
   _� 5X      ��6   Z� ��5Y      6  0WPFLocalizeExtension.Extensions.LocTextExtension  *DriverUtilites:Resources:Help+5[   
   �� 5\   	   -   5         	 ComboboxStyle#��[	 5\      .@�5^   
   �� ��6     *&$DriverUtilites:Resources:Preferences+6V   �� 5_   
   ��6     *-+DriverUtilites:Resources:RunOSMigrationTool+6]   �� 5`   
   ��6     *)'DriverUtilites:Resources:FeedbackReport+6Y   5a   
   �� 5b   	   -   5         
 MinimizeButton#��[
 5b       5d   
   _� 5e      ��6   Z� ��5f        *#!DriverUtilites:Resources:Minimize+5h   
   �� 5i   	   -   5          CloseButton#��[ 5i       5k   
   _� 5l      ��6   Z� ��5m        * DriverUtilites:Resources:Close+5o   
   5p   	   5v      5w      �� 5�      $��1��6     MainAreaBackGround#��[ 5�      }�.  K� ZIndex$ 10��5�      ._�5�      � $	��700��6   $��67��5�   	   }�. $	��Top=�5�   	   $
��Left=�5�   	    � ColumnDefinitions 5�      �� 5�   	   $	��115 �6   �� 5�   	   $	��585 �6    J� OpacityMask 5�      !� 5�   	    j� Stretch$
 None=�5�       !� Visual 5�      ��  �� ElementName$ Shape��5�      .+�5�      F� -   5         F�Name$ Shape��5�        PanelTitleFigure#��[ 5�   
   ���#FFFFFF �� Stretch$
 Fill=�5�   
   $	��710��5�   
   $	��Top=�5�   
   $
��-5,0q�5�   
   }�. $��2��5�   
     TitlePanelsBackgroundColor#���  5�   
   5�   	   F� 5�      #��[ 6   ���#FFFFFF$
 Fill=�5�   
   $	��Top=�5�   
     
NoiseBrush#��[ 5�   
   $	��710��5�   
   $
��-5,0q�5�   
   }�. $��2��5�   
   5�   	   F� 5�      #��[ 6   ���#FFFFFF$
 Fill=�5�   
   $	��710��5�   
   $	��Top=�5�   
   $
��-5,0q�5�   
   }�. $��2��5�   
    5�   	     5�   
   $ 0Q�6   $ 4Q�5�      $	 #000000��5�      5�   	   �� 5�      -	   5        $��PanelScanHeader��5�        PanelTitleText#��[ 5�      $y�Visible=�5�      $��2��5�      $��	30,15,0,0q�5�      ��5�      �� *PanelScanHeader+5�      �� 5�      -
   5        $��PanelScanExclusionsHeader��5�      #��[ 5�      $y�Hidden=�5�      $��2��5�      $��	30,15,0,0q�5�      ��5�        *)'DriverUtilites:Resources:ScanExclusions+5�      �� 5�      -   5        $!��PanelBackupAndRestoreHeader��5�      #��[ 5�      $y�Hidden=�5�      $��2��5�      $��	30,15,0,0q�5�      ��5�        *+)DriverUtilites:Resources:BackupAndRestore+5�      �� 5�      -   5        $��PanelPreferencesHeader��5�      #��[ 5�      $y�Hidden=�5�      $��2��5�      $��	30,15,0,0q�5�      ��5�        *&$DriverUtilites:Resources:Preferences+5�      5�      5�      � 5�      $��2��6   $��30,0,0,0q�5�      .+�5�      � -   5         �Name$ 	PanelScan��5�      $y�Visible=�5�   	   $
��Left=�5�   	   $	��Top=�5�   	   .+�5�      $   DriverUtilites.Views.PanelScan  $
��Left=�6   $	��Top=�5�      y�5�      ��  �� Path$ Status� �� 	Converter# [ &    DriverUtilites.Models.ScanStatus   
NotStarted �� ConverterParameter# Z -   'DriverUtilites.Views.PanelScanInProcess  5�      $
��Left=�6!   $	��Top=�5�      y�5�      �� $ Status�# [    ScanStarted# Z 0	   *DriverUtilites.Views.PanelScanUpdateNeeded	  5�      $
��Left=�6$   $	��Top=�5�      y�5�      �� $ Status�# [    ScanFinishedError# Z 3
   -DriverUtilites.Views.PanelScanUpdateInProcess
  5�      $
��Left=�6'   $	��Top=�5�      y�5�      �� $ Status�# [    UpdateStarted# Z 2   ,DriverUtilites.Views.PanelScanUpdateFinished  5�      $
��Left=�6&   $	��Top=�5�         ScanFinishedOK
  � Tag#  Z 5�      y�5�   	   `� 5�   
   ! `� 	Converter#! [ 6   "   UpdateFinished# `� ConverterParameter## Z" 5�      .1�5�      �� $	 Tag�6   $ �� RelativeSource$ 5�      �� % �� Mode$
% Self=��� 5�      $ Status�6   5�      5  	   5     .   (DriverUtilites.Views.PanelScanExclusions  5     -   5        &  Name$& PanelScanExclusions��5  !   .   (DriverUtilites.Views.PanelScanExclusions'   Loaded$' ElementLoaded��5     $y�Hidden=�5     $
��Left=�5     $	��Top=�5     � 5     -   5        $ PanelBackupAndRestore��5     $y�Hidden=�5  	   .+�5     0   *DriverUtilites.Views.PanelBackupAndRestore  $
��Left=�6$   $	��Top=�5      y�5!     �� $ BackupStatus�# [ (   "DriverUtilites.Models.BackupStatus(   
NotStarted# Z( 6   0DriverUtilites.Views.PanelBackupTargetsSelection  5#     $
��Left=�6*   $	��Top=�5$     y�5%     �� $ BackupStatus�# [ )   BackupTargetsSelection# Z) .   (DriverUtilites.Views.PanelBackupFinished  5'     $
��Left=�6"   $	��Top=�5(     y�5)     �� $ BackupStatus�# [ *   BackupFinished# Z* 7   1DriverUtilites.Views.PanelRestoreTargetsSelection  5+     $
��Left=�6+   $	��Top=�5,     y�5-     �� $ BackupStatus�# [ +   RestoreTargetsSelection# Z+ /   )DriverUtilites.Views.PanelRestoreFinished  5/     $
��Left=�6#   $	��Top=�50     y�51     �� $ BackupStatus�# [ ,   RestoreFinished# Z, 53     +   %DriverUtilites.Views.PanelPreferences  5=     -   5        -  Name$- PanelPreferences��5=     +   %DriverUtilites.Views.PanelPreferences.   Loaded$. ElementLoaded��5>     $y�Hidden=�5?     $
��Left=�5@     $	��Top=�5A     5C     � 5N     $��3��6    ! PanelSwitcherBorderBackground#��[ 5O     $y�Visible=�5P      5Q     �� 5R     $	��232 �6   �� 5S     $	��234 �6   �� 5T     $	��234 �6   .+�5W     �� -   5        $��LinkPanelScan��5Y      ! NavigationButtonFirstSelected#��[ 5W     ��5Z     �� 5[  	   / �� Orientation$/ 
Horizontal=�6   $	��Top=�5\     $��Center=�5]     .+�5^  
   ��   PanelSwitcherImage#��[ 6   $��	3,11,3,-3q�5_     $��Images/icon-autoscan.png��5`     $��24��5a     $��24��5b     �� 5c  
     PanelSwitcherText#��[ 6   ��5d       *(&DriverUtilites:Resources:AutomatedScan+5e  
   5g     �� 5i     -   5        $��LinkPanelScanExclusions��5k       NavigationButton#��[ 5i     $��1��5l     ��5m     �� 5n  	   $/ 
Horizontal=�6   $	��Top=�5o     $��Center=�5p     .+�5q  
   �� #��[ 6   $��	0,11,4,-3q�5r     $ ��Images/icon-exclusions.png��5s     �� 5t  
   #��[ 6   ��5u       *)'DriverUtilites:Resources:ScanExclusions+5v  
   5x     �� 5z     -   5        $��LinkPanelBackupAndRestore��5}     #��[ 5z     $��2��5{     ��5~     �� 5  	   $/ 
Horizontal=�6   $	��Top=�5�     $��Center=�5�     .+�5�  
   �� #��[ 6   $��	0,12,5,-3q�5�     $��Images/icon-backup.png��5�     �� 5�  
   #��[ 6   ��5�       *+)DriverUtilites:Resources:BackupAndRestore+5�  
   5�     5�     � 5�     $��4��6   .+�5�     ��   PanelFooterBorder#��[ 6   ._�5�     ��   PanelFooterBorderNoisy#��[ 6   ._�5�  	   ]� ��. .+�5�  
   �� #��[ 6   $��77��5�     $��Right=�5�     $��Right=�5�     $��	2,8,215,0q�5�     $��Images/bg-logo-small.png��5�     �� 5�  
     PanelFooterText#��[ 6   ��5�       *%#DriverUtilites:Resources:FooterText+5�  
   5�  	   5�     5�     5�     5�     5�     5�     