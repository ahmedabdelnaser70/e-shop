using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    // this calss because we have many params so it just access the params what we need throw this calss
    public class ProductSpecParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }

        private const int MaxPageSize = 50;
        private int _pageSize = 6;

        public int PageIndex { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }
}
