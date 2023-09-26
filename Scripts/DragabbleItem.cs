using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragabbleItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Transform parent;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parent=transform.parent;
        GameObject cell=transform.parent.gameObject;
        Position cellPosition=cell.GetComponent<Position>();
        GameManager.instance.TryMove(cell,cellPosition);
        transform.SetParent(GameManager.instance.pieceHolder);
        GetComponent<Image>().raycastTarget=false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position=Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parent);
        GameManager.instance.ClearAll();
        GetComponent<Image>().raycastTarget=true;
        
    }
}
