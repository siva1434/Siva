using System.Collections.Generic;

namespace _360LawGroup.CostOfSalesBilling.Data.Common
{
    public class SearchModel
    {
        public SearchModel()
        {
            limit = 10;
            offset = 0;
            search = new Dictionary<string, string>();
        }
        public Dictionary<string, string> search { get; set; }

        public int limit { get; set; }
        public int offset { get; set; }
        public string order { get; set; }
        public string sort { get; set; }

        public SearchModel Clone()
        {
            var cloneObj = new SearchModel();
            cloneObj.limit = this.limit;
            cloneObj.offset = this.offset;
            cloneObj.order = this.order;
            cloneObj.search = new Dictionary<string, string>(this.search);
            cloneObj.sort = this.sort;
            return cloneObj;
        }
    }
}