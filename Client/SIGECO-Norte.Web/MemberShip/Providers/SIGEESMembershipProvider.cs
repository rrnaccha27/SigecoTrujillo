
using System;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Security;

namespace SIGEES.Web.MemberShip.Providers
{
    public class SIGEESMembershipProvider : MembershipProvider
    {
        
        BusinessLogic.UsuarioSelBL oISeguridadService;
        public SIGEESMembershipProvider()
        {
            //InstanceContainer.UnitiyInstanceContainer.oIUnityContainer.BuildUp(this);
            oISeguridadService = new BusinessLogic.UsuarioSelBL();
        }

        public override string ApplicationName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new System.NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new System.NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new System.NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new System.NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new System.NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new System.NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (UserManager.USUARIO != null)
            {
                return null;// new MembershipUser("SIGEESMembershipProvider", username, UserManager.USUARIO.iCodUsuario, UserManager.USUARIO.vUsuario, null, null, true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            }
            else
            {
                return null;
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new System.NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new System.NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new System.NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new System.NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new System.NotImplementedException();
        }

        
        public override bool ValidateUser(string username, string password)
        { /*
            try
            {
                var oUSUARIO = new SIGEES.Entidades.UsuarioDTO();
                oUSUARIO.vUsuario = username;
                oUSUARIO.vPassword = password;
                oUSUARIO.iCodModulo = Int32.Parse(ConfigurationManager.AppSettings["AppCode"]);                
               oUSUARIO= oISeguridadService.Autenticar(oUSUARIO);

                if (oUSUARIO.iCodUsuario != 0)
                {
                    System.Web.HttpContext.Current.Items.Add("USUARIO", oUSUARIO);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
           * */
            return true;
        }

        public static string SerializeToJson(object objeto)
        {
            var jsonSerializer = new DataContractJsonSerializer(objeto.GetType());
            var ms = new MemoryStream();
            jsonSerializer.WriteObject(ms, objeto);
            var jsonResult = Encoding.Default.GetString(ms.ToArray());
            return jsonResult;
        }
    }
}