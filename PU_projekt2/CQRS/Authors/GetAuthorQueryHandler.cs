using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Authors
{
    public class GetAuthorQueryHandler: IQueryHandler<GetAuthorQuery, AuthorDTO>
    {
        //private readonly Database db;
        private IElasticClient elasticClient { get; }

        public GetAuthorQueryHandler(Database db, IElasticClient elasticClient)
        {
            //this.db = db;
            this.elasticClient = elasticClient;
        }

        public AuthorDTO Handle(GetAuthorQuery query)
        {
            AuthorDTO result;
            result = elasticClient.Get<AuthorDTO>(query.Id).Source;
            return result;
            /*
            return db.Authors
                .Include(b => b.Rates)
                .Include(b => b.Books)
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
                }).Where(b => b.Id == query.Id).Single();
                */
        }
    }
}
