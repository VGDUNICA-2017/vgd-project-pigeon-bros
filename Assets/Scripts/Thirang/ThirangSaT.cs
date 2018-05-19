using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirangSaT {
	//Movement States
	public static readonly int idleStateHash = Animator.StringToHash ("Base Layer.Idle");
	public static readonly int idleBackStateHash = Animator.StringToHash ("Base Layer.Back.Idle Back");
	public static readonly int walkStateHash = Animator.StringToHash ("Base Layer.Walk");
	public static readonly int walkBackStateHash = Animator.StringToHash ("Base Layer.Back.Walk Back");
	public static readonly int runStateHash = Animator.StringToHash ("Base Layer.Run");
	public static readonly int runBackStateHash = Animator.StringToHash ("Base Layer.Back.Run Back");

	//Idle Transitions
	public static readonly int idleTransIdleBack = Animator.StringToHash ("Base Layer.Idle -> Base Layer.Back.Idle Back");
	public static readonly int idleBackTransIdle = Animator.StringToHash ("Base Layer.Back.Idle Back -> Base Layer.Idle");

	//Fight States
	public static readonly int slash1StateHash = Animator.StringToHash ("Base Layer.Fight.Slash 1");
	public static readonly int slash2StateHash = Animator.StringToHash ("Base Layer.Fight.Slash 2");
	public static readonly int slash1BackStateHash = Animator.StringToHash ("Base Layer.Back.Fight Back.Slash 1 Back");
	public static readonly int slash2BackStateHash = Animator.StringToHash ("Base Layer.Back.Fight Back.Slash 2 Back");

	//Block States
	public static readonly int blockStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block");
	public static readonly int blockBackStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block Back");
	public static readonly int blockIdleStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block Idle");
	public static readonly int blockIdleBackStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block Idle Back");

	//Jump States
	public static readonly int jumpStart = Animator.StringToHash ("Base Layer.Jump.Jump Start");
	public static readonly int jumpIdle = Animator.StringToHash ("Base Layer.Jump.Jump Idle");
	public static readonly int jumpDown = Animator.StringToHash ("Base Layer.Jump.Jump Down");
	public static readonly int jumpBackStart = Animator.StringToHash ("Base Layer.Back.Jump Back.Jump Start");
	public static readonly int jumpBackIdle = Animator.StringToHash ("Base Layer.Back.Jump Back.Jump Idle");
	public static readonly int jumpBackDown = Animator.StringToHash ("Base Layer.Back.Jump Back.Jump Down");

	//Jump Transitions
	public static readonly int runTransJumpStart = Animator.StringToHash ("Base Layer.Run -> Base Layer.Jump.Jump Start");
	public static readonly int runBackTransJumpStartBack = Animator.StringToHash ("Base Layer.Back.Run -> Base Layer.Back.Jump Back.Jump Start");

	//Special Idles States
	public static readonly int specIdle = Animator.StringToHash ("Special Idles.Idle");
	public static readonly int specIdleBack = Animator.StringToHash ("Special Idles.Idle Back");

	//Abilities States
	public static readonly Dictionary <string, int> abilitiesStates = new Dictionary <string, int> (){
		{ "Berserk", Animator.StringToHash ("Base Layer.Berserk") },
		{ "Cyclone Spin", Animator.StringToHash ("Base Layer.Cyclone Spin") },
		{ "Magic Arrow", Animator.StringToHash ("Base Layer.Magic Arrow") },
		{ "BerserkBack", Animator.StringToHash ("Base Layer.Back.Berserk Back") },
		{ "Cyclone SpinBack", Animator.StringToHash ("Base Layer.Back.Cyclone Spin Back") },
		{ "Magic ArrowBack", Animator.StringToHash ("Base Layer.Back.Magic Arrow Back") }
	};
}