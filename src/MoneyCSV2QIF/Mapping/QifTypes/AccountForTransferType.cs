namespace MoneyCSV2QIF.Mapping.QifTypes
{
    public class AccountForTransferType: IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "L";
    }
}