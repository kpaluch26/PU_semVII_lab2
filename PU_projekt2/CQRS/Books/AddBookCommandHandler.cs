using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public class AddBookCommandHandler : ICommandHandler<AddBookCommand>
    {
        private Database db { get; }

        public AddBookCommandHandler(Database db)
        {
            this.db = db;
        }

        public void Handle(AddBookCommand command)
        {
            Book book = new Book
            {
                Title = command.Title,
                ReleaseDate = command.ReleaseDate
            };

            book.Authors = db.Authors.Where(a => command.AuthorsIDs.Contains(a.Id)).ToList();
            db.Books.Add(book);
            db.SaveChanges();
        }
    }
}
