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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

#endregion

namespace PPWCode.Vernacular.Exceptions.I
{
    /// <summary>
    /// Vehicle for communicating more than one <see cref="SemanticException"/>
    /// at once.
    /// </summary>
    /// <remarks>
    /// <para>After creation, element exceptions can be
    /// <see cref="AddElement">Added</see> to the <see cref="Elements"/>.
    /// Once the exception is
    /// <see cref="Closed"/>, no more element exceptions can be
    /// added.</para>
    /// <para>The exception should only be thrown if it is not
    /// <see cref="Empty"/>.</para>
    /// </remarks>
    [Serializable]
    public sealed class CompoundSemanticException :
        SemanticException
    {
        #region Constructors

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

        //public CompoundSemanticException(string message, Exception innerException)
        //    : base(message, innerException)
        //{
        //    Set = new HashSet<SemanticException>();
        //}

        private CompoundSemanticException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The element exceptions of this compound exception.
        /// </summary>
        /// <remarks>
        /// <para><see cref="Count"/> provides a little expensive
        /// way to find out how many exceptions there are in the set.
        /// <see cref="IsEmpty"/> provices a little expensive
        /// way to find out if there are any elements in the
        /// set.</para>
        /// </remarks>
        private HashSet<SemanticException> Set
        {
            get
            {
                return Data["Set"] as HashSet<SemanticException>;
            }
            set
            {
                Data["Set"] = value;
            }
        }

        /// <summary>
        /// There are no element exceptions in <see cref="Elements"/>.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                Contract.Ensures(Contract.Result<bool>() == (Count == 0));

                return !Set.Any();
            }
        }

        /// <summary>
        /// The element exceptions of this compound exception.
        /// </summary>
        /// <remarks>
        /// <para><see cref="Count"/> provides a little expensive
        /// way to find out how many exceptions there are in the set.
        /// <see cref="IsEmpty"/> provices a little expensive
        /// way to find out if there are any elements in the
        /// set.</para>
        /// </remarks>
        public ICollection<SemanticException> Elements
        {
            get
            {
                return Set.ToArray();
            }
        }

        /// <summary>
        /// The number of <see cref="Elements">element exceptions</see>.
        /// </summary>
        public int Count
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() == Elements.Count);

                return Set.Count;
            }
        }

        /// <summary>
        /// No more <see cref="Elements">element exceptions</see>
        /// can be added if this is <c>Closed</c>.
        /// <para>The setter is deprecated. Use <see cref="Close"/>
        /// instead.</para>
        /// </summary>
        public bool Closed
        {
            get
            {
                return (Data["Closed"] as bool?).GetValueOrDefault();
            }
            set
            {
                Contract.Requires(!Closed);
                Contract.Requires(value == true);
                Contract.Ensures(Closed);

                Data["Closed"] = value;
            }
        }

        /// <summary>
        /// Close the exception for the addition of <see cref="Elements"/>.
        /// </summary>
        public void Close()
        {
            Contract.Ensures(Closed);

            Closed = true;
        }

        #endregion

        /// <summary>
        /// Add an element exception to <see cref="Elements"/>.
        /// </summary>
        public void AddElement(SemanticException exception)
        {
            Contract.Requires(exception != null);
            Contract.Requires(!Closed);
            Contract.Ensures(
                exception is CompoundSemanticException
                    ? ((CompoundSemanticException)exception).Elements.All(ContainsElement)
                    : Elements.Contains(exception));

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
        /// This contains an <see cref="Elements">element</see>
        /// <see cref="SemanticException.Like"/> <paramref name="exception"/>.
        /// </summary>
        [Pure]
        public bool ContainsElement(SemanticException exception)
        {
            Contract.Ensures(Contract.Result<bool>() == Elements.Any(x => x.Like(exception)));

            return Set.Any(x => x.Like(exception));
        }

        /// <summary>
        /// This exception is semantically like the <paramref name="other"/>
        /// exception, and contains exceptions that are
        /// <see cref="Like">alike</see>.
        /// </summary>
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