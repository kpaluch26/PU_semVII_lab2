using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public class GetBooksQueryHandler : IQueryHandler<GetBooksQuery, List<BookDTO>>
    {
        private readonly Database db;
        public GetBooksQueryHandler(Database db)
        {
            this.db = db;
        }

        public List<BookDTO> Handle(GetBooksQuery query)
        {
            return db.Books
                .Include(b => b.Rates)
                .Include(b => b.Authors)
                .Skip(query.Page * query.Count)
                .Take(query.Count)
                .ToList()
                .Select(b => new BookDTO
                {
                    ID = b.Id,
                    ReleaseDate = b.ReleaseDate,
                    AvarageRate = b.Rates.Count > 0 ? b.Rates.Average(r => r.Value) : 0,
                    RatesCount = b.Rates.Count,
                    Title = b.Title,
                    Authors = b.Authors.Select(a => new BookAuthorDTO
                    {
                        FirstName = a.FirstName,
                        SecondName = a.SecondName,
                        Id = a.Id
                    }).ToList()
                }).ToList();
        }
    }
}
