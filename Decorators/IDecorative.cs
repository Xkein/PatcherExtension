using PatcherYRpp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extension.Decorators
{
    public interface IDecorative
    {
        public TDecorator CreateDecorator<TDecorator>(DecoratorId id, string description, params object[] parameters) where TDecorator : Decorator;
        public Decorator Get(DecoratorId id);
        public void Remove(DecoratorId id);
        public void Remove(Decorator decorator);
    }

    public interface IDecorative<TDecorator> : IDecorative where TDecorator : Decorator
    {
        public IEnumerable<TDecorator> GetDecorators();
    }

    public static class DecoratorHelpers
    {
        public static TDecorator Get<TDecorator>(this IDecorative decorative, DecoratorId id) where TDecorator : Decorator
        {
            return (TDecorator)decorative.Get(id);
        }

        public static object GetValue(this IDecorative<PairDecorator> decorative, DecoratorId id)
        {
            return decorative.Get<PairDecorator>(id).Value;
        }
        public static void SetValue(this IDecorative<PairDecorator> decorative, DecoratorId id, object value)
        {
            decorative.Get<PairDecorator>(id).Value = value;
        }
        public static TValue GetValue<TValue>(this IDecorative<PairDecorator> decorative, DecoratorId id)
        {
            return (TValue)decorative.Get<PairDecorator>(id).Value;
        }
        public static TValue GetValue<TKey, TValue>(this IDecorative<PairDecorator> decorative, DecoratorId id)
        {
            return decorative.Get<PairDecorator<TKey, TValue>>(id).Value;
        }
        public static void SetValue<TKey, TValue>(this IDecorative<PairDecorator> decorative, DecoratorId id, TValue value)
        {
            decorative.Get<PairDecorator<TKey, TValue>>(id).Value = value;
        }

        public static PairDecorator GetPairDecorator(this IDecorative<PairDecorator> decorative, object key)
        {
            foreach (var decorator in decorative.GetDecorators())
            {
                if (key.Equals(decorator.Key))
                {
                    return decorator;
                }
            }

            return null;
        }
        public static PairDecorator<TKey, TValue> GetPairDecorator<TKey, TValue>(this IDecorative<PairDecorator> decorative, TKey key)
        {
            foreach (var decorator in decorative.GetDecorators())
            {
                if (key.Equals(decorator.Key))
                {
                    return decorator as PairDecorator<TKey, TValue>;
                }
            }

            return null;
        }

        public static object GetValue(this IDecorative<PairDecorator> decorative, object key)
        {
            var decorator = decorative.GetPairDecorator(key);
            return decorator?.Value;
        }
        public static void SetValue(this IDecorative<PairDecorator> decorative, object key, object value)
        {
            var decorator = decorative.GetPairDecorator(key);
            if (decorator != null)
            {
                decorator.Value = value;
            }
        }

        public static TValue GetValue<TValue>(this IDecorative<PairDecorator> decorative, object key)
        {
            return (TValue)decorative.GetValue(key);
        }

        public static TValue GetValue<TKey, TValue>(this IDecorative<PairDecorator> decorative, TKey key)
        {
            var decorator = decorative.GetPairDecorator<TKey, TValue>(key);
            return decorator != null ? decorator.Value : default;
        }
        public static void SetValue<TKey, TValue>(this IDecorative<PairDecorator> decorative, TKey key, TValue value)
        {
            var decorator = decorative.GetPairDecorator<TKey, TValue>(key);
            if (decorator != null)
            {
                decorator.Value = value;
            }
        }


        public static TDecorator CreateDecorator<TDecorator>(this IDecorative decorative, string description, params object[] parameters) where TDecorator : Decorator
        {
            DecoratorId id = new DecoratorId(MathEx.Random.Next());
            return decorative.CreateDecorator<TDecorator>(id, description, parameters);
        }
    }
}
