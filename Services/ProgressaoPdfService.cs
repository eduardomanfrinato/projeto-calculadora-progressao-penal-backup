using System.IO;
using System.Linq;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using CalculadoraProgressaoWeb.Models;

namespace CalculadoraProgressaoWeb.Services
{
    public class ProgressaoPdfService
    {
        public byte[] GerarPdf(
            ProgressaoInput input,
            ProgressaoResultado resultado,
            PdfInstitucionalInput institucional,
            byte[]? logoBytes,
            bool isPro)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);

                    // ===============================
                    // WATERMARK (VERSÃO FREE)
                    // ===============================
                    if (!isPro)
                    {
                        page.Background().AlignCenter().AlignMiddle().Text("VERSÃO DEMO")
                            .FontSize(64)
                            .Bold()
                            .FontColor(Colors.Grey.Lighten2);
                    }

                    // ===============================
                    // CABEÇALHO
                    // ===============================
                    page.Header().Row(row =>
                    {
                        if (logoBytes != null)
                        {
                            row.ConstantItem(120)
                               .Height(80)
                               .Image(logoBytes)
                               .FitArea();
                        }

                        row.RelativeItem().Column(col =>
                        {
                            col.Item().Text(institucional.NomeEscritorioOuAdvogado)
                                .FontSize(14)
                                .SemiBold();

                            if (!string.IsNullOrWhiteSpace(institucional.Oab))
                            {
                                col.Item().Text($"OAB: {institucional.Oab}")
                                    .FontSize(10);
                            }

                            col.Item().Text("Relatório de Progressão de Regime")
                                .FontSize(11)
                                .Italic();
                        });
                    });

                    // ===============================
                    // CONTEÚDO
                    // ===============================
                    page.Content().Column(col =>
                    {
                        col.Spacing(12);

                        col.Item().Text("Dados do Apenado").SemiBold();

                        col.Item().Text($"Nome: {(isPro ? input.NomeApenado : "Disponível na versão PRO")}");
                        col.Item().Text($"Data-base: {(isPro ? input.DataBase.ToString("dd/MM/yyyy") : "Disponível na versão PRO")}");
                        col.Item().Text($"Pena total: {(isPro ? $"{input.PenaTotalMeses} meses e {input.PenaTotalDias} dias" : "Disponível na versão PRO")}");

                        col.Item().LineHorizontal(1);

                        col.Item().Text("Resultado da Progressão").SemiBold();

                        col.Item().Text($"Percentual aplicado: {(isPro ? $"{resultado.PercentualAplicado}%" : "Disponível na versão PRO")}");
                        col.Item().Text($"Data estimada para progressão: {(isPro ? resultado.DataProgressao.ToString("dd/MM/yyyy") : "Disponível na versão PRO")}");

                        col.Item().LineHorizontal(1);

                        col.Item().Text("Fundamentação Jurídica").SemiBold();

                        if (isPro)
                        {
                            col.Item()
                               .Text(resultado.FundamentacaoLegal)
                               .WrapAnywhere();
                        }
                        else
                        {
                            col.Item()
                               .Text("A fundamentação jurídica completa deste cálculo está disponível exclusivamente na versão profissional do relatório.")
                               .Italic();
                        }

                        if (isPro && resultado.AlertasJuridicos.Any())
                        {
                            col.Item().LineHorizontal(1);
                            col.Item().Text("Alertas Jurídicos").SemiBold();

                            foreach (var alerta in resultado.AlertasJuridicos)
                                col.Item().Text($"• {alerta}");
                        }

                        if (isPro && !string.IsNullOrWhiteSpace(institucional.ObservacoesAdvogado))
                        {
                            col.Item().LineHorizontal(1);
                            col.Item().Text("Observações do Advogado").SemiBold();
                            col.Item().Text(institucional.ObservacoesAdvogado);
                        }
                    });

                    // ===============================
                    // RODAPÉ
                    // ===============================
                    page.Footer()
                        .AlignCenter()
                        .Text("Documento gerado pela CalculadoraProgressaoWeb • Uso profissional")
                        .FontSize(9)
                        .Italic();
                });
            });

            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
