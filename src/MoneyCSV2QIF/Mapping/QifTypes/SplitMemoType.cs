namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class SplitMemoType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "E";
    }
}