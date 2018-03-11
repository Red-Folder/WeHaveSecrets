Function New-TeamCityHeaders {
    
    Begin
    {
    
    }
    
    Process
    {
        $base64AuthInfo = Get-TeamCitySuperUserToken | New-TeamCityCredentials

        $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $headers.Add("Authorization", ("Basic {0}" -f $base64AuthInfo))
        $headers.Add("Origin", 'http://localhost:8111')

        return $headers
    }
    
    End
    {
 
    }
}
    
    