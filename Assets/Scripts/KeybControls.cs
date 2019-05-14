using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KeybControls : MonoBehaviour {

	public float velX = 100;
	public float maxVelX = 10;
	public float shotPower = 1000;
	public float rechargeTime = 0.7f;
	public bool hasGun = false;

	public GameObject[] enemies;

	public GameObject[] roupas;
	int nRoupas;
	bool got1 = false;
	bool got2 = false;
	bool got3 = false;
	bool got4 = false;

	public Text textBox;
	public Text roupasBox;

	public GameObject guySprite;
	public GameObject gunSprite;
	public GameObject stickSprite;
	public GameObject tiro;

	DialogManager dialog;

	bool[] diags = {false, false, false, false, false, false};

	Vector3 mouse_pos, object_pos;
	float gunAngle;
	Rigidbody rb;

	bool shotReady = true;
	bool enragedEnemies = false;

	void Start() {
		rb = GetComponent<Rigidbody>();
		dialog = FindObjectOfType<DialogManager>();
	}

	private void OnTriggerEnter(Collider other) {
		switch(other.gameObject.name){

			case "FimFase":
				SceneManager.LoadScene("Fase2");
				break;

			case "DialogoInicial":
				if ( hasGun ) {
					if ( got4 ) {
						if ( !diags[5] ){
							dialog.DoDialog(new string[] { "Robson diz: MANHÊ!!!! PEGUEI TODAS AS ROUPAS" });
							diags[5] = true;
							rb.velocity = Vector3.zero;
							rb.angularVelocity = Vector3.zero;
						}
					} else {
						if (!diags[0]) {
							dialog.DoDialog(new string[] { "Robson diz: Afe, perdi o ônibus da escola!", "Agora vou ter que atravessar as colinas!", "Ainda bem que agora minha arma ativa mais rápido" });
							diags[0] = true;
							rb.velocity = Vector3.zero;
							rb.angularVelocity = Vector3.zero;
						}
					}
				}
				else {
					if ( !diags[0] ){
						dialog.DoDialog(new string[] { "Mãe diz: ROBSON!", "Mãe diz: Você está de castigo!!!", "Mãe diz: Enquanto não recolher as roupas do chão do quarto, não pode sair!!!" } );
						rb.velocity = Vector3.zero;
						rb.angularVelocity = Vector3.zero;
						diags[0] = true;
					}
				}
				break;

			case "AchouArma":
				if ( !diags[1] ){
					dialog.DoDialog(new string[] { "Robson diz: Minha arma está ali na frente!!!" });
					diags[1] = true;
					rb.velocity = Vector3.zero;
					rb.angularVelocity = Vector3.zero;
				}
				break;

			case "PegouArma":
				if (!hasGun && !diags[2]) {
					hasGun = true;
					Destroy(stickSprite.gameObject);
					dialog.DoDialog(new string[] { "Robson diz: Peguei minha arma!!!" });
					rb.velocity = Vector3.zero;
					rb.angularVelocity = Vector3.zero;
					diags[2] = true;
				}
				break;

			case "AposUltimaRoupa":
				if ( got4 && !diags[4] ) {
					dialog.DoDialog(new string[] { "Robson diz: Ei! Os ursinhos se voltaram contra mim!!!" });
					rb.velocity = Vector3.zero;
					rb.angularVelocity = Vector3.zero;
					diags[4] = true;
				}
				break;

			case "Roupa1":
				Destroy(roupas[0].gameObject);
				nRoupas = 1;
				got1 = true;
				break;

			case "Roupa2":
				Destroy(roupas[1].gameObject);
				nRoupas = 2;
				got2 = true;
				break;

			case "Roupa3":
				Destroy(roupas[2].gameObject);
				nRoupas = 3;
				got3 = true;
				break;

			case "Roupa4":
				Destroy(roupas[3].gameObject);
				nRoupas = 4;
				got4 = true;

				if ( !diags[3] ){
					dialog.DoDialog(new string[] { "Robson diz: Acho que peguei todas as roupas!", "Robson diz: Hora de voltar!!!" });
					rb.velocity = Vector3.zero;
					rb.angularVelocity = Vector3.zero;

					diags[3] = true;
					if (!enragedEnemies) {
						enragedEnemies = true;
						foreach (GameObject en in enemies) {
							en.GetComponent<firstBear>().Enrage();
						}
					}
				}
				
				break;



			//case "AchouArma":
			//	dialog.DoDialog(new string[] { "Robson diz: Minha arma está ali na frente!!!" });
			//	break;

			//case "AchouArma":
			//dialog.DoDialog(new string[] { "Robson diz: Minha arma está ali na frente!!!" });
			//break;

			default:
				break;
		}
	}

	void Update () {

		if ( roupasBox != null ) roupasBox.text = "Roupas: " + nRoupas;

		// MOVIMENTO DO PERSONAGEM
		float dirX = Input.GetAxis("Horizontal");
		float stepX = dirX * velX;
		float curVelX = rb.velocity.x;
		if ((dirX > 0 && curVelX < maxVelX) || (dirX < 0 && Mathf.Abs(curVelX) < maxVelX)){
			if ( !dialog.paused )
				rb.AddForce(new Vector3(stepX, 0, 0));
		}

		if (dirX < 0) guySprite.transform.localScale = new Vector3(-0.1f, 0.1f, 1);
		else if (dirX > 0) guySprite.transform.localScale = new Vector3(0.1f, 0.1f, 1);

		// OCULTANDO A ARMA QUANDO ESTIVER SEM, E EVITANDO DE PROCESSAR A POSIÇÃO DELA
		if (!hasGun) {
			gunSprite.gameObject.SetActive(false);
			return;
		}

		gunSprite.gameObject.SetActive(true);
		mouse_pos = Input.mousePosition;
		mouse_pos.z = 12; //The distance between the camera and object
		object_pos = Camera.main.WorldToScreenPoint(gunSprite.transform.position);
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;
		gunAngle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		gunSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));

		if (Input.GetMouseButtonUp(0)) Shot();
	}

	void Shot(){
		if (!shotReady) return;
		if (dialog.paused) return;

		// RECUO DO TIRO
		shotReady = false;
		Vector3 mouse_pos2 = Input.mousePosition;
		mouse_pos2.z = 10; 
		Vector3 object_pos2 = Camera.main.WorldToScreenPoint(transform.position);
		mouse_pos2.x = mouse_pos2.x - object_pos2.x;
		mouse_pos2.y = mouse_pos2.y - object_pos2.y;
		gunAngle = Mathf.Atan2(mouse_pos2.y, mouse_pos2.x);

		Vector3 shotDirection = shotPower * new Vector3( -1*Mathf.Cos(gunAngle) , -1*Mathf.Sin(gunAngle), 0);
		rb.AddForce( shotDirection );

		// MUNIÇÃOZINHA
		GameObject novoTiro = Instantiate(tiro);
		novoTiro.transform.position = gunSprite.transform.position;
		Rigidbody[] tiroRB = novoTiro.GetComponentsInChildren<Rigidbody>();
		foreach(Rigidbody r in tiroRB ){
			r.AddForce( -4 * shotDirection);
		}
		Destroy(novoTiro.gameObject, 2.0f);

		StartCoroutine( RechargeGun() );
	}

	IEnumerator RechargeGun(){
		yield return new WaitForSeconds(rechargeTime);
		shotReady = true;
	}

}
