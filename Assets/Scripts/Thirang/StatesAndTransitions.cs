using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesAndTransitions {
	//Base Layer States
	public readonly int idleStateHash = Animator.StringToHash ("Base Layer.Idle");
	public readonly int backIdleStateHash = Animator.StringToHash ("Base Layer.Back.Idle Back");
	public readonly int walkStateHash = Animator.StringToHash ("Base Layer.Walk");
	public readonly int walkBackStateHash = Animator.StringToHash ("Base Layer.Back.Walk Back");
	public readonly int runStateHash = Animator.StringToHash ("Base Layer.Run");
	public readonly int runBackStateHash = Animator.StringToHash ("Base Layer.Back.Run Back");

	//Idle Transitions
	public readonly int idleTransIdleBack = Animator.StringToHash ("Base Layer.Idle -> Base Layer.Back.Idle Back");
	public readonly int idleBackTransIdle = Animator.StringToHash ("Base Layer.Back.Idle Back -> Base Layer.Idle");

	//Fight States
	public readonly int slash1StateHash = Animator.StringToHash ("Base Layer.Fight.Slash 1");
	public readonly int slash2StateHash = Animator.StringToHash ("Base Layer.Fight.Slash 2");
	public readonly int slash1BackStateHash = Animator.StringToHash ("Base Layer.Back.Fight Back.Slash 1 Back");
	public readonly int slash2BackStateHash = Animator.StringToHash ("Base Layer.Back.Fight Back.Slash 2 Back");

	//Block States
	public readonly int blockStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block");
	public readonly int blockBackStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block Back");
	public readonly int blockIdleStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block Idle");
	public readonly int blockIdleBackStateHash = Animator.StringToHash ("Block.Block.Sword And Shield Block Idle Back");

	//Jump States
	public readonly int jumpStart = Animator.StringToHash ("Base Layer.Jump.Jump Start");
	public readonly int jumpIdle = Animator.StringToHash ("Base Layer.Jump.Jump Idle");
	public readonly int jumpDown = Animator.StringToHash ("Base Layer.Jump.Jump Down");
	public readonly int jumpBackStart = Animator.StringToHash ("Base Layer.Back.Jump Back.Jump Start");
	public readonly int jumpBackIdle = Animator.StringToHash ("Base Layer.Back.Jump Back.Jump Idle");
	public readonly int jumpBackDown = Animator.StringToHash ("Base Layer.Back.Jump Back.Jump Down");

	//Special Idles States
	public readonly int specIdle = Animator.StringToHash ("Special Idles.Idle");
	public readonly int specIdleBack = Animator.StringToHash ("Special Idles.Idle Back");

	//Abilities States
	public IDictionary <string, int> abilitiesStates = new Dictionary <string, int> ();

	public StatesAndTransitions () {
		abilitiesStates ["Berserk"] = Animator.StringToHash ("Base Layer.Berserk");
		abilitiesStates ["Cyclone Spin"] = Animator.StringToHash ("Base Layer.Cyclone Spin");
		abilitiesStates ["Magic Arrow"] = Animator.StringToHash ("Base Layer.Magic Arrow");
		abilitiesStates ["BerserkBack"] = Animator.StringToHash ("Base Layer.Back.Berserk Back");
		abilitiesStates ["Cyclone SpinBack"] = Animator.StringToHash ("Base Layer.Back.Cyclone Spin Back");
		abilitiesStates ["Magic ArrowBack"] = Animator.StringToHash ("Base Layer.Back.Magic Arrow Back");
	}
}