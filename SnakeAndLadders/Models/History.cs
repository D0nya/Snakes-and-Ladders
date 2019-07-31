using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SnakeAndLadders
{
  /// <summary>
  ///    Represents an observable collection of string records.
  /// </summary>
  class History : INotifyPropertyChanged
  {
    public History()
    {
      Hist = new ObservableCollection<string>
      {
        DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString()
      };
    }
    public ObservableCollection<string> Hist { get; }
    public void Add(string _event)
    {
      Hist.Add( _event);
      RaisePropertyChanged("Hist");
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
