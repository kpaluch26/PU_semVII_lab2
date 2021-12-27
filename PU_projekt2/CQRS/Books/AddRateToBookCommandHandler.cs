using Microsoft.EntityFrameworkCore;
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
    public class AddRateToBookCommandHandler : ICommandHandler<AddRateToBookCommand>
    {
        private Database db { get; }
        private IElasticClient elasticClient { get; }

        public AddRateToBookCommandHandler(Database db, IElasticClient elasticClient)
        {
            this.db = db;
            this.elasticClient = elasticClient;

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

            //Elastic Search
            book = db.Books.Where(x => x.Id == command.index).Single();

            UpdateResponse<BookDTO> updateResponse = elasticClient.Update<BookDTO>(command.index, u => u.Doc(new BookDTO
            {
                AvarageRate = book.Rates.Average(a => a.Value),
                RatesCount = book.Rates.Count()
            }));
            //var _book = db.Books.
            //       Include(b => b.Rates).
            //       Include(b => b.Authors).
            //       ToList().Select
            //       (b => new BookDTO
            //       {
            //           ID = b.Id,
            //           ReleaseDate = b.ReleaseDate,
            //           AvarageRate = b.Rates.Count > 0 ? b.Rates.Average(r => r.Value) : 0,
            //           RatesCount = b.Rates.Count(),
            //           Title = b.Title,
            //           Authors = b.Authors.Select(a => new BookAuthorDTO
            //           {
            //               FirstName = a.FirstName,
            //               Id = a.Id,
            //               SecondName = a.SecondName
            //           }).ToList()
            //       }
            //       ).Where(b => b.ID == command.index).Single();
            ////elasticClient.IndexDocument<BookDTO>(_book);
            //elasticClient.Index(_book, i => i.Index("booksIndex"));
        }
    }
}
