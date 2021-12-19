using CQRS;
using CQRS.Authors;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CQRSController : ControllerBase
    {
        private readonly CommandBus commandBus;
        private readonly QueryBus queryBus;
        private readonly IElasticClient elasticClient;

        public CQRSController(CommandBus commandBus, QueryBus queryBus, IElasticClient elasticClient)
        {
            this.commandBus = commandBus;
            this.queryBus = queryBus;
            this.elasticClient = elasticClient;
        }

        [HttpGet("/CQRS/books")]
        public List<BookDTO> GetBooks([FromQuery] GetBooksQuery query)
        {
            return queryBus.Handle<GetBooksQuery, List<BookDTO>>(query);
        }

        [HttpPost("/CQRS/book/add")]
        public void Post([FromBody] AddBookCommand command)
        {
            commandBus.Handle(command);
        }

        [HttpDelete("/CQRS/book/delete/{id}")]
        public void Delete(int id)
        {
            commandBus.Handle(new DeleteBookCommand(id));
        }

        [HttpGet("/CQRS/book")]
        public BookDTO GetBook([FromQuery] GetBookQuery query)
        {
            return queryBus.Handle<GetBookQuery, BookDTO>(query);
        }

        [HttpPost("/CQRS/book/rate/add")]
        public void AddRateToBook(int index, int rate)
        {
            commandBus.Handle(new AddRateToBookCommand(index, rate));
        }

        [HttpGet("/CQRS/authors")]
        public List<AuthorDTO> GetAuthors([FromQuery] GetAuthorsQuery query)
        {
            return queryBus.Handle<GetAuthorsQuery, List<AuthorDTO>>(query);
        }

        [HttpPost("/CQRS/author/add")]
        public void AddAuthor([FromBody] AddAuthorCommand command)
        {
            commandBus.Handle(command);
        }

        [HttpDelete("/CQRS/author/delete/{id}")]
        public void DeleteAuthor(int id)
        {
            commandBus.Handle(new DeleteAuthorCommand(id));
        }

        [HttpPost("/CQRS/author/rate/add")]
        public void AddRateToAuthor(int index, int rate)
        {
            commandBus.Handle(new AddRateToAuthorCommand(index, rate));
        }

        [HttpGet("/CQRS/author")]
        public AuthorDTO GetAuthor([FromQuery] GetAuthorQuery query)
        {
            return queryBus.Handle<GetAuthorQuery, AuthorDTO>(query);
        }
    }
}
