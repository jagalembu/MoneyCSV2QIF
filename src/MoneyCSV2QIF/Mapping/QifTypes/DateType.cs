namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class DateType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "D";
    }
}