﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.AddParameterCheck;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.CodeRefactorings;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.AddParameterCheck
{
    public class AddParameterCheckTests : AbstractCSharpCodeActionTest
    {
        protected override CodeRefactoringProvider CreateCodeRefactoringProvider(Workspace workspace, TestParameters parameters)
            => new CSharpAddParameterCheckCodeRefactoringProvider();

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParameterCheck)]
        public async Task TestSimpleReferenceType()
        {
            await TestInRegularAndScript1Async(
@"
using System;

class C
{
    public C([||]string s)
    {
    }
}",
@"
using System;

class C
{
    public C(string s)
    {
        if (s == null)
        {
            throw new ArgumentNullException(nameof(s));
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParameterCheck)]
        public async Task TestNullable()
        {
            await TestInRegularAndScript1Async(
@"
using System;

class C
{
    public C([||]int? i)
    {
    }
}",
@"
using System;

class C
{
    public C(int? i)
    {
        if (i == null)
        {
            throw new ArgumentNullException(nameof(i));
        }
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParameterCheck)]
        public async Task TestNotOnValueType()
        {
            await TestMissingInRegularAndScriptAsync(
@"
using System;

class C
{
    public C([||]int i)
    {
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParameterCheck)]
        public async Task TestUpdateExistingFieldAssignment()
        {
            await TestInRegularAndScript1Async(
@"
using System;

class C
{
    private string _s;

    public C([||]string s)
    {
        _s = s;
    }
}",
@"
using System;

class C
{
    private string _s;

    public C(string s)
    {
        _s = s ?? throw new ArgumentNullException(nameof(s));
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsAddParameterCheck)]
        public async Task TestUpdateExistingPropertyAssignment()
        {
            await TestInRegularAndScript1Async(
@"
using System;

class C
{
    private string S;

    public C([||]string s)
    {
        S = s;
    }
}",
@"
using System;

class C
{
    private string S;

    public C(string s)
    {
        S = s ?? throw new ArgumentNullException(nameof(s));
    }
}");
        }
    }
}