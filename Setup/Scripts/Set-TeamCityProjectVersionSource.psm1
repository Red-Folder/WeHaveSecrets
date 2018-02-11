Function Set-TeamCityProjectVersionSource {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$projectId,
        [Parameter(Mandatory=$True,ValueFromPipeline=$False,ValueFromPipelineByPropertyName=$False)]
        [string]$vcsId
    )

    Begin
    {
 
    }
    
    Process
    {

        $headers = New-TeamCityHeaders

$body = @"
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<projectFeatures>
	<projectFeature id="PROJECT_EXT_2"
		type="versionedSettings">
		<properties>
			<property name="buildSettings" value="ALWAYS_USE_CURRENT"/>
			<property name="credentialsStorageType" value="credentialsJSON"/>
			<property name="enabled" value="true"/>
			<property name="format" value="kotlin"/>
			<property name="rootId" value="${vcsId}"/>
			<property name="showChanges" value="false"/>
		</properties>
	</projectFeature>
</projectFeatures>
"@

        $result = Invoke-RestMethod -Method PUT  -Headers $headers -Uri http://localhost:8111/app/rest/projects/${projectId}/projectFeatures -ContentType application/xml -Body $body
    }
    
    End
    {
    
    }
}
    
    