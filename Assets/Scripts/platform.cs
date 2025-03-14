using UnityEngine;

public class platform : MonoBehaviour
{
    // the three gameobjects (hitbox, light, player detector) which are enabled
    [SerializeField] private GameObject a;
    [SerializeField] private GameObject b;
    [SerializeField] private GameObject c;
    [SerializeField] private GameObject player;
    private Transform trans;

    // the height which the player needs to be above
    [SerializeField] private float h;

    void Start()
    {
        trans = player.GetComponent<Transform>();
    }

    void Update()
    {
        // if player is above, set as active
        if(trans.position.y > h)
        {
            a.SetActive(true);
            b.SetActive(true);
            c.SetActive(true);
        }
    }
}
