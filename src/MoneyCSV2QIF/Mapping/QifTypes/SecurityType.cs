namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class SecurityType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "Y";
    }
}