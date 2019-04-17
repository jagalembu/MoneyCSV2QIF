namespace MoneyCSV2QIF.Mapping.QifTypes
{
    public class PriceType: IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "I";
    }
}