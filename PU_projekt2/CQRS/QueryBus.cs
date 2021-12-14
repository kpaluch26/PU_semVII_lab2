using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public class QueryBus
    {
        private readonly IServiceProvider container;

        public QueryBus(IServiceProvider container)
        {
            this.container = container;
        }

        public D Handle<T, D>(T query) where T : IQuery
        {
            IQueryHandler<T, D> queryHandler = container.GetService(typeof(IQueryHandler<T, D>)) as IQueryHandler<T, D>;

            if(queryHandler != null)
            {
                return queryHandler.Handle(query);
            }
            else
            {
                throw new Exception("Metoda " + typeof(T).Name + " jest nieobsłużona.");
            }
        }
    }
}
