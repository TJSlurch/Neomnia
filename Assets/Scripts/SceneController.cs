using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // switch between the main menu, tutorial and level at any time by pressing 1, 2 or 3
    void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Title");
        }
        if(Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Tutorial");
        }
        if(Input.GetKey(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Level");
        }
    }
}
