using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] PieceType type;
    public PieceColor color;
    public GameObject pawnToUpgrade=null;
    Image image;
    public void SetImage()
    {
        if (type == PieceType.Rook)
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Upgrade();
    }
    public void Upgrade(){
        pawnToUpgrade.GetComponent<Piece>().type=type;
        pawnToUpgrade.GetComponent<Piece>().SetImage();
        pawnToUpgrade=null;
        GameManager.instance.GameCanvas.SetActive(true);
        GameManager.instance.ClearAll();
        GameManager.instance.UpgradeMenu.SetActive(false);
        GameManager.instance.EndMenu.SetActive(false);
    }
}
