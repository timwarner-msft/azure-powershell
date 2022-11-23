// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Azure.PowerShell.AssemblyLoading
{
    public static class ConditionalAssemblyExtensions
    {
        public static IConditionalAssembly WithWindowsPowerShell(this IConditionalAssembly assembly)
        {
            return assembly.WithPowerShellVersion(new Version("5.0.0"), new Version("6.0.0"));
        }
        public static IConditionalAssembly WithPowerShellCore(this IConditionalAssembly assembly)
        {
            return assembly.WithPowerShellVersion(new Version("6.0.0"));
        }
        public static IConditionalAssembly WithPowerShellVersion(this IConditionalAssembly assembly, Version lower, Version upper = null)
        {
            bool shouldLoad = lower <= assembly.Context.PSVersion;
            if (upper != null)
            {
                shouldLoad = shouldLoad && assembly.Context.PSVersion < upper;
            }
            assembly.UpdateShouldLoad(shouldLoad);
            return assembly;
        }

        public static IConditionalAssembly WithWindows(this IConditionalAssembly assembly)
            => assembly.WithOS(OSPlatform.Windows);

        public static IConditionalAssembly WithMacOS(this IConditionalAssembly assembly)
            => assembly.WithOS(OSPlatform.OSX);

        public static IConditionalAssembly WithLinux(this IConditionalAssembly assembly)
            => assembly.WithOS(OSPlatform.Linux);

        private static IConditionalAssembly WithOS(this IConditionalAssembly assembly, OSPlatform os)
        {
            assembly.UpdateShouldLoad(assembly.Context.IsOSPlatform(os));
            return assembly;
        }

        public static IConditionalAssembly WithX86(this IConditionalAssembly assembly)
            => assembly.WithOSArchitecture(Architecture.X86);
        public static IConditionalAssembly WithX64(this IConditionalAssembly assembly)
            => assembly.WithOSArchitecture(Architecture.X64);
        public static IConditionalAssembly WithArm64(this IConditionalAssembly assembly)
            => assembly.WithOSArchitecture(Architecture.Arm64);

        private static IConditionalAssembly WithOSArchitecture(this IConditionalAssembly assembly, Architecture arch)
        {
            assembly.UpdateShouldLoad(assembly.Context.OSArchitecture.Equals(arch));
            return assembly;
        }
    }
}
