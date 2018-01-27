using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChangeLevel : MonoBehaviour {

	public void LoadLevel(string LevelName) {

        SceneManager.LoadScene(LevelName);

	}

    public void QuitGame()
    {

        Application.Quit();

    }

}
