
# Load the config from file
. $1

# Migrate existing database
grate \
    --connectionstring="User ID=$userId;Password=$pw;Host=$dbHost;Port=$dbPort;Database=$database;" \
    --sqlfilesdirectory=$dir \
    --databasetype="postgresql" \
    --environment=$env \
    --create=false \
    --verbosity="Information"  
