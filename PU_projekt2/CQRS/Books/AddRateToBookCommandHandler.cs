using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public class AddRateToBookCommandHandler : ICommandHandler<AddRateToBookCommand>
    {
        private Database db { get; }

        public AddRateToBookCommandHandler(Database db)
        {
            this.db = db;
        }

        public void Handle(AddRateToBookCommand command)
        {
            var book = db.Books.Where(x => x.Id == command.index).Single();

            db.BooksRate.Add(new BookRate
            {
                Type = RateType.BookRate,
                Book = book,
                FkBook = book.Id,
                Date = DateTime.Now,
                Value = (short)command.rate
            });

            db.SaveChanges();
        }
    }
}
