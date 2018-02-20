Function Add-TeamCityProjectToDefaultPool {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$id,
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$name
    )

    Begin
    {
 
    }
    
    Process
    {
        $headers = New-TeamCityHeaders
$body = @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<project id="${id}" name="${name}"/>
"@
        $result = Invoke-RestMethod -Method POST -Headers $headers -Uri http://localhost:8111/app/rest/agentPools/id:0/projects -ContentType application/xml -Body $body
    }
    
    End
    {
    
    }
}
    
    