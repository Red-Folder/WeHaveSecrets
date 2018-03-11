Function Initialize-TeamCityProject {
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

        $await = $True
        while ($await) {
            Write-Host -NoNewline "."
            Start-Sleep -s 5
            try {
                $result = Invoke-RestMethod -Method GET  -Headers $headers -Uri http://localhost:8111/app/rest/buildTypes

                #Write-Host $result.buildTypes.Count
                if ($result.buildTypes.Count -gt 0) {
                    $await = $False
                }
            } catch {
                #Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__ 
                #Write-Host "StatusDescription:" $_.Exception.Response.StatusDescription
            }
        }
        Write-Host
    }
    
    End
    {
    
    }
}
    
    