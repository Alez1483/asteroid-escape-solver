using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoardPacker
{
    //1, 10, 100, 1000,...
    private static int[] multipliers = new int[9];
    
    static BoardPacker()
    {
        int multiplier = 1;
        for (int i = 0; i < 9; i++)
        {
            multipliers[i] = multiplier;
            multiplier *= 10;
        }
    }

    public static int IndicesToInteger(int[,] indices)
    {
        int result = 0;
        int idx = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                result += indices[x, y] * multipliers[idx++];
            }
        }
        return result;
    }

    public static void IntegerToIndices(int[,] outIndices, int integer)
    {
        int idx = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                outIndices[x, y] = integer / multipliers[idx++] % 10;
            }
        }
    }
    public static int Vector2ToFlattenedIndex(Vector2Int idx)
    {
        return idx.x * 3 + idx.y;
    }

    //returns a board where empty is moved towards dir
    //will only work if the digit at idxOfZero is 0
    public static int MoveEmpty(int board, Vector2Int idxOfZero, Vector2Int dir)
    {
        int zeroIndex = Vector2ToFlattenedIndex(idxOfZero);
        int swapIndex = Vector2ToFlattenedIndex(idxOfZero + dir);

        int swapDigit = board / multipliers[swapIndex] % 10;
        board -= swapDigit * multipliers[swapIndex]; //remove swapDigit from swapIndex (leaves 0)
        board += swapDigit * multipliers[zeroIndex]; //add swapDigit to zeroIndex
        return board;
    }
}
