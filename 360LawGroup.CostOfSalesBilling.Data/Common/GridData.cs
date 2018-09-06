using System.Collections.Generic;

namespace _360LawGroup.CostOfSalesBilling.Data.Common
{
    public class GridData<T>
    {
        public GridData(List<T> list, SearchModel model, int totalRows, int timeInterval)
        {
            foreach(var r in list)
                r.NullToEmpty(timeInterval);
            this.rows = list;
            this.total = totalRows;
            this.CurrentSearch = model;
        }

        public SearchModel CurrentSearch { get; set; }

        public List<T> rows { get; set; }

        public int total { get; set; }

        public bool hasNext
        {
            get
            {
                var totalPages = total / CurrentSearch.limit;
                if(total % CurrentSearch.limit > 0 && total > CurrentSearch.limit)
                {
                    totalPages++;
                }
                return totalPages > 0 && CurrentSearch.offset <= totalPages;
            }
        }
    }
}
