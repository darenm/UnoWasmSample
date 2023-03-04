using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using UnoWasm.kahua.ktree.viewer.pdf;

using Windows.Foundation;

namespace UnoWasm.kahua.host.uno.ui.pdf
{
    public sealed partial class PdfPagesViewer : UserControl
    {
        private const int INITIAL_PAGE_POOL_SIZE = 6; // initial size of the page pool
        private int _lastPoolId = 1;
        private CancellationTokenSource _renderTokenSource;
        private List<PdfPageView> _pageViewPool; // all of the page instances
        private List<PdfPageView> _pageViewsInUse; // those that are currently in view
        private Stack<PdfPageView> _pageViewsAvailable; // hidden pages available for use

        public PdfDocumentViewModel PdfDocument
        {
            get { return (PdfDocumentViewModel)GetValue(PdfDocumentProperty); }
            set { SetValue(PdfDocumentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PdfDocument.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PdfDocumentProperty =
            DependencyProperty.Register("PdfDocument", typeof(PdfDocumentViewModel), typeof(PdfPagesViewer), new PropertyMetadata(null, OnPdfDocumentChanged));

        private static void OnPdfDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PdfPagesViewer viewer)
            {
                if (e.OldValue is PdfDocumentViewModel oldDoc)
                {
                    oldDoc.Pages.CollectionChanged -= viewer.Pages_CollectionChanged;
                    oldDoc.PropertyChanged -= viewer.Document_PropertyChanged;
                }
                if (e.NewValue is PdfDocumentViewModel newDoc)
                {
                    newDoc.Pages.CollectionChanged += viewer.Pages_CollectionChanged;
                    newDoc.PropertyChanged += viewer.Document_PropertyChanged;
                }
                viewer.UpdatePages();
            }
        }

        private void Document_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(PdfDocumentViewModel.Zoom):
                    UpdatePages();
                    break;
            }
        }

        public PdfPagesViewer()
        {
            this.InitializeComponent();

            ViewPortScroller.ViewChanged += _viewPortScroller_ViewChanged;
            ViewPortScroller.SizeChanged += _viewPortScroller_SizeChanged;
            this.Loaded += _pdfPagesViewer_Loaded;
            this.Unloaded += _pdfPagesViewer_Unloaded;
        }

