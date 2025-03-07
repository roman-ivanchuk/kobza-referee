#!/usr/bin/env bash

dotnet ef migrations remove \
    --project ../KobzaReferee.Persistence.Sqlite.csproj \
    --startup-project ../KobzaReferee.Persistence.Sqlite.csproj
