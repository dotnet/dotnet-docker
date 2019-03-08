#!/usr/bin/env pwsh

<#
.SYNOPSIS
Outputs pipeline variables with the names of statically defined image names we care
about.  Also returns back a hash object containing those names.  The hash object
can be hierarchical if the name of any files contains a period, which is used to
produce a nested object.  For example, a file named "imagebuilder.linux.txt" would
produce a property path like $imageNames.imagebuilder.linux as opposed to
$imageNames["imagebuilder.linux"].
#>

$files = Get-ChildItem scripts/image-names -Filter *.txt
$imageNames = @{}
foreach ($file in $files) {
    $imageName = (Get-Content $($file.FullName)).Trim()

    Write-Host "##vso[task.setvariable variable=imageNames.$($file.BaseName)]$imageName"

    $current = $imageNames
    # Split the file name into parts with '.' as the separator
    $fileNameParts = $file.BaseName.Split('.')
    for ($i = 0; $i -lt $fileNameParts.Length; $i++) {
        $fileNamePart = $fileNameParts[$i]

        # If this is the last part of the file name
        if ($i -eq $fileNameParts.Length - 1) {
            $current[$fileNamePart] = $imageName
        }
        elseif (-not $current[$fileNamePart]) {
            $current[$filenamepart] = @{}
        }

        $current = $current[$filenamepart]
    }
}

return $imageNames
