Function Start-TeamCity {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$project
    )

    Begin
    {
 
    }
    
    Process
    {
        docker-compose -p ${project} up -d teamcity-server
        docker-compose -p ${project} up -d teamcity-agent1        
    }
    
    End
    {
    
    }
}
    
    