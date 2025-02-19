using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Level")]
    public string newGame;

    public TMP_Text textoRecord;

    private void Start()
    {
        Record(); 
    }
    public void StartNewGame() // Cambia de escena.
    {
        SceneManager.LoadScene(newGame);
    }

    public void Record()
    {
        textoRecord.text = PlayerPrefs.GetString("Record");
    }
    public void ResetRecord()
    {
        PlayerPrefs.DeleteKey("Record");
        textoRecord.text = 0.ToString("");
    }
    public void Exit()
    {
        Debug.Log("SALIR");
        Application.Quit();
    }
}
