namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class MemoType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "M";
    }
}