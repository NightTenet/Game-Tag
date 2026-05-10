using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Game_Tag_WPF.CoreClasses;

namespace Game_Tag_WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Functionality _functionality;
    private bool _isGameStarted;
    private int _movesCount;
    private int _elapsedSeconds;
    private Button[,] _buttons;
    private DispatcherTimer _timer;

    public MainWindow()
    {
        InitializeComponent();

        _functionality = new Functionality();

        _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += Timer_Tick;

        _buttons = new Button[4, 4]
        {
            {Btn00, Btn01, Btn02, Btn03},
            {Btn10, Btn11, Btn12, Btn13},
            {Btn20, Btn21, Btn22, Btn23},
            {Btn30, Btn31, Btn32, Btn33}
        };

        _isGameStarted = false;
        _movesCount = 0;
        _elapsedSeconds = 0;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _elapsedSeconds++;
        TimeSpan time = TimeSpan.FromSeconds(_elapsedSeconds);
        TimeCounter.Text = "Time: " + time.ToString(@"mm\:ss");
    }
    
    private void RenewBoard()
    {
        int[,] arr = _functionality.GetCurrentBoardState();

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                if (arr[i, j] == 0)
                {
                    _buttons[i, j].Content = 0;
                    _buttons[i, j].Visibility = Visibility.Hidden;
                }
                else
                {
                    _buttons[i, j].Content = arr[i, j].ToString();
                    _buttons[i, j].Visibility = Visibility.Visible;
                }

                if (SolveGameButton.IsEnabled == false && _isGameStarted == true || _functionality.CheckWin())
                    _buttons[i, j].IsEnabled = false;
                else
                    _buttons[i, j].IsEnabled = true;
            }
        }

        MovesCount.Text = "Moves: " + _movesCount.ToString();
    }

    private void Tile_Click(object sender, RoutedEventArgs e)
    {
        Button clickedBtn = (Button)sender;
        int clickedBtnX = Grid.GetRow(clickedBtn);
        int clickedBtnY = Grid.GetColumn(clickedBtn);

        if (_functionality.Move(clickedBtnX, clickedBtnY))
        {
            Message.Text = "";
            _movesCount++;
            if (_functionality.CheckWin())
            {
                Message.Text = "Congratulations!\nYou solved the game!";
                _timer.Stop();
                _isGameStarted = false;
                StartGameButton.IsEnabled = true;
                SolveGameButton.IsEnabled = false;
            }
        }
        else
            Message.Text = "This tile can't be moved!";
        RenewBoard();
    }

    private void StartGame_ButtonClick(object sender, RoutedEventArgs e)
    {        
        StartGameButton.IsEnabled = false;
        SolveGameButton.IsEnabled = true;

        _elapsedSeconds = 0;
        TimeCounter.Text = "Time: 00:00";

        _timer.Start();
        _isGameStarted = true;

        _movesCount = 0;
        Message.Text = "";
        _functionality.Shuffle();
        RenewBoard();
    }

    private async void SolveGame_ButtonClick(object sender, RoutedEventArgs e)
    {
        SolveGameButton.IsEnabled = false;
        Message.Text = "Finding the path...";
        RenewBoard();
        
        List<(int x, int y)> solvePath = await Task.Run(() => _functionality.AutoSolve());

        Message.Text = "Solving...";
        foreach (var move in solvePath)
        {
            _functionality.Move(move.x, move.y);
            _movesCount++;
            await Task.Delay(300);
            RenewBoard();
        }

        Message.Text = "Game is solved!";
        _timer.Stop();
        _isGameStarted = false;

        StartGameButton.IsEnabled = true;
    }

    private void Rules_ButtonClick(object sender, RoutedEventArgs e)
    {
        string rules = "Rules of Tag\n\n" +
                        "1. Main goal: arrange the numbers in order from 1 to 15.\n" +
                        "2. Click on the tiles next to the empty space to move them.\n" +
                        "3. Try to do that as fast and with as few moves as possible!";
        MessageBox.Show(rules, "Rules");
    }
}