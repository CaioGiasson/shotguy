using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {

	public string[] textos;
	public int atual;

	public Image box, overlay;
	public Text dialogText, continueText;
	public bool paused;

	int test = 1;

	public void DoDialog(string[] textosMostrar){
		atual = 0;
		textos = textosMostrar;
		dialogText.text = textos[0];
		ShowDialog();
	}

	void ShowDialog(){
		Time.timeScale = 0;
		paused = true;
		box.gameObject.SetActive(true);
		overlay.gameObject.SetActive(true);
		dialogText.gameObject.SetActive(true);
		continueText.gameObject.SetActive(true);
	}

	void HideDialog(){
		Time.timeScale = 1;
		paused = false;
		box.gameObject.SetActive(false);
		overlay.gameObject.SetActive(false);
		dialogText.gameObject.SetActive(false);
		continueText.gameObject.SetActive(false);
	}

	void NextDialog(){
		atual++;
		test++;
		if ( atual < textos.Length ) 
			dialogText.text = textos[atual];
	}

	void Update() {

		if ( Input.GetMouseButtonDown(1) ){
			HideDialog();
		}

		if ( Input.GetMouseButtonDown(0) ){
			if ( atual + 1 == textos.Length ) HideDialog();
			else NextDialog();
		}
	}
}
