using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using MoneyCSV2QIF.Mapping;
using MoneyCSV2QIF.Mapping.QifTypes;

namespace MoneyCSV2QIF.Tranform {
    public class InvestmentAccountsCsvToQif : ICsvToQif<string> {
        public List<string> Convert (IQifMapping<string> inMapping, string csvfile, string acctType) {
            var outMapping = TranformMapping (inMapping as InvestmentAccountsMapping<string>);

            var headerColumns = new Dictionary<string, int> ();
            var qfiList = new List<string> ();

            using (var reader = new StreamReader (csvfile))
            using (var csv = new CsvReader (reader)) {
                qfiList.Add (CsvToQifHelper.HeaderName (acctType));
                csv.Read ();
                csv.ReadHeader ();
                while (csv.Read ()) {
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.DateField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.ActionField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.SecurityField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.PriceField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.QuantityField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.TransactionAmountField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.ClearedStatusField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.TextField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.MemoField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.CommisionField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.AccountForTransferField, csv));
                    qfiList.Add (CsvToQifHelper.QifValue (outMapping.AmountTransferedField, csv));
                    qfiList.Add (QifContants.EndOfEnty);

                }

            }
            return qfiList;
        }

        private InvestmentAccountsMapping<IQifType> TranformMapping (InvestmentAccountsMapping<string> inputMapping) {
            var outMapping = new InvestmentAccountsMapping<IQifType> ();

            outMapping.DateField = new DateType () { FieldName = inputMapping.DateField };
            outMapping.MemoField = new MemoType () { FieldName = inputMapping.MemoField };
            outMapping.ActionField = new ActionType () { FieldName = inputMapping.ActionField };
            outMapping.AccountForTransferField = new AccountForTransferType () { FieldName = inputMapping.AccountForTransferField };
            outMapping.AmountTransferedField = new AmountTransferredType () { FieldName = inputMapping.AmountTransferedField };
            outMapping.ClearedStatusField = new ClearedStatus () { FieldName = inputMapping.ClearedStatusField };
            outMapping.CommisionField = new CommisionType () { FieldName = inputMapping.CommisionField };
            outMapping.PriceField = new PriceType () { FieldName = inputMapping.PriceField };
            outMapping.QuantityField = new QuantityType () { FieldName = inputMapping.QuantityField };
            outMapping.TextField = new TextType () { FieldName = inputMapping.TextField };
            outMapping.SecurityField = new SecurityType () { FieldName = inputMapping.SecurityField };
            outMapping.TransactionAmountField = new TransactionAmountType () { FieldName = inputMapping.TransactionAmountField };

            return outMapping;
        }
    }
}