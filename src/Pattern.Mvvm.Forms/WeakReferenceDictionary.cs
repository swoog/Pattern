using System;
using System.Collections.Generic;
using System.Linq;

namespace Pattern.Mvvm.Forms
{
    public class WeakReferenceDictionary<TKey, TParameter> 
        where TKey : class
    {
        private class Key
        {
            public WeakReference KeyReference { get; set; }

            public override bool Equals(object obj)
            {
                if (!(obj is Key k))
                {
                    return false;
                }

                var val = k.KeyReference.Target;

                var val2 = this.KeyReference.Target;

                if (val == null || val2 == null)
                {
                    return false;
                }
                
                return val2.Equals(val);
            }

            public override int GetHashCode()
            {
                return 0;
            }
        }
        
        private readonly Dictionary<Key,  TParameter> values = new Dictionary<Key, TParameter>();
        
        public void Add(TKey key, TParameter parameter)
        {
            this.Clean();

            this.values.Add(new Key
            {
                KeyReference = new WeakReference(key)
            }, parameter);
        }

        private void Clean()
        {
            foreach (var valuesKey in this.values.Keys.ToArray())
            {
                if (!valuesKey.KeyReference.IsAlive)
                {
                    this.values.Remove(valuesKey);
                }
            }
        }

        public bool TryGetValue(TKey key, out TParameter o)
        {
            this.Clean();

            return this.values.TryGetValue(new Key
            {
                KeyReference = new WeakReference(key)
            }, out o);
        }

        public void Remove(object key)
        {
            this.values.Remove(new Key
            {
                KeyReference = new WeakReference(key)
            });
        }
    }
}