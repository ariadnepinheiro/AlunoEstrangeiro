using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.DTO;
using System.Xml;

namespace SRV.Common
{
    public class Menu
    {
        private const string XmlFileNameKey = "~/Web.sitemap";
        private const string XmlItemTag = "siteMapNode";

        public MenuItem ReadMenu(String perfil)
        {

            MenuItem retVal = new MenuItem(null);
            XmlTextReader reader = new XmlTextReader(HttpContext.Current.Server.MapPath(XmlFileNameKey));
            retVal.topMost = true;

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Attribute:
                        break;
                    case XmlNodeType.CDATA:
                        break;
                    case XmlNodeType.Comment:
                        break;
                    case XmlNodeType.Document:
                        break;
                    case XmlNodeType.DocumentFragment:
                        break;
                    case XmlNodeType.DocumentType:
                        break;
                    case XmlNodeType.Element:
                        if (reader.Name == XmlItemTag)
                        {
                            bool isempty = reader.IsEmptyElement;

                            MenuItem temp = new MenuItem();
                            temp.topMost = false;
                            int attributeCount = reader.AttributeCount;
                            if (attributeCount > 0)
                            {
                                for (int i = 0; i < attributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    SetAttributeValue(ref temp, reader.Name, reader.Value);
                                }
                            }

                            if (temp.roles == null || temp.roles.Split(',').Select(role => role.Trim().ToUpper()).Contains(perfil.ToUpper()))
                            {
                                retVal.children.Add(temp);

                                if (!isempty)
                                {
                                    temp.parent = retVal;
                                    retVal = temp;
                                }
                            }

                        }
                        break;
                    case XmlNodeType.EndElement:
                        string test = reader.Name;
                        if (retVal.parent != null && retVal.parent.parent != null)
                        {
                            retVal = retVal.parent;
                        }

                        break;
                    case XmlNodeType.EndEntity:
                        break;
                    case XmlNodeType.Entity:
                        break;
                    case XmlNodeType.EntityReference:
                        break;
                    case XmlNodeType.None:
                        break;
                    case XmlNodeType.Notation:
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        break;
                    case XmlNodeType.SignificantWhitespace:
                        break;
                    case XmlNodeType.Text:
                        break;
                    case XmlNodeType.Whitespace:
                        break;
                    case XmlNodeType.XmlDeclaration:
                        break;
                    default:
                        break;
                }
            }

            //Close the reader
            reader.Close();
            while (retVal.parent != null && !retVal.topMost)
            {
                retVal = retVal.parent;
            }

            return retVal;
        }

        private void SetAttributeValue(ref MenuItem menu, string name, string value)
        {
            switch (name)
            {
                case "title":
                    menu.title = value;
                    break;
                case "controller":
                    menu.controller = value;
                    break;
                case "action":
                    menu.action = value;
                    break;
                case "group":
                    if (Boolean.Parse(value))
                        menu.group = true;
                    break;
                case "roles":
                    menu.roles = value;
                    break;
                default:
                    break;
            }

        }

        public static CaminhoPao GetCaminhoPao(MenuItem menu, string controllerName)
        {
            CaminhoPao caminhoPao = new CaminhoPao();

            foreach (var sistema in menu.children[0].children)
            {
                caminhoPao.Nivel1 = sistema.title;

                foreach (var modulo in sistema.children)
                {
                    caminhoPao.Nivel2 = modulo.title;

                    if (!modulo.group && controllerName.Equals(modulo.controller))
                    {
                        return caminhoPao;
                    }
                    else
                    {
                        foreach (var projeto in modulo.children)
                        {
                            if (controllerName.Equals(projeto.controller))
                            {
                                caminhoPao.Nivel3 = projeto.title;
                                return caminhoPao;
                            }
                        }
                    }
                }
            }
            return new CaminhoPao();
        }

    }
}