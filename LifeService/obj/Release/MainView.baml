   M S B A M L   `   `   `  ����   LifeService    LifeService.MainView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  -     >�Name$  	Main_View�� � 	Resources  �� MergedDictionaries �� Source��  �� $ ButtonDictionary.xaml?��� $2 ,/LifeService;component/WindowDictionary.xaml?�%    .��� .+�� -    �Name$ 
windowGrid��.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ Loaded��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *ViewBoxCommand+	   CommandParameter	 �� 
 �� ElementName$
 
windowGrid��.+�(� -    (�Name$ Viewbox��$��5,5,5,5q�$	��150��$	��390��.��� $��0,0,0,0q�$	��150��$	��390��.+��� -    ��Name$ button��$	��Top=�$	��120��$	��110��   MainWeatherButton#��[  $��18,10,262,0q����� *WeatherCommand+���� $
 button��.���� $
��Fill=�����  �� Path$ WeatherIcon� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� -   $ button1��$��提醒��$
��Left=�$��
322,60,0,0q�$	��Top=�  MainReminderButton#��[ ���� *RemindCommand+���� $
 button1���� -   $ button2��$��闹钟��$
��Left=�$��
255,60,0,0q�$	��Top=�  MainAlarmClockButton#��[ ���� *MainAlarmClockCommand+���� $
 button2���� $
��Left=�$��
144,21,0,0q�$	��Top=�$��71��$	��115��$��60(�$��Century Gothic)�
����������� *ResultDegree+$ PropertyChanged=��� -    ��Name$ sorcllq��$��145,92,207,29q�$��Disabled=�$��Hidden=���Z�    $ Loaded��.    �� *CityScrollViewCommand+	 �� $
 sorcllq��.���� $
��Left=�$	��Top=�$��14(�$��Microsoft YaHei)�
����������� *
ResultCity+$ PropertyChanged=��� -   $ sorcll��$��55��$��188,92,147,29q�$��Disabled=�$��Hidden=���Z�    $ Loaded��.    �� *WeatherScrollViewCommand+	 �� $
 sorcll��.���� $
��Left=�$	��Top=�$��14(�$��Microsoft YaHei)�
����������� *ResultWeather+$ PropertyChanged=