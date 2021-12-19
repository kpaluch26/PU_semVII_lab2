﻿using Model;
using Model.DTO;
using Nest;
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
        private IElasticClient elasticClient { get; }

        public AddBookCommandHandler(Database db, IElasticClient elasticClient)
        {
            this.db = db;
            this.elasticClient = elasticClient;
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

            //Elastic Search
            BookDTO _bookDTO = new BookDTO 
            { 
                ID = book.Id, 
                Title = book.Title, 
                ReleaseDate = book.ReleaseDate 
            };
            elasticClient.IndexDocument<BookDTO>(_bookDTO);
        }
    }
}
