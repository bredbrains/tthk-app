using System.Collections.ObjectModel;

namespace tthk_app.Models
{
    public class ChangeGrouping<K, T> : ObservableCollection<T>
    {
        public K Name { get; set; }
    }
}