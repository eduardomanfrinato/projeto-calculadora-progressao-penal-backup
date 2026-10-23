using Microsoft.AspNetCore.Http;

namespace CalculadoraProgressaoWeb.Models
{
    public class PdfInstitucionalInput
    {
        public string NomeEscritorioOuAdvogado { get; set; } = string.Empty;

        public string? Oab { get; set; }

        public string? ObservacoesAdvogado { get; set; }

        public string? CidadeEstado { get; set; }

        // Logo opcional
        public IFormFile? Logo { get; set; }
    }
}
