using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(Piece))]
public class PieceEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        var rotateBtn = new Button(() => RotatePiece((Piece)target, "Rotate piece"));
        rotateBtn.text = "Rotate 90 deg clockwise";
    
        root.Add(rotateBtn);
    
        return root;
    }

    [MenuItem("Tools/Piece editing/Rotate selected pieces #r")]
    public static void RotateSelectedPieces()
    {
        foreach (var obj in Selection.gameObjects)
        {
            if (obj.TryGetComponent(out Piece piece))
            {
                RotatePiece(piece, "Rotate selected");
            }
        }
    }

    private static void RotatePiece(Piece piece, string msg)
    {
        Undo.RecordObject(piece, msg);
        Undo.RecordObject(piece.transform, msg);
        for (int i = 0; i < piece.localBarriers.Length; i++)
        {
            Bounds2DInt bounds = piece.localBarriers[i];
            Vector2Int min = bounds.min;
            Vector2Int max = bounds.max;

            bounds.min = new Vector2Int(min.y, -max.x);
            bounds.max = new Vector2Int(max.y, -min.x);

            piece.localBarriers[i] = bounds;
        }

        piece.transform.Rotate(0f, 0f, -90f);
    }
}
