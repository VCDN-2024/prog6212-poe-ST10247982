using POE_p2_s4.Data.Migrations;
using POE_p2_s4.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace POE_p2_s4.Services
{
    public class ReportGenerator : IReportGenerator
    {
     public Invoice _invoice { get; }

        public ReportGenerator(Invoice invoice)
        {
            _invoice = invoice;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container){
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Header().Height(100).Background(Colors.Grey.Lighten1);
                    page.Content().Background(Colors.Grey.Lighten3);
                    page.Footer().Height(50).Background(Colors.Grey.Lighten1);

                });
        }
    }
}
