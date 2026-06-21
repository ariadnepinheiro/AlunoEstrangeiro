using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Certificacao.DTOs
{
    public class HistoricoEscolarDTO
    {
        //Informações para o cabeçalho
        private string nomeAluno;

        public string NomeAluno
        {
            get { return nomeAluno; }
            set { nomeAluno = value; }
        }

        private string cep;

        public string Cep
        {
            get { return cep; }
            set { cep = value; }
        }


        private DateTime? dataNascimento;

        public DateTime? DataNascimento
        {
            get { return dataNascimento; }
            set { dataNascimento = value; }
        }

        private string nacionalidade;

        public string Nacionalidade
        {
            get { return nacionalidade; }
            set { nacionalidade = value; }
        }

        private string naturalidade;

        public string Naturalidade
        {
            get { return naturalidade; }
            set { naturalidade = value; }
        }

        private string filiacaoPai;

        public string FiliacaoPai
        {
            get { return filiacaoPai; }
            set { filiacaoPai = value; }
        }


        private string filiacaoMae;

        public string FiliacaoMae
        {
            get { return filiacaoMae; }
            set { filiacaoMae = value; }
        }
        
        private string matriculaAluno;

        public string MatriculaAluno
        {
            get { return matriculaAluno; }
            set { matriculaAluno = value; }
        }

        private string estabelecimento;

        public string Estabelecimento
        {
            get { return estabelecimento; }
            set { estabelecimento = value; }
        }

        private string endereco;

        public string Endereco
        {
            get { return endereco; }
            set { endereco = value; }
        }

        private string estadoDocumento;

        public string EstadoDocumento
        {
            get { return estadoDocumento; }
            set { estadoDocumento = value; }
        }

        private string municipio;

        public string Municipio
        {
            get { return municipio; }
            set { municipio = value; }
        }
        
        //Informações para a lista disciplinas

        public string serie;

        public string Serie
        {
            get { return serie; }
            set { serie = value; }
        }

        public string ano;

        public string Ano
        {
            get { return ano; }
            set { ano = value; }
        }

        private string unidade_ens;

        public string Unidade_ens
        {
            get { return unidade_ens; }
            set { unidade_ens = value; }
        }      

        public string escola;

        public string Escola
        {
            get { return escola; } 
            set { escola = value; }
        }

        public string grupoDisciplinas;

        public string GrupoDisciplinas
        {
            get { return grupoDisciplinas; }
            set { grupoDisciplinas = value;}
        }


        public string situacao;

        public string Situacao
        {
            get { return situacao; }
            set { situacao = value ; }
        }

        private string situacao_hist;

        public string Situacao_hist
        {
            get { return situacao_hist; }
            set { situacao_hist = value; }
        }

        public decimal? nota_1b;

        public decimal? Nota_1b
        {
            get { return nota_1b; }
            set { nota_1b = value; }
        }    
        
        public decimal? nota_2b;

        public decimal? Nota_2b
        {
            get { return nota_2b; }
            set { nota_2b = value; }
        }

        public decimal? nota_3b;

        public decimal? Nota_3b
        {
            get { return nota_3b; }
            set { nota_3b = value; }
        }

        public decimal? nota_4b;

        public decimal? Nota_4b
        {
            get { return nota_4b; }
            set { nota_4b = value; }
        }

        public decimal? nota_geral;

        public decimal? Nota_geral
        {
            get { return nota_geral; }
            set { nota_geral = value; }
        }
        
        public decimal? carga_horaria;

        public decimal? CargaHoraria
        {
            get { return carga_horaria; }
            set { carga_horaria = value; }
        }

        public decimal? percentual_presenca;

        public decimal? PercentualPresenca
        {
            get { return percentual_presenca; }
            set { percentual_presenca = value; }
        }

        private string disciplina;

        public string Disciplina
        {
            get { return disciplina; }
            set { disciplina = value; }
        }

        private string modalidadenivel;

        public string Modalidadenivel
        {
            get { return modalidadenivel; }
            set { modalidadenivel = value; }
        }

        private string decreto;

        public string Decreto
        {
            get { return decreto; }
            set { decreto = value; }
        }

        private DateTime? dataDO;

        public DateTime? DataDO
        {
            get { return dataDO; }
            set { dataDO = value; }
        }

        private string ua;

        public string Ua
        {
            get { return ua; }
            set { ua = value; }
        }

        public string num_rg;

        public string Num_rg
        {
            get { return num_rg; }
            set { num_rg = value; }
        }

        private string orgaoExpedidor;

        public string OrgaoExpedidor
        {
            get { return orgaoExpedidor; }
            set { orgaoExpedidor = value; }
        }

        private string dataExpedicao;

        public string DataExpedicao
        {
            get { return dataExpedicao; }
            set { dataExpedicao = value; }
        }

        public string usuarioID;


        public string UsuarioID
        {
            get { return usuarioID; }
            set { usuarioID = value; }
        }


        private decimal? freqtotal;

        public decimal? Freqtotal
        {
            get { return freqtotal; }
            set { freqtotal = value; }
        }


        private string situacaoFinal;

        public string SituacaoFinal
        {
            get { return situacaoFinal; }
            set { situacaoFinal = value; }
        }


        private string turma;

        public string Turma
        {
            get { return turma; }
            set { turma = value; }
        }


        private string semestre;

        public string Semestre
        {
            get { return semestre; }
            set { semestre = value; }
        }


        private string dependencia;

        public string Dependencia
        {
            get { return dependencia; }
            set { dependencia = value; }
        }


        private string disciplinaReferencia;

        public string DisciplinaReferencia
        {
            get { return disciplinaReferencia; }
            set { disciplinaReferencia = value; }
        }


        private string serieReferencia;

        public string SerieReferencia
        {
            get { return serieReferencia; }
            set { serieReferencia = value; }
        }


        private string notaFinal;

        public string NotaFinal
        {
            get { return notaFinal; }
            set { notaFinal = value; }
        }


        private string pessoa;
        
        public string Pessoa
        {
            get { return pessoa; }
            set { pessoa = value; }
        }

        private string agrupamento;

        public string Agrupamento
        {
            get { return agrupamento; }
            set { agrupamento = value; }
        }


        public HistoricoEscolarDTO() { }


        public HistoricoEscolarDTO(string nomeAluno,
                                DateTime dataNascimento,
                                string nacionalidade,
                                string naturalidade,
                                string filiacaoPai,
                                string filiacaoMae,
                                string matriculaAluno,
                                string estabelecimento,
                                string endereco,
                                string municipio)
        {

            this.nomeAluno = nomeAluno;
            this.dataNascimento = dataNascimento;
            this.nacionalidade = nacionalidade;
            this.naturalidade = naturalidade;
            this.filiacaoMae = filiacaoMae;
            this.filiacaoPai = filiacaoPai;
            this.matriculaAluno = matriculaAluno;
            this.estabelecimento = estabelecimento;
            this.endereco = endereco;
            this.municipio = municipio;
        }

    }


    }

