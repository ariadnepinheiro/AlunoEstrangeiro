using System;
using System.Collections.Specialized;
using System.Web.Security;
using Techne.Web.Hades;

namespace Techne.Web
{
    internal class HadesMembershipProvider : MembershipProvider
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

        public override bool EnablePasswordReset
        {
            get
            {
                return false;
            }
        }

        public override bool EnablePasswordRetrieval
        {
            get
            {
                return false;
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 999;
            }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 0;
            }
        }

        public override int MinRequiredPasswordLength
        {
            get
            {
                return 1;
            }
        }

        public override int PasswordAttemptWindow
        {
            get
            {
                return 100;
            }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return MembershipPasswordFormat.Encrypted;
            }
        }

        public override string PasswordStrengthRegularExpression
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return false;
            }
        }

        public override bool RequiresUniqueEmail
        {
            get
            {
                return false;
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.UserRejected;
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return false;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return new MembershipUserCollection();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return new MembershipUserCollection();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;
            return new MembershipUserCollection();
        }

        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }

        public override string GetPassword(string username, string answer)
        {
            return string.Empty;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            var u = Usuario.BuscaUsuario(username);
            if (u == null)
            {
                return null;
            }

            var usr = new MembershipUser("HadesMembershipProvider", u.Usuario, u.Usuario, u.Email, string.Empty, string.Empty, true, !u.Habilitado, DateTime.MinValue, 
                                         u.UltimoLogin == null ? DateTime.MinValue : (DateTime)u.UltimoLogin, 
                                         DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);

            return usr;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            return null;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name == null || name.Length == 0)
            {
                name = "HadesMembershipProvider";
            }

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Hades Membership provider");
            }

            base.Initialize(name, config);
        }

        public override string ResetPassword(string username, string answer)
        {
            return string.Empty;
        }

        public override bool UnlockUser(string userName)
        {
            return false;
        }

        public override void UpdateUser(MembershipUser user)
        {
        }

        public override bool ValidateUser(string username, string password)
        {
            return Usuario.UsuarioValido(username, password);
        }
    }
}