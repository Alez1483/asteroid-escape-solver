using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("First shall be empty and last the Spaceship")]
    public Piece[] pieceList;
    
    private void Awake()
    {
        Piece spacecraft = pieceList[8];

        var visits = Solver.FindReachablePositions(this);

        int count = 0;

        foreach (Visit visit in visits)
        {
            if (Solver.IsSolution(visit.board, pieceList))
            {
                var directions = Solver.VisitToPath(visit);
                //string arrows = "^>v<";
                //
                //foreach (var dir in directions)
                //{
                //    print(arrows[dir.Item1] + ": " + dir.Item2);
                //}
                print(directions.Count);
                
                //break;
            }
        }
        print(count);
    }
}
