#!/bin/bash
  
if [ "$ASPNETCORE_APPSETTINGS" ]
then
        echo "$ASPNETCORE_APPSETTINGS" > appsettings.json
fi

dotnet DentalDrill.CRM.dll