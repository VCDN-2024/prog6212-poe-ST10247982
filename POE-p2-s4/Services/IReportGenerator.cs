using QuestPDF.Infrastructure;

namespace POE_p2_s4.Services
{
    public interface IReportGenerator
    {
        DocumentMetadata GetMetadata();
        DocumentSettings GetSettings();
        void Compose(IDocumentContainer container);
    }
}
