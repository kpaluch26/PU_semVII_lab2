using System;
using Model.DTO;
using Nest;

namespace ASP_projekt.ElasticModels
{
	public class ElasticConnection : ConnectionSettings
	{
		public ElasticConnection(Uri uri = null) : base(uri)
        {
			this.DefaultMappingFor<BookDTO>(x => x.IndexName("index_books"));
			this.DefaultMappingFor<AuthorDTO>(x => x.IndexName("index_authors"));
		}
	}
}
