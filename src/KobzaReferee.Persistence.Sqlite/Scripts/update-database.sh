#!/usr/bin/env bash

# Optional argument to specify a target migration.
TARGET_MIGRATION=$1

if [ -z "$TARGET_MIGRATION" ]; then
    # No argument => update to the latest migration
    dotnet ef database update \
        --project ../KobzaReferee.Persistence.Sqlite.csproj \
        --startup-project ../KobzaReferee.Persistence.Sqlite.csproj
else
    # If user provided a specific migration
    dotnet ef database update "$TARGET_MIGRATION" \
        --project ../KobzaReferee.Persistence.Sqlite.csproj \
        --startup-project ../KobzaReferee.Persistence.Sqlite.csproj
fi
