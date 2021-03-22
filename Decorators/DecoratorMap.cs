using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Decorators
{
    class DecoratorMap
    {
        public DecoratorMap()
        {
            dictionary = new Dictionary<DecoratorId, Decorator>();

            pairs = new EnumerableBuffer<PairDecorator>(this);
            events = new EnumerableBuffer<EventDecorator>(this);
        }

        public TDecorator CreateDecorator<TDecorator>(DecoratorId id, string description, params object[] parameters) where TDecorator : Decorator
        {
            var decorator = Activator.CreateInstance(typeof(TDecorator), parameters) as TDecorator;
            decorator.Description = description;
            decorator.ID = id;

            this.Add(decorator);

            return decorator;
        }

        public Decorator Get(DecoratorId id)
        {
            if (this.TryGet(id, out Decorator decorator))
            {
                return decorator;
            }
            return null;
        }

        public bool TryGet(DecoratorId id, out Decorator decorator)
        {
            return dictionary.TryGetValue(id, out decorator);
        }

        public void Add(Decorator decorator)
        {
            dictionary.Add(decorator.ID, decorator);
            NotifyChanged();
        }

        public void Remove(Decorator decorator)
        {
            dictionary.Remove(decorator.ID);
            NotifyChanged();
        }

        public void Remove(DecoratorId id)
        {
            Decorator decorator = this.Get(id);
            if (decorator != null)
            {
                this.Remove(decorator);
            }
        }

        private Action NotifyChanged;

        public IEnumerable<PairDecorator> GetPairDecorators() => pairs.Get();
        public IEnumerable<EventDecorator> GetEventDecorators() => events.Get();

        Dictionary<DecoratorId, Decorator> dictionary;

        // convenient to remove when enumerating
        class EnumerableBuffer<TDecorator> where TDecorator : Decorator
        {
            DecoratorMap map;
            List<TDecorator> list;
            public bool hasChanged = true;

            public EnumerableBuffer(DecoratorMap map)
            {
                this.map = map;
                map.NotifyChanged += () => hasChanged = true;
            }

            public IEnumerable<TDecorator> Get()
            {
                if (hasChanged)
                {
                    list = map.dictionary.Values.Where(d => d is TDecorator).Select(d => d as TDecorator).ToList();
                    hasChanged = false;
                }
                return list;
            }
        }

        EnumerableBuffer<PairDecorator> pairs;
        EnumerableBuffer<EventDecorator> events;
    }
}
