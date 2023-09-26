using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    #region Sprites
    public Sprite whitePawn;
    public Sprite whiteRook;
    public Sprite whiteKnight;
    public Sprite whiteBishop;
    public Sprite whiteKing;
    public Sprite whiteQueen;
    public Sprite blackPawn;
    public Sprite blackRook;
    public Sprite blackKnight;
    public Sprite blackBishop;
    public Sprite blackKing;
    public Sprite blackQueen;
    #endregion
    public static Board instance;
    public Color lightColor;
    public Color darkColor;
    public GameObject cellPrefab;
    public GameObject piecePrefab;
    public Transform GameCanvas;

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
    private void Start()
    {
        StartGame();
        GameManager.instance.pieceHolder.transform.SetAsLastSibling();
        SetTableColor();
    }
    void StartGame()
    {
        GameObject tempCell;
        GameObject tempPiece;
        Position cellPosition;
        Piece pieceinfo;
        int start=0;
        int end=64;
        for (int i=start;i<end;i++){
            tempCell=Instantiate(cellPrefab,GameCanvas,false);
            cellPosition=tempCell.GetComponent<Position>();
            tempCell.name=i.ToString();
            tempCell.tag="BoardCell";
            cellPosition.SetPosition((int)(i/8),(int)(i%8));
            
            if((i>=0 && i<=7) || (i>=56 && i<=63)){
                tempPiece=Instantiate(piecePrefab,tempCell.transform,false);
                pieceinfo=tempPiece.GetComponent<Piece>();       
                int mod=i%8;
                switch (mod)
                {
                    case 0:
                        pieceinfo.type=PieceType.Rook;
                        break;
                    case 1:
                        pieceinfo.type=PieceType.Knight;
                        break;
                    case 2:
                        pieceinfo.type=PieceType.Bishop;
                        break;
                    case 3:
                        pieceinfo.type=PieceType.Queen;
                        break;
                    case 4:
                        pieceinfo.type=PieceType.King;
                        break;
                    case 5:
                        pieceinfo.type=PieceType.Bishop;
                        break;
                    case 6:
                        pieceinfo.type=PieceType.Knight;
                        break;
                    case 7:
                        pieceinfo.type=PieceType.Rook;
                        break;
                    default:
                        break;
                }
                if(i<=7){
                    pieceinfo.color=PieceColor.Black;
                    tempPiece.tag="BlackPiece";
                }
                else{
                    tempPiece.tag="WhitePiece";
                    pieceinfo.color=PieceColor.White;
                }
                pieceinfo.SetImage();
                pieceinfo.position=cellPosition.GetPosition();
            }
            if((i>=8 && i<=15) || (i>=48 && i<=55)){
                tempPiece=Instantiate(piecePrefab,tempCell.transform,false);
                pieceinfo=tempPiece.GetComponent<Piece>();
                pieceinfo.type=PieceType.Pawn;
                if(i<=15){
                    tempPiece.tag="BlackPiece";
                    pieceinfo.color=PieceColor.Black;
                }
                else{
                    tempPiece.tag="WhitePiece";
                    pieceinfo.color=PieceColor.White;
                }
                pieceinfo.SetImage();
                pieceinfo.position=cellPosition.GetPosition();
            }
        }
        
    }
    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetTableColor()
    {
        var cells=GameObject.FindGameObjectsWithTag("BoardCell");
        for(int i=0;i<cells.Length;i++){
            int pos=Int32.Parse(cells[i].name)/8 + Int32.Parse(cells[i].name)%8;
            SetColor(cells[i],pos);
        }
    }
    void SetColor(GameObject obj, int pos)
    {
        obj.GetComponent<Image>().color = (pos % 2 == 0) ? lightColor : darkColor;
    }
}