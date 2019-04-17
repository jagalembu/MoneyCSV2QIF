namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class CheckOrRefNumType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "N";
    }
}