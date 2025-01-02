using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        var spreadBtn = new Button(() => SpreadPieces((Board)target));
        spreadBtn.text = "Spread pieces around the board";

        root.Add(spreadBtn);

        return root;
    }

    private static readonly Vector2[] positions =
    {
        new Vector2(-1, 2),
        new Vector2(1, 2),
        new Vector2(2, 1),
        new Vector2(2, -1),
        new Vector2(1, -2),
        new Vector2(-1, -2),
        new Vector2(-2, -1),
        new Vector2(-2, 1)
    };

    [MenuItem("Tools/Piece editing/Spread pieces #e")]
    public static void SpreadPieces()
    {
        Board board = FindObjectOfType<Board>();
        if (board != null)
        {
            SpreadPieces(board);
        }
    }

    private static void SpreadPieces(Board board)
    {
        for (int i = 1; i < board.pieceList.Length; i++)
        {
            Undo.RecordObject(board.pieceList[i].transform, "Spread pieces");
            board.pieceList[i].transform.position = positions[i - 1];
        }
    }
}
