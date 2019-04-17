namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class ClearedStatus : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "C";
    }
}