namespace MoneyCSV2QIF.Mapping {
    public class NonInvestmentAccountsMapping<T> : IQifMapping<T> {
        public T DateField { get; set; }
        public T AmountField { get; set; }
        public T ClearedField { get; set; }
        public T RefOrCheckNumberField { get; set; }
        public T PayeeField { get; set; }
        public T MemoField { get; set; }
        public T AddressLine1Field { get; set; }
        public T AddressLine2Field { get; set; }
        public T AddressLine3Field { get; set; }
        public T AddressLine4Field { get; set; }
        public T AddressLine5Field { get; set; }
        public T AddressLine6Field { get; set; }
        public T CategoryField { get; set; }
        public T SplitIndicatorField { get; set; }
        public T SplitCategory { get; set; }
        public T SplitMemoField { get; set; }
        public T SplitAmountField { get; set; }

    }
}