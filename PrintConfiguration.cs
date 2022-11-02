using Bugarinov.Shared.Common.Core;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;

namespace Bugarinov.Shared.Printing
{
    public class PrintConfiguration : ObservableObject
    {
        public IEnumerable<string> Printers { get; }
        public PaperSize[] PaperSizes { get; }
        public Orientation[] Orientations { get; }



        private string SelectedPrinter_;
        public string SelectedPrinter
        {
            get => SelectedPrinter_;
            set
            {
                SelectedPrinter_ = value;
                RaisePropertyChangedEvent(nameof(SelectedPrinter));
            }
        }

        private PaperSize SelectedPaper_;
        public PaperSize SelectedPaper
        {
            get => SelectedPaper_;
            set
            {
                SelectedPaper_ = value;
                RaisePropertyChangedEvent(nameof(SelectedPaper));
            }
        }

        private Orientation SelectedOrientation_;
        public Orientation SelectedOrientation
        {
            get => SelectedOrientation_;
            set
            {
                SelectedOrientation_ = value;
                RaisePropertyChangedEvent(nameof(SelectedOrientation));
            }
        }

        private int Copies_;
        public int Copies
        {
            get => Copies_;
            set
            {
                Copies_ = value;
                RaisePropertyChangedEvent(nameof(Copies));
            }
        }



        public PrintConfiguration()
        {
            Printers = PrinterSettings.InstalledPrinters.Cast<string>();

            PaperSizes = new PaperSize[]
            {
                new PaperSize()
                {
                    Name = "A4 (8.3x11.7)",
                    PageMediaSizeName = PageMediaSizeName.ISOA4,
                    Width = 796.8,
                    Height = 1123.2
                },
                new PaperSize()
                {
                    Name = "Letter (8.5x11)",
                    PageMediaSizeName = PageMediaSizeName.NorthAmericaLetter,
                    Width = 816,
                    Height = 1056
                },
                new PaperSize()
                {
                    Name = "Folio (8.5x13)",
                    PageMediaSizeName = PageMediaSizeName.OtherMetricFolio,
                    Width = 816,
                    Height = 1248
                },
                new PaperSize()
                {
                    Name = "Half-Folio Lengthwise (4.25x13)",
                    PageMediaSizeName = PageMediaSizeName.Unknown,
                    Width = 816,
                    Height = 1248
                },
                new PaperSize()
                {
                    Name = "Legal (8.5x14)",
                    PageMediaSizeName = PageMediaSizeName.NorthAmericaLegal,
                    Width = 816,
                    Height = 1344
                },
            };

            Orientations = new Orientation[]
            {
                new Orientation() { Name = "Portrait", PageOrientation = PageOrientation.Portrait },
                new Orientation() { Name = "Landscape", PageOrientation = PageOrientation.Landscape },
            };

            Copies = 1;
        }

        public virtual void Read() { }

        public virtual void Write() { }
    }
}
