using System;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using MoneyCSV2QIF.Mapping;
using MoneyCSV2QIF.Tranform;

namespace MoneyCSV2QIF {
    class Program {

        public class Options {

            [Option ('a', "accoutnname", Required = true, HelpText = "Specify the name of the account name this CSV file belongs to.")]
            public string AccountName { get; set; }

            [Option ('t', "accountType", Required = true, HelpText = "Specify the type of account this CSV is associated with. Valid values: B (for Bank), C (for Cash), CC (for Credit Card), OA (for Asset), OL (for Liability), I (for Investment) ")]
            public string AccountType { get; set; }

            [Option ('m', "mappingfile", Required = true, HelpText = "Specify the location of the json mapping file.")]
            public string MappingFile { get; set; }

            // [Option ('t', "typeaccount", Required = true, HelpText = "Specify the type of account. Two options are: INV or NON. INV is for Investment Accounts and NON is for Noninvestment Accounts")]
            // public string TypeAccount { get; set; }

            [Option ('i', "csvfile", Required = true, HelpText = "Specify the CSV filename and path")]
            public string CsvFile { get; set; }

            [Option ('o', "output", Required = true, HelpText = "Specify file name and path for the output QIF file")]
            public string Output { get; set; }

        }

        static void Main (string[] args) {

            var pArgs = Parser.Default.ParseArguments<Options> (args).WithParsed<Options> (
                o => {
                    try {
                        var mapping = CsvToQifHelper.LoadJson (o.MappingFile, o.AccountType);
                        var acctList = CsvToQifHelper.BuidlAcctInformation (o.AccountName, o.AccountType);
                        var qifList = CsvToQifHelper.CreateQif (mapping, o.CsvFile, o.AccountType);
                        acctList.AddRange (qifList);
                        CsvToQifHelper.WriteQif (acctList, o.Output);
                    } catch (Exception exp) {
                        Console.WriteLine ("Failed processing : " + exp.Message);
                    }
                });

            // var converter = new CsvToQfi ();

            // var mapping = new NonInvestmentAccountsMapping<string> () {
            //     DateField = "Date", AmountField = "Amount",
            //     ClearedField = "Clr", RefOrCheckNumberField = "Reference",
            //     PayeeField = "Payee/Security", MemoField = "Memo/Notes",
            //     CategoryField = "Description/Category", SplitIndicatorField = "Split",
            //     SplitCategory = "Description/Category", SplitMemoField = "Memo/Notes",
            //     SplitAmountField = "Amount"

            // };

            // // converter.Convert (mapping, "/Users/gursaranbrarrad/play/gurmitmoney/GB-CheckingCiti-2019-03-29.csv",
            // //     "/Users/gursaranbrarrad/play/gurmitmoney/GB-CheckingCiti-2019-03-29.qif");
            // converter.Convert (mapping, "/Users/gursaranbrarrad/play/gurmitmoney/GB-SavingsWFB-2019-03-29.csv",
            //     "/Users/gursaranbrarrad/play/gurmitmoney/GB-SavingsWFB-2019-03-29.qif");
        }
    }
}