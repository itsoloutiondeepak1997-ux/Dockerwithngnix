#!/bin/bash

# Start SQL Server in the background
echo "Starting SQL Server..."
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
echo "Waiting for SQL Server to start..."
attempts=0
max_attempts=30
while ! /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Docker@123" -C -Q "SELECT 1" > /dev/null 2>&1; do
  echo "Waiting for SQL Server to start... (attempt $((attempts + 1)) of $max_attempts)"
  sleep 10
  attempts=$((attempts + 1))
  if [ $attempts -ge $max_attempts ]; then
    echo "SQL Server did not start in time."
    exit 1
  fi
done

echo "SQL Server is up and running."
echo "Listing /Init-db contents:"
ls -l /Init-db
# Extract database name from init.sql
db_name=$(grep -oP '(?<=CREATE DATABASE\s)\S+' /Init-db/init.sql | tr -d '[];')

if [ -z "$db_name" ]; then
  echo "No CREATE DATABASE statement found in init.sql."
  exit 1
fi

echo "Database name extracted from init.sql: $db_name"

# Check if the database already exists
db_exists=$(/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Docker@123" -C -Q "IF EXISTS (SELECT name FROM sys.databases WHERE name = '$db_name') SELECT 1 ELSE SELECT 0" -h -1 -W | tr -d '[:space:]')

if [ "$db_exists" -eq "1" ]; then
  echo "Database '$db_name' already exists. Skipping creation."
else
  echo "Database '$db_name' does not exist. Running the SQL script to create the database..."
  /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Docker@123" -C -i /Init-db/init.sql > /tmp/sqlcmd_output.log 2>&1
  
  if [ $? -eq 0 ]; then
    echo "SQL script executed successfully."
  else
    echo "SQL script execution failed. Check /tmp/sqlcmd_output.log for details."
    cat /tmp/sqlcmd_output.log
    exit 1
  fi
fi

# Keep the container running
echo "Keeping the container running..."
tail -f /dev/null
