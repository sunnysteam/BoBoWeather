   M S B A M L   `   `   `  ���� F  BLifeService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:LifeService.View.ReminderViewLifeService.View.ReminderView  3clr-namespace:LifeService.ViewLifeService.View  6    0LifeService.View.ReminderView.RemoteReminderView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     =local+clr-namespace:LifeService.View.ReminderView     /viewclr-namespace:LifeService.View     k gDotNetProjects.Wpf.Extended.Toolkit, Version=3.0.23.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4` \Xceed.Wpf.Toolkit, Version=3.3.17121.18230, Culture=neutral, PublicKeyToken=ba83ff368b7563c66xctk)http://schemas.xceed.com/wpf/xaml/toolkit   .��� !  LifeService.View.GridHelper    
ShowBorder  . $	��150��$	��292��.+�� -    �Name$ 	main_grid��$��Center=�$��Center=� � RowDefinitions �� $	��60* ��� $	��20* ��� $	��20* �.+��� $��0��.+�� .+��� -    ��Name$ label��$��提醒内容��$
��Left=�$��	10,10,0,0q�$	��Top=�$��67���� -    ��Name$ textBox��$
��Left=�$��63��$��88,0,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��188������ *ContentText+ �� UpdateSourceTrigger$ PropertyChanged=��� -    ��Name$ button1��$��语音��$
��Left=�$��	43,43,0,0q�$	��Top=�$��28�� J� RenderTransformOrigin$ -0.266,0.54&��� $��1��$��0,3,0,0q�.+�� .+�&   Xceed.Wpf.Toolkit.DateTimePicker  $��88,3,0,7q�$��20��,  &Xceed.Wpf.Toolkit.Primitives.InputBase	   Text	 �� *Time+$ PropertyChanged=��� -   $ label1��$��提醒时间��$
��Left=�$��10,0,0,0q�$	��Top=��� $��2��.+�� $��0,3,0,0q�.+��� $��	88,0,31,0q�
 �� SelectedValuePath$

 Name�� �� DisplayMemberPath$
 Name������ *ContactsList+ �� Mode$ OneWay=� �� SelectedItem �� *
Contacts+$ TwoWay=��� -   $ button��$	��√��$
��Left=�$��	250,0,0,0q�$	��Top=�$��26��$��25��$ 
0.08,0.541&����� *SaveButtonCommand+����  �� ElementName$ 	main_grid���� -   $
 labe��$��	联系人��$
��Left=�$��10,0,0,0q�$	��Top=