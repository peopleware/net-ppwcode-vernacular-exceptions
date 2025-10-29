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
using System.Text;

namespace PPWCode.Vernacular.Exceptions.V
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
    public sealed class CompoundSemanticException : SemanticException
    {
        private readonly ISet<SemanticException> _set = new HashSet<SemanticException>();

        public CompoundSemanticException(string? message = null)
            : base(message)
        {
        }

        /// <summary>
        ///     There are no element exceptions in <see cref="Elements" />.
        /// </summary>
        public bool IsEmpty
            => !_set.Any();

        /// <summary>
        ///     The element exceptions to this compound exception.
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
            => _set.ToArray();

        /// <summary>
        ///     The number of <see cref="Elements">element exceptions</see>.
        /// </summary>
        public int Count
            => _set.Count;

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
            private set => Data["Closed"] = value;
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
            if (exception is CompoundSemanticException cse)
            {
                foreach (SemanticException ex in cse.Elements)
                {
                    AddElement(ex);
                }
            }
            else
            {
                _set.Add(exception);
            }
        }

        /// <summary>
        ///     Add all semantic exceptions given by, <paramref name="ces" />, to <see cref="Elements" />.
        /// </summary>
        /// <param name="ces">The exceptions that must be added.</param>
        public void AddElements(IEnumerable<SemanticException> ces)
        {
            foreach (SemanticException ce in ces)
            {
                AddElement(ce);
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
            return _set.Any(x => x.Like(exception));
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
        public override bool Like(SemanticException? other)
            => base.Like(other)
               && other is CompoundSemanticException e
               && (e.Elements.Count == Elements.Count)
               && Elements.All(x => e.Elements.Any(x.Like))
               && e.Elements.All(x => Elements.Any(x.Like));

        public override string ToString()
        {
            try
            {
                StringBuilder sb = new (1024);
                foreach (SemanticException se in _set)
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
