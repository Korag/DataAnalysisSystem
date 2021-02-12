using System.Collections.Generic;

namespace DataAnalysisSystem.DTO.Dictionaries
{
    public static class EmailClassifierDictionary
    {
         public static readonly Dictionary<string, string> EmailTopic = new Dictionary<string, string>
         {
           {"register", "Data Analysis System - Registration of a new user account."},
           {"emailConfirmation", "Data Analysis System - Confirmation of the email address associated with your account."},
           {"changePassword", "Data Analysis System - Changing the password for a user account."},
           {"resetPassword", "Data Analysis System - Resetting the system password."},
           {"resetPasswordWithoutEmailConfirmation", "Data Analysis System - Attempting to reset the user account password."},
        };

        public static readonly Dictionary<string, string> HeaderOfEmailContent = new Dictionary<string, string>
         {
           {"register", "New account has been created."},
           {"emailConfirmation", "Confirm your e-mail address."},
           {"changePassword", "Your user account password has been changed."},
           {"resetPassword", "Reset your user account password."},
           {"resetPasswordWithoutEmailConfirmation", "Unauthorised attempt to reset a user account password has been detected."},
        };

        public static readonly Dictionary<string, string> PrimaryContent = new Dictionary<string, string>
         {
           {"register", "We have just noted the registration of a new user account associated with this email address. Before using the system you must confirm your email address by clicking on the button below."},
           {"emailConfirmation", "You want to log in to your user account but cannot do so? Login has been interrupted because you have not yet confirmed your email address. Would you like to enable login to your account by confirming this email address ? Press the button below to do so."},
           {"changePassword", "The password for the user account associated with this email address has just been reset."},
           {"resetPassword", "We have just registered a request to reset the password of the user account associated with this email address. If you wish to reset your password please click on the button below."},
           {"resetPasswordWithoutEmailConfirmation", "An attempt was detected to change the password for the account associated with this email address. This address has not yet been confirmed. If you wish to reset your password, please first confirm your email address by clicking on the button below."},
        };

        public static readonly Dictionary<string, string> SecondaryContent = new Dictionary<string, string>
         {
           {"register", "You have not created an account on our platform? Someone must have made a mistake when entering the data during registration. We are sorry for the inconvenience."},
           {"emailConfirmation", "Want to change your email address linked to your account? Contact your system administrator or confirm your current email address first and then change it in the relevant application panel."},
           {"changePassword", "If you have not reset your user account password, please contact the service administrator as soon as possible."},
           {"resetPassword", "Did you not give instructions to reset your account password? It may be that someone is in possession of your email address and has attempted to do so. Ignore this message or contact the service administrator."},
           {"resetPasswordWithoutEmailConfirmation", "If you wish to change your current email address, you can contact the system administrator or confirm your current address and then change it in the user panel."},
        };

        public static readonly Dictionary<string, string> URLActionText = new Dictionary<string, string>
         {
           {"register", "Confirm this email address"},
           {"emailConfirmation", "Confirm this email address."},
           {"changePassword", ""},
           {"resetPassword", "Reset your password."},
           {"resetPasswordWithoutEmailConfirmation", "Confirm this email address."},
        };
    }
}