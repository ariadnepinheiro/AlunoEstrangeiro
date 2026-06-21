using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Text;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Data;
using Techne.Web;
using Techne.Lyceum.CR;
using Techne.Library.Sql.Structure;

namespace Techne.Lyceum.RN
{
	public class LancFrequencia
	{
		[
			MethodDescription("Obtem a lista de alunos para lançamento de frequência"),
			ToolTip("Lista de alunos para lançamento de frequência"),
			ControlText("Lista de frequência"),
			Image("~/Images/Proc.gif"),
			]
		public static QueryTable ListaImagem(Number imagem, Number lista,
											   Number ano, Number semestre, VarChar disciplina, VarChar turma)
		{
			LyceumUser lyceumuser = LyceumUser.Get(Techne.Lyceum.LyceumUser.Current.User);
			bool pvprivil = lyceumuser.Privilegiado;
			//bool pvrestricao_simnao = lyceumuser.Restricao_SimNao;
			//bool pvrestricao_curso = lyceumuser.Restricao_Curso;
			bool pvrestricao_unidade = lyceumuser.Restricao_Unidade;
			bool pvrestricao_unid_fis = lyceumuser.Restricao_Unid_Fis;
			QueryTable qt;

			TConnection cn = Config.CreateConnection();
			qt = new QueryTable("");
			cn.Open();
			try
			{
				VarChar usuario = LyceumUser.Current.User.ToString();

				//        string sql = "usuario = ?";
				//        Ly_usuario_unidade usuario_unidade = Techne.Lyceum.CR.Ly_usuario_unidade.Query(cn,sql,usuario);                
				//        Ly_usuario_cursos usuario_cursos = Techne.Lyceum.CR.Ly_usuario_cursos.Query(cn,sql,usuario);

				VarChar sitturma = (VarChar)TCommand.ExecuteScalar(cn,
						   "SELECT sit_turma FROM ly_turma WHERE disciplina=? AND turma=? AND ano=? AND semestre=?",
									 disciplina, turma, ano, semestre);

				if (pvprivil)
				{
					if (sitturma != "Desativada")
					{
						qt = new QueryTable("SELECT ly_matricula.num_chamada, ly_aluno.aluno, ly_aluno.nome_compl, LY_COMP_LISTA.lista, " +
						  "       vw.disciplina, vw.turma, vw.ano, vw.semestre, vw.imagem, " +
						  "       CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Falta' THEN 'S' ELSE 'N' END falta, " +
						  "       CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Cancelamento' THEN 'S' ELSE 'N' END cancelada, " +
						  "       CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Abonada' THEN 'S' ELSE 'N' END abonada, " +
						  "      (SELECT COUNT (OCORRENCIA) " +
						  "       FROM LY_COMP_LISTA " +
						  "       WHERE ALUNO = vw.aluno AND ANO = vw.ano AND SEMESTRE = vw.semestre AND OCORRENCIA = 'Abonada') total " +
						  "FROM LY_ALUNO INNER JOIN " +
						  "     LY_MATRICULA ON LY_ALUNO.ALUNO = LY_MATRICULA.ALUNO INNER JOIN " +
						  "    (SELECT LY_COMP_IMAGEM.*, LY_LISTA_FREQ.lista " +
						  "     FROM LY_COMP_IMAGEM INNER JOIN " +
						  "	  LY_LISTA_FREQ ON LY_COMP_IMAGEM.disciplina = LY_LISTA_FREQ.disciplina AND  " +
						  "	                   LY_COMP_IMAGEM.turma = LY_LISTA_FREQ.turma AND " +
						  "	                   LY_COMP_IMAGEM.ano = LY_LISTA_FREQ.ano AND " +
						  "	                   LY_COMP_IMAGEM.semestre = LY_LISTA_FREQ.semestre AND  " +
						  "	                   LY_COMP_IMAGEM.imagem = LY_LISTA_FREQ.imagem " +
						  "     WHERE LY_COMP_IMAGEM.imagem = ? AND LY_COMP_IMAGEM.turma = ? AND " +
						  "           LY_COMP_IMAGEM.ano = ? AND LY_COMP_IMAGEM.semestre = ? AND " +
						  "           LY_COMP_IMAGEM.disciplina = ? and LY_LISTA_FREQ.lista = ? )vw " +
						  "      ON LY_MATRICULA.DISCIPLINA = vw.DISCIPLINA AND " +
						  "	 LY_MATRICULA.TURMA = vw.TURMA AND LY_MATRICULA.ANO = vw.ANO AND " +
						  "	 LY_MATRICULA.SEMESTRE = vw.SEMESTRE AND LY_ALUNO.ALUNO = vw.ALUNO LEFT JOIN " +
						  "	 LY_COMP_LISTA ON vw.disciplina = LY_COMP_LISTA.disciplina AND " +
						  "	                  vw.turma = LY_COMP_LISTA.turma AND " +
						  "	                  vw.ano = LY_COMP_LISTA.ano AND " +
						  "	                  vw.semestre = LY_COMP_LISTA.semestre AND " +
						  "	                  vw.lista = LY_COMP_LISTA.lista AND " +
						  "	                  vw.aluno = LY_COMP_LISTA.aluno " +
						  " ORDER BY ly_aluno.nome_compl");

						qt.Query(cn, imagem, turma, ano, semestre, disciplina, lista);

					}
					else
					{
						qt = new QueryTable("SELECT '0' as num_chamada, ly_aluno.aluno, ly_aluno.nome_compl, LY_COMP_LISTA.lista, " +
						  "vw.disciplina, vw.turma, vw.ano, vw.semestre, vw.imagem, " +
						  "CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Falta' THEN 'S' ELSE 'N' END falta, " +
						  "CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Cancelamento' THEN 'S' ELSE 'N' END cancelada, " +
						  "CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Abonada' THEN 'S' ELSE 'N' END abonada, " +
						  "(SELECT COUNT (OCORRENCIA) FROM LY_COMP_LISTA " +
						  "WHERE ALUNO = vw.aluno AND ANO = vw.ano AND  " +
						  "      SEMESTRE = vw.semestre AND OCORRENCIA = 'Abonada') total " +
						  "FROM LY_ALUNO INNER JOIN " +
						  "    (SELECT LY_COMP_IMAGEM.*, LY_LISTA_FREQ.lista " +
						  "     FROM LY_COMP_IMAGEM INNER JOIN " +
						  "	  LY_LISTA_FREQ ON LY_COMP_IMAGEM.disciplina = LY_LISTA_FREQ.disciplina AND  " +
						  "	                   LY_COMP_IMAGEM.turma = LY_LISTA_FREQ.turma AND " +
						  "	                   LY_COMP_IMAGEM.ano = LY_LISTA_FREQ.ano AND " +
						  "	                   LY_COMP_IMAGEM.semestre = LY_LISTA_FREQ.semestre AND  " +
						  "	                   LY_COMP_IMAGEM.imagem = LY_LISTA_FREQ.imagem " +
						  "     WHERE LY_COMP_IMAGEM.imagem = ? AND LY_COMP_IMAGEM.turma = ? AND " +
						  "           LY_COMP_IMAGEM.ano = ? AND LY_COMP_IMAGEM.semestre = ? AND " +
						  "           LY_COMP_IMAGEM.disciplina = ? and LY_LISTA_FREQ.lista = ? )vw " +
						  "	 ON LY_ALUNO.ALUNO = vw.ALUNO LEFT JOIN " +
						  "	 LY_COMP_LISTA ON vw.disciplina = LY_COMP_LISTA.disciplina AND " +
						  "	 vw.turma = LY_COMP_LISTA.turma AND " +
						  "	 vw.ano = LY_COMP_LISTA.ano AND " +
						  "	 vw.semestre = LY_COMP_LISTA.semestre AND " +
						  "	 vw.lista = LY_COMP_LISTA.lista AND " +
						  "	 vw.aluno = LY_COMP_LISTA.aluno " +
						  "  ORDER BY ly_aluno.nome_compl ");

						qt.Query(cn, imagem, turma, ano, semestre, disciplina, lista);

					}
				}

				if (!pvprivil && pvrestricao_unidade)
				{
					if (!pvprivil && pvrestricao_unid_fis)
					{
						if (sitturma != "Desativada")
						{
							qt = new QueryTable(@"
                                SELECT ly_matricula.num_chamada, ly_aluno.aluno, ly_aluno.nome_compl, LY_COMP_LISTA.lista, 
							         vw.disciplina, vw.turma, vw.ano, vw.semestre, vw.imagem, 
							         CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Falta' THEN 'S' ELSE 'N' END falta, 
							         CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Cancelamento' THEN 'S' ELSE 'N' END cancelada, 
							         CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Abonada' THEN 'S' ELSE 'N' END abonada, 
							        (SELECT COUNT (OCORRENCIA) 
							         FROM LY_COMP_LISTA 
							         WHERE ALUNO = vw.aluno AND ANO = vw.ano AND SEMESTRE = vw.semestre AND OCORRENCIA = 'Abonada') total 
							  FROM LY_ALUNO INNER JOIN 
							       LY_MATRICULA ON LY_ALUNO.ALUNO = LY_MATRICULA.ALUNO INNER JOIN 
							      (SELECT LY_COMP_IMAGEM.*, LY_LISTA_FREQ.lista 
							       FROM LY_COMP_IMAGEM INNER JOIN 
							  	  LY_LISTA_FREQ ON LY_COMP_IMAGEM.disciplina = LY_LISTA_FREQ.disciplina AND  
							  	                   LY_COMP_IMAGEM.turma = LY_LISTA_FREQ.turma AND 
							  	                   LY_COMP_IMAGEM.ano = LY_LISTA_FREQ.ano AND 
							  	                   LY_COMP_IMAGEM.semestre = LY_LISTA_FREQ.semestre AND  
							  	                   LY_COMP_IMAGEM.imagem = LY_LISTA_FREQ.imagem 
							       WHERE LY_COMP_IMAGEM.imagem = ? AND LY_COMP_IMAGEM.turma = ? AND 
							             LY_COMP_IMAGEM.ano = ? AND LY_COMP_IMAGEM.semestre = ? AND 
							             LY_COMP_IMAGEM.disciplina = ? and LY_LISTA_FREQ.lista = ? )vw 
							        ON LY_MATRICULA.DISCIPLINA = vw.DISCIPLINA AND 
							  	 LY_MATRICULA.TURMA = vw.TURMA AND LY_MATRICULA.ANO = vw.ANO AND 
							  	 LY_MATRICULA.SEMESTRE = vw.SEMESTRE AND LY_ALUNO.ALUNO = vw.ALUNO LEFT JOIN 
							  	 LY_COMP_LISTA ON vw.disciplina = LY_COMP_LISTA.disciplina AND 
							  	                  vw.turma = LY_COMP_LISTA.turma AND 
							  	                  vw.ano = LY_COMP_LISTA.ano AND 
							  	                  vw.semestre = LY_COMP_LISTA.semestre AND 
							  	                  vw.lista = LY_COMP_LISTA.lista AND 
							  	                  vw.aluno = LY_COMP_LISTA.aluno 
							    where 
                                    exists (select 1 from LY_USUARIO_UNIDADE uu where uu.USUARIO = ? and uu.FACULDADE = LY_ALUNO.UNIDADE_FISICA)
							    ORDER BY ly_aluno.nome_compl");

							qt.Query(cn, imagem, turma, ano, semestre, disciplina, lista, usuario);

						}
						else
						{
							qt = new QueryTable(@"
                            SELECT '0' as num_chamada, ly_aluno.aluno, ly_aluno.nome_compl, LY_COMP_LISTA.lista, 
							  vw.disciplina, vw.turma, vw.ano, vw.semestre, vw.imagem, 
							  CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Falta' THEN 'S' ELSE 'N' END falta, 
							  CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Cancelamento' THEN 'S' ELSE 'N' END cancelada, 
							  CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Abonada' THEN 'S' ELSE 'N' END abonada, 
							  (SELECT COUNT (OCORRENCIA) FROM LY_COMP_LISTA 
							  WHERE ALUNO = vw.aluno AND ANO = vw.ano AND  
							        SEMESTRE = vw.semestre AND OCORRENCIA = 'Abonada') total 
							  FROM LY_ALUNO INNER JOIN 
							      (SELECT LY_COMP_IMAGEM.*, LY_LISTA_FREQ.lista 
							       FROM LY_COMP_IMAGEM INNER JOIN 
							  	  LY_LISTA_FREQ ON LY_COMP_IMAGEM.disciplina = LY_LISTA_FREQ.disciplina AND  
							  	                   LY_COMP_IMAGEM.turma = LY_LISTA_FREQ.turma AND 
							                     LY_COMP_IMAGEM.ano = LY_LISTA_FREQ.ano AND 
							  	                   LY_COMP_IMAGEM.semestre = LY_LISTA_FREQ.semestre AND  
							  	                   LY_COMP_IMAGEM.imagem = LY_LISTA_FREQ.imagem 
							       WHERE LY_COMP_IMAGEM.imagem = ? AND LY_COMP_IMAGEM.turma = ? AND 
							             LY_COMP_IMAGEM.ano = ? AND LY_COMP_IMAGEM.semestre = ? AND 
							             LY_COMP_IMAGEM.disciplina = ? and LY_LISTA_FREQ.lista = ? )vw 
							  	 ON LY_ALUNO.ALUNO = vw.ALUNO LEFT JOIN 
							  	 LY_COMP_LISTA ON vw.disciplina = LY_COMP_LISTA.disciplina AND 
							  	 vw.turma = LY_COMP_LISTA.turma AND 
							  	 vw.ano = LY_COMP_LISTA.ano AND 
							  	 vw.semestre = LY_COMP_LISTA.semestre AND 
							  	 vw.lista = LY_COMP_LISTA.lista AND 
							  	 vw.aluno = LY_COMP_LISTA.aluno 
							    where 
                                    exists (select 1 from LY_USUARIO_UNIDADE uu where uu.USUARIO = ? AND uu.faculdade = LY_ALUNO.UNIDADE_FISICA)                                    
							    ORDER BY ly_aluno.nome_compl");

							qt.Query(cn, imagem, turma, ano, semestre, disciplina, lista, usuario);

						}

					}
					else
					{
						if (sitturma != "Desativada")
						{
							qt = new QueryTable(@"
                              SELECT ly_matricula.num_chamada, ly_aluno.aluno, ly_aluno.nome_compl, LY_COMP_LISTA.lista, 
							         vw.disciplina, vw.turma, vw.ano, vw.semestre, vw.imagem, 
							         CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Falta' THEN 'S' ELSE 'N' END falta, 
							         CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Cancelamento' THEN 'S' ELSE 'N' END cancelada, 
							         CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Abonada' THEN 'S' ELSE 'N' END abonada, 
							        (SELECT COUNT (OCORRENCIA) 
							         FROM LY_COMP_LISTA 
							         WHERE ALUNO = vw.aluno AND ANO = vw.ano AND SEMESTRE = vw.semestre AND OCORRENCIA = 'Abonada') total 
							  FROM LY_ALUNO INNER JOIN 
							       LY_MATRICULA ON LY_ALUNO.ALUNO = LY_MATRICULA.ALUNO INNER JOIN 
							      (SELECT LY_COMP_IMAGEM.*, LY_LISTA_FREQ.lista 
							       FROM LY_COMP_IMAGEM INNER JOIN 
							  	  LY_LISTA_FREQ ON LY_COMP_IMAGEM.disciplina = LY_LISTA_FREQ.disciplina AND  
							  	                   LY_COMP_IMAGEM.turma = LY_LISTA_FREQ.turma AND 
							  	                   LY_COMP_IMAGEM.ano = LY_LISTA_FREQ.ano AND 
							  	                   LY_COMP_IMAGEM.semestre = LY_LISTA_FREQ.semestre AND  
							  	                   LY_COMP_IMAGEM.imagem = LY_LISTA_FREQ.imagem 
							       WHERE LY_COMP_IMAGEM.imagem = ? AND LY_COMP_IMAGEM.turma = ? AND 
							             LY_COMP_IMAGEM.ano = ? AND LY_COMP_IMAGEM.semestre = ? AND 
							             LY_COMP_IMAGEM.disciplina = ? and LY_LISTA_FREQ.lista = ? )vw 
							        ON LY_MATRICULA.DISCIPLINA = vw.DISCIPLINA AND 
							  	 LY_MATRICULA.TURMA = vw.TURMA AND LY_MATRICULA.ANO = vw.ANO AND 
							  	 LY_MATRICULA.SEMESTRE = vw.SEMESTRE AND LY_ALUNO.ALUNO = vw.ALUNO LEFT JOIN 
							  	 LY_COMP_LISTA ON vw.disciplina = LY_COMP_LISTA.disciplina AND 
							  	                  vw.turma = LY_COMP_LISTA.turma AND 
							  	                  vw.ano = LY_COMP_LISTA.ano AND 
							  	                  vw.semestre = LY_COMP_LISTA.semestre AND 
							  	                  vw.lista = LY_COMP_LISTA.lista AND 
							  	                  vw.aluno = LY_COMP_LISTA.aluno 
                                where exists 
                                    (select 1 from ly_curso c, ly_usuario_unidade uu
							                             where ly_aluno.curso = c.curso and
							                                   c.faculdade = uu.faculdade and
							                                   uu.usuario = ?)
							    ORDER BY ly_aluno.nome_compl");

							qt.Query(cn, imagem, turma, ano, semestre, disciplina, lista, usuario);

						}
						else
						{
							qt = new QueryTable(@"
                              SELECT '0' as num_chamada, ly_aluno.aluno, ly_aluno.nome_compl, LY_COMP_LISTA.lista, 
							  vw.disciplina, vw.turma, vw.ano, vw.semestre, vw.imagem, 
							  CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Falta' THEN 'S' ELSE 'N' END falta,
							  CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Cancelamento' THEN 'S' ELSE 'N' END cancelada, 
							  CASE LY_COMP_LISTA.OCORRENCIA WHEN 'Abonada' THEN 'S' ELSE 'N' END abonada, 
							  (SELECT COUNT (OCORRENCIA) FROM LY_COMP_LISTA 
							  WHERE ALUNO = vw.aluno AND ANO = vw.ano AND  
							        SEMESTRE = vw.semestre AND OCORRENCIA = 'Abonada') total 
							  FROM LY_ALUNO INNER JOIN 
							      (SELECT LY_COMP_IMAGEM.*, LY_LISTA_FREQ.lista 
							       FROM LY_COMP_IMAGEM INNER JOIN 
							  	  LY_LISTA_FREQ ON LY_COMP_IMAGEM.disciplina = LY_LISTA_FREQ.disciplina AND 
							  	                   LY_COMP_IMAGEM.turma = LY_LISTA_FREQ.turma AND 
							  	                   LY_COMP_IMAGEM.ano = LY_LISTA_FREQ.ano AND 
							  	                   LY_COMP_IMAGEM.semestre = LY_LISTA_FREQ.semestre AND  
							  	                   LY_COMP_IMAGEM.imagem = LY_LISTA_FREQ.imagem 
							       WHERE LY_COMP_IMAGEM.imagem = ? AND LY_COMP_IMAGEM.turma = ? AND 
							             LY_COMP_IMAGEM.ano = ? AND LY_COMP_IMAGEM.semestre = ? AND 
							             LY_COMP_IMAGEM.disciplina = ? and LY_LISTA_FREQ.lista = ? )vw 
							  	 ON LY_ALUNO.ALUNO = vw.ALUNO LEFT JOIN 
							  	 LY_COMP_LISTA ON vw.disciplina = LY_COMP_LISTA.disciplina AND 
							  	 vw.turma = LY_COMP_LISTA.turma AND 
							  	 vw.ano = LY_COMP_LISTA.ano AND 
							  	 vw.semestre = LY_COMP_LISTA.semestre AND 
							  	 vw.lista = LY_COMP_LISTA.lista AND 
							  	 vw.aluno = LY_COMP_LISTA.aluno 
                                where exists
							        (select 1 from ly_curso c, ly_usuario_unidade uu
							         where ly_aluno.curso = c.curso and
							         c.faculdade = uu.faculdade and
							         uu.usuario = ?)
							    ORDER BY ly_aluno.nome_compl");

							qt.Query(cn, imagem, turma, ano, semestre, disciplina, lista, usuario);

						}
					}
				}

				return qt;
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
					cn.Close();
			}
		}

		[
		MethodDescription("Registra a falta ou o cancelamento ou o abono na lista de frequência."),
		ToolTip("Registra a falta ou o cancelamento ou o abono"),
		ControlText("Lança frequência"),
		Image("~/Images/Proc.gif"),
		]
		public static RetVal InsereRemoveOcorrencia(TConnectionWritable cn, VarChar aluno, Number lista,
												Number ano, Number semestre, VarChar disciplina, VarChar turma,
													  VarChar ocorrencia)
		{
			cn.Open(true);
			try
			{
				LyceumUser lyceumuser = LyceumUser.Get(Techne.Lyceum.LyceumUser.Current.User);
				bool pvprivil = lyceumuser.Privilegiado;

				Ly_comp_lista.Row rowLista = Ly_comp_lista.Row.Query(
					cn, lista, disciplina, turma, ano, semestre, aluno);

				VarChar aux = "";
				Number flag = 1;

				if (rowLista != null)
				{
					aux = rowLista.Ocorrencia;
					flag = 0;
					if (pvprivil || aux != "Falta")
					{
						Ly_comp_lista.Row.Delete(cn, lista, disciplina, turma, ano, semestre, aluno);
						flag = 1;
					}

					if (cn.ErrorCount > 0) return "Erro no remoção da ocorrencia: " + cn.GetErrors().ToString();
				}


				if (ocorrencia != aux && flag == 1)
				{
					Ly_comp_lista.Row.Insert(cn, lista, disciplina, turma, ano, semestre, aluno, ocorrencia);
					if (cn.ErrorCount > 0) return "Erro na inserção da ocorrencia: " + cn.GetErrors().ToString();
				}

				if (flag == 0) return RetVal.Success("Usuário sem permissão para remover/abonar Falta");
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
					cn.Close();
			}

			return RetVal.Success("Frequência lançada");
		}

		public static RetVal InserereRemoveTodasOcorrencias(TConnectionWritable cn, Number lista, Number ano,
			Number semestre, VarChar disciplina, VarChar turma, VarChar ocorrencia)
		{
			cn.Open(true);
			try
			{
				//Ly_comp_lista.Row row = Ly_comp_lista.Row.Query(cn,lista,disciplina,turma,ano,semestre);

				//        foreach(Ly_usuario_unidade.Row row in usuario_unidade.Rows){
				//          string unidade_fis = row.Faculdade.ToString();
				//          if(unidade_fis == unidade){
				//            item.Visible = true;
				//            break;
				//          }
				return string.Empty;
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
					cn.Close();
			}
		}
	}
}
