using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars
{
  class SnakesLadders
  {
    private struct Mover
    {
      public int from;
      public int to;
      public Mover(int f, int t)
      {
        from = f;
        to = t;
      }
    }

    private readonly Queue<Player> PQueue;
    private readonly List<Mover> movers;
    private bool gameOver = false;

    public SnakesLadders()
    {
      PQueue = new Queue<Player>();
      PQueue.Enqueue(new Player("Player 1"));
      PQueue.Enqueue(new Player("Player 2"));

      movers = new List<Mover>()
          {
            new Mover(2,38), new Mover(16,6), new Mover(7,14),
            new Mover(8,31), new Mover(15,26), new Mover(21,42),
            new Mover(28,84), new Mover(36,44), new Mover(46,25),
            new Mover(49,11), new Mover(51,67), new Mover(62,19),
            new Mover(64,60), new Mover(74, 53), new Mover(78,98),
            new Mover(87,94), new Mover(89, 68), new Mover(92,88),
            new Mover(95,75), new Mover(99, 80), new Mover(71, 91)
          };

    }

    public string play(int die1, int die2)
    {
      if (gameOver)
        return "Game over!";
      int dieSum = die1 + die2;
      Player currentPlayer = PQueue.Peek();

      currentPlayer.Square += dieSum;
      if (currentPlayer.Square > 100)
        currentPlayer.Square = (100 - (currentPlayer.Square - 100));
      if (currentPlayer.Square == 100)
      {
        gameOver = true;
        return string.Format(currentPlayer.Name + " Wins!");
      }

      foreach (Mover mover in movers)
      {
        // If player stands on mover - change players position
        if (currentPlayer.Square == mover.from)
        {
          currentPlayer.Square = mover.to;
          break;
        }
      }

      if (die1 != die2)
        PQueue.Enqueue(PQueue.Dequeue());

      return string.Format("{0} is on square {1}", currentPlayer.Name, currentPlayer.Square);
    }
  }
  class Player
  {
    public string Name { get; set; }
    public int Square { get; set; }

    public Player(string name)
    {
      Name = name;
      Square = 0;
    }
  }
}