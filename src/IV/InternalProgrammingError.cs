// Copyright 2026 by PeopleWare n.v..
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
#if NETSTANDARD2_0 || NET462_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace PPWCode.Vernacular.Exceptions.IV;

/// <summary>
///     This subclass of <see cref="ProgrammingError" />
///     indicates a programming error inside this backend. This typically means that
///     some internal code is breaking pre- or post-conditions, or invariants.
/// </summary>
#if NETSTANDARD2_0 || NET462_OR_GREATER
[Serializable]
#endif
public class InternalProgrammingError : ProgrammingError
{
    public InternalProgrammingError()
    {
    }

    public InternalProgrammingError(string message)
        : base(message)
    {
    }

    public InternalProgrammingError(string message, Exception innerException)
        : base(message, innerException)
    {
    }

#if NETSTANDARD2_0 || NET462_OR_GREATER
    protected InternalProgrammingError(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
#endif
}
