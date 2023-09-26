using UnityEngine;
public class Position : MonoBehaviour
{
    [SerializeField] Vector2 position;
    public void SetPosition(int x, int y)
    {
        position = new Vector2(x, y);
    }
    public Vector2 GetPosition()
    {
        return position;
    }
}