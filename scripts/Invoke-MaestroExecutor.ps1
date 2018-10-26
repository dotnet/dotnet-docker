# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

[cmdletbinding()]
param(
    [string]$ScriptFileName,
    [string]$Arguments,
    [hashtable]$Secrets
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Evaluate any of the PS expressions in the Arguments (to resolve the secret values)
$Arguments = Invoke-Expression "`"$Arguments`""

# Clear the rest of the secrets out, so the script doesn't have access to them
$Secrets = @{}

Invoke-Expression "& `"$(Get-Location)\$ScriptFileName`" $Arguments"
