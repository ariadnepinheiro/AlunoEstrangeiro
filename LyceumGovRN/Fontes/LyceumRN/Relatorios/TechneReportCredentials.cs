namespace Techne.Lyceum.RN.Relatorios
{
    using System.Net;
    using System.Security.Principal;
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TechneReportCredentials : IReportServerCredentials
    {
        private readonly string domain;

        private readonly string password;

        private readonly string userName;

        public TechneReportCredentials(string userName, string password, string domain)
        {
            this.userName = userName;
            this.password = password;
            this.domain = domain;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(this.userName, this.password, this.domain);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;

            return false;
        }
    }
}