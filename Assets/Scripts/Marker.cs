using UnityEngine;

public class MapMarker : MonoBehaviour
{
    public string direction; // "North", "South", "East", "West"
    public Transform newLocation; //where to place the player in the new cell

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //move the player to desired cell place
            GameManager.Instance.UpdateCell(direction);
        }
    }
}
