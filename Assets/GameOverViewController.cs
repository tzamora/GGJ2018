using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using matnesis.TeaTime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverViewController : MonoBehaviour {

    public Text gameOverText;

	// Use this for initialization
	void Start () {

        gameOverText.color = new Vector4(1f, 1f, 1f, 0f);

        this.tt("GameOverRoutine").Add(2).Loop(4f, (handler)=> {

            gameOverText.color = Color.Lerp(gameOverText.color, Color.white, handler.t);

        }).Loop(4f, (handler) => {

            gameOverText.color = Color.Lerp(gameOverText.color, new Vector4(1f, 1f, 1f, 0f), handler.t);

        }).Add(()=> {

            SceneManager.LoadScene("StartMenu");
        
        });

	}
}
