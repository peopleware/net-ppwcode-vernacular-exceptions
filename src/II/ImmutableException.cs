#region Using

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

#endregion

namespace PPWCode.Vernacular.Exceptions.II
{
    /// <summary>
    /// The method that throws this error is a mutating method,
    /// flagged not to be used, because the instance is flagged
    /// as immutable.
    /// </summary>
    [Serializable]
    public class ImmutableException :
        ProgrammingError
    {
        public ImmutableException()
            : base(UnspecifiedProgrammingErrorMessage)
        {
            Contract.Ensures((Message == UnspecifiedProgrammingErrorMessage) && (InnerException == null));
        }

        public ImmutableException(string message)
            : base(message ?? UnspecifiedProgrammingErrorMessage)
        {
            Contract.Ensures((message != null ? Message == message : Message == UnspecifiedProgrammingErrorMessage)
                             && (InnerException == null));
        }

        public ImmutableException(string message, Exception innerException)
            : base(message ?? (innerException == null ? UnspecifiedProgrammingErrorMessage : ExceptionWithProgrammingCauseMessage), innerException)
        {
            Contract.Ensures((message != null
                                  ? Message == message
                                  : Message == (innerException == null ? UnspecifiedProgrammingErrorMessage : ExceptionWithProgrammingCauseMessage))
                             && (InnerException == innerException));
        }

        protected ImmutableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}