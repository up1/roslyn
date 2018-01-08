﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using VSCommanding = Microsoft.VisualStudio.Commanding;

namespace Microsoft.CodeAnalysis.Editor.CSharp.EventHookup
{
    internal partial class EventHookupCommandHandler : IChainedCommandHandler<InvokeCompletionListCommandArgs>
    {
        public void ExecuteCommand(InvokeCompletionListCommandArgs args, Action nextHandler, CommandExecutionContext context)
        {
            AssertIsForeground();
            if (EventHookupSessionManager.QuickInfoSession == null || EventHookupSessionManager.QuickInfoSession.IsDismissed)
            {
                nextHandler();
            }
        }

        public VSCommanding.CommandState GetCommandState(InvokeCompletionListCommandArgs args, Func<VSCommanding.CommandState> nextHandler)
        {
            AssertIsForeground();
            return nextHandler();
        }
    }
}