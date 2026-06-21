using Techne.Data;
using Techne.Lyceum.CR;

namespace Techne.Lyceum.RN.Custom
{
    public class Ly_serie_sufixoCustom : Ly_serie_sufixo.CustomBase
    {
        public override string PreInsert(Ly_serie_sufixo.Row row, TConnectionWritable cn)
        {
            return string.Empty;
        }

        public override string PreUpdate(Ly_serie_sufixo.Row row, TConnectionWritable cn)
        {
            return string.Empty;
        }

        public override string PreDelete(Ly_serie_sufixo.Row row, TConnectionWritable cn)
        {
            string curso = row.Curso;
            string turno = row.Turno;
            string curriculo = row.Curriculo;
            decimal? serie = row.Serie;
            string sufixo = row.Sufixo;

            if (!RN.Serie.VerificarPodeExcluirSufixo(curso, turno, curriculo, serie, sufixo))
                return "Não é possível realizar a exclusão.\nExistem turmas utilizando este Sufixo.";
            else
                return string.Empty;
        }
    }
}
