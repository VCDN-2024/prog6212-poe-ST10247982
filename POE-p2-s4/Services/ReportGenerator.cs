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
        public void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(24).SemiBold().FontColor(Colors.Green.Medium).FontSize(24);
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"Invoice Id:{_invoice.Id}").Style(titleStyle);
                    column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").SemiBold();
                        text.Span($"{_invoice.CreatedDate:d}");
                    });


                });
                row.ConstantItem(100).Height(50).Placeholder();
            });
        }
        
        public void ComposeContent(IContainer container)
        {
            container
            .PaddingVertical(40)
            .Height(250)
            .Background(Colors.Grey.Lighten3)
            .AlignCenter()
            .AlignMiddle()
            .Text("Content").FontSize(16);
        }
       public void ComposeTable(IContainer container)
        {
            container
                .Height(250)
                .Background(Colors.Grey.Lighten3)
                .AlignCenter()
                .AlignMiddle()
                .Text("Table").FontSize(16);
        }

    }
}
