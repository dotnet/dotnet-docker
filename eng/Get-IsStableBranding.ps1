#!/usr/bin/env pwsh

<#
.SYNOPSIS
Returns a value indicating whether the specified version is associated with stable branding.
#>
[cmdletbinding()]
param(
    # Build version of the product
    [string]
    $Version
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version 2.0

if ($Version.Contains("-servicing") -or $Version.Contains("-rtm")) {
    return $true
}

return $false
