   M S B A M L   `   `   `  ���� F  BLifeService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:LifeService.View.ReminderViewLifeService.View.ReminderView  8    2LifeService.View.ReminderView.MemoReminderRingView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     =local+clr-namespace:LifeService.View.ReminderView     c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  -     �Name$  ReminderRingWindow�� � WindowStyle$
 None=� � WindowStartupLocation$ CenterScreen=�
������� � 
ResizeMode$ CanMinimize=� � Title$ MemoReminderRingView��$	��300��$	��500��.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ 	MouseDown��.  (System.Windows.Interactivity.TriggerBase   Actions. 6  0System.Windows.Interactivity.InvokeCommandAction     Command �� *$"ReminderRingWindowMouseDownCommand+	   CommandParameter	 �� 
 �� ElementName$
 ReminderRingWindow��.���� $��1q�
���   �._�(� .��� -    �Name$ maingird��$��90��.+��� $
��Left=�$��0,57,0,0q�$	��Top=�$��64��$��29��$��10(����� *NextAlarmTime+ �� UpdateSourceTrigger$ PropertyChanged=����� *NextButtonCommand+���� $
 ReminderRingWindow���� $��停止��$
��Left=�$��	69,57,0,0q�$	��Top=�$��61��$��29������ *CloseButtonCommand+���� $
 ReminderRingWindow����  �� 
IsReadOnly . $
��Auto=�$
��Left=� �� TextWrapping$
 Wrap=�$	��Top=�$��34��$	��130��$��0,18,0,0q����� *	Content+$ PropertyChanged=��� $��提醒时间到！��$
��Left=�$	��Top=�$��33�� J� RenderTransformOrigin$ 
0.444,0.16&�$��
-12,-6,0,0q�$��13(����	#FFE41616