        private void _viewPortScroller_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_renderTokenSource != null)
            {
                _renderTokenSource.Cancel();
                _renderTokenSource = null;
            }
            _renderTokenSource = new CancellationTokenSource();
            RenderViewPort(_renderTokenSource);
        }

        private void _viewPortScroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (_renderTokenSource != null)
            {
                _renderTokenSource.Cancel();
                _renderTokenSource = null;
            }
            _renderTokenSource = new CancellationTokenSource();
            RenderViewPort(_renderTokenSource);
        }

        private void _pdfPagesViewer_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Low,
                () =>
                {
                    _pageViewPool = new List<PdfPageView>();
                    _pageViewsAvailable = new Stack<PdfPageView>();
                    _pageViewsInUse = new List<PdfPageView>();

                    for (int i = 0; i < INITIAL_PAGE_POOL_SIZE; i++)
                    {
                        var page = new PdfPageView() { Visibility = Visibility.Collapsed, PoolID = $"P{_lastPoolId++:D3}" };
                        _pageViewPool.Add(page);
                        _pageViewsAvailable.Push(page);
                        PagesCanvas.Children.Add(page);
                    }

                    UpdatePages();
                });
        }

        private void _pdfPagesViewer_Unloaded(object sender, RoutedEventArgs e)
        {
            _pageViewPool.Clear();
            _pageViewPool = null;

            _pageViewsAvailable.Clear();
            _pageViewsAvailable = null;

            _pageViewsInUse.Clear();
            _pageViewsInUse = null;
        }

        private void RenderViewPort(CancellationTokenSource renderTokenSource)
        {
            if (!IsLoaded)
            {
                return;
            }

            // find all of the pages that should be visible in the viewport
            foreach (var page in PdfDocument.Pages)
            {
                if (renderTokenSource.IsCancellationRequested)
                {
                    break;
                }
                var viewPortRect = new Rect(ViewPortScroller.HorizontalOffset, ViewPortScroller.VerticalOffset, ViewPortScroller.ViewportWidth, ViewPortScroller.ViewportHeight);
                viewPortRect.Intersect(page.PageRect);
                var isInViewPort = viewPortRect.Height > 0 && viewPortRect.Width > 0;
                if (!isInViewPort && page.IsInViewPort)
                {
                    DeallocatePageView(page);
                }

                if (isInViewPort && !page.IsInViewPort)
                {
                    // page isn't currently allocated to a visible pageview
                    AllocatePageView(page);
                }

                page.IsInViewPort = isInViewPort;
            }
        }

        private void UpdatePages()
        {
            if (_renderTokenSource != null)
            {
                _renderTokenSource.Cancel();
                _renderTokenSource = null;
            }

            _renderTokenSource = new CancellationTokenSource();
            _ = RenderPagesAsync(_renderTokenSource);
        }

        private async Task RenderPagesAsync(CancellationTokenSource renderTokenSource)
        {
            try
            {
                var heightOffset = 0.0;
                foreach (var page in PdfDocument.Pages)
                {
                    page.PageRect = new Rect(PdfDocument.Width/2 - (page.Width * PdfDocument.Zoom)/2, heightOffset, page.Width * PdfDocument.Zoom, page.Height * PdfDocument.Zoom);
                    heightOffset += page.Height * PdfDocument.Zoom;
                    if (renderTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                }

                if (renderTokenSource.IsCancellationRequested)
                {
                    return;
                }
                RenderViewPort(renderTokenSource);
            }
            catch (TaskCanceledException)
            {
                // just exit
            }
            catch (Exception)
            {
                // logging?
                throw;
            }
        }

        private void Pages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // this is fired if pages are added/removed from the collection - won't usually happen
        }

        private void DeallocatePageView(PdfPageViewModel pdfPageViewModel)
        {
            var pageViews = _pageViewPool.Where(p => p.ViewModel == pdfPageViewModel);
            if (pageViews.Any())
            {
                foreach (var pageView in pageViews)
                {
                    System.Diagnostics.Debug.WriteLine($"- {pageView.PoolID}");
                    pageView.Visibility = Visibility.Collapsed;
                    pageView.ViewModel = null;
                    _pageViewsInUse.Remove(pageView);
                    _pageViewsAvailable.Push(pageView);
                }
            }
        }

        private void AllocatePageView(PdfPageViewModel pdfPageViewModel)
        {
            if (_pageViewsAvailable.Any())
            {
                // do we need to tweak the size of the pool downwards?
                if (_pageViewPool.Count > INITIAL_PAGE_POOL_SIZE && _pageViewsAvailable.Count > 2)
                {
                    while(_pageViewsAvailable.Count > 2)
                    {
                        // let's remove one from the pool
                        var pageViewToRemove = _pageViewsAvailable.Pop();
                        System.Diagnostics.Debug.WriteLine($"deleted {pageViewToRemove.PoolID}");
                        _pageViewPool.Remove(pageViewToRemove);
                        PagesCanvas.Children.Remove(pageViewToRemove);
                    }
                }
            }
            else
            {
                // we need a larger pool.
                var newPageView = new PdfPageView() { Visibility = Visibility.Collapsed, PoolID = $"P{_lastPoolId++:D3}" };
                System.Diagnostics.Debug.WriteLine($"created {newPageView.PoolID}");
                _pageViewPool.Add(newPageView);
                _pageViewsAvailable.Push(newPageView);
                PagesCanvas.Children.Add(newPageView);
            }

            var pageView = _pageViewsAvailable.Pop();
            _pageViewsInUse.Add(pageView);
            System.Diagnostics.Debug.WriteLine($"+ {pageView.PoolID}");
            pageView.ViewModel = pdfPageViewModel;
            pageView.Visibility = Visibility.Visible;
        }
    }
}
