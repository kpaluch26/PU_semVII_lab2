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
                    FirstName = _firstnames[rnd.Next(7)], 
                    SecondName = _surnames[rnd.Next(7)] 
                };
                authorsDTO.Add(author);

                db.Authors.Add(new Author 
                { 
                    FirstName = author.FirstName, 
                    SecondName = author.SecondName 
                });
            }

            for (int i = 0; i < 10; i++)
            {
                var book = new BookDTO 
                {
                    Title = _books[i], 
                    ReleaseDate = DateTime.Now 
                };
                booksDTO.Add(book);

                db.Books.Add(new Book 
                { 
                    Title = book.Title, 
                    ReleaseDate = book.ReleaseDate 
                });
            }

            db.SaveChanges();

            foreach (var _author in authorsDTO)
            {
                //elasticClient.IndexDocument<AuthorDTO>(_author);
                elasticClient.Index(_author, i => i.Index("authorsIndex"));
            }
            foreach (var _book in booksDTO)
            {
                //elasticClient.IndexDocument<BookDTO>(_book);
                elasticClient.Index(_book, i => i.Index("booksIndex"));
            }

        }
    }
}
