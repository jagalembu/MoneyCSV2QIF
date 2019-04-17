namespace MoneyCSV2QIF.Mapping.QifTypes
{
    public class CommisionType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "O";
    }
}