using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using matnesis.TeaTime;

public class GameContext : MonoSingleton<GameContext> {

	public GameObject player;

	public GameObject playerPrefab;

	public GameObject DieMenuPanel;

	public AudioClip ExplosionSound;

	public AudioClip BackgroundSound;

	public AudioClip ReactorSound;

	public int EnergyBlocksDestroyed = 0;

	public Transform startingPosition;

	void Start()
	{
		
		SoundManager.Get.PlayClip (BackgroundSound, true);
	
	}

	public void CameraShakeRoutine(bool isVisible){

		if (!isVisible)
			return;

		var currentCameraPosition = Camera.main.transform.position;

		SoundManager.Get.PlayClip (ExplosionSound, false);

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

	public void resetLevel(){
	
		Destroy (Camera.main.gameObject);

		GameObject player = Instantiate(playerPrefab, GameContext.Get.startingPosition.position, Quaternion.identity);

		//SceneManager.LoadScene(scene.name);
		GameContext.Get.player = player;
	
	}
}
