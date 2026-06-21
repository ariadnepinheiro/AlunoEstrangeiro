using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyCandidatoDocente : IEntity
    {
        public string DocenteCandidatoId { get; set; }

        public string Concurso { get; set; }

        public string Candidato { get; set; }

        public string Nome { get; set; }

        [AtributoCampo(Nome = "DT_NASC")]
        public DateTime? Dt_nasc { get; set; }

        public string Sexo { get; set; }

        [AtributoCampo(Nome = "NECESSIDADEESPECIALID")]
        public int? NecessidadeEspecialId { get; set; }

        [AtributoCampo(Nome = "NOME_MAE")]
        public string Nome_mae { get; set; }

        [AtributoCampo(Nome = "NOME_PAI")]
        public string Nome_pai { get; set; }

        [AtributoCampo(Nome = "ESTADO_CIVIL")]
        public string Estado_civil { get; set; }

        [AtributoCampo(Nome = "PAIS_NASC")]
        public string Pais_nasc { get; set; }

        public string Nacionalidade { get; set; }

        [AtributoCampo(Nome = "MUNICIPIO_NASC")]
        public string Municipio_nasc { get; set; }

        [AtributoCampo(Nome = "END_PAIS")]
        public string End_pais { get; set; }

        public string Cep { get; set; }

        [AtributoCampo(Nome = "END_MUNICIPIO")]
        public string End_municipio { get; set; }

        public string Endereco { get; set; }

        [AtributoCampo(Nome = "END_NUM")]
        public string End_num { get; set; }

        [AtributoCampo(Nome = "END_COMPL")]
        public string End_compl { get; set; }

        public string Bairro { get; set; }

        public string Fone { get; set; }

        public string Celular { get; set; }

        [AtributoCampo(Nome = "E_MAIL")]
        public string E_mail { get; set; }

        [AtributoCampo(Nome = "RG_TIPO")]
        public string Rg_tipo { get; set; }

        [AtributoCampo(Nome = "RG_NUM")]
        public string Rg_num { get; set; }

        [AtributoCampo(Nome = "RG_UF")]
        public string Rg_uf { get; set; }

        [AtributoCampo(Nome = "RG_EMISSOR")]
        public string Rg_emissor { get; set; }

        [AtributoCampo(Nome = "RG_DTEXP")]
        public DateTime? Rg_dtexp { get; set; }

        public string Cpf { get; set; }

        [AtributoCampo(Nome = "PIS_PASEP")]
        public string Pis_pasep { get; set; }

        [AtributoCampo(Nome = "CPROF_NUM")]
        public string Cprof_num { get; set; }

        [AtributoCampo(Nome = "CPROF_SERIE")]
        public string Cprof_serie { get; set; }

        [AtributoCampo(Nome = "CPROF_UF")]
        public string Cprof_uf { get; set; }

        [AtributoCampo(Nome = "CPROF_DTEXP")]
        public DateTime? Cprof_dtexp { get; set; }

        public string Categoria { get; set; }

        public string Nucleo { get; set; }

        [AtributoCampo(Nome = "AGRUPAMENTO_INGRESSO")]
        public string Agrupamento_ingresso { get; set; }

        public decimal Status { get; set; }

        [AtributoCampo(Nome = "STATUS_OBS")]
        public string StatusObservacao { get; set; }

        [AtributoCampo(Nome = "DT_CONVOCACAO")]
        public DateTime? DataConvocacao { get; set; }

        [AtributoCampo(Nome = "DT_APRESENTACAO")]
        public DateTime? DataApresentacao { get; set; }

        [AtributoCampo(Nome = "HORA_APRESENTACAO")]
        public DateTime? HoraApresentacao { get; set; }

        [AtributoCampo(Nome = "CDRH")]
        public string CDRH { get; set; }

        [AtributoCampo(Nome = "DT_CDRH")]
        public DateTime? DataCDRH { get; set; }

        [AtributoCampo(Nome = "CQHI")]
        public string CQHI { get; set; }

        [AtributoCampo(Nome = "DT_CQHI")]
        public DateTime? DataCQHI { get; set; }

        [AtributoCampo(Nome = "REMESSA_DOCUMENTO")]
        public string RemessaDocumento { get; set; }

        [AtributoCampo(Nome = "DT_REMESSA_DOCUMENTO")]
        public DateTime? DataRemessaDocumento { get; set; }

        [AtributoCampo(Nome = "DT_PROPOSTA")]
        public DateTime? Dt_proposta { get; set; }

        [AtributoCampo(Nome = "USULOGIN")]
        public string UsuarioLogin { get; set; }

        [AtributoCampo(Nome = "USUSENHA")]
        public string UsuarioSenha { get; set; }

        [AtributoCampo(Nome = "CARGA_HORARIA")]
        public int Carga_Horaria { get; set; }

        [AtributoCampo(Nome = "GI_SEPLAG")]
        public string GI_Seplag { get; set; }

        [AtributoCampo(Nome = "DHR_CADASTRO")]
        public DateTime? DataCadastro { get; set; }

        [AtributoCampo(Nome = "MUNICIPIO_PROC")]
        public string Municipio_proc { get; set; }

        public string Finalizado { get; set; }

        [AtributoCampo(Nome = "ETNIAID")]
        public int EtniaId { get; set; }

        [AtributoCampo(Nome = "COTAIDINSCRICAO")]
        public int CotaIdInscricao { get; set; }

        [AtributoCampo(Nome = "COTAIDCONVOCACAO")]
        public int CotaIdConvocacao { get; set; }

        [AtributoCampo(Nome = "IDFUNCIONAL")]
        public int? IdFuncional { get; set; }

        [AtributoCampo(Nome = "DATAVALIDACAOINSCRICAO")]
        public DateTime? DataValidacaoInscricao { get; set; }

        [AtributoCampo(Nome = "REGIONALID")]
        public int RegionalId { get; set; }

        public int Aulas_Alocadas { get; set; }
    }
}
