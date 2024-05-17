using System;
using System.Collections.Generic;

namespace Obscurum.TDT
{
    public sealed class Tracker<T> : Tracker
    {
        private T outcome;
        
        private event Action<T> _result;
        public event Action<T> result
        {
            add
            {
                lock (key)
                {
                    if (outcome != null) value.Invoke(outcome);
                    else _result += value;
                }
            }
            remove => _result -= value;
        }

        internal void Complete(T result)
        {
            Complete();
            
            lock (key)
            {    
                if (!complete) return;
                outcome = result; 
                _result?.Invoke(result);
            }
        }
    }
    
    public class Tracker
    {
        private readonly List<Exception> exceptions = new();
        private event Action<Exception> _trackers;
        private int done;
        internal readonly object key = new();
        internal bool complete;
        internal int amount = 1;
        
        public float percentage => 100f * done / amount;
        
        private event Action _success; 
        public event Action success
        {
            add
            {
                lock (key)
                {
                    if (complete) value.Invoke();
                    else _success += value;
                }
            }
            remove => _success -= value;
        }
        
        private event Action<List<Exception>> _exception;
        public event Action<List<Exception>> exception
        {
            add
            {
                lock (key)
                {
                    if (complete && exceptions.Count != 0) value.Invoke(exceptions);
                    else _exception += value;
                }
                
            }
            remove => _exception -= value;
        }
        
        public Tracker Combine(Tracker dependency)
        {
            lock (key)
            {
                var handle = new Tracker
                {
                    amount = amount + dependency.amount,
                    done = done + dependency.done
                };
                
                _trackers += handle.Complete;
                dependency._trackers += handle.Complete;
                            
                return handle;
            }
        }
        
        internal void Complete(Exception exception = null)
        {
            lock (key)
            {
                if (exception != null) exceptions.Add(exception);
            
                done = Math.Min(done + 1, amount);
                complete = done >= amount;
                     
                _trackers?.Invoke(exception);
            
                if (!complete) return;
                
                if (exceptions.Count != 0) _exception?.Invoke(exceptions);
                _success?.Invoke();
            }
        }
    }
}