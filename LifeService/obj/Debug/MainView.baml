   M S B A M L   `   `   `  ����  LifeService    LifeService.MainView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      -   5          >�Name$  	Main_View��5       � 	Resources 5
       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ ButtonDictionary.xaml?�6%   �� 5      $2 ,/LifeService;component/WindowDictionary.xaml?�6%   %    5      .��5      � .+�5   
   � -   5         �Name$ 
windowGrid��5      .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5         	EventName$ Loaded��6!   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6,   �� *ViewBoxCommand+	   CommandParameter	 6O   �� 
 �� ElementName$
 
windowGrid��5      .+�5      (� -   5         (�Name$ Viewbox��5      $��5,5,5,5q�6'   $	��150��68   $	��390��6E   .��5      � $��0,0,0,0q�6   $	��150��6(   $	��390��65   .+�5      �� -   5         ��Name$ button��5      $	��Top=�5      $	��120��65   $	��110��6B      MainWeatherButton#��[  6N   $��18,10,262,0q�5   p   ��6   �� *WeatherCommand+��6@   �� $
 button��.��5      �� $
��Fill=�6t   ��6    ��  �� Path$ WeatherIcon� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�6t   5      �� 5       -   5        $ button1��5       $��提醒��6.   $
��Left=�6;   $��
322,60,0,0q�6V   $	��Top=�5!        MainReminderButton#��[ 65   ��6a   �� *RemindCommand+��6�   �� $
 button1���� 5"      -   5        $ button2��5"      $��闹钟��6.   $
��Left=�6;   $��
255,60,0,0q�6V   $	��Top=�5#        MainAlarmClockButton#��[ 5$      ��6K   �� *MainAlarmClockCommand+��6u   �� $
 button2���� 5%      $
��Left=�6a   $��
144,21,0,0q�6|   $	��Top=�6�   $��71��6�   $	��115��6�   $��60(�6�   $��Century Gothic)�6�   
���������6   �� *ResultDegree+$ PropertyChanged=�6�   �� 5&      -   5         ��Name$ sorcllq��5&   #   $��145,92,207,29q�64   $��Disabled=�6K   $��Hidden=�6r   ��6�   Z�  5'        5(      $ Loaded��6-   . 5)   "      68   �� *CityScrollViewCommand+	 6b   �� $
 sorcllq��5*      .��5,      �� $
��Left=�6c   $	��Top=�6~   $��14(�6�   $��Microsoft YaHei)�6�   
���������6    �� *
ResultCity+$ PropertyChanged=�6�   5-      �� 5.      -   5        $ sorcll��5.   #   $��55��63   $��188,92,147,29q�6>   $��Disabled=�6U   $��Hidden=�6|   ��6�   Z�  5/        50      $ Loaded��6-   . 51   "      68   �� *WeatherScrollViewCommand+	 6e   �� $
 sorcll��52      .��54      �� $
��Left=�6g   $	��Top=�6�   $��14(�6�   $��Microsoft YaHei)�6�   
���������6    �� *ResultWeather+$ PropertyChanged=�6�   55      58      59      5:      5;      5<      