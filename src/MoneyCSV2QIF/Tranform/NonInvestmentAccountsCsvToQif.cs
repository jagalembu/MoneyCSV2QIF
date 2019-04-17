using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using MoneyCSV2QIF.Mapping;
using MoneyCSV2QIF.Mapping.QifTypes;

namespace MoneyCSV2QIF.Tranform {
    public class NonInvestmentAccountsCsvToQif : ICsvToQif<string> {

        public List<string> Convert (IQifMapping<string> inMapping, string csvfile, string acctType) {

            var outMapping = TranformMapping (inMapping as NonInvestmentAccountsMapping<string>);

            // var rowCnt = 1;
            var headerColumns = new Dictionary<string, int> ();
            var qfiList = new List<string> ();

            var splitIndicatorMapped = false;
            if (!string.IsNullOrEmpty (outMapping.SplitIndicatorField.FieldName)) {
                splitIndicatorMapped = true;
            }

            using (var reader = new StreamReader (csvfile))
            using (var csv = new CsvReader (reader)) {
                qfiList.Add (CsvToQifHelper.HeaderName(acctType));
                csv.Read ();
                csv.ReadHeader ();

                var readRecord = csv.Read ();
                while (readRecord) {

                    bool split = false;

                    if (splitIndicatorMapped) {
                        // Initialize
                        var saveDate = csv.GetField (outMapping.DateField.FieldName);
                        var savedPayee = csv.GetField (outMapping.PayeeField.FieldName);

                        var mainDate = CsvToQifHelper.QifValue (outMapping.DateField, csv);
                        var mainPayee = CsvToQifHelper.QifValue (outMapping.PayeeField, csv);
                        var mainCleared = CsvToQifHelper.QifValue (outMapping.ClearedField, csv);
                        var mainRef = CsvToQifHelper.QifValue (outMapping.RefOrCheckNumberField, csv);
                        var mainMemo = CsvToQifHelper.QifValue (outMapping.MemoField, csv);
                        var mainCategory = CsvToQifHelper.QifValue (outMapping.CategoryField, csv);
                        var mainAmount = CsvToQifHelper.QifValue (outMapping.AmountField, csv);

                        double totalAmount = 0.00;
                        var aggList = new List<string> ();

                        split = csv.GetField (outMapping.SplitIndicatorField.FieldName) == outMapping.SplitIndicatorField.Prefix;

                        if (split) {
                            do {
                                // split = csv.GetField (outMapping.SplitIndicatorField.FieldName) == outMapping.SplitIndicatorField.Prefix;
                                // // Aggregate
                                // if (split) {
                                aggList.Add (CsvToQifHelper.QifValue (outMapping.SplitCategory, csv));
                                aggList.Add (CsvToQifHelper.QifValue (outMapping.SplitMemoField, csv));
                                aggList.Add (CsvToQifHelper.QifValue (outMapping.SplitAmountField, csv));
                                totalAmount = totalAmount + double.Parse (csv.GetField (outMapping.SplitAmountField.FieldName));
                                // }
                                readRecord = csv.Read ();
                            }
                            while (readRecord && saveDate == csv.GetField (outMapping.DateField.FieldName) &&
                                savedPayee == csv.GetField (outMapping.PayeeField.FieldName) &&
                                csv.GetField (outMapping.SplitIndicatorField.FieldName) == outMapping.SplitIndicatorField.Prefix);

                        } else {
                            readRecord = csv.Read ();
                        }

                        qfiList.Add (mainDate);
                        if (aggList.Count > 0) {
                            qfiList.Add ($"T{totalAmount}");
                        } else {
                            qfiList.Add (mainAmount);
                        }

                        qfiList.Add (mainCleared);
                        qfiList.Add (mainRef);
                        qfiList.Add (mainPayee);
                        qfiList.Add (mainMemo);
                        qfiList.Add (mainCategory);
                        qfiList.AddRange (aggList);

                    } else {
                        qfiList.Add (CsvToQifHelper.QifValue (outMapping.DateField, csv));
                        qfiList.Add (CsvToQifHelper.QifValue (outMapping.AmountField, csv));
                        qfiList.Add (CsvToQifHelper.QifValue (outMapping.ClearedField, csv));
                        qfiList.Add (CsvToQifHelper.QifValue (outMapping.RefOrCheckNumberField, csv));
                        qfiList.Add (CsvToQifHelper.QifValue (outMapping.PayeeField, csv));
                        qfiList.Add (CsvToQifHelper.QifValue (outMapping.MemoField, csv));
                        qfiList.Add (CsvToQifHelper.QifValue (outMapping.CategoryField, csv));
                        readRecord = csv.Read ();
                    }

                    qfiList.Add (QifContants.EndOfEnty);

                }
            }

            return qfiList;
        }

        private NonInvestmentAccountsMapping<IQifType> TranformMapping (NonInvestmentAccountsMapping<string> inputMapping) {
            var outMapping = new NonInvestmentAccountsMapping<IQifType> ();

            outMapping.DateField = new DateType () { FieldName = inputMapping.DateField };
            outMapping.AmountField = new AmountType () { FieldName = inputMapping.AmountField };
            outMapping.ClearedField = new ClearedStatus () { FieldName = inputMapping.ClearedField };
            outMapping.RefOrCheckNumberField = new CheckOrRefNumType () { FieldName = inputMapping.RefOrCheckNumberField };
            outMapping.PayeeField = new PayeeType () { FieldName = inputMapping.PayeeField };
            outMapping.MemoField = new MemoType () { FieldName = inputMapping.MemoField };
            outMapping.CategoryField = new CategoryType () { FieldName = inputMapping.CategoryField };
            outMapping.SplitIndicatorField = new SplitIndicatorType () { FieldName = inputMapping.SplitIndicatorField };
            outMapping.SplitCategory = new SplitCategoryType () { FieldName = inputMapping.SplitCategory };
            outMapping.SplitMemoField = new SplitMemoType () { FieldName = inputMapping.SplitMemoField };
            outMapping.SplitAmountField = new SplitAmountType () { FieldName = inputMapping.SplitAmountField };

            return outMapping;
        }

    }
}