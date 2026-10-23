using System;
using System.Collections.Generic;

namespace CalculadoraProgressaoWeb.Models
{
    public class ProgressaoInput
    {
        // Identificação
        public string NomeApenado { get; set; } = string.Empty;
        public DateTime DataBase { get; set; } = DateTime.Today;
        public string RegimeInicial { get; set; } = "Fechado";

        // Pena
        public int PenaTotalMeses { get; set; }
        public int PenaTotalDias { get; set; }

        // Natureza do crime (CAMPO QUE ESTAVA FALTANDO)
        public TipoCrimeEnum TipoCrime { get; set; } = TipoCrimeEnum.Comum;

        // Qualificadoras
        public bool ViolenciaOuGraveAmeaca { get; set; }
        public bool ResultadoMorte { get; set; }
        public bool Feminicidio { get; set; }
        public bool OrganizacaoCriminosa { get; set; }

        // Situação jurídica
        public bool Reincidente { get; set; }
        public int DiasDetracao { get; set; }
        public int DiasRemicao { get; set; }
    }
}
