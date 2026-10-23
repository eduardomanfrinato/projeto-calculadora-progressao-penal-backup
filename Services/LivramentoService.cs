using CalculadoraProgressaoWeb.Models;

namespace CalculadoraProgressaoWeb.Services
{
    public class LivramentoService
    {
        public LivramentoResultado Calcular(ProgressaoInput input)
        {
            var resultado = new LivramentoResultado();

            // FEMINICÍDIO – VEDAÇÃO ABSOLUTA
            if (input.Feminicidio)
            {
                resultado.Cabivel = false;
                resultado.FundamentacaoLegal =
                    "O livramento condicional é vedado nos casos de feminicídio, nos termos do art. 83, V, do Código Penal.";
                return resultado;
            }

            // HEDIONDO COM RESULTADO MORTE
            if (input.TipoCrime == TipoCrimeEnum.Hediondo && input.ResultadoMorte)
            {
                resultado.Cabivel = false;
                resultado.FundamentacaoLegal =
                    "O livramento condicional é vedado para crimes hediondos com resultado morte, conforme art. 83, V, do Código Penal.";
                return resultado;
            }

            int percentual;

            if (input.TipoCrime == TipoCrimeEnum.Hediondo)
            {
                percentual = 66; // 2/3
            }
            else
            {
                percentual = input.Reincidente ? 50 : 33;
            }

            resultado.Cabivel = true;
            resultado.PercentualAplicado = percentual;

            int penaTotalDias =
                (input.PenaTotalMeses * 30) + input.PenaTotalDias;

            int diasNecessarios = (penaTotalDias * percentual) / 100;

            int diasAbatidos = input.DiasDetracao + input.DiasRemicao;
            if (diasAbatidos > diasNecessarios)
                diasAbatidos = diasNecessarios;

            resultado.DiasNecessarios = diasNecessarios - diasAbatidos;
            resultado.DataEstimada = input.DataBase.AddDays(resultado.DiasNecessarios);

            resultado.FundamentacaoLegal = GerarFundamentacao(input);

            return resultado;
        }

        private string GerarFundamentacao(ProgressaoInput input)
        {
            if (input.TipoCrime == TipoCrimeEnum.Hediondo)
                return "Nos termos do art. 83, V, do Código Penal, tratando-se de crime hediondo, exige-se o cumprimento de 2/3 da pena.";

            return input.Reincidente
                ? "Nos termos do art. 83, II, do Código Penal, sendo reincidente, exige-se o cumprimento de mais da metade da pena."
                : "Nos termos do art. 83, I, do Código Penal, sendo primário, exige-se o cumprimento de mais de 1/3 da pena.";
        }
    }
}
