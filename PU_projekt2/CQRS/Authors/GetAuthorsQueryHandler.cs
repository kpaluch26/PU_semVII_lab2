using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Authors
{
    public class GetAuthorsQueryHandler : IQueryHandler<GetAuthorsQuery, List<AuthorDTO>>
    {
        private readonly Database db;

        public GetAuthorsQueryHandler(Database db)
        {
            this.db = db;
        }
        public List<AuthorDTO> Handle(GetAuthorsQuery query)
        {
            return db.Authors.Include(b => b.Rates)
            .Include(b => b.Books)
            .Skip(query.Page * query.Count)
            .Take(query.Count)
            .ToList().Select(b => new AuthorDTO
            {
                Id = b.Id,
                FirstName = b.FirstName,
                SecondName = b.SecondName,
                AvarageRate = (b.Rates.Count() > 0 ? b.Rates.Average(r => r.Value) : 0),
                RatesCount = (b.Rates.Count() > 0 ? b.Rates.Count() : 0),
                Books = b.Books.Select(a => new AuthorBooksDTO
                {
                    Title = a.Title,
                    Id = a.Id
                }).ToList()
            }).ToList();
        }
    }
}
