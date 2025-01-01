using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [Header("First shall be empty and last the Spaceship")]
    public Piece[] pieceList;
    
    private void Awake()
    {
        Solver.FindReachablePositions(this);
    }
}
