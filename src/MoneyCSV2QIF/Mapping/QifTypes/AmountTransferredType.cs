namespace MoneyCSV2QIF.Mapping.QifTypes
{
    public class AmountTransferredType: IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "$";
    }
}