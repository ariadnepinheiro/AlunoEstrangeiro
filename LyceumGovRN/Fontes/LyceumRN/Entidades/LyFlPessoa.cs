namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class LyFlPessoa : IEntity
    {
        public Decimal Pessoa  { get; set; }

        /// <summary>
        /// local zona
        /// </summary>
        public string FlField01 { get; set; }

        /// <summary>
        /// Tipo Certidão
        /// </summary>
        public string FlField02 { get; set; }

        /// <summary>
        /// Recebe Escolarização em outro espaço
        /// </summary>
        public string FlField03 { get; set; }

        /// <summary>
        /// Gratuidade
        /// </summary>
        public string FlField04 { get; set; }

        /// <summary>
        /// Modais
        /// </summary>
        public string FlField05 { get; set; }

        /// <summary>
        /// Telefone do Responsavel pelo Aluno (Incluido pelo AOL)
        /// </summary>
        public string FlField06 { get; set; }

        /// <summary>
        /// Complemento Identidade
        /// </summary>
        public string FlField07 { get; set; }

        /// <summary>
        /// NIS / INEP
        /// </summary>
        public string FlField08 { get; set; }

        /// <summary>
        /// Modelo Certidao Civil
        /// </summary>
        public string FlField09 { get; set; }

        /// <summary>
        /// Transporte poder publico
        /// </summary>
        public string FlField10 { get; set; }

        /// <summary>
        /// Transporte rodoviario
        /// </summary>
        public string FlField11 { get; set; }

        /// <summary>
        /// Transporte aquaviario
        /// </summary>
        public string FlField12 { get; set; }

        /// <summary>
        /// Número de Dependentes
        /// </summary>
        public string FlField13 { get; set; }

        /// <summary>
        /// Número da CNH
        /// </summary>
        public string FlField14 { get; set; }

        /// <summary>
        /// Categoria da CNH
        /// </summary>
        public string FlField15 { get; set; }

        /// <summary>
        /// Validade da CNH
        /// </summary>
        public string FlField16 { get; set; }

        /// <summary>
        /// UF da CNH
        /// </summary>
        public string FlField17 { get; set; }

        /// <summary>
        /// UF do Titulo de Eleitor
        /// </summary>
        public string FlField18 { get; set; }

        /// <summary>
        /// UF do Certificado de Reservista
        /// </summary>
        public string FlField19 { get; set; }

        /// <summary>
        /// Operadora ônibus
        /// </summary>
        public string FlField20 { get; set; }

        /// <summary>
        /// Código do Povo Indígena
        /// </summary>
        public string FlField21 { get; set; } 

        /// <summary>
        /// Possui Transtorno que impacta o desenvolvimento da aprendizagem
        /// </summary>
        public string FlField22 { get; set; }

        /// <summary>
        /// Aluno se declara descendente de família integrante de comunidade quilombola ou indígena
        /// </summary>
        public string FlField23 { get; set; } 
    }
}
