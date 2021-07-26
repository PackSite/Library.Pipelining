Get-ChildItem -inc bin,obj -rec | Remove-Item -rec -force

# pack
dotnet pack --configuration Release

# push
$TargetSource = "C:\LocalNuGet"
Write-Host "Target source: '${TargetSource}'"

$NuGetSources = & dotnet nuget list source

$NuGetSources | Select-String 'Microsoft Visual Studio Offline Packages' -Context 0,1 | ForEach-Object {
    if ($_.Context.PostContext[0]) {
        $TargetSource = $_.Context.PostContext[0].Trim()
        Write-Host "Target source overriden with '${TargetSource}'"
    }
}

$NuGetSources | Select-String 'Local' -Context 0,1 | ForEach-Object {
    if ($_.Context.PostContext[0]) {
        $TargetSource = $_.Context.PostContext[0].Trim()
        Write-Host "Target source overriden with '${TargetSource}'"
    }
}

Copy-Item "./PackSite.Library.*/bin/Release/*.*nupkg" $TargetSource

