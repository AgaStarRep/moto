using System;
using System.Collections.Generic;
using System.Text;

namespace Motohusaria.DTO
{
        public class TableRequest
    {
        /// <summary>
        /// Która strone wyświetlić
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// Ile ma być wierszy na stronie
        /// </summary>
        public int rows { get; set; }

        /// <summary>
        /// Pole po któym dane mają być sortowane z jqGrid
        /// </summary>
        public string sidx { get; set; }

        /// <summary>
        /// Kolejnosć sortowania z jqGrid
        /// </summary>
        public string sord { get; set; }

        public bool includePrevousPages { get; set; }
    }
}
