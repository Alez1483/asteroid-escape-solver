using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Solver
{
    private static readonly Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public static void FindReachablePositions(Board board)
    {
        Piece[] pieces = board.pieceList;
        int[,] boardIndices = new int[3, 3];

        for (int i = 0; i < pieces.Length; i++)
        {
            Piece piece = pieces[i];

            if (piece == null)
            {
                continue;
            }

            Vector2Int index = Vector2Int.RoundToInt(piece.transform.position) + Vector2Int.one;
            boardIndices[index.x, index.y] = i;
        }

        Vector2Int posOfZero = FindIndexOfZero(boardIndices);

        Queue<Visit> queue = new Queue<Visit>();
        HashSet<Visit> visits = new HashSet<Visit>(); //used to find the routes later
        HashSet<int> exploredBoards = new HashSet<int>();

        int startingBoard = BoardPacker.IndicesToInteger(boardIndices);
        Visit firstVisit = new Visit(startingBoard, posOfZero, -1);
        queue.Enqueue(firstVisit);
        visits.Add(firstVisit);
        exploredBoards.Add(startingBoard);

        while (queue.Count != 0)
        {
            Visit currentVisit = queue.Dequeue();
            BoardPacker.IntegerToIndices(boardIndices, currentVisit.board); //unpack the board

            foreach (Vector2Int dir in directions)
            {
                if (!IsValidDirection(currentVisit.idxOfZero, dir))
                {
                    continue; //direction out of bounds
                }

                int newBoard = BoardPacker.MoveEmpty(currentVisit.board, currentVisit.idxOfZero, dir);

                if (exploredBoards.Contains(newBoard))
                {
                    continue; //already explored
                }

                if (Collides(boardIndices, pieces, currentVisit.idxOfZero, dir))
                {
                    continue; //collides with something
                }

                exploredBoards.Add(newBoard);
                Visit newVisit = new Visit(newBoard, currentVisit.idxOfZero + dir, currentVisit.board);
                visits.Add(newVisit);
                queue.Enqueue(newVisit);
            }
        }

        Debug.Log(visits.Count);
    }

    private static Vector2Int FindIndexOfZero(int[,] indices)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (indices[x, y] == 0)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return -Vector2Int.one;
    }

    private static bool IsValidDirection(Vector2Int idxOfZero, Vector2Int direction)
    {
        if (direction.x != 0) //horizontal direction
        {
            if (direction.x == 1) 
            {
                return idxOfZero.x < 2; //right
            }
            return idxOfZero.x > 0; //left
        }
        else //vertical direction
        {
            if (direction.y == 1)
            {
                return idxOfZero.y < 2; //up
            }
            return idxOfZero.y > 0; //left
        }
    }

    private static bool Collides(int[,] boardIndices, Piece[] pieces, Vector2Int idxOfZero, Vector2Int direction)
    {
        // only pieces around the empty square can possibly collide
        int possibleRangeMinX = Mathf.Max(idxOfZero.x - 1, 0);
        int possibleRangeMaxX = Mathf.Min(idxOfZero.x + 1, 2);

        int possibleRangeMinY = Mathf.Max(idxOfZero.y - 1, 0);
        int possibleRangeMaxY = Mathf.Min(idxOfZero.y + 1, 2);

        Vector2Int swapIdx = idxOfZero + direction;
        Piece swipingPiece = pieces[boardIndices[swapIdx.x, swapIdx.y]];

        foreach (Bounds2DInt bounds in swipingPiece.localBarriers)
        {
            Bounds2DInt sweptBounds = LocalToGlobalBounds(bounds, swapIdx);
            sweptBounds.SwipeBounds(direction * -5);

            for (int x = possibleRangeMinX; x <= possibleRangeMaxX; x++)
            {
                for (int y = possibleRangeMinY; y <= possibleRangeMaxY; y++)
                {
                    Piece piece = pieces[boardIndices[x, y]];

                    if (piece == null || piece == swipingPiece)
                    {
                        continue; //skip the blank piece and the swiping piece (self collision)
                    }

                    foreach (Bounds2DInt b in piece.localBarriers)
                    {
                        Bounds2DInt worldBounds = LocalToGlobalBounds(b, new Vector2Int(x, y));

                        if (sweptBounds.Intersects(worldBounds))
                        {
                            return true; //intersection found
                        }
                    }
                }
            }
        }

        return false;
    }

    private static Bounds2DInt LocalToGlobalBounds(Bounds2DInt localBounds, Vector2Int pos)
    {
        return localBounds + pos * 5;
    }

    private struct Visit
    {
        public int board;
        public Vector2Int idxOfZero;
        public int boardCameFrom;

        public Visit(int board, Vector2Int idxOfZero, int boardCameFrom)
        {
            this.board = board;
            this.idxOfZero = idxOfZero;
            this.boardCameFrom = boardCameFrom;
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
}
