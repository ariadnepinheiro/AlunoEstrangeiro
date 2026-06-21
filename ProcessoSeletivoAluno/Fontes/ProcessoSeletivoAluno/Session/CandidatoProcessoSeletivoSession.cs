using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivoAluno.Sessao
{
    [Serializable]
    public class CandidatoProcessoSeletivoSessao
    {
        public int CandidatoId
        {
            get
            {
                if (HttpContext.Current.Session["CandidatoId"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["CandidatoId"]);
                else
                    return int.MinValue;
            }
            set
            {
                HttpContext.Current.Session["CandidatoId"] = value;
            }
        }

        public string NomeCandidato
        {
            get
            {
                if (HttpContext.Current.Session["NomeCandidato"] != null)
                    return HttpContext.Current.Session["NomeCandidato"] as string;
                else
                    return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["NomeCandidato"] = value;
            }
        }

        public string NomeMae
        {
            get
            {
                if (HttpContext.Current.Session["NomeMae"] != null)
                    return HttpContext.Current.Session["NomeMae"] as string;
                else
                    return string.Empty;
            }
            set
            {
                HttpContext.Current.Session["NomeMae"] = value;
            }
        }

        public Int64 NumeroInscricao
        {
            get
            {
                if (HttpContext.Current.Session["NumeroInscricao"] != null)
                    return Convert.ToInt64(HttpContext.Current.Session["NumeroInscricao"]);
                else
                    return Int64.MinValue;
            }
            set
            {
                HttpContext.Current.Session["NumeroInscricao"] = value;
            }
        }

        public DateTime DataNascimento
        {
            get
            {
                if (HttpContext.Current.Session["DataNascimento"] != null)
                    return Convert.ToDateTime(HttpContext.Current.Session["DataNascimento"]);
                else
                    return Convert.ToDateTime(HttpContext.Current.Session["DataNascimento"]);
            }
            set
            {
                HttpContext.Current.Session["DataNascimento"] = value;
            }
        }

        public Boolean CandidatoLogado
        {
            get
            {
                if (HttpContext.Current.Session["CandidatoLogado"] != null)
                    return Convert.ToBoolean(HttpContext.Current.Session["CandidatoLogado"]);
                else
                    return Convert.ToBoolean(HttpContext.Current.Session["CandidatoLogado"]);
            }
            set
            {
                HttpContext.Current.Session["CandidatoLogado"] = value;
            }
        }

        public Boolean CandidatoAcordoEdital
        {
            get
            {
                if (HttpContext.Current.Session["CandidatoAcordoEdital"] != null)
                    return Convert.ToBoolean(HttpContext.Current.Session["CandidatoAcordoEdital"]);
                else
                    return Convert.ToBoolean(HttpContext.Current.Session["CandidatoAcordoEdital"]);
            }
            set
            {
                HttpContext.Current.Session["CandidatoAcordoEdital"] = value;
            }
        }

        public Int32 AgendaID
        {
            get
            {
                if (HttpContext.Current.Session["AgendaID"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["AgendaID"]);
                else
                    return int.MinValue;
            }
            set
            {
                HttpContext.Current.Session["AgendaID"] = value;
            }
        }

        public Int32 ProcessoSeletivoID
        {
            get
            {
                if (HttpContext.Current.Session["ProcessoSeletivoID"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["ProcessoSeletivoID"]);
                else
                    return int.MinValue;
            }
            set
            {
                HttpContext.Current.Session["ProcessoSeletivoID"] = value;
            }
        }

        public int InscricaoId
        {
            get
            {
                if (HttpContext.Current.Session["InscricaoId"] != null)
                    return Convert.ToInt32(HttpContext.Current.Session["InscricaoId"]);
                else
                    return int.MinValue;
            }
            set
            {
                HttpContext.Current.Session["InscricaoId"] = value;
            }
        }

        public void EncerrarSessao()
        {
            HttpContext.Current.Session.RemoveAll();
        }

        public void LimpaSessaoCandidatoInscrito()
        {
            CandidatoId = int.MinValue;
            InscricaoId = int.MinValue;
            NumeroInscricao = Int64.MinValue;
        }
    }
}
