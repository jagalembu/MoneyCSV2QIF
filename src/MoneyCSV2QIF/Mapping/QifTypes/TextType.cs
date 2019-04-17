namespace MoneyCSV2QIF.Mapping.QifTypes
{
    public class TextType: IQifType {
        public string FieldName { get; set; }
        public string Prefix { get; } = "P";
    }
    
}