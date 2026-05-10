namespace Game_Tag_WPF.CoreClasses;

public class Board
{
    private const int _boardSize = 4;
    private readonly Tile[,] _tiles;
    private int _emptyTileX;
    private int _emptyTileY;

    public Board()
    {
        EmptyTileX = _boardSize - 1;
        EmptyTileY = _boardSize - 1;

        _tiles = new Tile[_boardSize, _boardSize];
        for (int i = 0; i < _boardSize; i++)
        {
            for (int j = 0; j < _boardSize; j++)
            {
                if (i == _boardSize - 1 && j == _boardSize - 1)
                    _tiles[i, j] = new Tile(0);
                else
                    _tiles[i, j] = new Tile(i * _boardSize + j + 1);
            }
        }
    }

    public int EmptyTileX
    {
        get => _emptyTileX;
        private set
        {
            _emptyTileX = value;
        }
    }

    public int EmptyTileY
    {
        get => _emptyTileY;
        private set
        {
            _emptyTileY = value;
        }
    }

    public int BoardSize
    {
        get => _boardSize;
    }

    public int GetTileNumberAtPosition(int x, int y)
    {
        return _tiles[x, y].Number;
    }

    public void SwapTiles(int x1, int y1, int x2, int y2)
    {
        (_tiles[x1, y1].Number, _tiles[x2, y2].Number) = (_tiles[x2, y2].Number, _tiles[x1, y1].Number);

        if (_tiles[x1, y1].IsEmpty())
        {
            EmptyTileX = x1;
            EmptyTileY = y1;
        }
        else if (_tiles[x2, y2].IsEmpty())
        {
            EmptyTileX = x2;
            EmptyTileY = y2;
        }
    }
}