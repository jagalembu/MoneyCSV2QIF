namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class AmountType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "T";
    }
}