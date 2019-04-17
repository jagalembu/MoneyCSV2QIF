namespace MoneyCSV2QIF.Mapping.QifTypes {
    public interface IQifType {
        string FieldName { get; set; }
        string Prefix { get; }
    }
}