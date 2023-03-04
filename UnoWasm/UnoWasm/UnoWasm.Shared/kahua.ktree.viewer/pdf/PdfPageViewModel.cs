using kahua.ktree.viewmodel;

using Windows.Foundation;

namespace UnoWasm.kahua.ktree.viewer.pdf
{
    public class PdfPageViewModel : NotifiableBase
    {
        private double _width;
        public double Width { get => _width; set => Set(ref _width, value); }
        private double _height;
        public double Height { get => _height; set => Set(ref _height, value); }
        private bool _isInViewPort;
        public bool IsInViewPort { get => _isInViewPort; set => Set(ref _isInViewPort, value); }
        private int _pageNumber;
        public int PageNumber { get => _pageNumber; set => Set(ref _pageNumber, value); }
        public int PageIndex => PageNumber - 1;
        private Rect _pageRect;
        public Rect PageRect { get => _pageRect; set => Set(ref _pageRect, value); }
    }
}
