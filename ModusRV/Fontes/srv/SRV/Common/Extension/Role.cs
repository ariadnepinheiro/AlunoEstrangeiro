using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SRV.Models.DTO;

namespace SRV.Common.Extension
{
    public class Role
    {
        public static Boolean IsGranted(string roles)
        {
            UserState usuarioLogado = (UserState)HttpContext.Current.Session["user"];

            if (usuarioLogado != null)
                if (SplitString(roles).Length == 0 || SplitString(roles).Contains(usuarioLogado.Perfil.ToString()))
                    return true;

            return false;
        }

        internal static string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }

    }
}