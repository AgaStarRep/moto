using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DTO
{
    public class TableItems<T> where T : IEnumerable
    {
        public T rows { get; set; }

        /// <summary>
        /// Ile jest w sumie wierszy
        /// </summary>
        public int records { get; set; }
        /// <summary>
        /// Która jest akutalnie strona
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// Ile ma być stron w gridzie
        /// </summary>
        public int total { get; set; }

        public TableItems(T data, int total, int page, int lastPage)
        {
            this.rows = data;
            this.records = total;
            this.page = page;
            this.total = lastPage;
        }
    }

}
