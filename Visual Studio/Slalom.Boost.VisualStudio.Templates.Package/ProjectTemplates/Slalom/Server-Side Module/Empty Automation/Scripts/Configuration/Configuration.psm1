[System.Management.Automation.WildcardOptions]$script:WildCardOptions = [System.Management.Automation.WildcardOptions]::Compiled -bor [System.Management.Automation.WildcardOptions]::IgnoreCase;
Add-Type -assembly System.Configuration;

$ConnectionStrings = [configuration.configurationManager]::ConnectionStrings;

$Field = [configuration.configurationelementcollection].GetField("bReadOnly", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance);
$Field.SetValue($ConnectionStrings, $false);

function Add-ConnectionString
{
    [cmdletbinding()]
    param(
		
        [parameter(mandatory=$true)]
        [string] $Name
        
        ,[parameter(mandatory=$true)]
        [string] $ConnectionString
        
        ,[parameter()]
        [string] $ProviderName
   );

    process
    {    
        $c = New-Object system.configuration.connectionstringsettings -arg $Name, $ConnectionString, $ProviderName
        $ConnectionStrings.Add($c);
    }
}

$ApplicationSettings = [configuration.configurationManager]::AppSettings;

$Field = [system.collections.specialized.nameobjectcollectionbase].GetField("_readOnly", [System.Reflection.BindingFlags]([System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance));
$Field.SetValue($ApplicationSettings, $false);

$collType = [system.configuration.configurationmanager].assembly.getType('System.Configuration.KeyValueInternalCollection');
$Field = $collType.GetField("_root",[System.Reflection.BindingFlags]([System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance));

$AppSettingsValue = $Field.GetValue($ApplicationSettings);
$Field = [configuration.configurationelement].GetField("_bReadOnly", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance);
$Field.SetValue($AppSettingsValue, $false);

$Field = [configuration.AppSettingsSection].GetProperty("Settings", [System.Reflection.BindingFlags]([System.Reflection.BindingFlags]::public -bor [System.Reflection.BindingFlags]::Instance));
$AppSettingsSettings = $Field.GetValue($AppSettingsValue);

$Field = [configuration.configurationelementcollection].GetField("bReadOnly", [System.Reflection.BindingFlags]::NonPublic -bor [System.Reflection.BindingFlags]::Instance);
$Field.SetValue($AppSettingsSettings, $false);

function Add-AppSetting
{
    [cmdletbinding()]
    param(
        [parameter(mandatory=$true)]
        [string] $Name
        
        ,[parameter(mandatory=$true)]
        [string] $Value
   );

    process
    {    
        $AppSettingsSettings.add($Name, $Value);
    }
}

Remove-Item variable:/field;
Remove-Item variable:/colltype;