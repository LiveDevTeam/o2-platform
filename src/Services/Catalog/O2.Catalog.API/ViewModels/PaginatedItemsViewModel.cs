using System;
using System.Collections.Generic;

namespace O2.Catalog.API.ViewModels
{
    public class PadinatedItemsViewModel<TEntity> where TEntity :class
    {
       public int PageSize { get; set; }

       public int PageIndex { get; set; }

       public int Count { get; set; }

       public IEnumerable<TEntity> Data { get; set; }

        public PadinatedItemsViewModel(int pageIndex,int pageSize, int count, IEnumerable<TEntity> data)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.Data = data;
            this.Count = count;
        }

    }
}
