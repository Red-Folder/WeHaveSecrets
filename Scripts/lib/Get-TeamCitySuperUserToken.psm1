Function Get-TeamCitySuperUserToken {
    Begin
    {

    }

    Process
    {
        return (docker logs wehavesecrets_teamcity-server_1 | select-string -Pattern '(?<=^Super user authentication token: ")(\d*)').Matches.Value | Select-Object -last 1
    }

    End
    {

    }
}

