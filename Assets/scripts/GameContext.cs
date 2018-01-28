using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using matnesis.TeaTime;
using UnityEngine.SceneManagement;

public class GameContext : MonoSingleton<GameContext> {

	public List<UnitController> allyUnits;

    public List<UnitController> selectedPlayerUnits;

    public List<UnitController> enemyUnits;

    public List<UnitController> selectedEnemyUnits;

    public AudioClip BackgroundSound;

    public SpriteRenderer cursorHand;

    public int allyMineralAmount;

    public int enemyMineralAmount;

    void Start()
	{
		
		SoundManager.Get.PlayClip (BackgroundSound, true);

        this.tt("GameOverRoutine").Add(5).Loop((handler)=> {

            if (GameContext.Get.allyUnits.Count <= 0) {


                SceneManager.LoadScene("GameOver");
                // todo


            }

        });
	
	}

	public void CameraShakeRoutine(bool isVisible){

		if (!isVisible)
			return;

		var currentCameraPosition = Camera.main.transform.position;

		// SoundManager.Get.PlayClip (ExplosionSound, false);

		this.tt ().Add (0.01f, ()=>{

			//
			// move a little bit down
			//

			Camera.main.transform.position = Camera.main.transform.position + new Vector3(0f, -0.2f, 0f);

		}).Add (0.01f, ()=>{

			Camera.main.transform.position = Camera.main.transform.position + new Vector3(0f, 0.4f, 0f);

		}).Add (0.01f, ()=>{

			Camera.main.transform.position = Camera.main.transform.position + new Vector3(0.2f, 0f, 0f);

		}).Add (0.01f, ()=>{

			Camera.main.transform.position = Camera.main.transform.position + new Vector3(-0.4f, 0f, 0f);

		}).Add (0.01f, ()=>{

			Camera.main.transform.localPosition = Vector3.zero;

		});

	}
}
