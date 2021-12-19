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
    public class AddRateToAuthorCommandHandler : ICommandHandler<AddRateToAuthorCommand>
    {
        private Database db { get; }
        private IElasticClient elasticClient { get; }

        public AddRateToAuthorCommandHandler(Database db, IElasticClient elasticClient)
        {
            this.db = db;
            this.elasticClient = elasticClient;
        }

        public void Handle(AddRateToAuthorCommand command)
        {
            var author = db.Authors.Where(x => x.Id == command.index).Single();

            db.AuthorsRate.Add(new AuthorRate
            {
                Type = RateType.AuthorRate,
                Author = author,
                FkAuthor = author.Id,
                Date = DateTime.Now,
                Value = (short)command.rate
            });

            db.SaveChanges();

            //Elastic Search
            var _author = db.Authors
                .Include(a => a.Rates)
                .Include(a => a.Books)
                .ToList().Select(a => new AuthorDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    SecondName = a.SecondName,
                    AvarageRate = (a.Rates.Count() > 0 ? a.Rates.Average(r => r.Value) : 0),
                    RatesCount = (a.Rates.Count() > 0 ? a.Rates.Count() : 0),
                    Books = a.Books.Select(b => new AuthorBooksDTO
                    {
                        Title = b.Title,
                        Id = b.Id,
                    }).ToList()
                }).Where(b => b.Id == command.index).Single();
            elasticClient.IndexDocument<AuthorDTO>(_author);
        }
    }
}
