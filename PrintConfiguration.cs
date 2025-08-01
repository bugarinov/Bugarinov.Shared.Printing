﻿using Bugarinov.Shared.Common.Core;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Printing;

namespace Bugarinov.Shared.Printing
{
    public class PrintConfiguration : ObservableObject
    {
        private readonly string ConfigFile;

        public IEnumerable<string> Printers { get; }
        public PrintMode[] PrintModes { get; set; }
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


        private PrintMode SelectedPrintMode_;
        public PrintMode SelectedPrintMode
        {
            get => SelectedPrintMode_;
            set
            {
                SelectedPrintMode_ = value;
                RaisePropertyChangedEvent(nameof(SelectedPrintMode));
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



        
        public PrintConfiguration(string configFile, params PaperSize[] additionalPaperSizes)
        {
            ConfigFile = configFile;

            Printers = PrinterSettings.InstalledPrinters.Cast<string>();

            List<PaperSize> basicPapers = new List<PaperSize>()
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
                    Name = "Legal (8.5x14)",
                    PageMediaSizeName = PageMediaSizeName.NorthAmericaLegal,
                    Width = 816,
                    Height = 1344
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
                    Name = "US Envelope #10 (4.125x9.5)",
                    PageMediaSizeName = PageMediaSizeName.NorthAmericaNumber10Envelope,
                    Width = 396,
                    Height = 912
                },
            };

            PaperSizes =  basicPapers.Concat(additionalPaperSizes).ToArray();

            Orientations = new Orientation[]
            {
                new Orientation() { Name = "Portrait", PageOrientation = PageOrientation.Portrait },
                new Orientation() { Name = "Landscape", PageOrientation = PageOrientation.Landscape },
            };

            PrintModes = new PrintMode[]
            {
                new PrintMode() { Name = "Per Page", Duplexing = Duplexing.OneSided },
                new PrintMode() { Name = "All", Duplexing = Duplexing.OneSided },
                new PrintMode() { Name = "Duplex", Duplexing = Duplexing.TwoSidedLongEdge }
            };

            SelectedPrintMode = PrintModes.FirstOrDefault();

            Copies = 1;
        }

        public void Read()
        {
            SelectedPrinter = null;
            SelectedPaper = null;
            SelectedOrientation = null;

            if (File.Exists(ConfigFile))
            {
                string[] settings = File.ReadAllLines(ConfigFile);

                string defaultPrinter = settings.Length > 0 ? settings[0] : null;
                string defaultPaper = settings.Length > 1 ? settings[1] : null;
                string defaultOrientation = settings.Length > 2 ? settings[2] : null;
                string printMode = settings.Length > 3 ? settings[3] : null;

                SelectedPrinter = defaultPrinter;
                SelectedPaper = PaperSizes.FirstOrDefault(x => x.Name.Equals(defaultPaper));
                SelectedOrientation = Orientations.FirstOrDefault(x => x.Name.Equals(defaultOrientation));
                SelectedPrintMode = PrintModes.FirstOrDefault(x => x.Name.Equals(printMode)) ?? PrintModes.First();
            }
        }

        public void Write()
        {
            try
            {
                if (File.Exists(ConfigFile))
                {
                    File.Delete(ConfigFile);
                }

                File.WriteAllLines(ConfigFile, new string[]
                {
                    SelectedPrinter,
                    SelectedPaper?.Name,
                    SelectedOrientation?.Name,
                    SelectedPrintMode.Name
                });
            }
            catch
            {

            }
            
        }
    }
}
