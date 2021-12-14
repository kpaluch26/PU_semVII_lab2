using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Authors
{
    public class DeleteAuthorCommandHandler : ICommandHandler<DeleteAuthorCommand>
    {
        private readonly Database db;

        public DeleteAuthorCommandHandler(Database db)
        {
            this.db = db;
        }

        public void Handle(DeleteAuthorCommand command)
        {
            var author = db.Authors.Include(x => x.Books).Where(x => x.Id == command.index).FirstOrDefault();

            if (author.Books.Any() == false)
            {
                db.Authors.Remove(author);
                db.SaveChanges();
            }

        }
    }
}
