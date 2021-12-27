using CQRS;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_projekt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ElasticSearchController : ControllerBase
    {
        private Database db { get; }
        private IElasticClient elasticClient { get; }
        public ElasticSearchController(Database database, IElasticClient elasticClient)
        {
            this.db = database;
            this.elasticClient = elasticClient;
        }

        [HttpGet("rozruch")]
        public void InitialDatabase()
        {
            if (elasticClient.Indices.Exists("booksIndex").Exists)
            {
                elasticClient.Indices.Delete("booksIndex");
            }
            if (elasticClient.Indices.Exists("authorsIndex").Exists)
            {
                elasticClient.Indices.Delete("authorsIndex");
            }

            elasticClient.Indices.Create("authorsIndex", index => index.Map<AuthorDTO>(x => x.AutoMap()));
            elasticClient.Indices.Create("booksIndex", index => index.Map<BookDTO>(x => x.AutoMap()));

            db.Books.RemoveRange(db.Books);
            db.Authors.RemoveRange(db.Authors);
            db.AuthorsRate.RemoveRange(db.AuthorsRate);
            db.BooksRate.RemoveRange(db.BooksRate);
            db.SaveChanges();

            string firstnames = "Kamil,Tomek,Patryk,Sebastian,Ania,Krzysztof,Dawid";
            var _firstnames = firstnames.Split(',');

            string surnames = "Nowak,Kowalski,Nowakowski,Kowalczyk,Adamiakowa,Konstant,Merigold";
            var _surnames = surnames.Split(',');

            string books = "NIC,Romantic Psycho,Nadciśnienie,Atypowy,SIARA,Jarmark,Złota owca,Europa,100 dni do matury,Tamagotchi";
            var _books = books.Split(',');

            List<AuthorDTO> authorsDTO = new List<AuthorDTO>();
            List<BookDTO> booksDTO = new List<BookDTO>();
            Random rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                var author = new AuthorDTO 
                { 
                    FirstName = _firstnames[rnd.Next(0, 6)], 
                    SecondName = _surnames[rnd.Next(0, 6)],
                    CV = "brak"
                };
                authorsDTO.Add(author);
            }

            foreach(var a in authorsDTO)
            {
                var _newAuthor = new Author
                {
                    FirstName = a.FirstName,
                    SecondName = a.SecondName,
                    CV = a.CV
                };

                db.Authors.Add(_newAuthor);

                db.AuthorsRate.Add(new AuthorRate
                {
                    Type = RateType.AuthorRate,
                    Author = _newAuthor,
                    FkAuthor = _newAuthor.Id,
                    Date = DateTime.Now,
                    Value = (short)rnd.Next(1, 5)
                });
            }

            for (int i = 0; i < 10; i++)
            {
                int temp = rnd.Next(0, 9);
                AuthorDTO _authorDTO = authorsDTO[temp];
                List<BookAuthorDTO> badto = new List<BookAuthorDTO>();
                badto.Add(new BookAuthorDTO
                {
                    Id = _authorDTO.Id,
                    FirstName = _authorDTO.FirstName,
                    SecondName = _authorDTO.SecondName
                });
                var book = new BookDTO 
                {
                    Title = _books[i], 
                    Description = "do ręcznego uzupełnienia",
                    ReleaseDate = DateTime.Now,
                    Authors = badto
                };
                booksDTO.Add(book);
            }

            foreach (var b in booksDTO)
            {
                var _author = b.Authors[0];

                var _newBook = new Book
                {
                    Title = b.Title,
                    Description = b.Description,
                    ReleaseDate = b.ReleaseDate,
                    Authors = new List<Author>
                    {
                        new Author{
                            Id = _author.Id,
                            FirstName = _author.FirstName,
                            SecondName = _author.SecondName
                        }
                    }
                };

                db.Books.Add(_newBook);

                db.BooksRate.Add(new BookRate
                {
                    Type = RateType.BookRate,
                    Book = _newBook,
                    FkBook = _newBook.Id,
                    Date = DateTime.Now,
                    Value = (short)rnd.Next(1, 5)
                });
            }

            db.SaveChanges();

            foreach (var _author in authorsDTO)
            {
                elasticClient.IndexDocument<AuthorDTO>(_author);
                //elasticClient.Index(_author, i => i.Index("authorsIndex"));
            }
            foreach (var _book in booksDTO)
            {
                elasticClient.IndexDocument<BookDTO>(_book);
                //elasticClient.Index(_book, i => i.Index("booksIndex"));
            }

        }
    }
}
