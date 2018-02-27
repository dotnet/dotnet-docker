param(
    [string]$Branch='master',
    [string]$Manifest='manifest.json',
    [string]$ImageBuilderImageName='microsoft/dotnet-buildtools-prereqs:image-builder-jessie-20171122115946',
    [string]$RepoName
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Path "$PSScriptRoot" -Parent

if ([String]::IsNullOrWhiteSpace($RepoName))
{
    $remoteUrl = $null
    if ([Uri]::TryCreate(((git config --get remote.origin.url) | Out-String), [UriKind]::Absolute, [ref]$remoteUrl))
    {
        $RepoName = [System.IO.Path]::GetFileNameWithoutExtension($remoteUrl.ToString())
    }
    if ([String]::IsNullOrWhiteSpace($RepoName))
    {
        Write-Error 'Could not automatically determine repository name. Add -RepoName <REPO> to override.'
    }
}

& docker pull $ImageBuilderImageName

& docker run --rm `
    -v /var/run/docker.sock:/var/run/docker.sock `
    -v "${repoRoot}:/repo" `
    -w /repo `
    $ImageBuilderImageName `
    generateTagsReadme --update-readme --manifest ${Manifest} "https://github.com/dotnet/${RepoName}/blob/${Branch}"
