using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiController : MonoBehaviour
{
    public GameObject player = null;
    private float startPosition;
    public TextMeshProUGUI uiDistance;
    public TextMeshProUGUI uiHighScore;

    public GameObject menuCamera;
    public GameObject playerCamera;

    public static bool isInMenu = true;

    public static float distance = 0;
    public static float highScore = 0;
    private float oldDistance = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("Body");
        ProceduralGeneration procGenScript = GameObject.Find("Level").GetComponent<ProceduralGeneration>();
        if (procGenScript == null)Debug.LogError("REEEEeee!!");
        procGenScript.player = menuCamera;
        // procGenScript.Player = playerCamera.parent;
        startPosition = this.player.transform.position.z;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UiController.LoadGame();
    }
    public void OnNewGameBtnClick()
    {
        ProceduralGeneration procGenScript = GameObject.Find("Level").GetComponent<ProceduralGeneration>();
        procGenScript.player = this.player;
        UiController.isInMenu = false;
        menuCamera.SetActive(false);
        playerCamera.SetActive(true);
    }
    public static void SaveGame()
    {
        if (UiController.distance > UiController.highScore)
        {
            PlayerPrefs.SetInt("HighestScore", (int)UiController.distance);
            PlayerPrefs.Save();
        }
        Debug.Log("Game data saved!");
    }
    static void LoadGame()
    {
        if (PlayerPrefs.HasKey("HighestScore"))
        {
            UiController.highScore = PlayerPrefs.GetInt("HighestScore");
            Debug.Log(UiController.highScore);
        }
        else
        {
            UiController.highScore = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        UiController.distance = this.player.transform.position.z - startPosition;
        if (oldDistance < distance)
        {
            uiDistance.text = "Distance : " + distance.ToString("0");
            oldDistance = distance;
            if (distance > highScore)
            {
                highScore = distance;
            }
        }
        uiHighScore.text = "HighScore : " + UiController.highScore.ToString("0");
    }
    public void FixedUpdate(){
        this.menuCamera.transform.position = this.menuCamera.transform.position + Vector3.forward * 0.05f;
    }

    //MENU
    public void exitGame()
    {
        Application.Quit();
    }
}
