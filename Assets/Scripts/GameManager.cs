using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int moveCount=0;
    public bool whiteChecked = false, whiteMatted = false;
    public bool blackChecked = false, blackMatted = false;
    bool pieceSelected = false;
    public static GameManager instance;
    List<Vector2> whiteActiveList=new List<Vector2> (),blackActiveList=new List<Vector2> ();
    public List<Vector2> mainList1=new List<Vector2> ();
    public GameObject selectedPiece;
    [SerializeField] Transform destroyHolder;
    public Transform pieceHolder;
    List<GameObject> blackPieces=new List<GameObject> (),whitePieces=new List<GameObject> ();
    public GameObject enPassantableCell=null;
    public int whiteValidMoveCount=0;
    public int blackValidMoveCount=0;
    public GameObject GameCanvas;
    public GameObject UpgradeMenu;
    public GameObject EndMenu;
    public TextMeshProUGUI endText;
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

    //DONE
    public void EndWin(int moveCount){
        GameCanvas.SetActive(false);
        UpgradeMenu.SetActive(false);
        EndMenu.SetActive(true);
        moveCount--;
        if(moveCount%2==0){
            endText.text="White Win!";
        }
        else{
            endText.text="Black Win!";
        }
    }

    //DONE
    public void EndDraw(string textToWrite){
        GameCanvas.SetActive(false);
        UpgradeMenu.SetActive(false);
        EndMenu.SetActive(true);
        endText.text=textToWrite;
    }
    //DONE
    public bool IsPositionEndOfTheBoard(int x)
    {
        if (x == 0 || x == 7)
        {
            return true;
        }
        return false;
    }

    //DONE
    void MovePiece(Vector2 targetPosition)
    {
        Piece pieceinfo=selectedPiece.GetComponent<Piece>();
        GameObject destroyedPiece;
        if(enPassantableCell!=null){
            if(enPassantableCell.GetComponent<Position>().GetPosition()!=targetPosition){
                enPassantableCell=null;
            }
        }
        if(pieceinfo.type==PieceType.Pawn){
            MovePawnPiece(targetPosition);
            pieceinfo.movedOnce=true;
            pieceinfo.position=targetPosition;
            if(pieceinfo.color==PieceColor.White){
                whiteValidMoveCount=moveCount+1;
            }
            else{
                blackValidMoveCount=moveCount+1;
            }
        }
        else if(pieceinfo.type==PieceType.King){
            MoveKingPiece(targetPosition);
        }
        else{
            GameObject targetCell;
            int x=(int) targetPosition.x;
            int y=(int) targetPosition.y;
            targetCell=GetCellObject(x,y);
            if(targetCell.transform.childCount!=0){
                destroyedPiece=GetPieceObject(x,y);
                destroyedPiece.SetActive(false);
                destroyedPiece.transform.SetParent(destroyHolder);
                Destroy(destroyedPiece.gameObject);
                if(pieceinfo.color==PieceColor.White){
                whiteValidMoveCount=moveCount+1;
                }
                else{
                    blackValidMoveCount=moveCount+1;
                }
            }
            selectedPiece.GetComponent<DragabbleItem>().parent=targetCell.transform;
            selectedPiece.transform.SetParent(selectedPiece.GetComponent<DragabbleItem>().parent);
            pieceinfo.movedOnce=true;
            pieceinfo.position=targetPosition;
        }
        moveCount++;
        if(pieceinfo.type==PieceType.Pawn && IsPositionEndOfTheBoard((int)pieceinfo.position.x)){
            UpgradePawn();
        }
        CheckControl();
    }

    //DONE
    public void TryMove(GameObject cell,Position cellPosition)//bu sürükleme durumunda işe yarıyor bunu daha sonra yapacağım
    {
        if(!pieceSelected){//taş seçili değilse
            ClearAll();
            if(cell.transform.childCount!=0){//taşa tıkladıysam
                int x=(int)cellPosition.GetPosition().x;
                int y=(int)cellPosition.GetPosition().y;
                GameObject piece=GetPieceObject(x,y);
                Piece pieceinfo=piece.GetComponent<Piece>();
                if((pieceinfo.color==PieceColor.White && moveCount%2==0) || (pieceinfo.color==PieceColor.Black && moveCount%2==1)){
                    PieceSelected(pieceinfo.color,pieceinfo.type,pieceinfo.position,pieceinfo.movedOnce);
                }
            }
            else{
                ClearAll();
            }
        }
        else{
            bool inList=false;
            inList=mainList1.Any(pos=>pos==cellPosition.GetPosition());
            if(inList){
                MovePiece(cellPosition.GetPosition());
                EndCheck(moveCount);
                if((moveCount-blackValidMoveCount)>100 || (moveCount-whiteValidMoveCount)>100) {
                    EndDraw("Draw");
                }
            }
            ClearAll();
        }
    }

    //DONE
    void UpgradePawn(){
        GameObject[] upgraders;
        GameCanvas.SetActive(false);
        UpgradeMenu.SetActive(true);
        upgraders=GameObject.FindGameObjectsWithTag("Upgrader");
        Piece pieceinfo=selectedPiece.GetComponent<Piece>();
        for(int i=0;i<upgraders.Length;i++){
            upgraders[i].GetComponent<UpgradeController>().pawnToUpgrade=selectedPiece;
            upgraders[i].GetComponent<UpgradeController>().color=pieceinfo.color;
            upgraders[i].GetComponent<UpgradeController>().SetImage();
        }
    }

    //DONE
    void EndCheck(int count){
        count--;
        bool ischeck= count%2==0? blackChecked: whiteChecked;
        bool escape=Escape(count);
        if(ischeck==true && escape==false){
            if(count%2==0){
                whiteMatted=true;
            }
            else{
                blackMatted=true;
            }
            EndWin(moveCount);
        }
        else if(ischeck==false && escape==false){
            EndDraw("Pat");
        }
    }

    //DONE
    bool Escape(int count){
        Piece pieceinfo;
        if(count%2==0){
            blackPieces.Clear();
            blackPieces=GameObject.FindGameObjectsWithTag("BlackPiece").ToList<GameObject>();
            for (int i=0;i<blackPieces.Count;i++)
            {
                pieceinfo=blackPieces[i].GetComponent<Piece>();
                PieceSelected(pieceinfo.color,pieceinfo.type,pieceinfo.position,pieceinfo.movedOnce);
                if((mainList1!=null) && (mainList1.Count!=0)){
                    ClearAll();
                    return true;
                }
                ClearAll();
            }
        }
        else{
            whitePieces.Clear();
            whitePieces=GameObject.FindGameObjectsWithTag("WhitePiece").ToList<GameObject>();
            for (int i=0;i<whitePieces.Count;i++)
            {
                pieceinfo=whitePieces[i].GetComponent<Piece>();
                mainList1=PieceSelected(pieceinfo.color,pieceinfo.type,pieceinfo.position,pieceinfo.movedOnce);
                if(mainList1.Count!=0){
                    ClearAll();
                    return true;
                }
                ClearAll();
            }
        }
        return false;
    }

    //DONE
    public bool TempMove(GameObject cell,Position cellPosition){
        PieceColor color;
        GameObject tempPiece=null;
        bool valid=true;
        bool isCheck;
        if(selectedPiece.GetComponent<Piece>().type==PieceType.Pawn){
            return PawnTempMove(cell,cellPosition);
        }
        else if(selectedPiece.GetComponent<Piece>().type==PieceType.King){
            return KingTempMove(cell,cellPosition);
        }
        else{
            int x=(int)cellPosition.GetPosition().x;
            int y=(int)cellPosition.GetPosition().y;

            if(cell.transform.childCount!=0){
                tempPiece=GetPieceObject(x,y);
                tempPiece.transform.SetParent(destroyHolder);
                tempPiece.SetActive(false);
            }
            selectedPiece.transform.SetParent(cell.transform);

            x=(int)selectedPiece.GetComponent<Piece>().position.x;
            y=(int)selectedPiece.GetComponent<Piece>().position.y;
            color=selectedPiece.GetComponent<Piece>().color;
            CheckControl();
            isCheck= color==PieceColor.White? whiteChecked:blackChecked;

            if(isCheck){
                valid=false;
            }
            selectedPiece.transform.SetParent(GetCellObject(x,y).transform);

            if(tempPiece!=null){
                tempPiece.transform.SetParent(cell.transform);
                tempPiece.SetActive(true);
            }
            CheckControl();
            return valid;
        }
    }

    //DONE
    public bool PawnTempMove(GameObject cell,Position cellPosition){
        bool isCheck;
        bool valid=true;
        int x,y;
        GameObject tempDestroyed=null;
        Piece pieceinfo=selectedPiece.GetComponent<Piece>();
        Position enPassantPosition= enPassantableCell==null? null: enPassantableCell.GetComponent<Position>();
        if(enPassantableCell!=null){
            x=(int)enPassantPosition.GetPosition().x;
            y=(int)enPassantPosition.GetPosition().y;
            if(cellPosition.GetPosition()==enPassantPosition.GetPosition()){
                if(pieceinfo.color==PieceColor.White){
                    
                    tempDestroyed=GetPieceObject(x+1,y);
                }
                else{
                    tempDestroyed=GetPieceObject(x-1,y);
                }
                tempDestroyed.transform.SetParent(destroyHolder);
                tempDestroyed.SetActive(false);
                selectedPiece.transform.SetParent(GetCellObject(x,y).transform);
                CheckControl();
                isCheck = pieceinfo.color==PieceColor.White? whiteChecked:blackChecked;
                if(isCheck){
                    valid=false;
                }
                selectedPiece.transform.SetParent(GetCellObject((int)pieceinfo.position.x,(int)pieceinfo.position.y).transform);
                tempDestroyed.SetActive(true);
                if(pieceinfo.color==PieceColor.White){
                    
                    tempDestroyed.transform.SetParent(GetCellObject(x+1,y).transform);
                }
                else{
                    tempDestroyed.transform.SetParent(GetCellObject(x-1,y).transform);
                }
                return valid;
            }
        }
        x=(int)pieceinfo.position.x;
        y=(int)pieceinfo.position.y;
        if(cell.transform.childCount!=0){
            tempDestroyed=cell.transform.GetChild(0).gameObject;
            tempDestroyed.transform.SetParent(destroyHolder);
            tempDestroyed.SetActive(false);
        }
        selectedPiece.transform.SetParent(cell.transform);
        CheckControl();
        isCheck= pieceinfo.color==PieceColor.White? whiteChecked:blackChecked;
        if(isCheck){
            valid=false;
        }
        selectedPiece.transform.SetParent(GetCellObject(x,y).transform);
        if(tempDestroyed!=null){
            tempDestroyed.SetActive(true);
            tempDestroyed.transform.SetParent(cell.transform);
        }
        
        return valid;
        
    }

    //DONE
    void MovePawnPiece(Vector2 targetPosition){
        int x=(int)targetPosition.x;
        int y=(int)targetPosition.y;
        Piece pieceinfo=selectedPiece.GetComponent<Piece>();
        GameObject cell=GetCellObject(x,y);
        Position cellPosition=cell.GetComponent<Position>();
        GameObject destroyedPiece=null;
        if(enPassantableCell!=null){
            Position enPassantPosition=enPassantableCell.GetComponent<Position>();
            if(enPassantPosition.GetPosition()==targetPosition){
                if(pieceinfo.color==PieceColor.White){
                    destroyedPiece=GetPieceObject(x+1,y);
                    destroyedPiece.SetActive(false);
                    destroyedPiece.transform.SetParent(destroyHolder);
                    Destroy(destroyedPiece);
                }
                else{
                    destroyedPiece=GetPieceObject(x-1,y);
                    destroyedPiece.SetActive(false);
                    destroyedPiece.transform.SetParent(destroyHolder);
                    Destroy(destroyedPiece);
                }
                selectedPiece.GetComponent<DragabbleItem>().parent=cell.transform;
                selectedPiece.transform.SetParent(cell.transform);
                enPassantableCell=null;
            }
        }
        else{
            if(cell.transform.childCount!=0){
                destroyedPiece=cell.transform.GetChild(0).gameObject;
                destroyedPiece.SetActive(false);
                destroyedPiece.transform.SetParent(destroyHolder);
                Destroy(destroyedPiece);
            }
            selectedPiece.GetComponent<DragabbleItem>().parent=cell.transform;
            selectedPiece.transform.SetParent(cell.transform);
            if((int)(pieceinfo.position.x-x)==2 || (int)(pieceinfo.position.x-x)==-2){
                if(pieceinfo.color==PieceColor.White){
                    enPassantableCell=GetCellObject(x+1,y);
                }
                else{
                    enPassantableCell=GetCellObject(x-1,y);
                }
            }
        }
    }

    //DONE
    void MoveKingPiece(Vector2 targetPosition){
        Piece pieceinfo=selectedPiece.GetComponent<Piece>();
        GameObject rook=null;
        GameObject destroyed=null;
        GameObject cell=null;
        int x=(int)targetPosition.x;
        int y=(int)targetPosition.y;
        if(!pieceinfo.movedOnce && (y==0 || y==7)){
            Piece rookinfo;
            if(y==0){
                selectedPiece.GetComponent<DragabbleItem>().parent=GetCellObject(x,y+2).transform;
                rook=GetPieceObject(x,y);
                rook.transform.SetParent(GetCellObject(x,y+3).transform);
                rookinfo=rook.GetComponent<Piece>();
                rookinfo.position=new Vector2 (x,y+3);
                rookinfo.movedOnce=true;
                pieceinfo.position=new Vector2 (x,y+2);
                pieceinfo.movedOnce=true;
            }
            else if(y==7){
                selectedPiece.GetComponent<DragabbleItem>().parent=GetCellObject(x,y-1).transform;
                rook=GetPieceObject(x,y);
                rook.transform.SetParent(GetCellObject(x,y-2).transform);
                rookinfo=rook.GetComponent<Piece>();
                rookinfo.position=new Vector2 (x,y-2);
                rookinfo.movedOnce=true;
                pieceinfo.position=new Vector2 (x,y-1);
                pieceinfo.movedOnce=true;
            }
            selectedPiece.transform.SetParent(selectedPiece.GetComponent<DragabbleItem>().parent);
        }
        else{
            cell=GetCellObject(x,y);
            if(cell.transform.childCount!=0){
                destroyed=GetPieceObject(x,y);
                destroyed.transform.SetParent(destroyHolder);
                destroyed.SetActive(false);
                Destroy(destroyed);
            }
            selectedPiece.GetComponent<DragabbleItem>().parent=cell.transform;
            pieceinfo.position=new Vector2 (x,y);
            pieceinfo.movedOnce=true;
        }
        selectedPiece.transform.SetParent(selectedPiece.GetComponent<DragabbleItem>().parent);
    }

    //DONE
    bool KingTempMove(GameObject cell, Position cellPosition){
        Piece pieceinfo=selectedPiece.GetComponent<Piece>();
        GameObject tempdestroyed=null;
        bool isCheck;
        bool valid=true;
        int x=(int) cellPosition.GetPosition().x;
        int y=(int) cellPosition.GetPosition().y;
        if((!pieceinfo.movedOnce) && (y==0 || y==7)){
            if(y==0){
                selectedPiece.transform.SetParent(GetCellObject(x,y+2).transform);
                CheckControl();
                isCheck= pieceinfo.color==PieceColor.White? whiteChecked:blackChecked;
                if(isCheck){
                    valid=false;
                }
                selectedPiece.transform.SetParent(GetCellObject((int)pieceinfo.position.x,(int)pieceinfo.position.y).transform);
                CheckControl();
                return valid;
            }
            else{
                selectedPiece.transform.SetParent(GetCellObject(x,y-1).transform);
                CheckControl();
                isCheck= pieceinfo.color==PieceColor.White? whiteChecked:blackChecked;
                if(isCheck){
                    valid=false;
                }
                selectedPiece.transform.SetParent(GetCellObject((int)pieceinfo.position.x,(int)pieceinfo.position.y).transform);
                CheckControl();
                return valid;
            }
        }
        else{
            if(cell.transform.childCount!=0){
                tempdestroyed=GetPieceObject(x,y);
                tempdestroyed.transform.SetParent(destroyHolder);
                tempdestroyed.SetActive(false);
            }

            selectedPiece.transform.SetParent(cell.transform);

            CheckControl();

            isCheck= pieceinfo.color==PieceColor.White?whiteChecked:blackChecked;

            if(isCheck){
                valid=false;
            }
            
            selectedPiece.transform.SetParent(GetCellObject((int)pieceinfo.position.x,(int)pieceinfo.position.y).transform);

            if(tempdestroyed!=null){
                tempdestroyed.SetActive(true);
                tempdestroyed.transform.SetParent(cell.transform);
            }
            
            CheckControl();
            return valid;
        }
    }

    //DONE
    void CheckControl(){
        void WhiteCheckControl(){
            PieceColor enemyColor=PieceColor.Black;
            PieceType type=PieceType.King;
            whiteActiveList.Clear();
            whitePieces.Clear();
            whitePieces= GameObject.FindGameObjectsWithTag("WhitePiece").ToList<GameObject>();
            foreach (var piece in whitePieces)
            {
                Piece pieceinfo=piece.GetComponent<Piece>();
                whiteActiveList.AddRange(GetCheckList(pieceinfo.color,pieceinfo.type,pieceinfo.position));
            }
            Piece posinfo;
            foreach(var pos in whiteActiveList){
                posinfo=GetPieceInfo((int)pos.x,(int)pos.y);
                if(posinfo!=null){
                    if(posinfo.color==enemyColor && posinfo.type==type){
                        blackChecked=true;
                        return ;
                    }
                }
            }
            blackChecked= false;
            return ;
        }
        void BlackCheckControl(){
            PieceColor enemyColor=PieceColor.White;
            PieceType type=PieceType.King;
            blackActiveList.Clear();
            blackPieces.Clear();
            blackPieces= GameObject.FindGameObjectsWithTag("BlackPiece").ToList<GameObject>();
            foreach (var piece in blackPieces)
            {
                Piece pieceinfo=piece.GetComponent<Piece>();
                blackActiveList.AddRange(GetCheckList(pieceinfo.color,pieceinfo.type,pieceinfo.position));
            }
            Piece posinfo;
            foreach(var pos in blackActiveList){
                posinfo=GetPieceInfo((int)pos.x,(int)pos.y);
                if(posinfo!=null){
                    if(posinfo.color==enemyColor && posinfo.type==type){
                        whiteChecked=true;
                        return ;
                    }
                }
            }
            whiteChecked= false;
            return ;
        }
        WhiteCheckControl();
        BlackCheckControl();
    }

    //DONE
    private List<Vector2> GetCheckList(PieceColor color, PieceType type, Vector2 currentPos)
    {
        List<Vector2> passiveList=new List<Vector2>();
        switch (type)
        {
            case (PieceType.Pawn):
                PawnController.instance.KingMoves(currentPos, color,passiveList);
                break;
            case (PieceType.Knight):
                KnightController.instance.KingMoves(currentPos, color,passiveList);
                break;
            case (PieceType.Bishop):
                BishopController.instance.KingMoves(currentPos, color,passiveList);
                break;
            case (PieceType.Rook):
                RookController.instance.KingMoves(currentPos, color,passiveList);
                break;
            case (PieceType.Queen):
                QueenController.instance.KingMoves(currentPos, color,passiveList);
                break;
            case (PieceType.King):
                KingController.instance.KingMoves(currentPos, color,passiveList);
                break;
        }
        return passiveList;
    }

    //DONE
    public List<Vector2> PieceSelected(PieceColor color, PieceType type, Vector2 currentPos, bool movedBefore)
    {
        mainList1.Clear();
        pieceSelected=true;
        selectedPiece=GetPieceObject((int)currentPos.x,(int)currentPos.y);
        switch (type)
        {
            case (PieceType.Pawn):
                mainList1 = PawnController.instance.ValidMoves(currentPos, color, false, movedBefore);
                break;
            case (PieceType.Knight):
                mainList1 = KnightController.instance.ValidMoves(currentPos, color, false, movedBefore);
                break;
            case (PieceType.Bishop):
                mainList1 = BishopController.instance.ValidMoves(currentPos, color, false, movedBefore);
                break;
            case (PieceType.Rook):
                mainList1 = RookController.instance.ValidMoves(currentPos, color, false, movedBefore);
                break;
            case (PieceType.Queen):
                mainList1 = QueenController.instance.ValidMoves(currentPos, color, false, movedBefore);
                break;
            case (PieceType.King):
                mainList1 = KingController.instance.ValidMoves(currentPos, color, false, movedBefore);
                break;
        }
        CheckControl();
        HighLightMoves(mainList1);
        return mainList1;
    }

    //DONE
    public bool IsPositionInBoard(int x, int y)
    {
        if (x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            return true;
        }
        return false;
    }

    //DONE
    public void ClearAll(){
        if(selectedPiece!=null){
            selectedPiece.GetComponent<Image>().raycastTarget=true;
        }
        selectedPiece=null;
        pieceSelected=false;
        mainList1.Clear();
        Board.instance.SetTableColor();
    }

    //DONE
    public GameObject GetCellObject(int x, int y)
    {
        GameObject objectToFind;
        int name = (x * 8) + y;
        objectToFind = GameObject.Find(name.ToString());
        return objectToFind;
    }

    //DONE
    public Piece GetPieceInfo(int x, int y)
    {
        GameObject cell= IsPositionInBoard(x,y)?GetCellObject(x,y):null;
        Piece pieceInfo;
        if (cell ==null || cell.transform.childCount == 0)
        {
            return null;
        }
        else
        {
            GameObject child;
            child = cell.transform.GetChild(0).gameObject;
            pieceInfo = child.GetComponent<Piece>();
            return pieceInfo;
        }
    }

    //DONE
    public GameObject GetPieceObject(int x, int y)
    {
        if (IsPositionInBoard(x, y))
        {
            if(GetCellObject(x,y).transform.childCount!=0){
                return GetCellObject(x, y).transform.GetChild(0).gameObject;
            }
            return null;
        }
        return null;
    }
    
    //DONE
    public void HighLightMoves(List<Vector2> moveList)
    {
        GameObject cell;
        foreach (var position in moveList)
        {
            cell = GetCellObject((int)position.x, (int)position.y);
            cell.GetComponent<Image>().color = Color.red;
        }
    }
}