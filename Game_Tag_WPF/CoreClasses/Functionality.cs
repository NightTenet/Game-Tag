namespace Game_Tag_WPF.CoreClasses;

public class Functionality
{
    private Board _board;

    public Functionality()
    {
        _board = new Board();
    }

    private int InversionsCounter(int[] arr)
    {
        int result = 0 ;

        for (int i = 0; i < _board.BoardSize * _board.BoardSize - 1; i++)
        {
            for (int j = i + 1; j < _board.BoardSize * _board.BoardSize; j++)
            {
                if (arr[i] == 0 || arr[j] == 0)
                    continue;   
                if (arr[i] > arr[j])
                {
                    result++;
                }
            }
        }

        return result;
    }

    private void RandomTilesPositionChanger()
    {
        for (int i = 0; i < _board.BoardSize; i++)
        {
            for (int j = 0; j < _board.BoardSize; j++)
            {
                int randomX = Random.Shared.Next(0, _board.BoardSize);
                int randomY = Random.Shared.Next(0, _board.BoardSize);
                _board.SwapTiles(i, j, randomX, randomY);
            }
        }
    }

    private int[] GetOneDimArray()
    {
        int[] arr = new int[_board.BoardSize * _board.BoardSize];

        for (int i = 0; i < _board.BoardSize; i++)
        {
            for (int j = 0; j < _board.BoardSize; j++)
            {
                arr[i * _board.BoardSize + j] = _board.GetTileNumberAtPosition(i, j);
            }
        }

        return arr;
    }

    public void Shuffle()
    {
        RandomTilesPositionChanger();

        int[] oneDimArr = GetOneDimArray();
        int inverCount = InversionsCounter(oneDimArr);

        if ((inverCount % 2 == 0 && _board.EmptyTileX % 2 == 0)
            || (inverCount % 2 != 0 && _board.EmptyTileX % 2 != 0))
        {
            if (_board.GetTileNumberAtPosition(0,0) != 0 && _board.GetTileNumberAtPosition(0,1) != 0)
                _board.SwapTiles(0, 0, 0, 1);
            else
                _board.SwapTiles(0, 2, 0, 3);
        }
    }

    public bool CheckWin()
    {
        for (int i = 0; i < _board.BoardSize; i++)
        {
            for (int j = 0; j < _board.BoardSize; j++)
            {
                if (i == _board.BoardSize - 1 && j == _board.BoardSize - 1)
                    continue;
                if (_board.GetTileNumberAtPosition(i, j) != (i * _board.BoardSize) + j + 1)
                    return false;
            }
        }

        return true;
    }

    public bool Move(int x, int y)
    {
        if ((x == _board.EmptyTileX && Math.Abs(y - _board.EmptyTileY) == 1)
            || (y == _board.EmptyTileY && Math.Abs(x - _board.EmptyTileX) == 1))
        {
            _board.SwapTiles(x, y, _board.EmptyTileX, _board.EmptyTileY);
            return true;
        }
        return false;
    }

    public int[,] GetCurrentBoardState()
    {
        int[,] arr = new int[_board.BoardSize, _board.BoardSize];

        for (int i = 0; i < _board.BoardSize; i++)
        {
            for (int j = 0; j < _board.BoardSize; j++)
            {
                arr[i, j] = _board.GetTileNumberAtPosition(i, j);
            }
        }

        return arr;
    }

    public List<(int, int)> AutoSolve()
    {
        IDAStarSolver solver = new IDAStarSolver();
        return solver.AutoSolve(_board);
    }
}