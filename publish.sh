#!/bin/bash

if [ $# -lt 1 ]
then
        echo "Usage : $0 Runtime (options: 1 (windows) or  2 (mac os)"
        exit
fi

curr_dir=$(pwd)

case "$1" in

1)  echo "Building windows self contained zip"
    rm ./zip/win10-x64.MoneyCSV2QIF.zip
    rm ./MoneyCSV2QIF_Exe/*.*
    cd ./src/MoneyCSV2QIF
    dotnet publish -c Release -r win10-x64 -o ./../../MoneyCSV2QIF_Exe --self-contained
    cd ./../../
    zip -r -X ./zip/win10-x64.MoneyCSV2QIF.zip MoneyCSV2QIF_Exe
    ;;
2)  echo  "Building macos self contained zip"
    rm ./zip/macOS-x64.MoneyCSV2QIF.zip
    rm ./MoneyCSV2QIF_Exe/*.*
    cd ./src/MoneyCSV2QIF
    dotnet publish -c Release -r osx-x64 -o ./../../MoneyCSV2QIF_Exe --self-contained
    cd ./../../
    zip -r -X ./zip/macOS-x64.MoneyCSV2QIF.zip MoneyCSV2QIF_Exe
    ;;
*) echo "self contained zip option $1 is not supported"
   ;;
esac

cd $curr_dir