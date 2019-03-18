using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


	Vector3 mouse_pos, object_pos;
	float gunAngle;
	Rigidbody rb;

	bool shotReady = true;
	bool enragedEnemies = false;

	void Start () {
		rb = GetComponent<Rigidbody>();
		DoType("Mamãe diz: Robson, você está de castigo!\nEnquanto não recolher as roupas do chão do quarto, não pode sair!!!");
	}
	
	void Update () {

		roupasBox.text = "Roupas: " + nRoupas;

		// MOVIMENTO DO PERSONAGEM
		float dirX = Input.GetAxis("Horizontal");
		float stepX = dirX * velX;
		float curVelX = rb.velocity.x;
		if ((dirX > 0 && curVelX < maxVelX) || (dirX < 0 && Mathf.Abs(curVelX) < maxVelX))
			rb.AddForce(new Vector3(stepX, 0, 0));

		if (dirX < 0) guySprite.transform.localScale = new Vector3(-0.15f, 0.15f, 1);
		else if (dirX > 0) guySprite.transform.localScale = new Vector3(0.15f, 0.15f, 1);


		// ATIVANDO OS INIMIGOS
		if (transform.position.x > 41f && !enragedEnemies) {
			enragedEnemies = true;
			DoType("Robson diz: Acho que peguei todas as roupas!!!\nHora de voltar");
			foreach( GameObject en in enemies ){
				en.GetComponent<firstBear>().Enrage();
			}
		}

		// PEGANDO A ARMA
		if (transform.position.x>9.5f && !hasGun){
			hasGun = true;
			Destroy(stickSprite.gameObject);
			DoType("Robson diz: Peguei a arma!!!");
		} else

		// TEXTOS
		if (transform.position.x > -6 && transform.position.x < 3 && !hasGun && !enragedEnemies) {
			DoType("");
		} else
		if (transform.position.x > 3 && transform.position.x < 15 && !hasGun && !enragedEnemies) {
			DoType("Robson diz: Minha arma!!!!");
		} else
		if (transform.position.x > 15 && transform.position.x < 34 && hasGun && !enragedEnemies) {
			DoType("");
		} else
		if (transform.position.x < 34 && enragedEnemies) {
			DoType("Robson diz: Ei!!!\nOs ursinhos se voltaram contra mim!!!");
		}

		if ( transform.position.x < 0 && nRoupas==4 ){
			DoType("Robson diz: MANHÊ!!!! PEGUEI TODAS AS ROUPAS!!!\n(Fim do protótipo)");
		}

		// ROUPAS
		if ( transform.position.x > 19 && !got1){
			Destroy(roupas[0].gameObject);
			nRoupas = 1;
			got1 = true;
		}
		if (transform.position.x > 30 && !got2) {
			Destroy(roupas[1].gameObject);
			nRoupas = 2;
			got2 = true;
		}
		if (transform.position.x > 36 && !got3) {
			Destroy(roupas[2].gameObject);
			nRoupas = 3;
			got3 = true;
		} 
		if (transform.position.x > 41 && !got4) {
			Destroy(roupas[3].gameObject);
			nRoupas = 4;
			got4 = true;
		}

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

	void DoType(string text){
		textBox.text = text;
	}

	void Shot(){
		if (!shotReady) return;

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
			r.AddForce( -5 * shotDirection);
		}
		StartCoroutine(ClearTiro(novoTiro));

		StartCoroutine( RechargeGun() );
	}

	IEnumerator RechargeGun(){
		yield return new WaitForSeconds(rechargeTime);
		shotReady = true;
	}

	IEnumerator ClearTiro( GameObject t ){
		yield return new WaitForSeconds(2.0f);
		Destroy(t.gameObject);
	}
}
