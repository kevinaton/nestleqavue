using System.DirectoryServices.ActiveDirectory;
using System.Net;
using System.DirectoryServices.AccountManagement;
using System;

namespace HRD.WebApi.Authorization
{
    public static class AuthorizationHelper
    {
        public static bool ValidateUsernameAndPassword(string userName, string securePassword)
        {
            bool result = false;

            ContextType contextType = ContextType.Machine;

            if (InDomain())
            {
                contextType = ContextType.Domain;
            }

            try
            {
                using (PrincipalContext principalContext = new(contextType))
                {
                    result = principalContext.ValidateCredentials(userName,new NetworkCredential(string.Empty, securePassword).Password);
                }
            }
            catch (PrincipalOperationException)
            {
                // Account disabled? Considering as Login failed
                result = false;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        ///     Validate: computer connected to domain?   
        /// </summary>
        /// <returns>
        ///     True -- computer is in domain
        ///     <para>False -- computer not in domain</para>
        /// </returns>
        public static bool InDomain()
        {
            bool result = true;

            try
            {
                Domain domain = Domain.GetComputerDomain();
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                result = false;
            }

            return result;
        }
    }
}
