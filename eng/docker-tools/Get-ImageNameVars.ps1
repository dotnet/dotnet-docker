# Returns a hashtable of variable name-to-value mapping representing the image name variables
# used by the common build infrastructure.

$vars = @{}
Get-Content $PSScriptRoot/templates/variables/docker-images.yml |
    Where-Object { $_.Trim().Length -gt 0 -and $_.Trim() -notlike 'variables:' -and $_.Trim() -notlike '# *' } |
    ForEach-Object {
        $parts = $_.Split(':', 2)
        $vars[$parts[0].Trim()] = $parts[1].Trim()
    }

return $vars
