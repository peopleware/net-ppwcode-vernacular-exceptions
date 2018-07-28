// Copyright 2017 by PeopleWare n.v..
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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PPWCode.Vernacular.Exceptions.III
{
    /// <summary>
    ///     Vehicle for communicating more than one <see cref="SemanticException" />
    ///     at once.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         After creation, element exceptions can be
    ///         <see cref="AddElement">Added</see> to the <see cref="Elements" />.
    ///         Once the exception is
    ///         <see cref="Closed" />, no more element exceptions can be
    ///         added.
    ///     </para>
    ///     <para>
    ///         The exception should only be thrown if it is not
    ///         <see cref="IsEmpty" />.
    ///     </para>
    /// </remarks>
    [Serializable]
    public sealed class CompoundSemanticException : SemanticException
    {
        public CompoundSemanticException()
            : base(null, null)
        {
            Set = new HashSet<SemanticException>();
        }

        public CompoundSemanticException(string message)
            : base(message)
        {
            Set = new HashSet<SemanticException>();
        }

        private CompoundSemanticException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     The element exceptions of this compound exception.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="Count" /> provides a little expensive
        ///         way to find out how many exceptions there are in the set.
        ///         <see cref="IsEmpty" /> provides a little expensive
        ///         way to find out if there are any elements in the
        ///         set.
        ///     </para>
        /// </remarks>
        private HashSet<SemanticException> Set
        {
            get => Data["Set"] as HashSet<SemanticException>;
            set => Data["Set"] = value;
        }

        /// <summary>
        ///     There are no element exceptions in <see cref="Elements" />.
        /// </summary>
        public bool IsEmpty
            => !Set.Any();

        /// <summary>
        ///     The element exceptions of this compound exception.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="Count" /> provides a little expensive
        ///         way to find out how many exceptions there are in the set.
        ///         <see cref="IsEmpty" /> provides a little expensive
        ///         way to find out if there are any elements in the
        ///         set.
        ///     </para>
        /// </remarks>
        public ICollection<SemanticException> Elements
            => Set.ToArray();

        /// <summary>
        ///     The number of <see cref="Elements">element exceptions</see>.
        /// </summary>
        public int Count
            => Set.Count;

        /// <summary>
        ///     No more <see cref="Elements">element exceptions</see>
        ///     can be added if this is <c>Closed</c>.
        ///     <para>
        ///         The setter is deprecated. Use <see cref="Close" />
        ///         instead.
        ///     </para>
        /// </summary>
        public bool Closed
        {
            get => (Data["Closed"] as bool?).GetValueOrDefault();
            set => Data["Closed"] = value;
        }

        /// <summary>
        ///     Close the exception for the addition of <see cref="Elements" />.
        /// </summary>
        public void Close()
        {
            Closed = true;
        }

        /// <summary>
        ///     Add an element exception to <see cref="Elements" />.
        /// </summary>
        /// <param name="exception">The exception that must be added.</param>
        public void AddElement(SemanticException exception)
        {
            CompoundSemanticException cse = exception as CompoundSemanticException;
            if (cse == null)
            {
                Set.Add(exception);
            }
            else
            {
                foreach (SemanticException ex in cse.Elements)
                {
                    AddElement(ex);
                }
            }
        }

        /// <summary>
        ///     This contains an <see cref="Elements">element</see>
        ///     <see cref="SemanticException.Like" /> <paramref name="exception" />.
        /// </summary>
        /// <param name="exception">The exception to compare with.</param>
        /// <returns>
        ///     A boolean indicating whether <see cref="CompoundSemanticException">this</see>
        ///     contains a <see cref="SemanticException" /> like <paramref name="exception" />.
        /// </returns>
        [Pure]
        public bool ContainsElement(SemanticException exception)
        {
            return Set.Any(x => x.Like(exception));
        }

        /// <summary>
        ///     This exception is semantically like the <paramref name="other" />
        ///     exception, and contains exceptions that are
        ///     <see cref="Like">alike</see>.
        /// </summary>
        /// <param name="other">The <see cref="SemanticException" /> to compare against.</param>
        /// <returns>
        ///     A boolean indicating whether <see cref="CompoundSemanticException">this</see>
        ///     and <paramref name="other" /> are alike.
        /// </returns>
        public override bool Like(SemanticException other)
        {
            if (!base.Like(other))
            {
                return false;
            }

            CompoundSemanticException ce = (CompoundSemanticException)other;
            return (ce.Elements.Count == Elements.Count)
                   && Elements.All(x => ce.Elements.Any(x.Like))
                   && ce.Elements.All(x => Elements.Any(x.Like));
        }

        public override string ToString()
        {
            try
            {
                StringBuilder sb = new StringBuilder(1024);
                foreach (SemanticException se in Set)
                {
                    sb.AppendLine(se.ToString());
                }

                return sb.Length == 0 ? Message : sb.ToString();
            }
            catch
            {
                return Message;
            }
        }
    }
}
