using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using techne;
using techne.library.sql.structure;

namespace Techne.Controls
{
    public interface IDepender
    {
        ChangedEventHandler ChangedHandler { get; }

        string[] Dependees { get; }

        string SqlWhere { get; }
    }

    public interface IDependee
    {
        Control[] Dependers { get; }

        void AddDepender(Control depender);
    }

    public class DependerLib
    {
        /// <summary>
        ///   Utilizado pela propriedade Dependees dos controles que implementam IDepender.
        ///   Parseia o SqlWhere do controle e devolve uma lista dos id's dos controles referenciados pela cláusula.
        /// </summary>
        public static string[] GetDependees(IDepender depender)
        {
            var container = depender is TControl ? ((TControl)depender).RecordContainer
                                : TControl.GetRecordContainer((Control)depender);

            var names = new ArrayList();

            var root = RootPointer.parse(depender.SqlWhere, Language.CSharp);

            // Sintaxe #controle#
            foreach (ControlIdentifier control in root.selectByType(JLib.typeToClass(typeof (ControlIdentifier))))
            {
                if (!names.Contains(control.getName()))
                {
                    names.Add(control.getName());
                }
            }

            // Sintaxe $coluna$
            if (container != null)
            {
                foreach (ColumnValueIdentifier identifier in root.selectByType(JLib.typeToClass(typeof (ColumnValueIdentifier))))
                {
                    foreach (Control control in TControl.GetChildTControls((Control)container, identifier.getColumnName(), false))
                    {
                        if (!names.Contains(control.ID))
                        {
                            names.Add(control.ID);
                        }
                    }
                }
            }

            return (string[])names.ToArray(typeof (string));
        }

        /// <summary>
        ///   Chamado no OnLoad() dos controles que implementam IDepender.
        ///   Verifica quais os controles referenciados pelo controle informado.
        ///   Adiciona-se à lista de dependência (Dependers) desses controles.
        /// </summary>
        public static void RegisterDepender(IDepender depender)
        {
            var dependencyControls = GetDependencyControls(depender);

            foreach (var dependencyControl in dependencyControls)
            {
                if (dependencyControl is ITControlEditable)
                {
                    ((ITControlEditable)dependencyControl).Changed += depender.ChangedHandler;
                }

                // Registra-se em cada um dos controles dos quais dependa.
                if (dependencyControl is IDependee)
                {
                    ((IDependee)dependencyControl).AddDepender((Control)depender);
                }
            }
        }

        public static void WriteDebugInfo(HtmlTextWriter writer, IDepender depender)
        {
            writer.WriteLine("<B>Dependees: </B>" + StrLib.EnumerableToStr(depender.Dependees, ", "));
        }

        public static void WriteDebugInfo(HtmlTextWriter writer, IDependee dependee)
        {
            var dependerArray = TechLib.EnumerableItemProperty(dependee.Dependers, "ID", true);
            var dependerList = dependerArray.Length > 0 ? (string[])dependerArray : new string[0];
            writer.Write("<B>Dependers: </B>" + StrLib.EnumerableToStr(dependerList) + "<BR/>");
        }

        /// <summary>
        ///   Devolve os ITControl's correspondentes aos nomes informados.
        ///   Utilizado por controles que têm propriedade SqlWhere ou equivalente.
        /// </summary>
        private static ITControl[] GetDependencyControls(IDepender depender)
        {
            var dependencies = depender.Dependees;
            var dependentControl = (Control)depender;

            try
            {
                var namingContainer = dependentControl.NamingContainer;
                var controls = new ITControl[dependencies.Length];
                var childControls = TControl.GetChildTControls(dependentControl);

                for (var i = 0; i < dependencies.Length; i++)
                {
                    try
                    {
                        controls[i] = TControl.FindTControl(dependencies[i], (INamingContainer)namingContainer);
                    }
                    catch (ArgumentException)
                    {
                        throw new ArgumentException("O controle referenciado (" + dependencies[i] + ") por " + dependentControl.UniqueID + " não é derivado de ITControl.");
                    }

                    if (controls[i] == null)
                    {
                        throw new ArgumentException("O controle referenciado (" + dependencies[i] + ") por " + dependentControl.UniqueID + " não foi encontrado na página.");
                    }

                    if (Array.IndexOf(childControls, controls[i]) >= 0)
                    {
                        throw new ArgumentException("O controle referenciado (" + dependencies[i] + ") não pode estar contido no controle que o referencia (" + dependentControl.UniqueID + ").");
                    }
                }

                return controls;
            }
            finally
            {
            }
        }
    }
}