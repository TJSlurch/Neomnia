using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // the coordinates which it cycles between
    [SerializeField] private float startX;
    [SerializeField] private float startY;
    [SerializeField] private float endX;
    [SerializeField] private float endY;

    [SerializeField] private GameObject playerObject;
    private PlayerStateManager player;
    private Vector3 startPoint;
    private Vector3 endPoint;

    // how fast it moves
    [SerializeField] private float speed = 1.0f;

    // tracks how far the object is along
    private float pos = 0f;

    void Start()
    {
        player = playerObject.GetComponent<PlayerStateManager>();
        startPoint = new Vector3(startX, startY, 0);
        endPoint = new Vector3(endX, endY, 0);
    }

    void Update()
    {
        if (player.getCurrentColor() == "pink")
        {
            // smoothly increase pos
            pos += Time.deltaTime * speed;

            // if reached the end, swap the target position
            if (pos > 1f)
            {
                Vector3 temp = startPoint;
                startPoint = endPoint;
                endPoint = temp;
                pos = 0f;
            }

            // move the platform
            transform.position = Vector3.Lerp(startPoint, endPoint, pos);
        }
    }
}
