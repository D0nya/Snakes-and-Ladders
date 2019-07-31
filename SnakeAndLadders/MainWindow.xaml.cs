using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SnakeAndLadders
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    // Creating GameManager object
    private readonly GameManager gm = new GameManager();
    public MainWindow()
    {
      InitializeComponent();
      // Set DataContext as GameManager, to use it in MainWindow.xaml
      DataContext = gm;
    }

    // Button "Roll" click event
    private void Roll_Button_Click(object sender, RoutedEventArgs e)
    {
      gm.MakeTurn(Player1, Player2);
    }

    // Expand and show last record in history ListBox 
    // on mouse enter event
    private void LstBox_MouseEnter(object sender, MouseEventArgs e)
    {
      lstBox.Margin = new Thickness(10, -125, 10, 10);
      lstBox.ScrollIntoView(lstBox.Items[lstBox.Items.Count - 1]);
    }

    // Minimize ListBox on mouse leave event
    private void LstBox_MouseLeave(object sender, MouseEventArgs e)
    {
      lstBox.Margin = new Thickness(10, 10, 10, 10);
    }
  }
}
