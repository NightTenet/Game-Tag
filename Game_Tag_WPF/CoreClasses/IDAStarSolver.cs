namespace Game_Tag_WPF.CoreClasses;

public class IDAStarSolver : ISolve
{
    private List<(int, int)> _solvePath;

    public IDAStarSolver()
    {
        _solvePath = new List<(int, int)>();
    }

    public List<(int, int)> AutoSolve(Board board)
    {
        _solvePath.Clear();

        int startLimit = GetManhattanDistance(board);
        int currentLimit = startLimit;

        while (true)
        {
            int result = SearchPath(board, 0, currentLimit, -1, -1, startLimit);

            if (result == -1)
            {
                _solvePath.Reverse();
                return _solvePath;
            }
            else
                currentLimit = result;
        }
    }

    private int GetManhattanDistance(Board board)
    {
        int totalDistance = 0;

        for (int i = 0; i < board.BoardSize; i++)
        {
            for (int j = 0; j < board.BoardSize; j++)
            {
                int currentNumber = board.GetTileNumberAtPosition(i, j);

                if (currentNumber == 0)
                    continue;
                
                int currentDistance = Math.Abs(i - ((currentNumber - 1) / board.BoardSize)) +
                    Math.Abs(j - ((currentNumber - 1) % board.BoardSize));

                totalDistance += currentDistance;
            }
        }

        return totalDistance;
    }

    private List<(int, int)> GetCurrentLegalMoves(Board board)
    {
        List<(int, int)> legalMoves = new List<(int, int)>();

        if (board.EmptyTileX - 1 >= 0)
            legalMoves.Add((board.EmptyTileX - 1, board.EmptyTileY));
        if (board.EmptyTileX + 1 < board.BoardSize)
            legalMoves.Add((board.EmptyTileX + 1, board.EmptyTileY));
        if (board.EmptyTileY - 1 >= 0)
            legalMoves.Add((board.EmptyTileX, board.EmptyTileY - 1));
        if (board.EmptyTileY + 1 < board.BoardSize)
            legalMoves.Add((board.EmptyTileX, board.EmptyTileY + 1));
        
        return legalMoves;
    }

    private int SearchPath(Board board, int makedMoves, int movesLimit, int previousX, int previousY, int currentDistance)
    {
        int F = makedMoves + currentDistance;

        if (F > movesLimit)
            return F;
        if (currentDistance == 0)
            return -1;

        int minLimit = int.MaxValue;
        List<(int x, int y)> legalMoves = GetCurrentLegalMoves(board);

        foreach (var currentMove in legalMoves)
        {
            if (currentMove.x == previousX && currentMove.y == previousY)
                continue;
            
            int emptyX = board.EmptyTileX;
            int emptyY = board.EmptyTileY;

            int movedTileNumber = board.GetTileNumberAtPosition(currentMove.x, currentMove.y);

            int targetX = (movedTileNumber - 1) / board.BoardSize;
            int targetY = (movedTileNumber - 1) % board.BoardSize;

            int oldDistance = Math.Abs(currentMove.x - targetX) + Math.Abs(currentMove.y - targetY);
            int newDistance = Math.Abs(emptyX - targetX) + Math.Abs(emptyY - targetY);
            int nextDistance = currentDistance - oldDistance + newDistance;

            board.SwapTiles(currentMove.x, currentMove.y, emptyX, emptyY);
            int nextMove = SearchPath(board, makedMoves + 1, movesLimit, emptyX, emptyY, nextDistance);
            board.SwapTiles(currentMove.x, currentMove.y, emptyX, emptyY);

            if (nextMove == -1)
            {
                _solvePath.Add((currentMove.x, currentMove.y));
                return -1;
            }
                
            else if (minLimit > nextMove)
                minLimit = nextMove;
        }

        return minLimit;
    }
}