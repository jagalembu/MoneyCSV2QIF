namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class PayeeType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "P";
    }
}