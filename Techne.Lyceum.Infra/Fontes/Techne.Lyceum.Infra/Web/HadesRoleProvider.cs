namespace Techne.Web
{
    using System.Web.Security;
    using Techne.Web.Hades;

    public class HadesRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                return "Lyceum";
            }

            set
            {
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
        }

        public override void CreateRole(string roleName)
        {
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            return false;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            return new string[] { };
        }

        public override string[] GetAllRoles()
        {
            return new string[] { };
        }

        public override string[] GetRolesForUser(string username)
        {
            return TechneHttpApplication.ReadRolesFromCookie(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            return new string[] { };
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            var roles = TechneHttpApplication.ReadRolesFromCookie(username);

            if (roles != null)
            {
                foreach (var r in roles)
                {
                    if (r == roleName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
        }

        public override bool RoleExists(string roleName)
        {
            return Padaces.PadacesValido(roleName);
        }
    }
}