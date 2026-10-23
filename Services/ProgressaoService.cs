using CalculadoraProgressaoWeb.Models;

namespace CalculadoraProgressaoWeb.Services
{
    public class ProgressaoService
    {
        public ProgressaoResultado Calcular(ProgressaoInput input)
        {
            var resultado = new ProgressaoResultado();

            // ===============================
            // VALIDAÇÕES JURÍDICAS
            // ===============================

            if (input.DataBase.Date > DateTime.Today)
            {
                resultado.AlertasJuridicos.Add(
                    "A data-base não pode ser futura.");
            }

            if (input.Feminicidio)
            {
                input.TipoCrime = TipoCrimeEnum.Hediondo;
                input.ResultadoMorte = true;
                input.ViolenciaOuGraveAmeaca = true;
            }

            if (input.ResultadoMorte)
            {
                input.ViolenciaOuGraveAmeaca = true;
            }

            // ===============================
            // DEFINIÇÃO DO PERCENTUAL
            // ===============================

            int percentual;

            if (input.TipoCrime == TipoCrimeEnum.Hediondo)
            {
                if (input.ResultadoMorte)
                {
                    percentual = input.Reincidente ? 70 : 50;
                }
                else
                {
                    percentual = input.Reincidente ? 60 : 40;
                }
            }
            else
            {
                percentual = input.Reincidente ? 20 : 16;
            }

            resultado.PercentualAplicado = percentual;

            // ===============================
            // CÁLCULO DA PENA
            // ===============================

            int penaTotalDias =
                (input.PenaTotalMeses * 30) + input.PenaTotalDias;

            resultado.PenaTotalDias = penaTotalDias;

            int diasNecessarios = (penaTotalDias * percentual) / 100;
            resultado.DiasNecessarios = diasNecessarios;

            int diasAbatidos = input.DiasDetracao + input.DiasRemicao;
            resultado.DiasAbatidos = diasAbatidos;

            if (diasAbatidos > diasNecessarios)
            {
                diasAbatidos = diasNecessarios;
                resultado.AlertasJuridicos.Add(
                    "Remição e detração superam o tempo necessário para a progressão.");
            }

            resultado.DiasRestantes = diasNecessarios - diasAbatidos;
            resultado.DataProgressao = input.DataBase.AddDays(resultado.DiasRestantes);

            resultado.FundamentacaoLegal = GerarFundamentacao(input);

            return resultado;
        }

        private string GerarFundamentacao(ProgressaoInput input)
        {
            if (input.TipoCrime == TipoCrimeEnum.Hediondo)
            {
                if (input.ResultadoMorte)
                {
                    return input.Reincidente
                        ? "Nos termos do art. 112, inciso VIII, da Lei de Execução Penal, tratando-se de crime hediondo com resultado morte e reincidência, exige-se o cumprimento de 70% da pena."
                        : "Nos termos do art. 112, inciso VII, da Lei de Execução Penal, tratando-se de crime hediondo com resultado morte, exige-se o cumprimento de 50% da pena.";
                }

                return input.Reincidente
                    ? "Nos termos do art. 112, inciso VI, da Lei de Execução Penal, tratando-se de crime hediondo, exige-se o cumprimento de 60% da pena."
                    : "Nos termos do art. 112, inciso V, da Lei de Execução Penal, tratando-se de crime hediondo, exige-se o cumprimento de 40% da pena.";
            }

            return input.Reincidente
                ? "Nos termos do art. 112, inciso II, da Lei de Execução Penal, sendo reincidente, exige-se o cumprimento de 20% da pena."
                : "Nos termos do art. 112, inciso I, da Lei de Execução Penal, sendo primário, exige-se o cumprimento de 16% da pena.";
        }
    }
}
