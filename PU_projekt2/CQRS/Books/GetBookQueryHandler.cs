using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public class GetBookQueryHandler : IQueryHandler<GetBookQuery, BookDTO>
    {
        private readonly Database db;
        private IElasticClient elasticClient { get; }

        public GetBookQueryHandler(IElasticClient elasticClient, Database db )
        {
            this.db = db;
            this.elasticClient = elasticClient;
        }

        public BookDTO Handle(GetBookQuery query)
        {
            BookDTO result = elasticClient.Search<BookDTO>(s => s.Index("booksIndex").Query(q => q.QueryString(qs => qs.Fields(p => p.Field(x => x.ID)).Query(query.Id.ToString())))).Documents.First();
            return result;
            /*
            return db.Books
                .Include(b => b.Rates)
                .Include(b => b.Authors)
                .ToList().Select(b => new BookDTO
                {
                    ID = b.Id,
                    ReleaseDate = b.ReleaseDate,
                    AvarageRate = (b.Rates.Count() > 0 ? b.Rates.Average(r => r.Value) : 0),
                    RatesCount = (b.Rates.Count() > 0 ? b.Rates.Count() : 0),
                    Title = b.Title,
                    Authors = b.Authors.Select(a => new BookAuthorDTO
                    {
                        FirstName = a.FirstName,
                        Id = a.Id,
                        SecondName = a.SecondName
                    }).ToList()
                }).Where(b => b.ID == query.Id ).Single();
            */
        }
    }
}
