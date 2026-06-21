using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework.Exception;


namespace Proderj.DOL.Exception
{
	public class SubPeriodoLetivoException : ApplicationException
	{
		private readonly string mensagem;
		public override string Message
		{
			get
			{
				return GeraMensagem();
			}
		}


		private readonly TipoEnum tipo;
		public enum TipoEnum
		{
			NaoExisteSubPeriodoAtivo,
			NaoExisteSubPeriodoAtivoParaLancamentoDeCurriculoMinimo,
			NaoExistemBimestresValidosParaExibicao,
			NaoExistemSubperiodosValidosParaExibicao,
			SubperiodoInvalidoParaExibicao
		}

		public string TipoDeErro { get { return tipo.ToString(); } }
		
		public short Ano { get; private set; }
		public short Periodo { get; private set; }

		public SubPeriodoLetivoException(short ano, short periodo, TipoEnum tipoDeExcecao)
		{
			Ano = ano;
			Periodo = periodo;
			tipo = tipoDeExcecao;
			mensagem = GeraMensagem();
		}

		protected string GeraMensagem()
		{

			if (mensagem != null)
			{
				return mensagem;
			}
			else
			{
				string mensagemNova = SubPeriodoLetivoResource.ResourceManager.GetString(tipo.ToString());

				mensagemNova = String.Format(mensagemNova, Ano, Periodo);

				return mensagemNova;
			}
		}
	}
}
