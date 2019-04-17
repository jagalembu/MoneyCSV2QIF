using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using MoneyCSV2QIF.Mapping;
using MoneyCSV2QIF.Mapping.QifTypes;

namespace MoneyCSV2QIF.Tranform {
    public class CsvToQfi {
        public void Convert (NonInvestmentAccountsMapping<string> inMapping, string csvfile, string qfiFile) {

            var outMapping = TranformMapping (inMapping);

            // var rowCnt = 1;
            var headerColumns = new Dictionary<string, int> ();
            var qfiList = new List<string> ();

            var splitIndicatorMapped = false;
            if (!string.IsNullOrEmpty (outMapping.SplitIndicatorField.FieldName)) {
                splitIndicatorMapped = true;
            }

            var splitcnt = 0;
            string saveDate = "";
            string savePayee = "";

            using (var reader = new StreamReader (csvfile))
            using (var csv = new CsvReader (reader)) {
                qfiList.Add ("!Type:Bank");
                csv.Read ();
                csv.ReadHeader ();
                while (csv.Read ()) {

                    bool split = false;

                    headerColumns.TryGetValue ("Split", out int pos);

                    if (splitIndicatorMapped) {
                        if (!string.IsNullOrEmpty (outMapping.SplitIndicatorField.FieldName)) {
                            split = csv.GetField (outMapping.SplitIndicatorField.FieldName) == outMapping.SplitIndicatorField.Prefix;
                        }

                        if (splitcnt > 0) {
                            if (saveDate != csv.GetField (outMapping.DateField.FieldName) ||
                                savePayee != csv.GetField (outMapping.PayeeField.FieldName)) {
                                qfiList.Add (QifContants.EndOfEnty);
                                splitcnt = 0;
                            }

                        }

                        if (split) {
                            saveDate = csv.GetField (outMapping.DateField.FieldName);
                            savePayee = csv.GetField (outMapping.PayeeField.FieldName);
                            splitcnt++;
                        } else {
                            saveDate = "";
                            savePayee = "";
                        }
                    }

                    if (splitcnt <= 1) {
                        qfiList.Add ($"{outMapping.DateField.Prefix}{csv.GetField(outMapping.DateField.FieldName)}");
                    }

                    if (!split) {
                        qfiList.Add ($"{outMapping.AmountField.Prefix}{csv.GetField(outMapping.AmountField.FieldName)}");
                    }
                    if (!string.IsNullOrEmpty (outMapping.ClearedField.FieldName) &&
                        !string.IsNullOrEmpty (csv.GetField (outMapping.ClearedField.FieldName)) && splitcnt <= 1) {
                        qfiList.Add ($"{outMapping.ClearedField.Prefix}*");
                    }
                    if (!string.IsNullOrEmpty (outMapping.RefOrCheckNumberField.FieldName) &&
                        !string.IsNullOrEmpty (csv.GetField (outMapping.RefOrCheckNumberField.FieldName)) &&
                        splitcnt <= 1) {
                        qfiList.Add ($"{outMapping.RefOrCheckNumberField.Prefix}{csv.GetField(outMapping.RefOrCheckNumberField.FieldName)}");
                    }

                    if (splitcnt <= 1) {
                        qfiList.Add ($"{outMapping.PayeeField.Prefix}{csv.GetField(outMapping.PayeeField.FieldName)}");
                    }

                    if (!split) {
                        if (!string.IsNullOrEmpty (outMapping.MemoField.FieldName)) {
                            qfiList.Add ($"{outMapping.MemoField.Prefix}{csv.GetField(outMapping.MemoField.FieldName)}");
                        }

                    }

                    if (!split) {
                        if (!string.IsNullOrEmpty (outMapping.CategoryField.FieldName)) {
                            qfiList.Add ($"{outMapping.CategoryField.Prefix}{csv.GetField(outMapping.CategoryField.FieldName)}");
                        }

                    }

                    if (split) {
                        if (!string.IsNullOrEmpty (outMapping.SplitCategory.FieldName)) {
                            qfiList.Add ($"{outMapping.SplitCategory.Prefix}{csv.GetField(outMapping.SplitCategory.FieldName)}");
                        }
                        if (!string.IsNullOrEmpty (outMapping.SplitMemoField.FieldName)) {
                            qfiList.Add ($"{outMapping.SplitMemoField.Prefix}{csv.GetField(outMapping.SplitMemoField.FieldName)}");
                        }
                        qfiList.Add ($"{outMapping.SplitAmountField.Prefix}{csv.GetField(outMapping.SplitAmountField.FieldName)}");
                    }

                    if (!split) {
                        qfiList.Add (QifContants.EndOfEnty);
                    }

                }
            }

            // using (var reader = new StreamReader (csvfile)) {
            //     while (!reader.EndOfStream) {
            //         var line = reader.ReadLine ();

            //         var cleanLine = line.Replace ("\"", "");

            //         var values = cleanLine.Split (',');

            //         if (rowCnt == 1) {
            //             //build dict for headers text as key and poisiotn as value
            //             for (int i = 0; i < values.Length; i++) {
            //                 headerColumns.TryAdd (values[i], i);
            //             }
            //             //need to pass argument for type
            //             qfiList.Add ("!Type:Bank");
            //             rowCnt++;
            //         } else {

            //             bool split = false;

            //             headerColumns.TryGetValue ("Split", out int pos);

            //             if (splitIndicatorMapped) {
            //                 if (!string.IsNullOrEmpty (outMapping.SplitIndicatorField.FieldName)) {
            //                     split = values[headerColumns[outMapping.SplitIndicatorField.FieldName]] == outMapping.SplitIndicatorField.Prefix;
            //                 }

            //                 if (splitcnt > 0) {
            //                     if (saveDate != values[headerColumns[outMapping.DateField.FieldName]] ||
            //                         savePayee != values[headerColumns[outMapping.PayeeField.FieldName]]) {
            //                         qfiList.Add (QifContants.EndOfEnty);
            //                         splitcnt = 0;
            //                     }

            //                 }

            //                 if (split) {
            //                     saveDate = values[headerColumns[outMapping.DateField.FieldName]];
            //                     savePayee = values[headerColumns[outMapping.PayeeField.FieldName]];
            //                     splitcnt++;
            //                 } else {
            //                     saveDate = "";
            //                     savePayee = "";
            //                 }
            //             }

            //             qfiList.Add ($"{outMapping.DateField.Prefix}{values[headerColumns[outMapping.DateField.FieldName]]}");
            //             if (!split) {
            //                 qfiList.Add ($"{outMapping.AmountField.Prefix}{values[headerColumns[outMapping.AmountField.FieldName]]}");
            //             }
            //             if (!string.IsNullOrEmpty (outMapping.ClearedField.FieldName)) {
            //                 qfiList.Add ($"{outMapping.ClearedField.Prefix}*");
            //             }
            //             if (!string.IsNullOrEmpty (outMapping.RefOrCheckNumberField.FieldName)) {
            //                 qfiList.Add ($"{outMapping.RefOrCheckNumberField.Prefix}{values[headerColumns[outMapping.RefOrCheckNumberField.FieldName]]}");
            //             }

            //             qfiList.Add ($"{outMapping.PayeeField.Prefix}{values[headerColumns[outMapping.PayeeField.FieldName]]}");

            //             if (!split) {
            //                 if (!string.IsNullOrEmpty (outMapping.MemoField.FieldName)) {
            //                     qfiList.Add ($"{outMapping.MemoField.Prefix}{values[headerColumns[outMapping.MemoField.FieldName]]}");
            //                 }

            //             }

            //             if (!split) {
            //                 if (!string.IsNullOrEmpty (outMapping.CategoryField.FieldName)) {
            //                     qfiList.Add ($"{outMapping.CategoryField.Prefix}{values[headerColumns[outMapping.CategoryField.FieldName]]}");
            //                 }

            //             }

            //             if (split) {
            //                 if (!string.IsNullOrEmpty (outMapping.SplitCategory.FieldName)) {
            //                     qfiList.Add ($"{outMapping.SplitCategory.Prefix}{values[headerColumns[outMapping.SplitCategory.FieldName]]}");
            //                 }
            //                 if (!string.IsNullOrEmpty (outMapping.SplitMemoField.FieldName)) {
            //                     qfiList.Add ($"{outMapping.SplitMemoField.Prefix}{values[headerColumns[outMapping.SplitMemoField.FieldName]]}");
            //                 }
            //                 qfiList.Add ($"{outMapping.SplitAmountField.Prefix}{values[headerColumns[outMapping.SplitAmountField.FieldName]]}");
            //             }

            //             if (!split) {
            //                 qfiList.Add (QifContants.EndOfEnty);
            //             }

            //         }

            //     }
            // }

            using (StreamWriter file =
                new StreamWriter (qfiFile)) {
                foreach (var item in qfiList) {

                    file.WriteLine (item);

                }
            }

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