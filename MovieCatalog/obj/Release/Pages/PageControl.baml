   M S B A M L   `   `   `  ����   MovieCatalog$    MovieCatalog.Pages.PageControl   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     -   $	��497��$��739.1��  � DataContext  ��  �� RelativeSource �� *Self+.���    ContentRoot#��[  $��10q�$
��auto��$
��auto��$��Stretch=�$��Stretch=� � RowDefinitions �� $	��22* ��� $
��288* ��� $
��167* � � ColumnDefinitions �� $
��425* ��� $
��294* �.+��� -    ��Name$ 	btnFilter��$��Apply Filter��$
��Left=�$	��Top=�$	��120��$��
301,72,0,0q�$��26��$��2���� -   $��tbName��$
��Left=�$��23��$��	10,65,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$	��120��$��2���� -    ��Name$ lvMovies��$
��Left=�$	��218��$	��Top=�$	��257��$��0,29,0,0q�$��1������ *MovieCollection+ �� UpdateSourceTrigger$ PropertyChanged=� �� Mode$ TwoWay=�	 �� View	 �� .P��� 
 �� Width$	
 150��$
��Name�� �� DisplayMemberBinding �� *name+�� $
 50��$
��Year�� �� *year+�� -    ��Name$ 	tbCatalog��  Heading2#��[ $��10,0,0,0q����� *DisplayString+�� $��DESCRIPTION��#��[ $��	28,0,20,0q�$��1��$��1���� -    ��Name$ 	imgPoster��$
��Left=�$	��218��$��
271,29,0,0q�$	��Top=�$	��150��$��1���� -    ��Name$ lblOnlineRating��$
��Left=�$��	6,227,0,0q�$	��Top=�$��1��$��1������ *OnlineRatingDisplay+�� -   $ lblUserRating��$	��...��$
��Left=�$��256,227,0,0q�$	��Top=�$��16��$��8��$��1��$��1���� $	��Top=�$	��148��$	��278��$
��Left=�$��6,29,0,0q�$��1��$��1��.���� -	   $ txtDescription��$
��Wrap=�$	��229������  �� Path$ DescriptionDisplay��� -
   $��tbGenres��$
��Left=� �� 
IsReadOnly . $��23��$��	6,248,0,0q�$
 Wrap=�$	��Top=�$	��278��$ �� HorizontalScrollBarVisibility$ Visible=�$��1��$��1������ *OnlineRatingDisplay+�� $��Filtering Options��$
��Left=�$��	10,24,0,0q�$	��Top=�$��16��$��88��$��2���� $��
Movie Name��$
��Left=�$��	14,45,0,0q�$	��Top=�$��16��$��68��$��2���� -   $ btnReset��$��Reset��$
��Left=�$��301,103,0,0q�$	��Top=�$��75��$��26��$��2���� $��Button��$
��Left=�$��1��$��281,248,0,0q�$	��Top=�$��0��$��1���� -   $ 	btnRemove��$��Remove Selections��$
��Left=�$��
10,252,0,0q�$	��Top=�$	��147��$��26��$��1���� 
�������$��1q�$
��Left=�$	��124��$��0,24,0,0q�$��2��$	��Top=�$	��433��$��2��