using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace tthk_app.Models
{
    public class ChangeGrouping<K, T> : ObservableCollection<T>
    {
        public K Name { get; private set; }

        public ChangeGrouping(K name, IEnumerable<T> items)
        {
            Name = name;
            foreach (T item in items)
                Items.Add(item);
        }
    }
}