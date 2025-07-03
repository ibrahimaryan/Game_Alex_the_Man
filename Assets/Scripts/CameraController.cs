using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;
    // Follow player (tidak digunakan sekarang)
    //[SerializeField] private Transform player;
    //[SerializeField] private float aheadDistance;
    //[SerializeField] private float cameraSpeed;
    //private float lookAhead;

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            new Vector3(currentPosX, transform.position.y, transform.position.z),
            ref velocity,
            speed
        );
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }

    // Fungsi tambahan untuk langsung snap ke posisi kamar saat respawn
    public void SnapToRoom(Transform room)
    {
        if (room != null)
        {
            currentPosX = room.position.x;
            transform.position = new Vector3(currentPosX, transform.position.y, transform.position.z);
        }
    }
}
