#! /bin/bash

# Load the config from file
. ./db.conf


# Drop the database

# Second way - Use psql
psql -d "host=$dbHost port=$dbPort dbname=$defaultDb user=$adminUserId password=$adminPw" -c "select pg_terminate_backend(pid) from pg_stat_activity where datname='$database';"
psql -d "host=$dbHost port=$dbPort dbname=$defaultDb user=$adminUserId password=$adminPw" -c "DROP DATABASE IF EXISTS $database;"

# Create new database
grate \
    --connectionstring="User ID=$userId;Password=$pw;Host=$dbHost;Port=$dbPort;Database=$database;" \
    --adminconnectionstring "Host=$dbHost;Database=$defaultDb;Username=$adminUserId;Password=$adminPw" \
    --sqlfilesdirectory=$dir \
    --databasetype="postgresql" \
    --environment=$env \
    --create=true \
    --verbosity="Information"
