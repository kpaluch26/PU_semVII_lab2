﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public record AddBookCommand(string Title, DateTime ReleaseDate, List<int> AuthorsIDs) : ICommand;
}
