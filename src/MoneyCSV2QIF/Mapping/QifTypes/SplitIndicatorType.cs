namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class SplitIndicatorType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "S";
    }
}