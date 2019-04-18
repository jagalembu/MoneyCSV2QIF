#!/bin/bash

if [ $# -lt 1 ]
then
        echo "Usage : $0 Runtime (options: 1 (windows) or  2 (mac os)"
        exit
fi

curr_dir=$(pwd)

case "$1" in

1)  echo "Building windows executable"
    rm ./exe/win10-x64.MoneyCSV2QIF.zip
    cd ./src/MoneyCSV2QIF
    dotnet publish -c Release -r win10-x64 --self-contained
    zip -r -X ./../../exe/win10-x64.MoneyCSV2QIF.zip ./bin/Release/netcoreapp3.0/win10-x64
    ;;
2)  echo  "Building macos executable"
    rm ./exe/macos-x64.MoneyCSV2QIF.zip
    cd ./src/MoneyCSV2QIF
    dotnet publish -c Release -r osx-x64 --self-contained
    zip -r -X ./../../exe/macos-x64.MoneyCSV2QIF.zip ./bin/Release/netcoreapp3.0/osx-x64
    ;;
*) echo "Runtime $1 is not supported"
   ;;
esac

cd $curr_dir