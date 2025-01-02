using UnityEngine;

public class Piece : MonoBehaviour
{
    //each piece area is 5x5 so that the middle one is (0, 0)
    //barriers may extend beyond the piece base area
    public Bounds2DInt[] localBarriers;

    private void OnDrawGizmosSelected()
    {
        if (localBarriers == null)
        {
            return;
        }

        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);

        foreach (var barrier in localBarriers)
        {
            Vector2 size = (Vector2)barrier.size / 5f;
            Vector2 center = barrier.center / 5f + (Vector2)transform.position;
            Gizmos.DrawCube(center, size);
        }
    }
}
