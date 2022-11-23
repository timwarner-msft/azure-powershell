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

namespace Microsoft.Azure.PowerShell.AssemblyLoading
{
    public class ConditionalAssembly : IConditionalAssembly
    {
        public ConditionalAssembly(IConditionalAssemblyContext context, string name, string framework, Version version)
        {
            Context = context;
            Name = name;
            Framework = framework;
            Version = version;
            ShouldLoad = true;
        }

        public bool ShouldLoad { get; private set; }

        public void UpdateShouldLoad(bool shouldLoad)
        {
            ShouldLoad = ShouldLoad && shouldLoad;
        }

        public IConditionalAssemblyContext Context { get; }

        public Version Version { get; }

        public string Name { get; }

        public string Framework { get; }

    }
}