using DocumentFormat.OpenXml.Packaging;
using UglyToad.PdfPig;

namespace NooshApp.Api.Helpers
{
    // A utility helper class designed to extract raw text content from uploaded CV files (PDF and DOCX)
    public static class CvTextExtractor
    {
        // Public entry point: determines file type by extension and delegates to the appropriate extraction method
        public static string ExtractText(string filePath)
        {
            // Get the file extension in lowercase to ensure reliable matching
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            
            // Switch expression to route the file to the correct text parser based on its format
            return extension switch
            {
                ".pdf" => ExtractFromPdf(filePath),
                ".docx" => ExtractFromDocx(filePath),
                _ => string.Empty // Returns an empty string if the file format is unsupported
            };
        }

        // Helper method to extract text out of a PDF document using the PdfPig library
        private static string ExtractFromPdf(string filePath)
        {
            // Open the PDF document securely using a disposable context
            using var document = PdfDocument.Open(filePath);
            var textBuilder = new System.Text.StringBuilder();
            
            // Loop through each page in the PDF, extracting its text and appending it to the builder
            foreach (var page in document.GetPages())
                textBuilder.AppendLine(page.Text);
                
            return textBuilder.ToString();
        }

        // Helper method to extract text out of a Word (.docx) document using OpenXML SDK
        private static string ExtractFromDocx(string filePath)
        {
            // Open the Word document in read-only mode (false) using a disposable context
            using var document = WordprocessingDocument.Open(filePath, false);
            
            // Access the main document body part
            var body = document.MainDocumentPart?.Document?.Body;
            
            // Return all internal text from the body, or an empty string if it's null/empty
            return body?.InnerText ?? string.Empty;
        }
    }
}