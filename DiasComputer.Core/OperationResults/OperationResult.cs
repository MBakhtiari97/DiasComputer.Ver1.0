using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiasComputer.Core.OperationResults
{
    public class OperationResult
    {
        public enum Result
        {
            Success,
            Failure,
            DuplicateEmail,
            InvalidExtension,
            UnAuthorized,
            ExistedWallet,
            SuccessSignUp,
            DisabledAccount,
            Welcome,
            Recovery,
            SuccessNewsletter,
            SuccessComment,
            SuccessReport,
            RecaptchaFailed
        }
    }
}
