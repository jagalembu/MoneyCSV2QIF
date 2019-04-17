namespace MoneyCSV2QIF.Mapping.QifTypes {
    public class SplitCategoryType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get;  } = "S";
    }
}