Function Start-DockerEnvironment {
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
        docker-compose -p ${project} up -d
    }
    
    End
    {
    
    }
}
    
    