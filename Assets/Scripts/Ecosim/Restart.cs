using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public GameObject restartPanel;
    
    bool isRestarting = false;

    private void Start()
    {
        EntityGroup.AllPacks = new System.Collections.Generic.List<EntityPack>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isRestarting = false;

        restartPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isRestarting)
        {
            restartPanel.SetActive(true);
            isRestarting = true;

            SceneManager.LoadScene("Ecosim");
        }
    }
}
