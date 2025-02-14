﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.CompilerServices;
using osu.Framework.Testing;

namespace osu.Game.Tests
{
    /// <summary>
    /// A headless host which cleans up before running (removing any remnants from a previous execution).
    /// </summary>
    public class CleanRunHeadlessGameHost : TestRunHeadlessGameHost
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="gameSuffix">An optional suffix which will isolate this host from others called from the same method source.</param>
        /// <param name="bindIPC">Whether to bind IPC channels.</param>
        /// <param name="realtime">Whether the host should be forced to run in realtime, rather than accelerated test time.</param>
        /// <param name="callingMethodName">The name of the calling method, used for test file isolation and clean-up.</param>
        public CleanRunHeadlessGameHost(string gameSuffix = @"", bool bindIPC = false, bool realtime = true, [CallerMemberName] string callingMethodName = @"")
            : base(callingMethodName + gameSuffix, bindIPC, realtime)
        {
        }

        protected override void SetupForRun()
        {
            Storage.DeleteDirectory(string.Empty);

            // base call needs to be run *after* storage is emptied, as it updates the (static) logger's storage and may start writing
            // log entries from another source if a unit test host is shared over multiple tests, causing a file access denied exception.
            base.SetupForRun();
        }
    }
}
