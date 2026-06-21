using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Reflection;
using Seeduc.Infra.MapeamentoAtributos;
using System.ComponentModel;
using System.Web.UI;
using DevExpress.Web.ASPxTabControl;
using System.Web.UI.WebControls;
using Techne.Web;

namespace Techne.Lyceum.Net.Util
{
    public static class Utils
    {
        public static string CriptografaTexto(this string queryString)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(queryString);

            return Convert.ToBase64String(bytesToEncode);
        }

        public static string SubstituirVirgulaPorPonto(this  decimal valor)
        {
            return valor.ToString().Replace(",", ".");
        }

        public static string SubstituirPontoPorVirgula(this decimal valor)
        {
            return valor.ToString().Replace(".", ",");
        }

        /// <summary>
        /// Retira os pontos e o traço
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static string RetirarMascaraCPF(this string cpf)
        {
            return cpf.Replace(".", "").Replace("-", "");
        }

        /// <summary>
        /// Retira os traços e os parentes
        /// </summary>
        /// <param name="telefone"></param>
        /// <returns></returns>
        public static string RetirarMascaraTelefone(this string telefone)
        {
            return telefone.Replace("(", "").Replace(")", "").Replace("-", "");
        }

        /// <summary>
        /// Retira os pontos e o traço
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static string RetirarMascaraRG(this string cpf)
        {
            return cpf.Replace(".", "").Replace("-", "");
        }

        public static string RetornaQueryString<T>(T objeto)
        {
            string queryString = string.Empty;
            string atributo = string.Empty;
            ParamentrosQueryString parametro;

            IEnumerable<PropertyInfo> propriedades = objeto.GetType().GetProperties();
            
            foreach (PropertyInfo propriedade in propriedades)
            {
                parametro = EnumeraParametrosDefinidosNoAtributo(typeof(T), propriedade.Name);
               
                if (parametro.Disponivel)
                {
                    if (!propriedade.Name.Equals(propriedades.First().Name))
                        queryString = string.Concat(queryString, "&");

                    atributo = string.IsNullOrEmpty(parametro.Nome) ? propriedade.Name : parametro.Nome;

                    queryString = string.Concat(queryString, atributo, "=", propriedade.GetValue(objeto, null));
                }
            }

            return queryString;
        }        

        private static ParamentrosQueryString EnumeraParametrosDefinidosNoAtributo(Type tipoEntidade, string nomePropriedade)
        {
            IEnumerable<object> atributos =
                tipoEntidade.GetProperty(nomePropriedade).GetCustomAttributes(typeof(ParamentrosQueryString), true).ToList();

            return atributos.Count() > 0 
                ? atributos.First() as ParamentrosQueryString 
                : new ParamentrosQueryString();
        }

