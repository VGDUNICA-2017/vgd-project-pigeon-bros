using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	protected Animator anim;
	protected GameObject thirang;
	protected Thirang th;
	protected Enemy enemy;

	public bool isFacingLeft { get; set; }
	public bool isAttacking { get; set; }

	protected bool deathStart;
	protected bool newLevelSet;

	protected float thirangDistance;
	private float dist;
	public float distanceThreshold;

	protected void UpdatePosition() {
		//Lock enemy rotation and Z position
		this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y);
		this.transform.eulerAngles = new Vector3 (this.transform.eulerAngles.x, -90, this.transform.eulerAngles.z);

		this.dist = this.transform.position.x - thirang.transform.position.x;
		thirangDistance = Mathf.Abs (dist);

		if (this.dist >= 0) {
			this.anim.SetBool ("IsFacingLeft", true);
			this.isFacingLeft = true;
		}
		if (this.dist < 0) {
			this.anim.SetBool ("IsFacingLeft", false);
			this.isFacingLeft = false;
		}
	}

	protected void EnemyDead () {
		if (this.enemy.health <= 0 && !this.deathStart) {
			
			this.anim.SetTrigger ("Death");
			this.deathStart = true;

			if (enemy is Orc)
				((Orc)enemy).OnDeath ();
			else 
				if (enemy is Archer)
					((Archer)enemy).OnDeath ();
				else
					if (enemy is Mage)
						((Mage)enemy).OnDeath ();
					else 
						if (enemy is Demon)
							((Demon)enemy).OnDeath ();
						else 
							if (enemy is GiantMutant)
								((GiantMutant)enemy).OnDeath ();
							else
								if (enemy is LavaMonster)
									((LavaMonster)enemy).OnDeath ();
		}
	}

	protected void DeathAnimation (AnimatorStateInfo stateInfo) {
		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				this.anim.enabled = false;	//Executed when Death animation will finish
				this.enemy.fadingDeath = true;
			}
		}
	}

	protected void DeathAnimation (AnimatorStateInfo stateInfo, string levelToLoad) {
		if (stateInfo.fullPathHash == EnemySaT.deathStateHash || stateInfo.fullPathHash == EnemySaT.deathBackStateHash) {
			if (stateInfo.normalizedTime > 1f) {
				this.anim.enabled = false;	//Executed when Death animation will finish
				this.enemy.fadingDeath = true;
				if (!this.newLevelSet) {
					this.gameObject.AddComponent<TeleportBehaviour> ().BossKilled (levelToLoad);
					newLevelSet = true;
				}
			}
		}
	}
}
