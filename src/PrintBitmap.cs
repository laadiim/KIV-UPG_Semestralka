using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

public class BitmapPrinter
{
    private Bitmap bitmapToPrint;

    public BitmapPrinter(Bitmap bitmap)
    {
        this.bitmapToPrint = bitmap;
    }

    public void PrintBitmap()
    {
        // Create a PrintDocument object
        PrintDocument printDocument = new PrintDocument();

        // Set the default page orientation to landscape
        printDocument.DefaultPageSettings.Landscape = true;

        // Hook up the event to print the bitmap
        printDocument.PrintPage += new PrintPageEventHandler(PrintPage);

        // Show the print dialog to choose printer settings
        PrintDialog printDialog = new PrintDialog();
        printDialog.Document = printDocument;

        if (printDialog.ShowDialog() == DialogResult.OK)
        {
            // Start printing if the user clicks OK
            printDocument.Print();
        }
    }

    private void PrintPage(object sender, PrintPageEventArgs e)
    {
        // Calculate the destination rectangle to center the image on the page
        Rectangle destRect = new Rectangle(0, 0, e.PageBounds.Width, e.PageBounds.Height);

        // Draw the bitmap onto the print page
        e.Graphics.DrawImage(bitmapToPrint, destRect);

        // Indicate that no more pages are to be printed
        e.HasMorePages = false;
    }
}