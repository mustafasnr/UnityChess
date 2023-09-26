using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public PieceColor color;
    public PieceType type;
    public Vector2 position;
    public bool movedOnce=false;

    private void Start()
    {
        
    }
    public void SetImage()
    {
        if (type == PieceType.Pawn)
        {
            GetComponent<Image>().sprite = color == PieceColor.White ? Board.instance.whitePawn : Board.instance.blackPawn;
        }
        else if (type == PieceType.Rook)
        {
            GetComponent<Image>().sprite = color == PieceColor.White ? Board.instance.whiteRook : Board.instance.blackRook;
        }
        else if (type == PieceType.Bishop)
        {
            GetComponent<Image>().sprite = color == PieceColor.White ? Board.instance.whiteBishop : Board.instance.blackBishop;
        }
        else if (type == PieceType.Knight)
        {
            GetComponent<Image>().sprite = color == PieceColor.White ? Board.instance.whiteKnight : Board.instance.blackKnight;
        }
        else if (type == PieceType.Queen)
        {
            GetComponent<Image>().sprite = color == PieceColor.White ? Board.instance.whiteQueen : Board.instance.blackQueen;
        }
        else if (type == PieceType.King)
        {
            GetComponent<Image>().sprite = color == PieceColor.White ? Board.instance.whiteKing : Board.instance.blackKing;
        }
    }
}
public interface IPieceController
{
    List<Vector2> ValidMoves(Vector2 currentPosition, PieceColor color,bool isChecked,bool movedBefore);
    bool IsMoveValid(Vector2 currentPosition, Vector2 targetPosition);
    void KingMoves(Vector2 currentPosition, PieceColor color,List<Vector2> list);
}
public enum PieceColor
{
    White,
    Black
}
public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}