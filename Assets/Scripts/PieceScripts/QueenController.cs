using System.Collections.Generic;
using UnityEngine;

public class QueenController : MonoBehaviour, IPieceController
{
    public static QueenController instance;
    List<Vector2> queenMoveList = new List<Vector2>();
    GameObject cell=null;
    Position position=null;
    Piece pieceinfo=null;
    PieceColor enemyColor=PieceColor.White;
    public bool IsMoveValid(Vector2 currentPosition, Vector2 targetPosition)
    {
        int x=(int)targetPosition.x;
        int y=(int)targetPosition.y;
        cell=GameManager.instance.GetCellObject(x,y);
        position=cell.GetComponent<Position>();
        if(GameManager.instance.TempMove(cell,position)){
            return true;
        }
        return false;
    }


    public List<Vector2> ValidMoves(Vector2 currentPosition, PieceColor color, bool isChecked, bool movedBefore)
    {
        queenMoveList.Clear();
        Direction((int)currentPosition.x, (int)currentPosition.y, 1, 1, color, queenMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, -1, -1, color, queenMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, 1, -1, color, queenMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, -1, 1, color, queenMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, 1, 0, color, queenMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, -1, 0, color, queenMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, 0, 1, color, queenMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, 0, -1, color, queenMoveList);
        return queenMoveList;
    }
    public void KingMoves(Vector2 currentPosition, PieceColor color,List<Vector2> list)
    {
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, 1, 1, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, -1, -1, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, 1, -1, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, -1, 1, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, 1, 0, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, -1, 0, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, 0, 1, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, 0, -1, color, list);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    void Direction(int x, int y, int xchange, int ychange, PieceColor color, List<Vector2> list)
    {
        while(true){
            x+=xchange;
            y+=ychange;
            pieceinfo=null;
            if(GameManager.instance.IsPositionInBoard(x,y)){
                pieceinfo=GameManager.instance.GetPieceInfo(x,y);
                cell=GameManager.instance.GetCellObject(x,y);
                position=cell.GetComponent<Position>();
                if(pieceinfo!=null){
                    if((pieceinfo.color!=color) && IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,position.GetPosition())){
                        list.Add(new Vector2 (x,y));
                        break;
                    }
                    break;
                }
                else if(pieceinfo==null){
                    if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,position.GetPosition())){
                        list.Add(new Vector2 (x,y));
                    }
                }
            }
            else{
                break;
            }
        }
    }
    void KingInDirection(int x, int y, int xchange, int ychange, PieceColor color, List<Vector2> list)
    {
        enemyColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        while (true)
        {
            x += xchange;
            y += ychange;
            if (GameManager.instance.IsPositionInBoard(x, y))
            {
                pieceinfo = GameManager.instance.GetPieceInfo(x, y);
                if(pieceinfo!=null){
                    if (pieceinfo.color == enemyColor && pieceinfo.type==PieceType.King)
                    {
                        list.Add(new Vector2(x, y));
                    }
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }
}