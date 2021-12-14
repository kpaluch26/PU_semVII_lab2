using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class BookRequestDTO
    {
        public string Title { get; set; }

        public DateTime ReleaseDate { get; set; }
        public List<int> AuthorsIDs { get; set; }
    }
}
