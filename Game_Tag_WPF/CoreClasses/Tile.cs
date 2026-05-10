namespace Game_Tag_WPF.CoreClasses;

public class Tile
{
    private int _number;

    public Tile(int number)
    {
        Number = number;
    }

    public int Number
    {
        get => _number;
        set
        {
            _number = value;
        }
    }

    public bool IsEmpty()
    {
        return Number == 0;
    }
}