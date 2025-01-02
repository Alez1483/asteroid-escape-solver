using UnityEngine;

public class Visit
{
    public int board;
    public Vector2Int idxOfZero;
    public Visit visitCameFrom;
    public byte direction;

    public Visit(int board, Vector2Int idxOfZero, Visit visitCameFrom, byte direction)
    {
        this.board = board;
        this.idxOfZero = idxOfZero;
        this.visitCameFrom = visitCameFrom;
        this.direction = direction;
    }

    public override int GetHashCode()
    {
        return board.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        return ((Visit)obj).board == board;
    }
}