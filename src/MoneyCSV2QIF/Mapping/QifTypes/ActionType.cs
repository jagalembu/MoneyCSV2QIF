namespace MoneyCSV2QIF.Mapping.QifTypes
{
    public class ActionType : IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "N";
    }
}