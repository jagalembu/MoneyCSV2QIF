# MoneyCSV2QIF

MoneyCSV2QIF lets you convert a CSV file of bank or investment transaction into QIF format.

MoneyCSV2QIF a command line application is written in C# using Net Core SDK and is supported on Windows 10 (64 bit) and macOS.

Why did i do this?

My sister uses a Mac version of Quicken and decided to leave the Apple universe and joined the Microsoft evil empire. As a result, Quicken a desktop application has its limitation when moving their files across operating system platforms.
macOS -> Windows - Only option is QFX file format. The problem, it does not carry categories and my sister went into panic mode. She is one of those people that records all her financial transactions.

How did I solve the problem?

macOS->Quicken allows you the option to export all your transaction into a CSV file. windows->Quicken allows you to import QIF files which does not loose the categories. So the idea for this program began and the family promise to alleviate my sisters stress.

By the way Quicken is a paid product.

## Table of Content

- [Mapping Json File](#mapping-json-file)

  - [Noninvestment](#noninvestment)

  - [Investment](#investment)

- [How to run MoneyCSV2QIF](#how-to-run-MoneyCSV2QIF)

  - [macOS](#macos)

  - [Windows](#windows)

## Mapping Json File

Create mapping file with association of CSV file headers. The mapping file is needed to retrieve CSV corresponding QIF specification.

### Noninvestment

Create a json file with following content. Associate it CSV file header.

According to QIF specification noninvestment accounts are bank, cash, credit card, other liability, or other asset accounts.
So if you choose any of the B (for Bank), C (for Cash), CC (for Credit Card), OA (for Asset), OL (for Liability) a noninvestment mapping file need to be created.

Ex. noninvestment.json

```sh
{
  "DateField": "Name of CSV header column",
  "AmountField": "Name of CSV header column",
  "ClearedField": "Name of CSV header column",
  "RefOrCheckNumberField": "Name of CSV header column",
  "PayeeField": "Name of CSV header column",
  "MemoField": "Name of CSV header column",
  "CategoryField": "Name of CSV header column",
  "SplitIndicatorField": "Name of CSV header column",
  "SplitCategory": "Name of CSV header column",
  "SplitMemoField": "Name of CSV header column",
  "SplitAmountField": "Name of CSV header column"
}
```

### Investment

Create a json file with following content. Associate it CSV file header.

According to QIF specification investment are investment accounts only.
So if you choose I (for Investment) a investment mapping file needs to be created.

Ex. investment.json

```sh
{
  "DateField": "Name of CSV header column",
  "ActionField": "Name of CSV header column",
  "SecurityField": "Name of CSV header column",
  "PriceField": "Name of CSV header column",
  "QuantityField": "Name of CSV header column",
  "TransactionAmountField": "Name of CSV header column",
  "ClearedStatusField": "Name of CSV header column",
  "TextField": "Name of CSV header column",
  "MemoField": "Name of CSV header column",
  "CommisionField": "Name of CSV header column",
  "AccountForTransferField": "Name of CSV header column",
  "AmountTransferedField": "Name of CSV header column"
}
```

## How to run MoneyCSV2QIF

### macOS

**Download `macOS-x64.MoneyCSV2QIF.zip`**

You only need to download it once.

```sh
curl -Lo MoneyCSV2QIF.zip https://github.com/jagalembu/MoneyCSV2QIF/releases/download/v0.1.0/macOS-x64.MoneyCSV2QIF.zip

unzip MoneyCSV2QIF.zip
```

**Run `MoneyCSV2QIF`**

Running without the required options:

```sh
MoneyCSV2QIF_Exe/MoneyCSV2QIF
MoneyCSV2QIF 0.1.0
Copyright (c) 2019 Gursaran Brarrad

ERROR(S):
  Required option 'a, accoutnname' is missing.
  Required option 't, accountType' is missing.
  Required option 'm, mappingfile' is missing.
  Required option 'i, csvfile' is missing.
  Required option 'o, output' is missing.

  -a, --accoutnname    Required. Specify the name of the account name this CSV file belongs to.

  -t, --accountType    Required. Specify the type of account this CSV is associated with. Valid values: B (for Bank), C (for
                       Cash), CC (for Credit Card), OA (for Asset), OL (for Liability), I (for Investment)

  -m, --mappingfile    Required. Specify the location of the json mapping file.

  -i, --csvfile        Required. Specify the CSV filename and path

  -o, --output         Required. Specify file name and path for the output QIF file

  --help               Display this help screen.

  --version            Display version information.
```

Runing with the required options for noninvestment example

```sh
MoneyCSV2QIF_Exe/MoneyCSV2QIF -a "Test Bank Account" -t B -m "./noninvestment.json" -i "./testbankaccount.csv" -o "./testbankaccount.qif"
```

Runing with the required options for investment example

```sh
MoneyCSV2QIF_Exe/MoneyCSV2QIF -a "Test investment Account" -t B -m "./investment.json" -i "./testinvstaccount.csv" -o "./testinvstaccount.qif"
```

### Windows

**Download `win10-x64.MoneyCSV2QIF.zip`**

You only need to download it once. Powershell commands.

```powershell
PS C:\yourfolder> [Net.ServicePointManager]::SecurityProtocol = "tls12, tls11, tls" ; Invoke-WebRequest https://github.com/jagalembu/MoneyCSV2QIF/releases/download/v0.1.0/win10-x64.MoneyCSV2QIF.zip -OutFile MoneyCSV2QIF.zip

PS C:\yourfolder>Expand-Archive -Path MoneyCSV2QIF.zip
```

**Run `MoneyCSV2QIF`**

```powershell
PS C:\yourfolder> .\MoneyCSV2QIF\MoneyCSV2QIF_Exe\MoneyCSV2QIF
MoneyCSV2QIF 0.1.0
Copyright (c) 2019 Gursaran Brarrad

ERROR(S):
  Required option 'a, accoutnname' is missing.
  Required option 't, accountType' is missing.
  Required option 'm, mappingfile' is missing.
  Required option 'i, csvfile' is missing.
  Required option 'o, output' is missing.

  -a, --accoutnname    Required. Specify the name of the account name this CSV file belongs to.

  -t, --accountType    Required. Specify the type of account this CSV is associated with. Valid values: B (for Bank), C
                       (for Cash), CC (for Credit Card), OA (for Asset), OL (for Liability), I (for Investment)

  -m, --mappingfile    Required. Specify the location of the json mapping file.

  -i, --csvfile        Required. Specify the CSV filename and path

  -o, --output         Required. Specify file name and path for the output QIF file

  --help               Display this help screen.

  --version            Display version information.
```

Runing with the required options for noninvestment example

```powershell
PS C:\yourfolder> .\MoneyCSV2QIF\MoneyCSV2QIF_Exe\MoneyCSV2QIF -a "Test Bank Account" -t B -m "./noninvestment.json" -i "./testbankaccount.csv" -o "./testbankaccount.qif"
```

Runing with the required options for investment example

```powershell
PS C:\yourfolder> .\MoneyCSV2QIF\MoneyCSV2QIF_Exe\MoneyCSV2QIF -a "Test investment Account" -t B -m "./investment.json" -i "./testinvstaccount.csv" -o "./testinvstaccount.qif"
```