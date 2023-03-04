using kahua.ktree.viewmodel;

using System.Collections.ObjectModel;
using System.Linq;

namespace UnoWasm.kahua.ktree.viewer.pdf
{
    public class PdfDocumentViewModel : NotifiableBase
    {
        private ObservableCollection<PdfPageViewModel> _pages;
        public ObservableCollection<PdfPageViewModel> Pages { get => _pages; set => Set(ref _pages, value); }
        private double _zoom;
        public double Zoom
        {
            get => _zoom;
            set
            {
                Set(ref _zoom, value);
                OnPropertyChanged(nameof(Height));
                OnPropertyChanged(nameof(Width));
            }
        }
        private int currentPageNumber;
        public int CurrentPageNumber { get => currentPageNumber; set => Set(ref currentPageNumber, value); }
        public double Height => Pages.Sum(p => p.Height) * Zoom;
        public double Width => Pages.Max(p => p.Width) * Zoom;
    }
}
