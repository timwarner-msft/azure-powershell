$assemblyRootPath = [System.IO.Path]::Combine($PSScriptRoot, "..", "lib")
$conditionalAssemblyContext = [Microsoft.Azure.PowerShell.AssemblyLoading.ConditionalAssemblyContext]::new($assemblyRootPath, $Host.Version)
[Microsoft.Azure.PowerShell.AssemblyLoading.ConditionalAssemblyProvider]::Initialize($conditionalAssemblyContext)

if ($PSEdition -eq 'Desktop') {
  try {
    [Microsoft.Azure.Commands.Profile.Utilities.CustomAssemblyResolver]::Initialize()
  }
  catch {
    Write-Warning $_
  }
}
else {
  try {
    Add-Type -Path ([System.IO.Path]::Combine($PSScriptRoot, "..", "Microsoft.Azure.PowerShell.AuthenticationAssemblyLoadContext.dll")) | Out-Null
    Write-Debug "Registering Az shared AssemblyLoadContext for path: '$assemblyRootPath'."
    [Microsoft.Azure.PowerShell.AuthenticationAssemblyLoadContext.AzAssemblyLoadContextInitializer]::RegisterAzSharedAssemblyLoadContext($assemblyRootPath)
    Write-Debug "AssemblyLoadContext registered."
  }
  catch {
    Write-Warning $_
  }
}