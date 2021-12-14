using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public double AvarageRate { get; set; }
        public int RatesCount { get; set; }
        public List<AuthorBooksDTO> Books { get; set; }
        public string CV { get; set; }
    }
}
