using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("First shall be empty and last the Spaceship")]
    public Piece[] pieceList;
    public float pieceSlidingTime = 0.5f;
    
    private void Awake()
    {
        double time = Time.realtimeSinceStartupAsDouble;

        Piece spacecraft = pieceList[8];

        var visits = Solver.FindReachablePositions(this);

        List<int> shortestPath = null; //directions of slides requires (0 = down, 1 = left, 2 = up, 3 = right)

        foreach (Visit visit in visits)
        {
            if (Solver.IsSolution(visit.board, pieceList))
            {
                var directions = Solver.VisitToPath(visit);

                if (shortestPath == null || directions.Count < shortestPath.Count)
                {
                    shortestPath = directions;
                }
            }
        }
        time = Time.realtimeSinceStartupAsDouble - time;

        if (shortestPath == null)
        {
            print("The given arrangement does not have solutions, make sure you put every piece correctly");
            return;
        }

        string[] directionTexts = { "Down v", "Left <", "Up ^", "Right >" };

        print("Moves for the shortest path are as follows:");

        foreach (var dir in shortestPath)
        {
            print(directionTexts[dir]);
        }
        print("Done!");
        print("Search took " + time + " seconds in total. Shortest path consists of " + shortestPath.Count + " moves");

        StartCoroutine(BoardAnimation(shortestPath));
    }

    public int[,] PiecepositionsToIndices(int[,] outIndices = null)
    {
        if (outIndices == null)
        {
            outIndices = new int[3, 3];
        }

        for (int i = 0; i < pieceList.Length; i++)
        {
            Piece piece = pieceList[i];

            if (piece == null)
            {
                continue;
            }

            Vector2Int index = Vector2Int.RoundToInt(piece.transform.position) + Vector2Int.one;
            outIndices[index.x, index.y] = i;
        }

        return outIndices;
    }

    private IEnumerator BoardAnimation(List<int> directions)
    {
        int[,] boardIndices = PiecepositionsToIndices();
        Vector2Int idxOfZero = Solver.FindIndexOfZero(boardIndices);

        float pieceAnimTime = 0f;

        for (int i = 0; i < directions.Count; i++)
        {
            int dir = directions[i];
            Vector2Int dirVec = Board.directions[dir];
            Vector2Int idxOfMovingObj = idxOfZero + dirVec;
            Piece pieceToMove = pieceList[boardIndices[idxOfMovingObj.x, idxOfMovingObj.y]];
            Transform pieceTrans = pieceToMove.transform;
            Vector2 startPos = pieceTrans.position;
            Vector2 targetPos = idxOfZero - Vector2Int.one;

            if (i == directions.Count - 1) //handle the last one separately
            {
                pieceTrans = pieceList[^1].transform;
                startPos = pieceTrans.position;
                targetPos = new Vector2(0f, -2f);
            }

            while ((pieceAnimTime += Time.deltaTime) < pieceSlidingTime)
            {
                float t = pieceAnimTime / pieceSlidingTime;

                pieceTrans.position = Vector2.Lerp(startPos, targetPos, Mathf.SmoothStep(0f, 1f, t));

                yield return null;
            }

            pieceAnimTime %= pieceSlidingTime;
            pieceTrans.position = targetPos;
            SwapIndices(idxOfZero, idxOfMovingObj, boardIndices);
            idxOfZero = idxOfMovingObj;
        }
    }

    private static readonly Vector2Int[] directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    private void SwapIndices(Vector2Int idx1, Vector2Int idx2, int[,] indices)
    {
        int temp = indices[idx1.x, idx1.y];
        indices[idx1.x, idx1.y] = indices[idx2.x, idx2.y];
        indices[idx2.x, idx2.y] = temp;
    }
}
