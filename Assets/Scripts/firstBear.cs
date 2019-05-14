using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstBear : MonoBehaviour {

	public float timeBetweenJumps = 3.0f;
	public float jumpPower = 500f;
	public Sprite rageSprite;

	public bool enraged = false;

	SpriteRenderer theSprite;
	Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		theSprite = GetComponent<SpriteRenderer>();
		StartCoroutine(DoJump() );
	}
	
	void Update () {

	}

	IEnumerator DoJump(){
		if (enraged){
			Vector3 jumpDirection = jumpPower * new Vector3(0, 1, 0);
			rb.AddForce(jumpDirection);
			
		}
		yield return new WaitForSeconds(timeBetweenJumps);
		StartCoroutine( DoJump() );
	}

	public void Enrage(){
		theSprite.sprite = rageSprite;
		enraged = true;
		rb.constraints = RigidbodyConstraints.None;
		rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
		theSprite.size.Set(2, 2);
	}

	//private void OnTriggerEnter(Collider other) {
	//	print(other.gameObject.name);
	//}

	//private void OnCollisionEnter(Collision collision) {
	//	print(collision.gameObject.name);
	//}

	//private void OnTriggerExit2D(Collider2D collision) {
	//	print(collision.gameObject.name);
	//}

}
