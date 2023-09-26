using System.Collections.Generic;
using UnityEngine;

public class BishopController : MonoBehaviour, IPieceController
{
    public static BishopController instance;
    List<Vector2> bishopMoveList = new List<Vector2>();
    GameObject cell=null;
    Position position=null;
    Piece pieceinfo=null;
    PieceColor enemyColor=PieceColor.White;
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

    public List<Vector2> ValidMoves(Vector2 currentPosition, PieceColor color, bool isChecked, bool movedBefore)
    {
        bishopMoveList.Clear();
        Direction((int)currentPosition.x, (int)currentPosition.y, 1, 1, color, bishopMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, -1, -1, color, bishopMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, 1, -1, color, bishopMoveList);
        Direction((int)currentPosition.x, (int)currentPosition.y, -1, 1, color, bishopMoveList);
        return bishopMoveList;
    }

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
    void Direction(int x, int y, int xchange, int ychange, PieceColor color,List<Vector2> list)
    {
        while(true){
            x+=xchange;
            y+=ychange;
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
                else{
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
    void KingInDirection(int x, int y, int xchange, int ychange, PieceColor color,List<Vector2> list)
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

    public void KingMoves(Vector2 currentPosition, PieceColor color,List<Vector2> list)
    {
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, 1, 1, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, -1, -1, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, 1, -1, color, list);
        KingInDirection((int)currentPosition.x, (int)currentPosition.y, -1, 1, color, list);
    }
}