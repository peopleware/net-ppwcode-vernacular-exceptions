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

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace PPWCode.Vernacular.Exceptions.I
{
    /// <summary>
    /// Exception that signals a refusal to perform
    /// the nominal effect by a method, because of
    /// security reasons.
    /// </summary>
    [Serializable]
    public class SecurityException : 
        ApplicationException
    {
        public SecurityException()
            : base()
        {
        }

        public SecurityException(string message)
            : base(message)
        {
        }

        public SecurityException(string message, System.Exception innerException)
            : base(message, innerException)
        {
            Contract.Ensures(Message == message && InnerException == innerException);
        }

        protected SecurityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
