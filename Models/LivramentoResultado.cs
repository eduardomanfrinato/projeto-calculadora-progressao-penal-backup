using System;
using System.Collections.Generic;

namespace CalculadoraProgressaoWeb.Models
{
    public class LivramentoResultado
    {
        public bool Cabivel { get; set; }
        public int PercentualAplicado { get; set; }
        public int DiasNecessarios { get; set; }
        public DateTime? DataEstimada { get; set; }
        public string FundamentacaoLegal { get; set; } = string.Empty;
        public List<string> AlertasJuridicos { get; set; } = new();
    }
}
