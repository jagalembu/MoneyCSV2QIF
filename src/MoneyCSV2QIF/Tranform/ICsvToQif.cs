using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyCSV2QIF.Mapping;

namespace MoneyCSV2QIF.Tranform {
    public interface ICsvToQif<T> {
        List<string> Convert (IQifMapping<T> inMapping, string csvfile, string acctType);
    }
}