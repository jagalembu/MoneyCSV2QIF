using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using MoneyCSV2QIF.Mapping;
using MoneyCSV2QIF.Mapping.QifTypes;
using MoneyCSV2QIF.Tranform;
using Xunit;

namespace MoneyCSV2QIF.Tests.Transform {
    public class CsvToQifHelperTests : IClassFixture<FileFixture> {

        private readonly FileFixture _fileFixture;

        public CsvToQifHelperTests (FileFixture fileFixture) {
            _fileFixture = fileFixture;
        }

        [Fact]
        public void LoadJsonMapping_Inv () {

            var mapping = CsvToQifHelper.LoadJson ($"{Directory.GetCurrentDirectory()}/Transform/TestFiles/noninvmap.json", "NON");

            var nonMap = mapping as NonInvestmentAccountsMapping<string>;
            Assert.Equal ("Date", nonMap.DateField);
            Assert.Equal ("Amount", nonMap.AmountField);
            Assert.Equal ("Clr", nonMap.ClearedField);
            Assert.Equal ("Description/Category", nonMap.CategoryField);
            Assert.Equal ("Memo/Notes", nonMap.MemoField);
            Assert.Equal ("Payee/Security", nonMap.PayeeField);
            Assert.Equal ("Reference", nonMap.RefOrCheckNumberField);
            Assert.Equal ("Amount", nonMap.SplitAmountField);
            Assert.Equal ("Description/Category", nonMap.SplitCategory);
            Assert.Equal ("Memo/Notes", nonMap.SplitMemoField);
            Assert.Equal ("Split", nonMap.SplitIndicatorField);
        }

        [Fact]
        public void LoadJsonMapping_Non () {
            var mapping = CsvToQifHelper.LoadJson ($"{Directory.GetCurrentDirectory()}/Transform/TestFiles/invmap.json", "INV");

            var nonMap = mapping as InvestmentAccountsMapping<string>;

            Assert.Equal ("Date", nonMap.DateField);
            Assert.Equal ("Action", nonMap.ActionField);
            Assert.Equal ("Security", nonMap.SecurityField);
            Assert.Equal ("Price", nonMap.PriceField);
            Assert.Equal ("Quantity", nonMap.QuantityField);
            Assert.Equal ("Transaction Amount", nonMap.TransactionAmountField);
            Assert.Equal ("Clr", nonMap.ClearedStatusField);
            Assert.Equal ("Text", nonMap.TextField);
            Assert.Equal ("Memo", nonMap.MemoField);
            Assert.Equal ("Commission", nonMap.CommisionField);
            Assert.Equal ("Account Transfer", nonMap.AccountForTransferField);
            Assert.Equal ("Amount Transfer", nonMap.AmountTransferedField);

        }

        [Fact]
        public void LoadJsonMapping_IncorrectType_Exception () {
            var exp = Assert.Throws<Exception> (() =>
                CsvToQifHelper.LoadJson ($"{Directory.GetCurrentDirectory()}/Transform/TestFiles/invmap.json", "XX"));
            Assert.Equal ("Missing account type", exp.Message);
        }

        [Fact]
        public void QifValue_ActionType_WithValue () {

            using (var reader = new StreamReader ($"{Directory.GetCurrentDirectory()}/Transform/TestFiles/noninvmap_all_splits.csv"))
            using (var csv = new CsvReader (reader)) {
                csv.Read ();
                csv.ReadHeader ();
                while (csv.Read ()) {
                    var dtVal = CsvToQifHelper.QifValue (new DateType () { FieldName = "Date" }, csv);
                    Assert.Equal ("D5/22/2017", dtVal);
                }
            }

        }

        [Fact]
        public void QifValue_ActionType_With_Null_Value () {

            using (var reader = new StreamReader ($"{Directory.GetCurrentDirectory()}/Transform/TestFiles/noninvmap_all_splits.csv"))
            using (var csv = new CsvReader (reader)) {
                csv.Read ();
                csv.ReadHeader ();
                while (csv.Read ()) {
                    var dtVal = CsvToQifHelper.QifValue (new ClearedStatus () { FieldName = "Clr" }, csv);
                    Assert.Null (dtVal);
                }
            }

        }

        [Fact]
        public void QifValue_ActionType_With_No_MappedValue_Expected_Null () {

            using (var reader = new StreamReader ($"{Directory.GetCurrentDirectory()}/Transform/TestFiles/noninvmap_all_splits.csv"))
            using (var csv = new CsvReader (reader)) {
                csv.Read ();
                csv.ReadHeader ();
                while (csv.Read ()) {
                    var dtVal = CsvToQifHelper.QifValue (new ClearedStatus () { }, csv);
                    Assert.Null (dtVal);
                }
            }

        }

        [Fact]
        public void WriteQif_WithNull_ListItems () {
            var qifList = new List<string> ();

            qifList.Add ("Item1");
            qifList.Add (null);
            qifList.Add ("Item2");

            CsvToQifHelper.WriteQif (qifList, _fileFixture.TempFolder + "qifout.qif");

            _fileFixture.AssertFiles ($"{Directory.GetCurrentDirectory()}/Transform/TestFiles/Expected/qifout.qif", _fileFixture.TempFolder + "qifout.qif");

        }

        // [Fact]
        // public void WriteQif_NonInvestment_With_SplitTransactionsOnly () {
        //     var mapping = CsvToQifHelper.LoadJson ($"{Directory.GetCurrentDirectory()}/Transform/TestFiles/noninvmap.json", "NON");
        //     var qifList = CsvToQifHelper.CreateQif (mapping, $"{Directory.GetCurrentDirectory()}/Transform/TestFiles/noninvmap_all_splits.csv");
        // }

    }

}