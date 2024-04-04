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

using System;
#if NETSTANDARD2_0
using System.Runtime.Serialization;
#endif

namespace PPWCode.Vernacular.Exceptions.IV
{
    /// <summary>
    ///     Exception that signals a refusal to perform
    ///     the nominal effect by a method, because it is not allowed.
    /// </summary>
#if NETSTANDARD2_0
    [Serializable]
#endif
    public class IllegalOperationException : SemanticException
    {
        public IllegalOperationException()
        {
        }

        public IllegalOperationException(string message)
            : base(message)
        {
        }

        public IllegalOperationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if NETSTANDARD2_0
        protected IllegalOperationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
