using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerActions : PlayerActionSet
{
	public PlayerAction MoveLeft;
	public PlayerAction MoveRight;
	public PlayerAction MoveForward;
	public PlayerAction MoveReverse;

	public PlayerTwoAxisAction Move;

	public PlayerAction MoveUp;
	public PlayerAction MoveDown;

	public PlayerOneAxisAction Elevate;

	public PlayerAction RollLeft;
	public PlayerAction RollRight;

	public OneAxisInputControl Roll;

	public PlayerAction LookUp;
	public PlayerAction LookDown;
	public PlayerAction LookLeft;
	public PlayerAction LookRight;

	public PlayerTwoAxisAction Look;

	public PlayerAction Shoot;
	public PlayerAction Ability;
	public PlayerAction Pause;

	public PlayerActions ()
	{
		MoveLeft = CreatePlayerAction ("MoveLeft");
		MoveRight = CreatePlayerAction ("MoveRight");
		MoveReverse = CreatePlayerAction ("MoveReverse");
		MoveForward = CreatePlayerAction ("MoveForward");

		Move = CreateTwoAxisPlayerAction (MoveLeft, MoveRight, MoveReverse, MoveForward);

		MoveDown = CreatePlayerAction ("MoveDown");
		MoveUp = CreatePlayerAction ("MoveUp");

		Elevate = CreateOneAxisPlayerAction (MoveDown, MoveUp);

		RollLeft = CreatePlayerAction ("RollLeft");
		RollRight = CreatePlayerAction ("RollRight");

		Roll = CreateOneAxisPlayerAction (RollLeft, RollRight);

		LookLeft = CreatePlayerAction ("LookLeft");
		LookRight = CreatePlayerAction ("LookRight");
		LookDown = CreatePlayerAction ("LookDown");
		LookUp = CreatePlayerAction ("LookUp");

		Look = CreateTwoAxisPlayerAction (LookLeft, LookRight, LookDown, LookUp);

		Shoot = CreatePlayerAction ("Shoot");
		Ability = CreatePlayerAction ("Ability");
		Pause = CreatePlayerAction ("Pause");
	}
}
