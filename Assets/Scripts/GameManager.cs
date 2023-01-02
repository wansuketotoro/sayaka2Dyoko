using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public void GameOver()
    {
        RestartScene();
    }
    public void GameClear()
    {
        Debug.Log("ƒQ[ƒ€ƒNƒŠƒA  ");
    }
    void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
