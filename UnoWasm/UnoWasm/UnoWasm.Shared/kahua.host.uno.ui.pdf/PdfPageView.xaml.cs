using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.ComponentModel;

using UnoWasm.kahua.ktree.viewer.pdf;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UnoWasm.kahua.host.uno.ui.pdf
{
    public sealed partial class PdfPageView : UserControl
    {

        #region ViewModel
        public PdfPageViewModel ViewModel
        {
            get { return (PdfPageViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PdfPageViewModel), typeof(PdfPageView), new PropertyMetadata(null, OnViewModelChanged));

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PdfPageView pageView)
            {
                if (e.OldValue is PdfPageViewModel oldPage)
                {
                    oldPage.PropertyChanged -= pageView.Page_PropertyChanged;
                }
                if (e.NewValue is PdfPageViewModel newPage)
                {
                    newPage.PropertyChanged += pageView.Page_PropertyChanged;
                }

                pageView.UpdatePage();
            }
        }

        private void Page_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(PdfPageViewModel.PageRect):
                    UpdatePage();
                    break;
            }
        }

        #endregion



        public string PoolID
        {
            get { return (string)GetValue(PoolIDProperty); }
            set { SetValue(PoolIDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PoolID.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PoolIDProperty =
            DependencyProperty.Register("PoolID", typeof(string), typeof(PdfPageView), new PropertyMetadata(""));



        public PdfPageView()
        {
            this.InitializeComponent();
        }

        private void UpdatePage()
        {
            if (ViewModel == null)
            {
                this.Width = 0;
                this.Height = 0;
                Canvas.SetLeft(this, 0);
                Canvas.SetTop(this, 0);
            }
            else
            {
                this.Width = ViewModel.PageRect.Width;
                this.Height = ViewModel.PageRect.Height;
                Canvas.SetLeft(this, ViewModel.PageRect.Left);
                Canvas.SetTop(this, ViewModel.PageRect.Top);
            }

            Bindings.Update();
        }

        private string FormattedPageID(int pageNumber) => $"{pageNumber}{(PoolID != "" ? $" : {PoolID}" : "")}";
    }
}
