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

using Microsoft.Azure.PowerShell.AssemblyLoading;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Microsoft.Azure.Commands.Profile.Utilities
{
    public static class CustomAssemblyResolver
    {
        private static IDictionary<string, (string Framework, Version Version)> NetFxPreloadAssemblies = ConditionalAssemblyProvider.GetAssemblies();

        private static string PreloadAssemblyFolder { get; set; }

        public static void Initialize()
        {
            //This function is call before loading assemblies in PreloadAssemblies folder, so NewtonSoft.Json could not be used here
            var accountFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PreloadAssemblyFolder = Path.Combine(accountFolder, "lib");
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <summary>
        /// When the resolution of an assembly fails, if will try to redirect to the higher version
        /// </summary>
        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                AssemblyName name = new AssemblyName(args.Name);
                if (NetFxPreloadAssemblies.TryGetValue(name.Name, out var assembly))
                {
                    //For Newtonsoft.Json, allow to use bigger version to replace smaller version
                    if (assembly.Version >= name.Version
                        && (assembly.Version.Major == name.Version.Major
                            || string.Equals(name.Name, "Newtonsoft.Json", StringComparison.OrdinalIgnoreCase)))
                    {
                        string requiredAssembly = Path.Combine(PreloadAssemblyFolder, assembly.Framework, $"{name.Name}.dll");
                        return Assembly.LoadFrom(requiredAssembly);
                    }
                }
            }
            catch
            {
            }
            return null;
        }
    }
}
