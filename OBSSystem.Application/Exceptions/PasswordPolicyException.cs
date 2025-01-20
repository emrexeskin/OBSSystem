using System;
using System.Collections.Generic;

namespace OBSSystem.Application.Exceptions
{
    public class PasswordPolicyException : Exception
    {
        public List<string> Errors { get; }

        public PasswordPolicyException(List<string> errors)
            : base("Password does not meet the required security standards.")
        {
            Errors = errors;
        }
    }
}
