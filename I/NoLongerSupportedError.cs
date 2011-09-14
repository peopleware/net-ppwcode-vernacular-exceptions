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
using System.Runtime.Serialization;

namespace PPWCode.Vernacular.Exceptions.I
{
    /// <summary>
    /// This exception is thrown by a method defined in an API,
    /// in a later version of the API, if the method that was
    /// defined in an earlier version is no longer supported in
    /// new versions.
    /// </summary>
    /// <remarks>
    /// <para>That the exception can be thrown is implicit for all
    /// API methods.</para>
    /// <para>Normally, when a new version of an API is created,
    /// it is kept backward compatible with older versions. This
    /// is often a non-trivial endeavour. Although, if it is
    /// decided to keep backward compatibility with older versions,
    /// all possible effort should be made to make the backward
    /// compatibility complete, in some cases this is logically
    /// not feasible or too costly. It than often happens that
    /// backward compatibility can be attained for all but a limited
    /// number of methods, possibly for all but a limited number
    /// of cases. It may then be interesting to ship this new
    /// not-completely-backward-compatible version, if a lot
    /// of clients do not use the not-backward-compatible part.</para>
    /// <para>This exception allows the developers of the new version
    /// to signal which method, in which circumstances, is no
    /// longer supported.</para>
    /// </remarks>
    [Serializable]
    public class NoLongerSupportedError : 
        ExternalError
    {
        public NoLongerSupportedError()
        {
        }

        public NoLongerSupportedError(string message)
            : base(message)
        {
        }

        public NoLongerSupportedError(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        public NoLongerSupportedError(System.Exception innerException)
            : base(innerException)
        {
        }
        protected NoLongerSupportedError(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
