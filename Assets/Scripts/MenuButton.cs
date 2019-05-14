using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

	public Sprite BtnDown, BtnUp;
	public GameObject text;

	[Range(1, 5)] public int action;

	SpriteRenderer sr;
	bool over;

	void Start () {
		sr = GetComponent<SpriteRenderer>();
		
	}
	
	void Update () {
		if (Input.GetMouseButton(0) && over) TouchDown();
		else Release();


	}

	private void TouchDown(){
		sr.sprite = BtnDown;
		text.transform.localPosition = new Vector3(0.0f, 0.0f);
		StartCoroutine("PerformMenuButtonAction");
	}

	private void Release(){
		sr.sprite = BtnUp;
		text.transform.localPosition = new Vector3(-0.1f, 0.1f);
	}

	private void OnMouseEnter() {
		over = true;
	}

	private void OnMouseExit() {
		over = false;
	}

	IEnumerator PerformMenuButtonAction(){
		yield return new WaitForSeconds(0.2f);
		switch(action){
			case 1:
				SceneManager.LoadScene("Tutorial");
				break;
			case 2:
				SceneManager.LoadScene("Tutorial");
				break;
			case 3:
				SceneManager.LoadScene("Credits");
				break;
			case 4:
				Application.Quit();
				break;
			case 5:
				SceneManager.LoadScene("MainMenu");
				break;
			default: break;
		}
	}



}
