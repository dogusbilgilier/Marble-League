using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;

    Vector3 desiredPosition;

    [Range(0, 1)]
    public float takipX, takipY;
    public float offsetY;

    float x, y, z;

    void FixedUpdate()
    {
        desiredPosition = player.position;
        x = Mathf.Lerp(transform.position.x, desiredPosition.x, takipX);
        y = Mathf.Lerp(transform.position.y, desiredPosition.y+offsetY, takipY);
        z = transform.position.z;

        transform.position = new Vector3(x,y,z);
    }
}
