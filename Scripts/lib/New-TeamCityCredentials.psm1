Function New-TeamCityCredentials {

    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$True,ValueFromPipeline=$True,ValueFromPipelineByPropertyName=$False)]
        [string]$superUserToken
     )

    Begin
    {
 
    }
    
    Process
    {
        return [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes((":{0}" -f $superUserToken)))
    }
    
    End
    {
    
    }
}
    
    