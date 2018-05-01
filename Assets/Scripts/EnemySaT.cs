using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySaT : MonoBehaviour {
	//Movement States
	public static readonly int idleStateHash = Animator.StringToHash ("Base Layer.Idle");
	public static readonly int idleBackStateHash = Animator.StringToHash ("Base Layer.Back.Idle Back");
	public static readonly int runStateHash = Animator.StringToHash ("Base Layer.Run");
	public static readonly int runBackStateHash = Animator.StringToHash ("Base Layer.Back.Run Back");

	//Fight States
	public static readonly int attackStateHash = Animator.StringToHash ("Base Layer.Attack");
	public static readonly int attackBackStateHash = Animator.StringToHash ("Base Layer.Back.Attack Back");
	public static readonly int reactionHitStateHash = Animator.StringToHash ("Base Layer.Reaction Hit");
	public static readonly int reactionHitBackStateHash = Animator.StringToHash ("Base Layer.Back.Reaction Hit Back");

	//Death States
	public static readonly int deathStateHash = Animator.StringToHash ("Base Layer.Death");
	public static readonly int deathBackStateHash = Animator.StringToHash ("Base Layer.Back.Death Back");
}
