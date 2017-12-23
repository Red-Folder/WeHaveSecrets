Function Add-TeamCityVcs {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$id,
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$url
    )

    Begin
    {
 
    }
    
    Process
    {

        $headers = New-TeamCityHeaders

$body = @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
    <vcs-root id="${id}"
            name="${url}"
            vcsName="jetbrains.git">
    <project id="_Root"/>
    <properties>
        <property name="agentCleanFilesPolicy" value="ALL_UNTRACKED"/>
        <property name="agentCleanPolicy" value="ON_BRANCH_CHANGE"/>
        <property name="authMethod" value="ANONYMOUS"/>
        <property name="branch" value="refs/heads/master"/>
        <property name="ignoreKnownHosts" value="true"/>
        <property name="submoduleCheckout" value="CHECKOUT"/>
        <property name="url" value="${url}"/>
        <property name="useAlternates" value="true"/>
        <property name="usernameStyle" value="USERID"/>
        </properties>
</vcs-root>
"@
        Invoke-RestMethod -Method POST -Headers $headers -Uri http://localhost:8111/app/rest/vcs-roots -ContentType application/xml -Body $body        
    }
    
    End
    {
    
    }
}
    
    