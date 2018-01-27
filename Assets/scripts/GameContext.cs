using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using matnesis.TeaTime;

public class GameContext : MonoSingleton<GameContext> {

	public List<UnitController> playerUnits;

    public List<UnitController> selectedUnits;

    public List<UnitController> enemyUnits;

    public AudioClip BackgroundSound;

	void Start()
	{
		
		SoundManager.Get.PlayClip (BackgroundSound, true);
	
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
