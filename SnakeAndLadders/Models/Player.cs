namespace SnakeAndLadders
{
  /// <summary>
  ///    Represents a player with name and position.
  /// </summary>
  class Player
  {
    public Player()
    {
      Name = "Player";
    }
    public Player(string name)
    {
      Name = name;
    }
    public byte CurrentPosition { get; set; }
    public string Name { get; set; }
  }
}
