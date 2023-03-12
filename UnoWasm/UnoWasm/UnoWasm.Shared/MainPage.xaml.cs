using System;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

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
            var tempPages = new System.Collections.ObjectModel.ObservableCollection<PdfPageViewModel>(); ;

            for (int i = 0; i < 1000; i++)
            {
                tempPages.Add(new PdfPageViewModel { Height = _random.Next(600, 1000), Width = _random.Next(300, 1000), PageNumber = i+1 });
            }
            ViewModel.Pages =tempPages;

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
