using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour, IPieceController
{
    public static KingController instance;
    List<Vector2> kingMoveList = new List<Vector2>();
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
        bool isCheck= color==PieceColor.White?GameManager.instance.whiteChecked:GameManager.instance.blackChecked;
        kingMoveList.Clear();
        MoveOne((int)currentPosition.x, (int)currentPosition.y, -1, -1, color, kingMoveList);
        MoveOne((int)currentPosition.x, (int)currentPosition.y, -1, 0, color, kingMoveList);
        MoveOne((int)currentPosition.x, (int)currentPosition.y, -1, 1, color, kingMoveList);
        MoveOne((int)currentPosition.x, (int)currentPosition.y, 0, 1, color, kingMoveList);
        MoveOne((int)currentPosition.x, (int)currentPosition.y, 1, 1, color, kingMoveList);
        MoveOne((int)currentPosition.x, (int)currentPosition.y, 1, 0, color, kingMoveList);
        MoveOne((int)currentPosition.x, (int)currentPosition.y, 1, -1, color, kingMoveList);
        MoveOne((int)currentPosition.x, (int)currentPosition.y, 0, -1, color, kingMoveList);
        if ((!movedBefore) & (!isCheck))
        {
            CheckRooks(color,kingMoveList);
        }
        return kingMoveList;
    }
    public void KingMoves(Vector2 currentPosition, PieceColor color,List<Vector2> list)
    {
        KingOne((int)currentPosition.x, (int)currentPosition.y, -1, -1, color, list);
        KingOne((int)currentPosition.x, (int)currentPosition.y, -1, 0, color, list);
        KingOne((int)currentPosition.x, (int)currentPosition.y, -1, 1, color, list);
        KingOne((int)currentPosition.x, (int)currentPosition.y, 0, 1, color, list);
        KingOne((int)currentPosition.x, (int)currentPosition.y, 1, 1, color, list);
        KingOne((int)currentPosition.x, (int)currentPosition.y, 1, 0, color, list);
        KingOne((int)currentPosition.x, (int)currentPosition.y, 1, -1, color, list);
        KingOne((int)currentPosition.x, (int)currentPosition.y, 0, -1, color, list);
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
    void MoveOne(int x, int y, int xChange, int yChange, PieceColor color, List<Vector2> list)
    {
        x += xChange;
        y += yChange;
        if (GameManager.instance.IsPositionInBoard(x, y))
        {
            pieceinfo = GameManager.instance.GetPieceInfo(x, y);
            if(pieceinfo!=null){
                if((pieceinfo.color!=color)){
                    if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,new Vector2 (x,y))){
                        list.Add(new Vector2(x, y));
                    }
                }
            }
            else{
                if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,new Vector2 (x,y))){
                    list.Add(new Vector2(x, y));
                }
            }
        }
    }
    void KingOne(int x, int y, int xChange, int yChange, PieceColor color, List<Vector2> list)
    {
        x += xChange;
        y += yChange;
        if (GameManager.instance.IsPositionInBoard(x, y))
        {
            cell = GameManager.instance.GetCellObject(x, y);
            pieceinfo = GameManager.instance.GetPieceInfo(x, y);
            if(pieceinfo!=null){
                if((pieceinfo.color!=color) && pieceinfo.type==PieceType.King){
                    list.Add(new Vector2 (x,y));
                }
            }
        }
    }
    void CheckRooks(PieceColor color,List<Vector2> list)
    {
        if(color==PieceColor.Black){
            pieceinfo=GameManager.instance.GetPieceInfo(0,0);
            if(pieceinfo!=null){
                if(checkLong(0)){
                    if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,new Vector2 (0,0))){
                        list.Add(new Vector2 (0,0));
                    }
                }
            }
            pieceinfo=GameManager.instance.GetPieceInfo(0,7);
            if(pieceinfo!=null){
                if(checkShort(0)){
                    if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,new Vector2 (0,7))){
                        list.Add(new Vector2 (0,7));
                    }
                }
            }
        }
        else{
            pieceinfo=GameManager.instance.GetPieceInfo(7,0);
            if(pieceinfo!=null){
                if(checkLong(7)){
                    if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,new Vector2 (7,0))){
                        list.Add(new Vector2 (7,0));
                    }
                }
            }
            pieceinfo=GameManager.instance.GetPieceInfo(7,7);
            if(pieceinfo!=null){
                if(checkShort(7)){
                    if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,new Vector2 (7,7))){
                        list.Add(new Vector2 (7,7));
                    }
                }
            }
        }
    }
    bool checkLong(int xstart){
        int x=xstart;
        int y=4;
        while (true){
            y-=1;
            if(GameManager.instance.IsPositionInBoard(x,y)){
                pieceinfo=GameManager.instance.GetPieceInfo(x,y);
                if(pieceinfo!=null){
                    if(pieceinfo.type==PieceType.Rook && pieceinfo.movedOnce==false){
                        return true;
                    }
                    return false;
                }
            }
            else{
                return false;
            }
        }
    }
    bool checkShort(int xstart){
        int x=xstart;
        int y=4;
        while (true){
            y+=1;
            if(GameManager.instance.IsPositionInBoard(x,y)){
                pieceinfo=GameManager.instance.GetPieceInfo(x,y);
                if(pieceinfo!=null){
                    if(pieceinfo.type==PieceType.Rook && pieceinfo.movedOnce==false){
                        return true;
                    }
                    return false;
                }
            }
            else{
                return false;
            }
        }
    }
}
