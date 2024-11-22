using POE_p2_s4.Data;
using POE_p2_s4.Data.Migrations;
using POE_p2_s4.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Security.Claims;

namespace POE_p2_s4.Services
{
    public class ReportGenerator : IReportGenerator
    {
        public Invoice _invoice { get; }
        public readonly ApplicationDbContext _context;
        public ReportGenerator(Invoice invoice, ApplicationDbContext context)
        {
            _invoice = invoice;
            _context = context;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
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
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(5);

               

                column.Item().Element(ComposeTable);

                var totalPrice = _invoice.Claims.Sum(x => x.ClaimExpenses * _invoice.Claims.Count );
                column.Item().AlignRight().Text($"Cost to company: R {totalPrice}").FontSize(14);

                
            });
        }
        public void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                // step 1
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    switch (_invoice.Claims[0].ClaimType)
                    {
                        case "Travel":
                            columns.RelativeColumn();
                            break;
                        case "Leave":
                            columns.RelativeColumn();
                            break;
                    }
                });

                // step 2
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("#");
                    header.Cell().Element(CellStyle).Text("Claim type");
                    header.Cell().Element(CellStyle).AlignRight().Text("Description");
                    header.Cell().Element(CellStyle).AlignRight().Text("Claim date");
                    header.Cell().Element(CellStyle).AlignRight().Text("Email");
                    switch (_invoice.Claims[0].ClaimType)
                    {
                        case "Travel":
                            header.Cell().Element(CellStyle).AlignRight().Text("Kilometers travelled");
                            break;
                        case "Leave":
                            header.Cell().Element(CellStyle).AlignRight().Text("Leave days");
                            break;
                    }
                    header.Cell().Element(CellStyle).AlignRight().Text("Claim Expenses");
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    }
                });

                // step 3
                int count = 0;
                foreach (var claim in _invoice.Claims)
                {
                    Lecturer lecturer = (Lecturer)_context.Users.FirstOrDefault(u => u.Id == claim.UserId);
                    table.Cell().Element(CellStyle).Text(count++);
                    table.Cell().Element(CellStyle).Text(claim.ClaimType);

                    table.Cell().Element(CellStyle).AlignRight().Text(claim.Description);
                    table.Cell().Element(CellStyle).AlignRight().Text($"{claim.ClaimDate}");
                    table.Cell().Element(CellStyle).AlignRight().Text($"{lecturer.Email}");
                    switch (claim.ClaimType)
                    {
                        case "Travel":
                            table.Cell().Element(CellStyle).AlignRight().Text($"{claim.KilometersTravelled}");
                            break;
                        case "Leave":
                            table.Cell().Element(CellStyle).AlignRight().Text($"{claim.LeaveDays}");
                            break;
                    }
                    table.Cell().Element(CellStyle).AlignRight().Text($"R {claim.ClaimExpenses}");

                    static IContainer CellStyle(IContainer container)
                    {
                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                }
            });

        }
    }
}
