using System;

namespace OBSSystem.Application.Exceptions
{
    public class EmailAlreadyTakenException : Exception
    {
        public EmailAlreadyTakenException(string message) : base(message) { }
    }
}
