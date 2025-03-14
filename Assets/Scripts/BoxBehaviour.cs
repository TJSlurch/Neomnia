using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    // allows me to easily tick which color the box is, in the inspector
    [SerializeField] private bool red;
    [SerializeField] private bool orange;
    [SerializeField] private bool yellow;
    [SerializeField] private bool green;
    [SerializeField] private bool blue;
    [SerializeField] private bool pink;
    [SerializeField] private bool white;

    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject lighty;
    private PlayerStateManager player;

    // if box is red, these coordinates are used for where to respawn the player
    [SerializeField] private float redX;
    [SerializeField] private float redY;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {  
            // upon touching the player, enable the associated light
            lighty.SetActive(true);
            player = playerObject.GetComponent<PlayerStateManager>();

            if(red)
            {
                // change the player's respawn coordinates
                player.setRespawnX(redX);
                player.setRespawnY(redY);
                player.setRed();
            }
            else if(orange)
            {
                player.setOrange();
            }
            else if(yellow)
            {
                player.setYellow();
            }
            else if(green)
            {
                player.setGreen();
            }
            else if(blue)
            {
                player.setBlue();
            }
            else if(pink)
            {
                player.setPink();
            }
            else if(white)
            {
                player.setWhite();
            }
            else
            {
                Debug.Log("Error Colour");
            }
        }
    }
}
