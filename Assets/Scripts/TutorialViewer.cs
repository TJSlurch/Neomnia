using UnityEngine;

public class TutorialViewer : MonoBehaviour
{
    // the text gameobject
    [SerializeField] private GameObject tutorial;

    // if touched by player, make the tutorial text visible
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorial.SetActive(true);
        }
    }

    // when no longer touching the player, make the tutorial text invisible
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorial.SetActive(false);
        }
    }
}
