namespace MoneyCSV2QIF.Mapping.QifTypes
{
    public class TransactionAmountType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "T";
    }
}