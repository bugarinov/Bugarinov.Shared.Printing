using Bugarinov.Shared.Common.Core;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Bugarinov.Shared.Printing
{
    public class PrintingManager : ObservableObject
    {
        private PrintDialog PrintDialog;

        public double PrintableAreaWidth => PrintDialog?.PrintableAreaWidth ?? 0;
        public double PrintableAreaHeight => PrintDialog?.PrintableAreaHeight ?? 0;

        public void Init(
            string selectedPrinter,
            PaperSize paperSize,
            Orientation orientation,
            int copies = 1,
            PrintMode printMode = null)
        {
            string[] printer = ExtractPrinter(selectedPrinter);
            string printerHost = printer[0];
            string printerName = printer[1];

            PrintTicket printTicket = new PrintTicket()
            {
                CopyCount = copies,
                PageMediaSize = new PageMediaSize(paperSize.PageMediaSizeName, paperSize.Width, paperSize.Height),
                PageMediaType = PageMediaType.None,
                PageOrientation = orientation.PageOrientation,
                Duplexing = printMode?.Duplexing ?? Duplexing.OneSided
            };

            PrintDialog = new PrintDialog()
            {
                PrintQueue = new PrintQueue(new PrintServer(printerHost), printerName)
            };

            PrintDialog.PrintQueue.UserPrintTicket = PrintDialog.PrintQueue
                    .MergeAndValidatePrintTicket(PrintDialog.PrintQueue.DefaultPrintTicket, printTicket)
                    .ValidatedPrintTicket;

            PrintDialog.PrintQueue.Commit();
        }

        public void Print(IDocumentPaginatorSource document, PaperSize paperSize, string description)
        {
            document.DocumentPaginator.PageSize = new System.Windows.Size(paperSize.Width, paperSize.Height);
            PrintDialog.PrintDocument(document.DocumentPaginator, description);
        }

        private string[] ExtractPrinter(string printerName)
        {
            string printer = printerName.Trim('\\');

            if (printer.Contains("\\"))
            {
                string[] printerInfo = printer.Split('\\');

                return new string[]
                {
                    $"\\\\{printerInfo[0]}",
                    printerInfo[1]
                };
            }

            return new string[]
            {
                null,
                printer
            };
        }
    }
}
