   M S B A M L   `   `   `  ����   MovieCatalog#    MovieCatalog.Pages.SearchPage   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     $��678.744��  � DataContext  ��  �� RelativeSource �� *Self+.��� $��0,0,10,0q� � RowDefinitions �� $
��131* ��� $
��346* �.+��� $��SEARCH TITLE��   Heading2#��[  $��10,10,456,87q���  �� Orientation$ 
Horizontal=�$��
0,44,456,6q�.+��� -    ��Name$ 	txtSearch��$
��auto��$��	10,34,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$
��Left=�$	��196��$
��Auto=��� -    ��Name$ 	lvResults��$
��Left=�$	��241��$��	10,56,0,0q�$	��Top=�$	��331��$��1������ *MovieCollection+����  �� 
TargetType" ��.���  �� Property � ��	 �� Value$	 Stretch=�
 � 	Resources
   NameTemplate��%@   &	       �� .R��� $��0,0,1,1q�
�������$��	-6,0,-6,0q� �� View �� .P���  �� Width$	 140��$
��Name�� �� CellTemplate# [  �� DisplayMemberBinding �� *name+�� $	 140��$
��Year�� �� *year+�� $	 140��$	��MID�� �� *mid+�� -    ��Name$ 	btnSearch��$��search��$	��Top=�$
��Left=�$��75��$��10,9,0,0q�$��26��$��1���� -   $��btnAdd��$��Add Selected��$
��Left=�$��
10,302,0,0q�$	��Top=�$	��120��$��26��$��1���� -    ��Name$ imageSearch��$
��Left=�$	��253��$��
405,39,0,0q�$	��Top=�$	��207��$��2���� $	��Top=�$	��148��$	��278��$
��Left=�$��381,166,0,0q�$��1��.���� -    ��Name$ txtDescription��$
��Wrap=�$	��229������  �� Path$ Description��� -    ��Name$ lblTitle��$
��Left=�$��
405,10,0,0q�$	��Top=�$��24��$	��219������ *TitleDisplay+