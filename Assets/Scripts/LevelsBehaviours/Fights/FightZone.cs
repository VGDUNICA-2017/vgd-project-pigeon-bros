using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightZone : MonoBehaviour {
	/*
	 * It must be positoned over the top side of ground with a distance equals to 4f
	 */ 


	public int archers;
	public int orcs;

	GameObject[] enemies;

	float horzExtent;
	float minX, maxX;

	BoxCollider c;

	bool eventTriggered;
	bool enemiesInstantiated;

	// Use this for initialization
	void Start () {
		horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;

		enemies = new GameObject[archers + orcs];

		c = GetComponent<Collider> () as BoxCollider;
	}

	void Update() {
		if (enemiesInstantiated) {
			bool allEnemiesKilled = true;

			foreach (var g in enemies) {
				if (g) {
					if (g.GetComponent<Enemy> ().health > 0)
						allEnemiesKilled = false;
				}
			}

			if (allEnemiesKilled) {
				Camera.main.GetComponent <LookAt> ().enabled = true;
				Camera.main.GetComponent <LookAt> ().target.gameObject.GetComponent<Thirang> ().onFight = false;
				Destroy (this.gameObject);
			}
		}
	}

	void OnTriggerEnter (Collider other) {
		if (!eventTriggered && other.transform.root.CompareTag ("Player")) {
			eventTriggered = true;

			Camera.main.GetComponent <LookAt> ().enabled = false;

			Vector3 cameraPos = Camera.main.transform.position;
			cameraPos.x = transform.position.x;
			Camera.main.transform.position = cameraPos;

			maxX = transform.position.x + horzExtent;
			minX = transform.position.x - horzExtent;

			GameObject g = new GameObject ("LeftBound");
			BoxCollider cLeft = g.AddComponent<BoxCollider> ();
			cLeft.size = c.size;
			cLeft.isTrigger = false;
			Vector3 newPos = new Vector3 (minX, transform.position.y, 0);
			g.transform.position = newPos;
			g.transform.parent = transform;

			GameObject g2 = new GameObject ("RightBound");
			BoxCollider cRight = g2.AddComponent<BoxCollider> ();
			cRight.size = c.size;
			cRight.isTrigger = false;
			newPos = new Vector3 (maxX, transform.position.y, 0);
			g2.transform.position = newPos;
			g2.transform.parent = transform;

			StartCoroutine (InstantiateEnemies ());

			other.transform.root.GetComponent<Thirang> ().onFight = true;

			Destroy (this.GetComponent<Collider> ());
		}
	}

	IEnumerator InstantiateEnemies () {
		for (int i = 0; i < enemies.Length; i++) {
			int rand = Random.Range (1, 21);

			Enemy.EnemyType e;

			if (orcs > 0) {
				e = Enemy.EnemyType.Orc;
				orcs--;
			}
			else
				e = Enemy.EnemyType.Archer;

			Vector3 enemyPos = new Vector3 (0, transform.position.y - 4f);
			
			switch (rand <= 10) {
				case true:
					enemies [i] = Enemy.InstantiateEnemy (e, enemyPos + new Vector3 (maxX, 0, 0), Quaternion.identity, true);
					break;
				case false:
					enemies [i] = Enemy.InstantiateEnemy (e, enemyPos + new Vector3 (minX, 0, 0), Quaternion.identity, false);
					break;
				default:
					throw new System.Exception ("Error in Random Range");
			}

			yield return new WaitForSecondsRealtime (0.5f);
		}

		enemiesInstantiated = true;
	}
}
