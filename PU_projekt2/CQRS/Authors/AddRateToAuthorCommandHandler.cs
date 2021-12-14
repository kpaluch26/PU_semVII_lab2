using Model;
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

        public AddRateToAuthorCommandHandler(Database db)
        {
            this.db = db;
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
        }
    }
}
