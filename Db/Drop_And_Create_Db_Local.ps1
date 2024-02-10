Import-Module -Name "./Utils.psm1" -Function Get-IniContent

# Load the config from file
$configs = (Get-IniContent $args[0])["db"]

$env = $configs.env
$database = $configs.database
$dbHost = $configs.dbHost
$dbPort = $configs.dbPort
$userId = $configs.userId
$pw = $configs.pw
$dir = $configs.dir
$defaultDb = $configs.defaultDb
$adminUserId = $configs.adminUserId
$adminPw = $configs.adminPw

# Drop the database
# First way - Use dropdb.exe
# & "$Env:Programfiles\PostgreSQL\15\bin\dropdb.exe" --force --if-exists --host=$dbHost --port=$dbPort --username=$adminUserId --password $database

# Second way - Use psql
psql -d "host=$dbHost port=$dbPort dbname=$defaultDb user=$adminUserId password=$adminPw" -c "select pg_terminate_backend(pid) from pg_stat_activity where datname='$database';"
psql -d "host=$dbHost port=$dbPort dbname=$defaultDb user=$adminUserId password=$adminPw" -c "DROP DATABASE IF EXISTS $database;"

# Create new database
grate `
    --connectionstring="User ID=$userId;Password=$pw;Host=$dbHost;Port=$dbPort;Database=$database;" `
    --adminconnectionstring "Host=$dbHost;Database=$defaultDb;Username=$adminUserId;Password=$adminPw" `
    --sqlfilesdirectory=$dir `
    --databasetype="postgresql" `
    --environment=$env `
    --create=true `
    --verbosity="Information" `
