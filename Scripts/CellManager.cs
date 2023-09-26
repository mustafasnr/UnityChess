using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CellManager : MonoBehaviour, IPointerClickHandler,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameManager.instance.TryMove(gameObject,GetComponent<Position>());
        eventData.pointerDrag.GetComponent<Image>().raycastTarget=true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CellClicked();
    }
    void CellClicked(){
        GameObject cell=gameObject;
        Position cellPosition=cell.GetComponent<Position>();
        GameManager.instance.TryMove(cell,cellPosition);
    }
}