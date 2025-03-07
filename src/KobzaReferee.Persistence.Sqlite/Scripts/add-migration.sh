#!/usr/bin/env bash

MIGRATION_NAME=$1
if [ -z "$MIGRATION_NAME" ]; then
    echo "No migration name provided!"
    echo "Usage: ./add-migration.sh <MigrationName>"
    exit 1
fi

dotnet ef migrations add "$MIGRATION_NAME" \
    --project ../KobzaReferee.Persistence.Sqlite.csproj \
    --startup-project ../KobzaReferee.Persistence.Sqlite.csproj
