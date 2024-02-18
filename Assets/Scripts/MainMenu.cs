using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToSceneByName(string name) => SceneManager.LoadScene(name);
    public void ExitGame() => Application.Quit();
}
