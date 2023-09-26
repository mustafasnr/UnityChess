using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour, IPieceController
{
    public static PawnController instance;
    List<Vector2> pawnMoveList = new List<Vector2>();
    GameObject cell=null;
    Position position=null;
    Piece pieceinfo=null;
    PieceColor enemyColor;
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
        pawnMoveList.Clear();
        if (color == PieceColor.White)
        {
            PawnMove((int)currentPosition.x, (int)currentPosition.y, -1, color, movedBefore);
        }
        else
        {
            PawnMove((int)currentPosition.x, (int)currentPosition.y, +1, color, movedBefore);
        }
        return pawnMoveList;
    }
    public void KingMoves(Vector2 currentPosition, PieceColor color,List<Vector2> list)
    {
        int direction= color==PieceColor.White? -1:1;
        int x=(int)currentPosition.x;
        int y=(int)currentPosition.y;
        if(GameManager.instance.IsPositionInBoard(x+direction,y+1)){
            pieceinfo=GameManager.instance.GetPieceInfo(x+direction,y+1);
            if(pieceinfo!=null){
                if((pieceinfo.color!=color) && pieceinfo.type==PieceType.King){
                    list.Add(new Vector2 (x+direction,y+1));
                }
            }
        }
        if(GameManager.instance.IsPositionInBoard(x+direction,y-1)){
            pieceinfo=GameManager.instance.GetPieceInfo(x+direction,y-1);
            if(pieceinfo!=null){
                if((pieceinfo.color!=color) && pieceinfo.type==PieceType.King){
                    list.Add(new Vector2 (x+direction,y-1));
                }
            }
        }
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void PawnMove(int x, int y, int direction, PieceColor color, bool pieceMovedBefore)
    {
        
        if (GameManager.instance.IsPositionInBoard(x + direction, y))//ileri
        {
            pieceinfo = GameManager.instance.GetPieceInfo(x + direction, y);
            if (pieceinfo == null)
            {
                cell=GameManager.instance.GetCellObject(x+direction,y);
                position=cell.GetComponent<Position>();
                if(GameManager.instance.TempMove(cell,position)){
                    pawnMoveList.Add(new Vector2(x + direction, y));
                }
            }
        }
        pieceinfo = GameManager.instance.GetPieceInfo(x + direction, y);
        if(!pieceMovedBefore && (pieceinfo==null)){
            pieceinfo=GameManager.instance.GetPieceInfo(x+2*direction,y);
            if(pieceinfo==null){
                cell=GameManager.instance.GetCellObject(x+2*direction,y);
                position=cell.GetComponent<Position>();
                if(GameManager.instance.TempMove(cell,position)){
                    pawnMoveList.Add(new Vector2(x+2*direction,y));
                }
            }
        }
        PawnAttack(x,y,direction,color);
    }
    void PawnAttack(int x,int y,int direction,PieceColor color){
        Piece pieceinfo;
        Position enpassantPosition=GameManager.instance.enPassantableCell==null?null:GameManager.instance.enPassantableCell.GetComponent<Position>();
        if(GameManager.instance.IsPositionInBoard(x+direction,y-1)){
            GameObject cell=GameManager.instance.GetCellObject(x+direction,y-1);
            Position cellPosition=cell.GetComponent<Position>();
            pieceinfo=GameManager.instance.GetPieceInfo(x+direction,y-1);
            if(pieceinfo!=null){
                if((pieceinfo.color!=color)){
                    if(GameManager.instance.TempMove(cell,cellPosition)){
                        pawnMoveList.Add(new Vector2 (x+direction,y-1));
                    }
                }
            }   
            else{
                if(enpassantPosition!=null){
                    if(enpassantPosition.GetPosition()==cellPosition.GetPosition()){
                        if(GameManager.instance.TempMove(cell,cellPosition)){
                            pawnMoveList.Add(new Vector2 (x+direction,y-1));
                        }
                    }
                }
            }         
        }
        if(GameManager.instance.IsPositionInBoard(x+direction,y+1)){
            GameObject cell=GameManager.instance.GetCellObject(x+direction,y+1);
            Position cellPosition=cell.GetComponent<Position>();
            pieceinfo=GameManager.instance.GetPieceInfo(x+direction,y+1);
            if(pieceinfo!=null){
                if((pieceinfo.color!=color)){
                    if(GameManager.instance.TempMove(cell,cellPosition)){
                        pawnMoveList.Add(new Vector2 (x+direction,y+1));
                    }
                }
            }   
            else{
                if(enpassantPosition!=null){
                    if(enpassantPosition.GetPosition()==cellPosition.GetPosition()){
                        if(GameManager.instance.TempMove(cell,cellPosition)){
                            pawnMoveList.Add(new Vector2 (x+direction,y+1));
                        }
                    }
                }
            }         
        }
    }
}