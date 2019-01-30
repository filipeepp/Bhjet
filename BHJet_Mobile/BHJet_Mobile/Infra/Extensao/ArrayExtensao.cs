using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BHJet_Mobile.Infra.Extensao
{
    public static class ArrayExtensao
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> original)
        {
            return new ObservableCollection<T>(original);
        }
    }
}
