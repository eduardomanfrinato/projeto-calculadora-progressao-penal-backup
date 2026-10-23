using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CalculadoraProgressaoWeb.Models;
using CalculadoraProgressaoWeb.Services;

namespace CalculadoraProgressaoWeb.Pages
{
    public class CalculadoraModel : PageModel
    {
        private readonly ProgressaoService _progressaoService;
        private readonly LivramentoService _livramentoService;
        private readonly ProgressaoPdfService _pdfService;

        public CalculadoraModel(
            ProgressaoService progressaoService,
            LivramentoService livramentoService,
            ProgressaoPdfService pdfService)
        {
            _progressaoService = progressaoService;
            _livramentoService = livramentoService;
            _pdfService = pdfService;
        }

        [BindProperty]
        public ProgressaoInput Input { get; set; } = new();

        [BindProperty]
        public PdfInstitucionalInput PdfInstitucional { get; set; } = new();

        public ProgressaoResultado? Resultado { get; set; }
        public LivramentoResultado? ResultadoLivramento { get; set; }

        // 🔒 CONTROLE PRO (versão demo)
        public bool IsPro => false;

        public void OnGet() { }

        // ============================
        // CALCULAR
        // ============================
        public void OnPostCalcular()
        {
            Resultado = _progressaoService.Calcular(Input);
            ResultadoLivramento = _livramentoService.Calcular(Input);
        }

        // ============================
        // GERAR PDF
        // ============================
        public IActionResult OnPostGerarPdf()
        {
            Resultado = _progressaoService.Calcular(Input);

            byte[]? logoBytes = null;

            if (PdfInstitucional.Logo != null)
            {
                using var ms = new MemoryStream();
                PdfInstitucional.Logo.CopyTo(ms);
                logoBytes = ms.ToArray();
            }

            var pdfBytes = _pdfService.GerarPdf(
                Input,
                Resultado,
                PdfInstitucional,
                logoBytes,
                IsPro);

            return File(
                pdfBytes,
                "application/pdf",
                "Relatorio_Progressao_Regime.pdf");
        }
    }
}
