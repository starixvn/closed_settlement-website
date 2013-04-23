using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Data.DataAbstract.Queue
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public class QueueHandler<T>
    {
        private readonly Queue<T> queue = new Queue<T>();

        public event ChangedEventHandler OnEnqueue;

        protected virtual void OnChanged(object sender, EventArgs e)
        {
            if (OnEnqueue != null)
            {
                OnEnqueue(sender, e);
            }
        }

        public virtual void Enqueue(T obj)
        {
            queue.Enqueue(obj);
            OnChanged(obj, EventArgs.Empty);
        }

        public int Count { get { return queue.Count; } }

        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            return item;  
        }

        public virtual IList<T> ToList()
        {
            if (queue.Count > 0) 
                return queue.ToList<T>();
            return new List<T>();
        }
    }
}
