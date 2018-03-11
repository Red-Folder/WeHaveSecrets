Function Add-TeamCityProject {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$name
    )

    Begin
    {
 
    }
    
    Process
    {

        $headers = New-TeamCityHeaders

        $result = Invoke-RestMethod -Method POST  -Headers $headers -Uri http://localhost:8111/app/rest/projects -ContentType text/plain -Body $name
    }
    
    End
    {
    
    }
}
    
    