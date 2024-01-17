using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public GameObject playButton;
    public GameObject exitButton;
    public GameObject backpackObj;
    public GameObject invObj;
    public KeyCode backpackKey = KeyCode.B;
    public KeyCode invKey = KeyCode.B;

    public FirstPersonLook look;

    void Start()
    {
        if (backpackObj == null)
        {
            Debug.LogError("Target object not assigned in ObjectToggle script!");
            look = GetComponent<FirstPersonLook>();
        }

        backpackObj.SetActive(false);
        invObj.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(backpackKey))
        {
            ToggleBackpackVisibility();
        }

        if (Input.GetKeyDown(invKey))
        {
            ToggleInvVisibility();
        }
    }
    void ToggleBackpackVisibility()
    {
        if (backpackObj.activeSelf)
        {
            backpackObj.SetActive(false);
            CursorLock();
        }
        else
        {
            backpackObj.SetActive(true);
            CursorUnlock();
        }
    }

    void ToggleInvVisibility()
    {
        if (invObj.activeSelf)
        {
            invObj.SetActive(false);
            CursorLock();
        }
        else
        {
            invObj.SetActive(true);
            CursorUnlock();
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void CursorLock()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        look.enabled = true;
    }

    public void CursorUnlock()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        look.enabled = false;
    }
}
