using System;
using System.Collections.Generic;

namespace O2.Catalog.API.ViewModels
{
    public class PaginatedItemsViewModel<TEntity> where TEntity :class
    {
       public int PageSize { get; set; }

       public int PageIndex { get; set; }

       public long Count { get; set; }

       public IEnumerable<TEntity> Data { get; set; }

        public PaginatedItemsViewModel(int pageIndex,int pageSize, long count, IEnumerable<TEntity> data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Data = data;
            this.Count = count;
        }

    }
}
