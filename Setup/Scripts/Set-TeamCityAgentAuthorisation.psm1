Function Set-TeamCityAgentAuthorisation {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$id
    )

    Begin
    {
 
    }
    
    Process
    {

        $headers = New-TeamCityHeaders

        Invoke-RestMethod -Method PUT -Headers $headers -Uri http://localhost:8111/app/rest/agents/${id}/authorized -Body "true"
    }
    
    End
    {
    
    }
}
    
    