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
            author = db.Authors.Where(x => x.Id == command.index).Single();

            UpdateResponse<AuthorDTO> updateResponse = elasticClient.Update<AuthorDTO>(command.index, u => u.Doc(new AuthorDTO
            {
                AvarageRate = author.Rates.Average(a => a.Value),
                RatesCount = author.Rates.Count()
            }));
        }
    }
}
