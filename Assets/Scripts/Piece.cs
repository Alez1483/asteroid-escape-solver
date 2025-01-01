using UnityEngine;

public class Piece : MonoBehaviour
{
    //each piece area is 5x5 so that the middle one is (0, 0)
    //barriers may extend beyond the piece base area
    public Bounds2DInt[] localBarriers;

    private void OnDrawGizmos()
    {
        if (localBarriers == null)
        {
            return;
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, Vector2.one);
        Gizmos.color = Color.black;

        foreach (var barrier in localBarriers)
        {
            Vector2 size = (Vector2)barrier.size / 5f;
            Vector2 center = barrier.center / 5f + (Vector2)transform.position;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
