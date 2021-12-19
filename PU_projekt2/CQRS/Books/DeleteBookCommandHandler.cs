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
    public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
    {
        private readonly Database db;
        private IElasticClient elasticClient { get; }

        public DeleteBookCommandHandler(Database db, IElasticClient elasticClient)
        {
            this.db = db;
            this.elasticClient = elasticClient;
        }

        public void Handle(DeleteBookCommand command)
        {
            var book = db.Books.Find(command.Id);
            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();
            }
            elasticClient.Delete<BookDTO>(command.Id);
        }

    }
}
