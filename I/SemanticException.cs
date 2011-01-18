//Copyright 2010 - $Date: 2008-11-15 23:58:07 +0100 (za, 15 nov 2008) $ by PeopleWare n.v..

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
    /// Supertype for exceptions related to semantics: the nominal effect of a method could
    /// not be reached, because doing so under the given circumstances would violate semantics
    /// (often type invariants).
    /// </summary>
    [Serializable]
    public class SemanticException :
        ApplicationException
    {
        public SemanticException()
        {
        }

        public SemanticException(string message)
            : base(message)
        {
            Contract.Ensures(Message == message);
        }

        public SemanticException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SemanticException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public virtual string ExceptionCode
        {
            get
            {
                return Message;
            }
        }

        /// <summary>
        /// This most be override and strengtenth to include extra properties in subclasses
        /// </summary>
        [Pure]
        public virtual bool Like(SemanticException other)
        {
            Contract.Ensures(other == null
                                 ? Contract.Result<bool>() == false
                                 : true);
            Contract.Ensures(other == this
                                 ? Contract.Result<bool>() == true
                                 : true);
#if EXTRA_CONTRACTS
// pragma to avoid warning, not an error: GetType() isn't Pure
            Contract.Ensures(other != null && GetType() != other.GetType()
                                 ? Contract.Result<bool>() == false
                                 : true);
#endif
            Contract.Ensures(other != null && other.Message != Message
                                 ? Contract.Result<bool>() == false
                                 : true);
            Contract.Ensures(other != null && other.InnerException != InnerException
                                 ? Contract.Result<bool>() == false
                                 : true);

            if (other == null || GetType() != other.GetType())
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