        public static string GetEnumDescription(Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Atribui ou retira a propriedade Visible de componentes web.
        /// </summary>
        /// <param name="controles">Array de WebControl com os compomentes</param>
        /// <param name="exibir">Valor para o campo Visible</param>
        public static void ExibeOcultaControles(WebControl[] controles, bool exibir)
        {
            foreach (WebControl wc in controles)
                wc.Visible = exibir;
        }
        
        public static void HabilitaDesabilitaTabPages(TabPageCollection abas, bool habilitar, int? inicio)
		{
            if (inicio != null)
            {
                for (int i = inicio.Value; i < abas.Count; i++)
                    abas[i].Enabled = habilitar;
            }
            else
            {
                foreach (TabPage tp in abas)
                    tp.Enabled = habilitar;
            }
		}

        public static void HabilitaDesabilitaControlesWeb(WebControl[] controles, bool habilitar)
		{
            foreach (WebControl wc in controles)
            {
                wc.Enabled = habilitar;
            }
		}

        public static void AtribuiSomenteLeitura(Control[] controles, bool somenteLeitura)
		{
            foreach (Control wc in controles)
            {
                if (wc is TextBox)
                    (wc as TextBox).ReadOnly = somenteLeitura;
            }
		}

        public static void CarregaDropDownList(DropDownList drop, object data, string defaultValue)
		{
            drop.SelectedIndex = -1;
            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("<Nenhum>", "");
            drop.Items.Add(itemVazio);

            if (defaultValue != string.Empty)
                drop.SelectedValue = defaultValue;
            else
                drop.SelectedValue = "";
		}

        /// <summary>
        /// Método que limpa todos os campos do tipo textBox , RichTextBox , MaskedTextBox,
        /// herdados de ListControl (BulletedList, CheckboxList, DropDownList, ListBox, RadionButtonList), 
        /// CheckBox, RadioButton, TSearchBox, TSearch e ASPxCheckBox; contidos dentro de um controle.
        /// </summary>
        /// <param name="container"></param>
        public static void LimpaCampos(Control container)
        {
            foreach (Control ctrControl in container.Controls)
            {
                if (object.ReferenceEquals(ctrControl.GetType(), typeof(TextBox)))
                    ((TextBox)ctrControl).Text = string.Empty;
                else if (object.ReferenceEquals(ctrControl.GetType().BaseType, typeof(ListControl)))
                    ((ListControl)ctrControl).ClearSelection();
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(CheckBox)))
                    ((CheckBox)ctrControl).Checked = false;
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(RadioButton)))
                    ((RadioButton)ctrControl).Checked = false;
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(TSearchBox)))
                    ((TSearchBox)ctrControl).ResetValue();
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(TSearch)))
                    ((TSearch)ctrControl).ResetValue();
                else if (object.ReferenceEquals(ctrControl.GetType(), typeof(DevExpress.Web.ASPxEditors.ASPxCheckBox)))
                    ((DevExpress.Web.ASPxEditors.ASPxCheckBox)ctrControl).Checked = false;

                if (ctrControl.Controls.Count > 0)
                    LimpaCampos(ctrControl);
            }
        }

        /// <summary>
        /// Apenas para campos TextBox.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="nomeDoCampo"></param>
        public static void ValidaCampoNumerico(Control controle, string nomeDoCampo)
		{
            const string MSG_ERRO = "Só é possível inserir número no campo {0}.<br />";
            decimal num = 0;
            string auxStr = string.Empty;
            const int DECIMAL_LENGHT = sizeof(decimal);

            try
            {
                if (controle is TextBox)
                {
                    TextBox t = (controle as TextBox);
                    auxStr = t.Text;

                    if (!string.IsNullOrEmpty(auxStr))
                    {
                        if (auxStr.Length > DECIMAL_LENGHT)
                            auxStr = auxStr.Remove(DECIMAL_LENGHT);

                        if (!decimal.TryParse(auxStr, out num))
                            throw new Exception(string.Format(MSG_ERRO, nomeDoCampo));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
		}

        /// <summary>
        /// Este método considera apenas campos TextBox.
        /// Retorna uma exceção com todos os erros encontrados.
        /// </summary>
        /// <param name="textBoxes">Array de TextBoxes</param>
        /// <param name="nomesDosCampos">Nomes dos campos para a mensagem</param>
        public static void ValidaCamposNumericos(Control[] controles, string[] nomesDosCampos)
        {
            StringBuilder erros = new StringBuilder();

            try
            {
                for (int i = 0; i < controles.Length; i++)
                {
                    try
                    {
                        ValidaCampoNumerico(controles[i], nomesDosCampos[i]);
                    }
                    catch (Exception ex)
                    {
                        erros.Append(ex.Message);
                    }
                }

                if (erros.Length > 0)
                    throw new Exception(erros.ToString());
            }
            catch (Exception ex)
            {                
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Define a aba ativa para a primeira aba habilitada.
        /// </summary>
        /// <param name="pc"></param>
        public static void SetActiveTabIndexByFirstEnabled(ASPxPageControl pc)
		{
            foreach (TabPage tp in pc.TabPages)
            {
                if (tp.Enabled)
                {
                    pc.ActiveTabIndex = tp.Index;
                    break;
                }
            }
		}
    }
}
