Import-Module -Name "./Utils.psm1" -Function Get-IniContent

# Load the config from file
$configs = (Get-IniContent .\db.conf)["db"]

$env = $configs.env
$database = $configs.database
$dbHost = $configs.dbHost
$dbPort = $configs.dbPort
$userId = $configs.userId
$pw = $configs.pw
$dir = $configs.dir

# Migrate existing database
grate `
    --connectionstring="User ID=$userId;Password=$pw;Host=$dbHost;Port=$dbPort;Database=$database;" `
    --sqlfilesdirectory=$dir `
    --databasetype="postgresql" `
    --environment=$env `
    --create=false `
    --verbosity="Information" `
