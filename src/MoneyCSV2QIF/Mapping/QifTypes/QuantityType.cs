namespace MoneyCSV2QIF.Mapping.QifTypes
{
    public class QuantityType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "Q";
    }
}