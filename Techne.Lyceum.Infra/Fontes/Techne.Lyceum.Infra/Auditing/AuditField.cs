using System;
using System.Reflection;
using System.Web.UI;
using Techne.Controls;
using Techne.Library.Sql.Structure;

namespace Techne.Auditing
{
    internal class AuditField
    {
        private static readonly string[] camposHistTrans = new[] { "usuario", "transacao", "pagina", "estacao" };

        private readonly string caption;

        private readonly string controlTypeName;

        private readonly DbType dataType;

        private readonly string field;

        public AuditField(string field, string caption, string controlTypeName, DbType dataType)
        {
            this.caption = caption;
            this.controlTypeName = controlTypeName;
            this.dataType = dataType;
            this.field = field;
        }

        /// <summary>
        ///   Texto que será utilizado como título do controle.
        /// </summary>
        public string Caption
        {
            get
            {
                return this.caption;
            }
        }

        /// <summary>
        ///   Nome do tipo do controle que será utilizado para filtrar o campo da auditoria.
        ///   Se informado "[list]", o controle será um TDropDownList contendo os valores que ocorrem
        ///   no campo (select distinct).
        /// </summary>
        public string ControlTypeName
        {
            get
            {
                return this.controlTypeName;
            }
        }

        public DbType DataType
        {
            get
            {
                return this.dataType;
            }
        }

        /// <summary>
        ///   Campo da auditoria sendo filtrado. Uma coluna da tabela Hist.
        ///   Colunas de busca customizada deverá ser precedido pela palavra "Busca".
        /// </summary>
        public string Field
        {
            get
            {
                return this.field;
            }
        }

        /// <summary>
        ///   Adiciona o controle-filtro associado ao campo a um controle informado.
        /// </summary>
        /// <param name = "container">
        ///   Controle que deve conter o controle-filtro. Normalmente é um Panel.
        /// </param>
        public TControl AddControlToContainer(Control container, Assembly assembly)
        {
            if (this.controlTypeName.Length == 0)
            {
                throw new InvalidOperationException("O tipo do controle não foi fornecido no construtor. A geração não é possível.");
            }

            

            TControl control;

            // [list]: cria TDropDownList com valores que aparecem na coluna
            if (this.controlTypeName == "[list]")
            {
                control = new TDropDownList();
                control.DataType = this.dataType;
            }

                // [input]: cria TTextBox simples
            else if (this.controlTypeName == "[input]")
            {
                control = new TTextBox();
                control.DataType = this.dataType;
            }

                // Tenta instanciar o controle a partir de seu nome
            else
            {
                if (assembly == null)
                {
                    throw new ArgumentNullException("assembly", "O parâmetro assembly deve ser informado.");
                }

                try
                {
                    var controlType = TechLib.FindType(this.controlTypeName, assembly);
                    control = (TControl)controlType.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
                }
                catch
                {
                    // Se não conseguir, não mostra nenhum controle para filtro.
                    control = null;
                }
            }

            if (control != null)
            {
                control.ID = "ctl" + StrLib.ToProper(this.field);
                this.InitControlPrivate(control);
            }

            

            if (control != null)
            {
                container.Controls.Add(control);
                container.Controls.Add(new LiteralControl("<BR>"));
            }

            return control;
        }

        /// <summary>
        ///   Inicializa o controles básicos. Não deve ser utilizado quando o tipo do controle tiver sido informado no construtor.
        ///   TDropDownList's são preenchidos com valores distintos que aparecem na coluna à qual estão associadas.
        ///   TTextBox's somente têm seu DataType setado.
        /// </summary>
        public void InitControl(TControl control)
        {
            if (this.controlTypeName.Length > 0)
            {
                throw new InvalidOperationException("Método não permitido para campos com controle gerado.");
            }

            this.InitControlPrivate(control);
        }

        private void InitControlPrivate(TControl control)
        {
            if (control is TDropDownListBase)
            {
                ((TDropDownListBase)control).SelectAllAllowed = true;
            }

            if (control is TDropDownList)
            {
                var tabela = Array.IndexOf(camposHistTrans, this.field.ToLower()) >= 0
                                 ? HistoryLib.TabHistTrans
                                 : HistoryLib.TabHist;

                ((TDropDownList)control).SqlSelect = SqlSelect.Parse("SELECT DISTINCT " + this.field + " FROM " + tabela);
                ((TDropDownList)control).SQLWhere = this.field + " <> NULL";
                ((TDropDownList)control).SQLOrder = this.field;
            }

            if (this.caption.Length > 0)
            {
                control.Caption = this.caption;
            }

            control.DataType = this.dataType;
        }
    }
}