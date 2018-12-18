using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BHJet_Mobile.Infra
{
    public class PropertyChangedClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void OnPropertyChangedByName(string name)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private bool loading;
        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        private bool offloading = true;
        public bool OffLoading
        {
            get { return offloading; }
            set
            {
                offloading = value;
                OnPropertyChanged();
            }
        }

        private DateTime dataAtual = DateTime.Now.ToUniversalTime();
        public DateTime DataAtual
        {
            get { return dataAtual; }
            set
            {
                dataAtual = value;
                OnPropertyChanged();
            }
        }

    }
}
