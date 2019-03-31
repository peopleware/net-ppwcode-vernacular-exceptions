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
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace PPWCode.Vernacular.Exceptions.IV
{
    /// <summary>
    ///     An <c>Error</c> signals undefined behavior of code, and thus that the application is in an undefined state. The
    ///     execution context must stop immediately (crash).
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         An error signals undefined behavior of code, and thus that the application is in an undefined state.
    ///         From now on, none of the reasonings, proofs, or tests done to make sure any code behaves correctly is valid
    ///         anymore, because all reasonings, proofs or tests started from a defined state. The execution context must
    ///         stop immediately to prevent further harm, and notify appropriate people.
    ///     </para>
    ///     <para>
    ///         This type is superfluous, since any <see cref="Exception" /> that is not defined as an expected non-nominal
    ///         outcome of a method that occurs during the execution of a method is an <c>Error</c>. This is because the
    ///         caller of that method can only have reasoned about expected outcomes of that method, which implies they have
    ///         to be defined. The type and its subtypes are used to tag exceptions explicitly as <c>Errors</c>, in the
    ///         rare situation where we can.
    ///     </para>
    ///     <para>
    ///         Almost all possible <c>Errors</c> are violations of explicit or implicit, internal or external
    ///         preconditions. The only exception known to us at this time is when we want to signal that execution has
    ///         reached a branch we believed to be unreachable.
    ///     </para>
    ///     <para>
    ///         During non-sandboxed execution, the execution context is the application itself. The occurrence of an
    ///         <c>Error</c> must be logged as <c>fatal</c>, and the application as a whole must terminate immediately
    ///         with a non-zero exit code (crash).
    ///     </para>
    ///     <para>
    ///         During sandboxed execution, e.g., during a request-response cycle in a service, the execution context is the
    ///         sandbox, assuming it is well-coded. This implies that no state leaks out of the sandbox, so that the sandbox
    ///         state being undefined is ensured not to infect other execution contexts. The occurrence of an <c>Error</c>
    ///         must be logged as <c>error</c>, and the sandbox as a whole must terminate immediately with an appropriate
    ///         response code (e.g., <c>500</c> for HTTP).
    ///     </para>
    ///     <para>
    ///         As <see cref="Exception" />s, <c>Errors</c> should never be caught in code, except at the border before
    ///         leaving the execution context. For non-sandboxed executions, this is the <c>main</c> method. For sandboxed
    ///         executions, this is where the sandbox starts and ends. There, 1 catch-all should be implemented, that
    ///         catches any exceptions that reach that point. This code must be limited to logging the occurence
    ///         appropriately, and terminating the execution context, translating the <c>Error</c> in the appropriate way
    ///         (e.g., into a non-zero exit code, or generic HTTP status <c>500</c>). This translation can only be generic,
    ///         since the <c>Error</c> is undefined. Any more specific reporting implies reasoning about the <c>Error</c>,
    ///         and then it is no longer an <c>Error</c>. This catch-all code can be tested using mocks, that throw an
    ///         uncaught exception.
    ///     </para>
    ///     <para>
    ///         Catching an <c>Error</c> on the way up through the stack to the border of the execution context is an
    ///         anti-pattern. Often, this is done to log the <c>Error</c>. Yet, this is not necessary, because it will be
    ///         logged by the catch-all. It is an anti-pattern because 1) this intermediate catch-clause is code like any
    ///         other, and thus must be tested. Yet, that is difficult, and an extra cost in initial development and
    ///         maintenance. 2) We far too often see programmers writing out the <c>Error</c>, and not rethrowing it. The
    ///         <c>Error</c> has now been eaten, and the application is in an undefined state. This bad code is difficult
    ///         to find, and impossible to detect with unit tests. 3) There is not anything sensible you can do in the
    ///         catch-clause, since the occurence of the <c>Error</c> is by definition undefined and thus unexpected. You
    ///         cannot do anything sensible in the catch-clause because you cannot reason about the occurrence. If you can
    ///         reason about an <see cref="Exception" /> in a catch-clause, it was defined as a non-nominal outcome of the
    ///         code in the try-clause, and thus to be expected, and its occurrence is not an <c>Error</c>. 4) Any code can
    ///         have bugs. Code you do not write is mathematically correct.
    ///     </para>
    ///     <para>
    ///         It is possible that in your reasoning about a defined <see cref="Exception" /> you decide its occurence is an
    ///         error in a particular situation. In that case, your handling of the <see cref="Exception" /> could be to
    ///         translate it into an <c>Error</c>, with the <see cref="Exception" /> as an
    ///         <see cref="Exception.InnerException" />, and to throw the <c>Error</c>. On the other hand, you can also let
    ///         the original exception fall through your method in the situations where you deem it an error, and not define
    ///         that type of <see cref="Exception" /> as an expected non-nominal outcome of your method. That is less code,
    ///         and thus less chance for bugs, and less maintenance. If you do translate the <see cref="Exception" /> into an
    ///         <c>Error</c>, that code that must be tested like any other code. Since you deem the occurrence of the
    ///         <see cref="Exception" /> an error in this situation, it will be difficult to produce a test case that covers
    ///         this code.
    ///     </para>
    /// </remarks>
    [Serializable]
    public class Error : Exception
    {
        public Error()
        {
        }

        public Error(string message)
            : base(message)
        {
        }

        public Error(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected Error([NotNull] SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
