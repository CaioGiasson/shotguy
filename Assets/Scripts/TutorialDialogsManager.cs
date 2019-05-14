using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogsManager : MonoBehaviour {

	public static TutorialDialogsManager manager;

	public GameObject[] dialogTriggers;

	private void Awake() {
		manager = this;
	}
}
