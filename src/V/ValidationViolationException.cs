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

using System.ComponentModel.DataAnnotations;

namespace PPWCode.Vernacular.Exceptions.V;

public class ValidationViolationException : SemanticException
{
    private const string MemberNameskey = nameof(MemberNameskey);

    public ValidationViolationException(ValidationResult validationResult)
        : this(validationResult.MemberNames, validationResult.ErrorMessage)
    {
    }

    public ValidationViolationException(IEnumerable<string> memberNames, string? errorMessage = null)
        : base(errorMessage)
    {
        MemberNames = memberNames.ToArray();
    }

    public string[] MemberNames
    {
        get => (string[])Data[MemberNameskey]!;
        init => Data[MemberNameskey] = value;
    }

    public override bool Like(SemanticException? other)
        => base.Like(other)
           && other is ValidationViolationException e
           && MemberNames.SequenceEqual(e.MemberNames);

    public override string ToString()
        => $"{base.ToString()}, MemberNames: {string.Join(", ", MemberNames)}";
}
