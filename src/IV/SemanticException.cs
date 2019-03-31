// Copyright 2019 by PeopleWare n.v..
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace PPWCode.Vernacular.Exceptions.IV
{
    /// <summary>
    ///     Super type for exceptions related to semantics: the nominal effect of a method could
    ///     not be reached, because doing so under the given circumstances would violate semantics
    ///     (often type invariants).
    /// </summary>
    [Serializable]
    public class SemanticException : ApplicationException
    {
        public SemanticException()
        {
        }

        public SemanticException(string message)
            : base(message)
        {
        }

        public SemanticException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SemanticException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     The <see cref="Exception.Message" /> can not be overridden
        ///     in this hierarchy. This property is sealed.
        /// </summary>
        [Pure]
        public sealed override string Message
            => base.Message;

        /// <summary>
        ///     Deprecated. Use <see cref="Message" /> instead.
        /// </summary>
        [Pure]
        [Obsolete("ExceptionCode is deprecated. Use Message instead.")]
        public virtual string ExceptionCode
            => Message;

        /// <summary>
        ///     This must be overridden and strengthened to include extra properties in subclasses.
        /// </summary>
        /// <param name="other">The <see cref="SemanticException" /> to compare against.</param>
        /// <returns>
        ///     A boolean indicating whether <see cref="SemanticException">this</see>
        ///     and <paramref name="other" /> are alike.
        /// </returns>
        [Pure]
        public virtual bool Like(SemanticException other)
        {
            if ((other == null) || (GetType() != other.GetType()))
            {
                return false;
            }

            if (other == this)
            {
                return true;
            }

            return (other.Message == Message) && (other.InnerException == InnerException);
        }
    }
}
