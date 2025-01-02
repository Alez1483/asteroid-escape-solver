using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("First shall be empty and last the Spaceship")]
    public Piece[] pieceList;
    
    private void Awake()
    {
        double time = Time.realtimeSinceStartupAsDouble;

        Piece spacecraft = pieceList[8];

        var visits = Solver.FindReachablePositions(this);

        List<int> shortestPath = null;

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
        }

        string[] directionTexts = { "Down v", "Left <", "Up ^", "Right >" };

        print("Moves for the shortest path are as follows:");

        foreach (var dir in shortestPath)
        {
            print(directionTexts[dir]);
        }
        print("Done!");
        print("Search took " + time + " seconds in total. Shortest path consists of " + shortestPath.Count + " moves");
    }
}
