using System;
using System.ComponentModel;
using System.Threading;

namespace SnakeAndLadders
{
  /// <summary>
  ///   Represents a hexaqonal dice. Provides method to
  ///   get random number from 1 to 6 and implements
  ///   INotifyPropertyChanged interface
  /// </summary>
  class Dice : INotifyPropertyChanged
  {
    private byte _value;
    public byte Value
    {
      get { return _value; }
      set
      {
        _value = value;
        RaisePropertyChanged("Value");
      }
    }
    public void Roll()
    {
      Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
      Thread.Sleep(20);
      Value = (byte)r.Next(1, 7);
    }
    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public static byte operator +(Dice d1, Dice d2)
    {
      return (byte)(d1.Value + d2.Value);
    }
    public override int GetHashCode()
    {
      return Value.GetHashCode();
    }
    public override bool Equals(object obj)
    {
      if (obj is Dice other)
      {
        if (Value == other.Value)
          return true;
        else
          return false;
      }
      else
        return false;
    }
    public static bool operator ==(Dice d1, Dice d2)
    {
      return d1.Equals(d2);
    }
    public static bool operator !=(Dice d1, Dice d2)
    {
      return !d1.Equals(d2);
    }
  }
}
