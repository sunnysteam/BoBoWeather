   M S B A M L   `   `   `  ����  LifeService4    .LifeService.View.ReminderView.ReminderRingView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   -   5          �Name$  ReminderRingViewWindow��5   	    � WindowStyle$
 None=�6)    � 
ResizeMode$ NoResize=�6=    � AllowsTransparency . 
������  � ShowInTaskbar .   J� OpacityMask
 �����$
��1280��5	   f   $	��800��6s    � WindowStartupLocation$ CenterScreen=�6�    � WindowState$ 	Maximized=�6�    � 	Resources 5
      	 �� MergedDictionaries
 �� Source�� 5   
   	 5      �� 5      $2
 ,/LifeService;component/ButtonDictionary.xaml?�6%   �� 5      $2
 ,/LifeService;component/WindowDictionary.xaml?�6%   %    5      .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *LoadedCommand+   CommandParameter 6F   ��  �� ElementName$ ReminderRingViewWindow��5      .��5      � ���	#CC000000.+�5   
   � $��0,0,0,0q�6    J� Opacity$ 1Q�6    $	��500��6,   $	��500��68   .+�5      �� $��Center=�6R   $	��Top=�6o   ���#a4a4a4$��Microsoft YaHei)�6�   $��24(�6�   $��Center=�6�   ��6   �� *Label+ �� UpdateSourceTrigger$ PropertyChanged=���6�   Z� 6�   �� 5      $��WrapWithOverflow=�6U   $	��380��6v   $	��185��6�   $��60,104,60,211q�6�   $��18(�6�   $��Microsoft YaHei)�6�   
�������$��Center=�6�   $
��Bold"�6  ��6   �� *	Content+$ PropertyChanged=�6  �� 5      $��Center=�6Y   $��Bottom=�6v   
�������$��Century Gothic)�6�   $��48(�6�   $��Bottom=�6�   $��	0,0,0,137q�6�   $��Center=�6  ��6   �� *ReminderTime+$ PropertyChanged=�6  �� 5      $��Center=�6|   $��112,377,112,0q�6�   $	��Top=�6�      AlarmClockRingStopButton#��[  6�   ��6   �� *CloseButtonCommand+��6<   �� $ ReminderRingViewWindow��6�   �� 5      $��稍后提醒��6     OnlyContentButtonStyle#��[ 6�   ���	#FF0490FF$��Microsoft YaHei)�6�   $��18(�5      $
��Left=�6#   $
��Left=�6>   $	��Top=�6`   $	��Top=�6   $��213,439,0,0q�6�   ��5   $   �� *NextButtonCommand+��6J   �� $ ReminderRingViewWindow����6�   Z� ��6�   Z� 5   �   5      5       5!      