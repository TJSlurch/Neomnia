using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    private Transform player;

    // how much the background moves in relation to the player
    [SerializeField] private float parallaxFactor = 0.5f;

    private Vector3 lastPlayerPosition;

    void Start()
    {
        player = playerObject.GetComponent<Transform>();
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        // calculate the player's change in position since the last frame
        Vector3 playerMovement = player.position - lastPlayerPosition;

        // move in the opposite direction of player movement
        transform.position += playerMovement * parallaxFactor;

        // update the player position
        lastPlayerPosition = player.position;
    }
}

