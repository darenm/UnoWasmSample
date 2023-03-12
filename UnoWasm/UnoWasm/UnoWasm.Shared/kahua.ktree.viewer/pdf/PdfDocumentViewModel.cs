using System.Collections.ObjectModel;
using System.Linq;

using kahua.ktree.viewmodel;

namespace UnoWasm.kahua.ktree.viewer.pdf
{
    public class PdfDocumentViewModel : NotifiableBase
    {
        private ObservableCollection<PdfPageViewModel> _pages;
        public ObservableCollection<PdfPageViewModel> Pages { get => _pages; set => set(ref _pages, value); }
        private double _zoom;
        public double Zoom
        {
            get => _zoom;
            set
            {
                set(ref _zoom, value);
                onNotifyPropertyChanged(nameof(Height));
                onNotifyPropertyChanged(nameof(Width));
            }
        }
        private int _currentPageNumber;
        public int CurrentPageNumber { get => _currentPageNumber; set => set(ref _currentPageNumber, value); }
        private int _displayPageNumber;
        public int DisplayPageNumber { get => _displayPageNumber; set => set(ref _displayPageNumber, value); }
        public double Height => Pages.Sum(p => p.Height) * Zoom;
        public double Width => Pages.Max(p => p.Width) * Zoom;
    }
}
