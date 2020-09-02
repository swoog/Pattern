isGenericTypeParam(
  [string]$versionNumber
)
Write-Output "Pack all projects"
$projects = Get-ChildItem -Path "src" -Recurse "*.csproj"
Write-Output $projects
foreach($project in $projects){
	Write-Output "dotnet pack --no-build $($project.FullName)"
	dotnet pack -c Release --no-build $($project.FullName) --version-suffix $versionNumber
}