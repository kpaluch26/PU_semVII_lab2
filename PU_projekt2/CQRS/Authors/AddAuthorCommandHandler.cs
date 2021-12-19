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
    public class AddAuthorCommandHandler : ICommandHandler<AddAuthorCommand>
    {
        private Database db { get; }
        private IElasticClient elasticClient { get; }

        public AddAuthorCommandHandler(Database db, IElasticClient elasticClient)
        {
            this.db = db;
            this.elasticClient = elasticClient;
        }

        public void Handle(AddAuthorCommand command)
        {
            var author = new Author
            {
                FirstName = command.FirstName,
                SecondName = command.SecondName

            };
            author.Books = db.Books.Where(a => command.BooksIDs.Contains(a.Id)).ToList();
            db.Authors.Add(author);
            db.SaveChanges();

            //Elastic Search
            AuthorDTO _authorDTO = new AuthorDTO 
            { 
                Id = author.Id, 
                FirstName = author.FirstName, 
                SecondName = author.SecondName 
            };
            //elasticClient.IndexDocument<AuthorDTO>(_authorDTO);
            elasticClient.Index(_authorDTO, i => i.Index("authorsIndex"));
        }
    }
}
