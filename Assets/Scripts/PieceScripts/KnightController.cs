using System.Collections.Generic;
using UnityEngine;

public class KnightController : MonoBehaviour, IPieceController
{
    public static KnightController instance;
    List<Vector2> knightMoveList = new List<Vector2>();
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
        //sağ y+ aşağı x+
        knightMoveList.Clear();
        KnightMove((int)currentPosition.x, (int)currentPosition.y, -1, -2, color,knightMoveList);
        KnightMove((int)currentPosition.x, (int)currentPosition.y, -2, -1, color,knightMoveList);
        KnightMove((int)currentPosition.x, (int)currentPosition.y, -2, 1, color,knightMoveList);
        KnightMove((int)currentPosition.x, (int)currentPosition.y, -1, 2, color,knightMoveList);
        KnightMove((int)currentPosition.x, (int)currentPosition.y, 2, 1, color,knightMoveList);
        KnightMove((int)currentPosition.x, (int)currentPosition.y, 1, 2, color,knightMoveList);
        KnightMove((int)currentPosition.x, (int)currentPosition.y, 2, -1, color,knightMoveList);
        KnightMove((int)currentPosition.x, (int)currentPosition.y, 1, -2, color,knightMoveList);
        return knightMoveList;
    }
    public void KingMoves(Vector2 currentPosition, PieceColor color,List<Vector2> list)
    {
        KingInKnightMove((int)currentPosition.x, (int)currentPosition.y, -1, -2, color,list);
        KingInKnightMove((int)currentPosition.x, (int)currentPosition.y, -2, -1, color,list);
        KingInKnightMove((int)currentPosition.x, (int)currentPosition.y, -2, 1, color,list);
        KingInKnightMove((int)currentPosition.x, (int)currentPosition.y, -1, 2, color,list);
        KingInKnightMove((int)currentPosition.x, (int)currentPosition.y, 2, 1, color,list);
        KingInKnightMove((int)currentPosition.x, (int)currentPosition.y, 1, 2, color,list);
        KingInKnightMove((int)currentPosition.x, (int)currentPosition.y, 2, -1, color,list);
        KingInKnightMove((int)currentPosition.x, (int)currentPosition.y, 1, -2, color,list);
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
    void KnightMove(int x, int y, int xchange, int ychange, PieceColor color,List<Vector2> list)
    {
        x += xchange;
        y += ychange;
        if (GameManager.instance.IsPositionInBoard(x, y))
        {
            pieceinfo=GameManager.instance.GetPieceInfo(x,y);
            if(pieceinfo!=null){
                if((pieceinfo.color!=color)){ 
                    if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,new Vector2 (x,y))){
                        list.Add(new Vector2 (x,y));
                    }
                }
            }
            else{
                if(IsMoveValid(GameManager.instance.selectedPiece.GetComponent<Piece>().position,new Vector2 (x,y))){
                    list.Add(new Vector2 (x,y));
                }
                
            }
        }
    }
    void KingInKnightMove(int x, int y, int xchange, int ychange, PieceColor color,List<Vector2> list)
    {
        x += xchange;
        y += ychange;
        enemyColor = color == PieceColor.White ? PieceColor.Black : PieceColor.White;
        if (GameManager.instance.IsPositionInBoard(x, y))
        {
            pieceinfo=GameManager.instance.GetPieceInfo(x,y);
            if(pieceinfo!=null){
                if((pieceinfo.color!=color) && pieceinfo.type==PieceType.King){
                    list.Add(new Vector2 (x,y));
                }
            }
        }
    }
}
