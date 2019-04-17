namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class CategoryType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "L";
    }
}