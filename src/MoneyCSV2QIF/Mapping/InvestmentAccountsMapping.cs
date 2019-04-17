namespace MoneyCSV2QIF.Mapping {
    public class InvestmentAccountsMapping<T> : IQifMapping<T> {
        public T DateField { get; set; }
        public T ActionField { get; set; }
        public T SecurityField { get; set; }
        public T PriceField { get; set; }
        public T QuantityField { get; set; }
        public T TransactionAmountField { get; set; }
        public T ClearedStatusField { get; set; }
        public T TextField { get; set; }
        public T MemoField { get; set; }
        public T CommisionField { get; set; }
        public T AccountForTransferField { get; set; }
        public T AmountTransferedField { get; set; }
    }
}