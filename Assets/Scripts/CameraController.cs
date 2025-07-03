using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //Follow player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
        //Room camera
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);

        //Follow player
        //transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        //lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    public void MoveToNewRoom(Transform _newRoom, bool instant = false)
    {
        if (_newRoom != null)
        {
            currentPosX = _newRoom.position.x;

            if (instant)
            {
                transform.position = new Vector3(currentPosX, transform.position.y, transform.position.z);
            }
        }
    }

    public void SnapToRoom(Transform room)
    {
        if (room != null)
        {
            currentPosX = room.position.x;
            transform.position = new Vector3(currentPosX, transform.position.y, transform.position.z);
        }
    }
}