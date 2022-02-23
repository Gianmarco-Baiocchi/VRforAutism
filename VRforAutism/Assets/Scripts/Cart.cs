using UnityEngine;

public class Cart : MonoBehaviour
{
    [SerializeField] private Renderer itemArea;
    public Vector3 GetInsideCartPosition()
    {
        var xDim = itemArea.bounds.size.x;
        var zDim = itemArea.bounds.size.z;
        var p = itemArea.gameObject.transform.position;
        return new Vector3((p.x + Random.Range(-xDim / 4, xDim / 4)), p.y, p.z + Random.Range(-zDim / 4, zDim / 4));
    }


    public void AddInsideCart(Item item)
    {
        if (!item.IsInsideCart)
        {
            item.IsInsideCart = true;
        }
    }

    public void RemoveFromCart(Item item)
    {
        if (item.IsInsideCart)
        {
            item.IsInsideCart = false;
        }
    }
}
