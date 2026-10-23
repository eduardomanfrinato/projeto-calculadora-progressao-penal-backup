using System;
using System.Collections.Generic;

namespace CalculadoraProgressaoWeb.Models
{
    public class ProgressaoResultado
    {
        public int PenaTotalDias { get; set; }
        public int PercentualAplicado { get; set; }
        public int DiasNecessarios { get; set; }

        public int DiasAbatidos { get; set; }
        public int DiasRestantes { get; set; }

        public DateTime DataProgressao { get; set; }

        public string FundamentacaoLegal { get; set; } = string.Empty;

        public bool IsPro { get; set; } = false;

        public bool CalculoBloqueado { get; set; }

        public List<string> ErrosBloqueantes { get; set; } = new();

        // NOVO: Alertas jurídicos
        public List<string> AlertasJuridicos { get; set; } = new();
    }
}
