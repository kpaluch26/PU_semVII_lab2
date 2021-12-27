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
    public class GetBooksQueryHandler : IQueryHandler<GetBooksQuery, List<BookDTO>>
    {
        //private readonly Database db;
        private IElasticClient elasticClient { get; }
        public GetBooksQueryHandler(Database db, IElasticClient elasticClient)
        {
            //this.db = db;
            this.elasticClient = elasticClient;
        }

        public List<BookDTO> Handle(GetBooksQuery query)
        {
            List<BookDTO> result;
            result = elasticClient.Search<BookDTO>(
                x => x.Size(query.Count).Skip(query.Count * query.Page).Query(
                    q => q.MatchAll())).Documents.ToList();

            return result;
            /*
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
            */
        }
    }
}
