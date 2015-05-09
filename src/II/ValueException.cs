// Copyright 2014 by PeopleWare n.v..
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace PPWCode.Vernacular.Exceptions.II
{
    /// <summary>
    ///     In many cases, a property exception is needed that reports the original value of the property.
    ///     This value can be used to generate sensible end-user messages of the form &quot;Unable to change
    ///     {<see cref="PropertyException.PropertyName" />} for {<see cref="PropertyException.Sender" />}
    ///     from {<see cref="OldValue" />} to
    ///     {<see cref="NewValue" />}&quot;.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This exception is a generalized version of a <see cref="PropertyException" /> that carries
    ///         information about the <see cref="OldValue" /> and the
    ///         <see cref="NewValue" />.
    ///         It is a bore to create separate exceptions for each of those specific cases. It would
    ///         be nice to use generics for the type of the property value, but that is something for a later
    ///         version.
    ///     </para>
    ///     <para>
    ///         This exception can be used for simple properties of all kinds: simple properties of reference type,
    ///         as well as simple properties of value types, both of mutable types and immutable types.
    ///         Values should be considered read-only, also if they are or reference type.
    ///     </para>
    ///     <para>
    ///         This kind of exception cannot be thrown in a constructor, since there is no
    ///         original value then.
    ///     </para>
    /// </remarks>
    [Serializable]
    public class ValueException :
        PropertyException
    {
        public ValueException()
        {
        }

        public ValueException(string message)
            : base(message)
        {
        }

        public ValueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ValueException(object sender, string propertyName, string message, Exception innerException)
            : base(sender, propertyName, message, innerException)
        {
        }

        public ValueException(object sender, string propertyName, object oldValue, object newValue, string message, Exception innerException)
            : base(sender, propertyName, message, innerException)
        {
            Contract.Requires(propertyName != null);
            Contract.Ensures(Sender == sender);
            Contract.Ensures(PropertyName == propertyName);
            Contract.Ensures(Message == message);
            Contract.Ensures(InnerException == innerException);
            Contract.Ensures(OldValue == oldValue);
            Contract.Ensures(NewValue == newValue);

            OldValue = oldValue;
            NewValue = newValue;
        }

        protected ValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Contains the original value of the property.
        /// </summary>
        public object OldValue
        {
            get { return Data["OldValue"]; }
            private set { Data["OldValue"] = value; }
        }

        /// <summary>
        ///     Contains the value that could not be stored in the property.
        /// </summary>
        public object NewValue
        {
            get { return Data["NewValue"]; }
            private set { Data["NewValue"] = value; }
        }

        [Pure]
        public override bool Like(SemanticException other)
        {
            Contract.Ensures((base.Like(other)
                              && Equals(((ValueException)other).OldValue, OldValue)
                              && Equals(((ValueException)other).NewValue, NewValue))
                             == Contract.Result<bool>());

            if (!base.Like(other))
            {
                return false;
            }

            ValueException ve = (ValueException)other;
            return Equals(ve.OldValue, OldValue) && Equals(ve.NewValue, NewValue);
        }

        public override string ToString()
        {
            try
            {
                return string.Format("Fault {0} for {1} old {2} new {3}.", Message, PropertyName, OldValue, NewValue);
            }
            catch
            {
                return Message;
            }
        }
    }
}