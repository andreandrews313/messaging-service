#!/bin/bash

set -e

echo "Starting the application..."
echo "Environment: ${ENV:-development}"

dotnet restore


dotnet ef database update

dotnet run --urls=http://localhost:8080

echo "Application started successfully!"
