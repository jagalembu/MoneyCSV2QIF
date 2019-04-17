using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using MoneyCSV2QIF.Mapping;
using MoneyCSV2QIF.Mapping.QifTypes;
using Newtonsoft.Json;

namespace MoneyCSV2QIF.Tranform {
    public static class CsvToQifHelper {

        public static string QifValue (IQifType qfi, CsvReader csv) {

            if (!string.IsNullOrEmpty (qfi.FieldName)) {
                var val = csv.GetField (qfi.FieldName);
                if (string.IsNullOrEmpty (val))
                    return null;

                if ((qfi is CategoryType || qfi is SplitCategoryType) &&
                    (val.Contains ("Transfer") || val.Contains ("transfer"))) {
                    val = val.Replace ("Transfer", "TXRF");
                    val = val.Replace ("transfer", "TXRF");
                }

                return $"{qfi.Prefix}{val}";
            }
            return null;
        }

        public static List<string> BuidlAcctInformation (string acctName, string acctType) {
            var qifList = new List<string> ();
            qifList.Add (QifContants.HeaderTypeAccount);
            qifList.Add ($"N{acctName}");
            qifList.Add ($"T{AccountTypeText(acctType)}");
            qifList.Add (QifContants.EndOfEnty);

            return qifList;
        }

        public static void WriteQif (List<string> qifList, string qifFile) {
            using (StreamWriter file =
                new StreamWriter (qifFile)) {
                foreach (var item in qifList) {
                    if (item != null)
                        file.WriteLine (item);
                }
            }
        }

        public static IQifMapping<string> LoadJson (string jsonFile, string type) {
            using (StreamReader r = new StreamReader (jsonFile)) {
                string json = r.ReadToEnd ();
                switch (type) {
                    case QifContants.OptionInvestment:
                        return JsonConvert.DeserializeObject<InvestmentAccountsMapping<string>> (json);

                    case QifContants.OptionBank:
                    case QifContants.OptionCash:
                    case QifContants.OptionCreditCard:
                    case QifContants.OptionOtherAsset:
                    case QifContants.OptionOtherLiability:
                        return JsonConvert.DeserializeObject<NonInvestmentAccountsMapping<string>> (json);

                    default:
                        throw new Exception ("Incorrect account type");
                }

            }
        }

        public static string HeaderName (string type) {
            switch (type) {
                case QifContants.OptionInvestment:
                    return QifContants.HeaderTypeInvestment;
                case QifContants.OptionBank:
                    return QifContants.HeaderTypeBank;
                case QifContants.OptionCash:
                    return QifContants.HeaderTypeCash;
                case QifContants.OptionCreditCard:
                    return QifContants.HeaderTypeCCard;
                case QifContants.OptionOtherAsset:
                    return QifContants.HeaderTypeOtherA;
                case QifContants.OptionOtherLiability:
                    return QifContants.HeaderTypeOtherL;
                default:
                    throw new Exception ("Missing header name for an account type");
            }

        }

        public static string AccountTypeText (string type) {
            switch (type) {
                case QifContants.OptionInvestment:
                    return QifContants.HeaderTypeInvestment.Replace ("!Type:", "");
                case QifContants.OptionBank:
                    return QifContants.HeaderTypeBank.Replace ("!Type:", "");
                case QifContants.OptionCash:
                    return QifContants.HeaderTypeCash.Replace ("!Type:", "");
                case QifContants.OptionCreditCard:
                    return QifContants.HeaderTypeCCard.Replace ("!Type:", "");
                case QifContants.OptionOtherAsset:
                    return QifContants.HeaderTypeOtherA.Replace ("!Type:", "");
                case QifContants.OptionOtherLiability:
                    return QifContants.HeaderTypeOtherL.Replace ("!Type:", "");
                default:
                    throw new Exception ("Missing header name for an account type");
            }

        }

        public static List<string> CreateQif (IQifMapping<string> mapping, string csvFile, string acctType) {
            switch (mapping) {
                case InvestmentAccountsMapping<string> invMap:
                    return new InvestmentAccountsCsvToQif ().Convert (invMap, csvFile, acctType);

                case NonInvestmentAccountsMapping<string> nonMap:
                    return new NonInvestmentAccountsCsvToQif ().Convert (nonMap, csvFile, acctType);

                default:
                    throw new Exception ("Missing a map for account type.");
            }

        }
    }

}