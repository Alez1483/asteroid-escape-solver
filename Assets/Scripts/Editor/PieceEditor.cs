using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(Piece))]
public class PieceEditor : Editor
{
    //public override VisualElement CreateInspectorGUI()
    //{
    //    var baseRoot = base.CreateInspectorGUI();
    //    var rotateBtn = new Button();
    //    rotateBtn.text = "Rotate 90 deg clockwise";
    //
    //    baseRoot.Add(rotateBtn);
    //
    //    return baseRoot;
    //}
    //
    //private void RotatePiece()
    //{
    //    Debug.Log("Rotate");
    //}
}
