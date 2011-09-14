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

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace PPWCode.Vernacular.Exceptions.I
{
    /// <summary>
    /// Exceptions that are not semantically relevant.
    /// When an exception of this type occurs, code should
    /// fail gracefully, and, in the best of cases, shut
    /// down the application, after warning the appropriate
    /// people.
    /// </summary>
    [Serializable]
    public class Error : 
        System.Exception
    {
        public Error()
            : base()
        {
            Contract.Ensures((Message == null) && (InnerException == null));
        }

        public Error(string message)
            : base(message)
        {
            Contract.Ensures((Message == message) && (InnerException == null));
        }

        public Error(string message, System.Exception innerException)
            : base(message, innerException)
        {
            Contract.Ensures((Message == message) && (InnerException == innerException));
        }

        protected Error(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
