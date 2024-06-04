using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionMenuScript : MonoBehaviour
{
    GameManager gameManager; 
    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null) {
            FindAnyObjectByType<GameManager>(); 
        }
    }

    // Save the Game 
    public void SaveGame() {
        if (gameManager != null) {
            gameManager.SavePlayerData(); 
            Debug.Log("Game saved successfully"); 
        } else {
            Debug.Log("Game manager set to null"); 
        }
    }

    // Back to previous scene 
    public void BackToMain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); 
    }

    public void QuitGame() {
        Debug.Log("Quit Game"); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
