﻿using FirstFloor.ModernUI.Windows.Controls;
using MovieCatalog.Helper_Functions;
using MovieCatalogLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MovieCatalogLibrary.DatabaseHandling;
using FirstFloor.ModernUI.Presentation;

namespace MovieCatalog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            FileHandler fileHandler = new FileHandler(FileHandler.platformType.Windows);
            fileHandler.verifyUserFile();
            InitializeComponent();
            Global.userLink = this.TitleLinks.ElementAt(1);
        }
    }
}
