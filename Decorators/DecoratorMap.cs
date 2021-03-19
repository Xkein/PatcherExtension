using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Decorators
{
    class DecoratorMap
    {
        public TDecorator CreateDecorator<TDecorator>(DecoratorId id, string description, params object[] parameters) where TDecorator : Decorator
        {
            var decorator = Activator.CreateInstance(typeof(TDecorator), parameters) as TDecorator;
            decorator.Description = description;
            decorator.ID = id;

            map.Add(id, decorator);

            return decorator;
        }

        public Decorator Get(DecoratorId id)
        {
            if (map.TryGetValue(id, out Decorator decorator))
            {
                return decorator;
            }
            return null;
        }

        public void Remove(Decorator decorator)
        {
            map.Remove(decorator.ID);
        }

        public void Remove(DecoratorId id)
        {
            Decorator decorator = Get(id);
            if (decorator != null)
            {
                Remove(decorator);
            }
        }

        public IEnumerable<PairDecorator> GetPairDecorators()
        {
            return map.Values.Where(d => d is PairDecorator).Select(d => d as PairDecorator).ToArray();
        }
        public IEnumerable<EventDecorator> GetEventDecorators()
        {
            return map.Values.Where(d => d is EventDecorator).Select(d => d as EventDecorator).ToArray();
        }

        Dictionary<DecoratorId, Decorator> map = new();
        List<PairDecorator> pairs = new();
        List<EventDecorator> events = new();
    }
}
