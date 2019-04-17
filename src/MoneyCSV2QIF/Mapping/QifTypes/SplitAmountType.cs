namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class SplitAmountType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "$";
    }
}