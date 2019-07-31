using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace SnakeAndLadders
{
  /// <summary>
  ///    Represents a timer, that counts time
  ///    from the beginning of an event.
  /// </summary>
  class GameTimer : INotifyPropertyChanged
  {
    private readonly DispatcherTimer timer;
    private DateTime startTime;
    public GameTimer()
    {
      startTime = DateTime.Now;
      timer = new DispatcherTimer();
      timer.Tick += new EventHandler((object sender, System.EventArgs e) => 
      {

        Span = (DateTime.Now - startTime).ToString("h'h 'm'm 's's'");
        RaisePropertyChanged("Span");
      });
      timer.Interval = new TimeSpan(0, 0, 1);
      timer.Start();
    }
    public string Span { get; set; }
    public void Reset()
    {
      startTime = DateTime.Now;
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
