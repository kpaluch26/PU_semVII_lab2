using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    [ElasticsearchType(IdProperty = nameof(ID))]
    public class BookDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double AvarageRate { get; set; }
        public int RatesCount { get; set; }
        public List<BookAuthorDTO> Authors { get; set; }
        public string Description { get; set; }
    }
}
