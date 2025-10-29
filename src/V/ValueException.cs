// Copyright 2024 by PeopleWare n.v..
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Diagnostics.Contracts;

namespace PPWCode.Vernacular.Exceptions.V
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
    public class ValueException : PropertyException
    {
        public ValueException(
            object sender,
            string propertyName,
            object? oldValue = null,
            object? newValue = null,
            string? message = null,
            Exception? innerException = null)
            : base(sender, propertyName, message, innerException)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        ///     Contains the original value of the property.
        /// </summary>
        public object? OldValue
        {
            get => Data["OldValue"];
            private init => Data["OldValue"] = value;
        }

        /// <summary>
        ///     Contains the value that could not be stored in the property.
        /// </summary>
        public object? NewValue
        {
            get => Data["NewValue"];
            private init => Data["NewValue"] = value;
        }

        [Pure]
        public override bool Like(SemanticException? other)
            => base.Like(other)
               && other is ValueException ve
               && Equals(ve.OldValue, OldValue)
               && Equals(ve.NewValue, NewValue);

        public override string ToString()
            => $"Fault {Message} for {PropertyName} old {OldValue} new {NewValue}.";
    }
}
