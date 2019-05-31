namespace Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class BadRequestException : Exception
    {
        public override string Message
        {
            get
            {
                if (Errors != null && Errors.Count() > 0)
                {
                    return string.Join(",", Errors);
                }
                else
                {
                    return base.Message;
                }
            }
        }

        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(IDictionary<string, string> errors)
        {
            Errors = errors;
        }

        public IDictionary<string, string> Errors { get; set; }
    }
}
