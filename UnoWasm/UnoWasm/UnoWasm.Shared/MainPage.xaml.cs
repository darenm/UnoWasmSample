using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Windows.Foundation;
using Windows.Foundation.Collections;

using Uno.Extensions;
using UnoWasm.kahua.ktree.viewer.pdf;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UnoWasm
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random _random = new Random();
        public PdfDocumentViewModel ViewModel { get; set; } = new PdfDocumentViewModel 
        { 
            Zoom = 1.0
        };
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel.Pages = new System.Collections.ObjectModel.ObservableCollection<PdfPageViewModel>
            {
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 1},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 2},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 3},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 4},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 5},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 6},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 7},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 8},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 9},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 10},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 11},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 12},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 13},
                new PdfPageViewModel{ Height = _random.Next(600,1000), Width = _random.Next(300,1000), PageNumber = 14},
            };

            ViewModel.CurrentPageNumber = 5;

            this.Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // uno platform workaround for textbox paste support on webassembly

        }

        private void NextPage()
        {
            ViewModel.CurrentPageNumber = Math.Min(ViewModel.Pages.Count, ViewModel.DisplayPageNumber + 1);
        }

        private void PrevPage()
        {
            ViewModel.CurrentPageNumber = Math.Max(1, ViewModel.DisplayPageNumber - 1);
        }

    }
}
