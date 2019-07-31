using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SnakeAndLadders
{
  /// <summary>
  ///   Represents a View-Model object that connects
  ///   interface with models and implements methods
  ///   to manage the game.
  /// </summary>
  class GameManager : INotifyPropertyChanged
  {
    /// <summary>  
    ///    Represents a mover object (snake or ladder).
    ///    Moves player @from position @to position.
    /// </summary>
    private struct Mover
    {
      public byte from;
      public byte to;
      public Mover(byte f, byte t)
      {
        from = f;
        to = t;
      }
    }
    private uint turn;

    public GameManager()
    {
      Dice1 = new Dice();
      Dice2 = new Dice();
      History = new History();
      Timer = new GameTimer();
      TurnNumber = 1;

      Players = new Player[] { new Player("Player 1"), new Player("Player 2") };
      Pqueue = new Queue<Player>();
      Pqueue.Enqueue(Players[0]);
      Pqueue.Enqueue(Players[1]);

      Movers = new List<Mover>()
      {
        new Mover(2,38), new Mover(16,6), new Mover(7,14),
        new Mover(8,31), new Mover(15,26), new Mover(21,42),
        new Mover(28,84), new Mover(36,44), new Mover(46,25),
        new Mover(49,11), new Mover(51,67), new Mover(62,19),
        new Mover(64,60), new Mover(74, 53), new Mover(78,98),
        new Mover(87,94), new Mover(89, 68), new Mover(92,88),
        new Mover(95,75), new Mover(99, 80)
      };
    }

    public Dice Dice1 { get; }
    public Dice Dice2 { get; }
    public History History { get; }
    public uint TurnNumber
    {
      get { return turn; }
      set
      {
        turn = value;
        RaisePropertyChanged("TurnNumber");
      }
    }
    public GameTimer Timer { get; }
    public Player[] Players { get; }
    public Queue<Player> Pqueue { get; }
    private List<Mover> Movers { get; }

    // Contains main logic of the game
    public void MakeTurn(Ellipse player1, Ellipse player2)
    {
      Player currentPlayer = Pqueue.Peek();
      byte lastPos = currentPlayer.CurrentPosition;

      Dice1.Roll();
      Dice2.Roll();
      currentPlayer.CurrentPosition += (Dice1 + Dice2);

      // Bounce when player rolls more than 100
      if (currentPlayer.CurrentPosition > 100)
      {
        currentPlayer.CurrentPosition = (byte)(100 - (currentPlayer.CurrentPosition - 100));
        History.Add(string.Format("<{0}> Turn {1}\n{2} rolls {3}\n{2} bounces and flies back.\n{2}: {4} -> {5}",
                                   Timer.Span, TurnNumber, currentPlayer.Name, (Dice1 + Dice2),
                                   lastPos, currentPlayer.CurrentPosition));
      }
      else
        History.Add(string.Format("<{0}> Turn {1}\n{2} rolls {3}\n{2}: {4} -> {5}",
                                          Timer.Span, TurnNumber, currentPlayer.Name, (Dice1 + Dice2),
                                          lastPos, currentPlayer.CurrentPosition));

      // Check if player stands on a snake or a ladder
      foreach (Mover mover in Movers)
      {
        // If player stands - change players position
        if (currentPlayer.CurrentPosition == mover.from)
        {
          if (mover.from < mover.to)
            History.Add(string.Format("Ladder. Getting up!!!\n{0}: {1} -> {2}", currentPlayer.Name, mover.from, mover.to));
          else
            History.Add(string.Format("Oh no. It's a snake. Sliding down...\n{0}: {1} -> {2}", currentPlayer.Name, mover.from, mover.to));
          currentPlayer.CurrentPosition = mover.to;
          break;
        }
      }

      // Visual changes
      // Move players' chips
      if (currentPlayer == Players[0])
        SetChipPosition(player1);
      else
        SetChipPosition(player2);
      if (Players[0].CurrentPosition == Players[1].CurrentPosition)
      {
        Canvas.SetBottom(player1, Canvas.GetBottom(player1) + 15);
        Canvas.SetBottom(player2, Canvas.GetBottom(player2) - 15);
      }

      // If player hits 100 - GameOver
      if (currentPlayer.CurrentPosition == 100)
      {
        History.Add(string.Format("<{0}> Turn {1}\n{2} rolls {3}\n{2} gets 100! VICTORY!!!.\n{2}: {4} -> {5}",
                            Timer.Span, TurnNumber, currentPlayer.Name, (Dice1 + Dice2),
                            lastPos, currentPlayer.CurrentPosition));
        MessageBox.Show($"GAME OVER!\n{currentPlayer.Name} WINS!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);

        // Restart the game
        EndGame();
        SetChipPosition(player1);
        SetChipPosition(player2);
        Canvas.SetBottom(player1, Canvas.GetBottom(player1) + 15);
        Canvas.SetBottom(player2, Canvas.GetBottom(player2) - 15);
        return;
      }

      // If player rolls a double - takes another roll
      if (Dice1 == Dice2)
      {
        History.Add(string.Format("{0} rolls a DOUBLE!\n{0} takes another go!", currentPlayer.Name));
      }
      else
        Pqueue.Enqueue(Pqueue.Dequeue());

      // end of a turn
      TurnNumber++;
    }

    // Changes players' chips positions on canvas.
    private void SetChipPosition(Ellipse player)
    {
      int pos = Pqueue.Peek().CurrentPosition;
      int height;
      int[] positions = new int[] { 25, 90, 155, 220, 285, 350, 415, 480, 545, 610 };

      if (pos % 10 == 0)
        height = (pos / 10) - 1;
      else
        height = pos / 10;
      Canvas.SetBottom(player, 25 + 65 * height);

      if (height % 2 == 0)
      {
        if (pos % 10 == 0)
          Canvas.SetLeft(player, positions[9]);
        else if (pos % 10 == 1)
          Canvas.SetLeft(player, positions[0]);
        else
          Canvas.SetLeft(player, positions[pos % 10 - 1]);
      }
      else
      {
        if (pos % 10 == 0)
          Canvas.SetLeft(player, positions[0]);
        else if (pos % 10 == 1)
          Canvas.SetLeft(player, positions[9]);
        else
          Canvas.SetLeft(player, positions[positions.Length - (pos % 10)]);
      }
    }

    // Reset every object to default
    private void EndGame()
    {
      Dice1.Value = 0;
      Dice2.Value = 0;
      History.Hist.Clear();
      History.Add(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
      TurnNumber = 1;
      Timer.Reset();
      foreach (Player player in Players)
      {
        player.CurrentPosition = 1;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void RaisePropertyChanged(string property)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
  }
}
