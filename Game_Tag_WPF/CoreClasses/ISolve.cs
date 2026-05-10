namespace Game_Tag_WPF.CoreClasses;

public interface ISolve
{
    List<(int, int)> AutoSolve(Board board);
}