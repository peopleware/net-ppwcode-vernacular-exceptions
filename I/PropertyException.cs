//Copyright 2004 - $Date: 2008-11-15 23:58:07 +0100 (za, 15 nov 2008) $ by PeopleWare n.v..

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

#region Using

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

#endregion

namespace PPWCode.Vernacular.Exceptions.I
{
    /// <summary>
    /// <c>PropertyExceptions</c> are exceptions that carry with them information about the property for which they
    /// occurred. They are usually thrown by a property setter during validation.
    /// </summary>
    /// <remarks>
    /// <para>If the <see cref="PropertyName"/> is <c>null</c>,
    /// it means that the exception could not be attributed to a specific property of <see cref="Sender"/>.</para>
    /// <para>The <see cref="Sender"/>
    /// should not be <c>null</c>, except when the exception is thrown during
    /// construction of an object, that could not be completed. Carrying
    /// the reference to the object would expose an incompletely initialized object,
    /// as the exception signals a failure to complete the initialization. 
    /// TODO: add type property for that case.</para>
    /// <para>
    /// A <c>PropertyException</c> reports on an issue with one object. If there is a need to communicate
    /// an issue over more than one issue, use a <see cref="CompoundSemanticException"/>.</para>
    /// <para>Specific property exception subtypes will
    /// make these advises binding in most cases.</para>
    /// </remarks>
    [Serializable]
    public class PropertyException :
        SemanticException
    {
        /// <summary>
        /// A string that can be used, if you wish, as the message to signal that
        /// the property is mandatory, but was not filled out.
        /// </summary>
        public const string MandatoryMessage = "MANDATORY";

        public PropertyException()
        {
        }

        public PropertyException(string message)
            : base(message)
        {
        }

        public PropertyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public PropertyException(object sender, string propertyName, string message, Exception innerException)
            : base(message, innerException)
        {
            Contract.Requires(sender != null);
            Contract.Ensures(Sender == sender);
            Contract.Ensures(PropertyName == propertyName);
            Contract.Ensures(Message == message);
            Contract.Ensures(InnerException == innerException);

            Sender = sender;
            PropertyName = propertyName;
        }

        protected PropertyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public object Sender
        {
            get
            {
                return Data["Sender"];
            }
            private set
            {
                Data["Sender"] = value;
            }
        }

        public string PropertyName
        {
            get
            {
                return Data["PropertyName"] as string;
            }
            private set
            {
                Data["PropertyName"] = value;
            }
        }

        [Pure]
        public override bool Like(SemanticException other)
        {
            Contract.Ensures((base.Like(other)
                              && ((PropertyException)other).Sender == Sender
                              && ((PropertyException)other).PropertyName == PropertyName)
                                 == Contract.Result<bool>());

            if (!base.Like(other))
            {
                return false;
            }

            PropertyException pe = (PropertyException)other;
            return (pe.PropertyName == PropertyName) && (pe.Sender == Sender);
        }

        public override string ToString()
        {
            try
            {
                return string.Format("Fault {0} for {1}.", Message, PropertyName);
            }
            catch
            {
                return Message;
            }
        }
    }
}