using System.Diagnostics.Contracts;

namespace PPWCode.Vernacular.Exceptions.V
{
    /// <summary>
    ///     <c>PropertyExceptions</c> are exceptions that carry with them information about the property for which they
    ///     occurred. They are usually thrown by a property setter during validation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         If the <see cref="PropertyName" /> is <c>null</c>,
    ///         it means that the exception could not be attributed to a specific property of <see cref="Sender" />.
    ///     </para>
    ///     <para>
    ///         The <see cref="Sender" />
    ///         should not be <c>null</c>, except when the exception is thrown during
    ///         construction of an object, that could not be completed. Carrying
    ///         the reference to the object would expose an incompletely initialized object,
    ///         as the exception signals a failure to complete the initialization.
    ///         TODO: add type property for that case.
    ///     </para>
    ///     <para>
    ///         A <c>PropertyException</c> reports on an issue with one object. If there is a need to communicate
    ///         an issue over more than one issue, use a <see cref="CompoundSemanticException" />.
    ///     </para>
    ///     <para>
    ///         Specific property exception subtypes will
    ///         make these advises binding in most cases.
    ///     </para>
    /// </remarks>
    public class PropertyException : SemanticException
    {
        /// <summary>
        ///     A string that can be used, if you wish, as the message to signal that
        ///     the property is mandatory, but was not filled out.
        /// </summary>
        public const string MandatoryMessage = "MANDATORY";

        /// <summary>
        ///     A string that can be used, if you wish, as the message to signal that
        ///     the property show be <c>default</c> value for the type of the concerning property.
        /// </summary>
        public const string DefaultValueExpectedMessage = "DEFAULT_VALUE_EXPECTED";

        public PropertyException(
            object sender,
            string propertyName,
            string? message = null,
            Exception? innerException = null)
            : base(message, innerException)
        {
            Sender = sender;
            PropertyName = propertyName;
        }

        public object Sender
        {
            get => Data["Sender"]!;
            private init => Data["Sender"] = value;
        }

        public string PropertyName
        {
            get => (string)Data["PropertyName"]!;
            private init => Data["PropertyName"] = value;
        }

        [Pure]
        public override bool Like(SemanticException? other)
            => base.Like(other)
               && other is PropertyException e
               && (e.PropertyName == PropertyName)
               && (e.Sender == Sender);

        public override string ToString()
            => $"Fault {Message} for {PropertyName}.";
    }
}
