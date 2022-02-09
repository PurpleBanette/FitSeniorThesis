using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class ModifiedTPC : MonoBehaviour
{
	public static ModifiedTPC instance;
	CharacterActions customCharActions;
	InputAction Move;
	InputAction Look;
	InputAction Jump;
	InputAction Dodge;
	InputAction Attack;
	InputAction Block;
	InputAction LockToTarget;


	[Header("Player")]
	[Tooltip("Move speed of the character in m/s")]
	public float MoveSpeed = 3.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	public float SprintSpeed = 5.335f;
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)]
	public float RotationSmoothTime = 0.12f;
	[Tooltip("Acceleration and deceleration")]
	public float SpeedChangeRate = 10.0f;

	[Space(10)]
	[Tooltip("The height the player can jump")]
	public float JumpHeight = 2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float Gravity = -15.0f;

	[Space(10)]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	public float JumpTimeout = 0.50f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	public float FallTimeout = 0.15f;

	[Header("Player Grounded")]
	[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
	public bool Grounded = true;
	[Tooltip("Useful for rough ground")]
	public float GroundedOffset = -0.04f;
	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
	public float GroundedRadius = 0.5f;
	[Tooltip("What layers the character uses as ground")]
	public LayerMask GroundLayers;

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;
	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 70.0f;
	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -30.0f;
	[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
	public float CameraAngleOverride = 0.0f;
	[Tooltip("For locking the camera position on all axis")]
	public bool LockCameraPosition = false;

	// cinemachine
	private float _cinemachineTargetYaw;
	private float _cinemachineTargetPitch;

	// player
	private float _speed;
	private float _animationBlend;
	private float _targetRotation = 0.0f;
	private float _rotationVelocity;
	private float _verticalVelocity;
	private float _terminalVelocity = 53.0f;
	float targetSpeed;

	// timeout deltatime
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;

	// animation IDs
	private int _animIDSpeed;
	private int _animIDGrounded;
	private int _animIDJump;
	private int _animIDFreeFall;
	private int _animIDMotionSpeed;

	private Animator _animator;
	private CharacterController _controller;
	private GameObject _mainCamera;

	private const float _threshold = 0.01f;

	private bool _hasAnimator;

	//public GameMaster gm;
	public bool blocking;
	GameObject boss;
	Animator bossAni;
	GameObject weaponobj;
	public Collider weapon;
	Animator charAni;
	Vector2 Lookvalues = new Vector2();
	Vector2 MoveValues = new Vector2();
	int health = 100;
	public bool canFall = true;
	bool dead;
	[SerializeField]
	Slider healthBar;
	[SerializeField]
	Text loser;
	[SerializeField]
	float blockTimeout = 0.5f;
	[SerializeField]
	float dodgeTimeout = 0.2f;

	float dodgeTimeoutDelta;
	float blockTimeoutDelta;
	public int comboSet;
	bool canAttack = true;
	//public bool inputRecieved = false;
	public bool inAttack1;
	public bool inAttack2;
	public bool inAttack3;
	[SerializeField] GameObject dodgeBox;
	bool lockedOn;

	//Picking up Boss's weapon
	obsidianWeaponPickup obsWep;
	public GameObject bossWeaponPosition, bossWeaponPrefab;
	public bool playerHasBossWeapon;

	//Hurtbox ticks
	public List<GameObject> hurtboxesPlayer;
	public bool playerHitTick;
	public float InvincibleFrameTimerPlayer = 1f;



	private void Awake()
	{
		instance = this;
		customCharActions = new CharacterActions();
		_controller = GetComponent<CharacterController>();
		_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		charAni = GetComponent<Animator>();

        if (dodgeBox.activeInHierarchy)
        {
			dodgeBox.SetActive(false);
        }
	}

	private void OnEnable()
	{
		
		Move = customCharActions.Player.Move;
		Move.Enable();
		
		

		Look = customCharActions.Player.Look;
		Look.Enable();

		customCharActions.Player.Jump.performed += DoJump;
		customCharActions.Player.Jump.Enable();

		customCharActions.Player.Dodge.performed += DoDodge;
		customCharActions.Player.Dodge.Enable();

		customCharActions.Player.Attack.performed += DoAttack;
		customCharActions.Player.Attack.Enable();

		customCharActions.Player.Block.performed += DoBlock;
		customCharActions.Player.Block.Enable();

		customCharActions.Player.LockToTarget.performed += DoLockToTarget;
		customCharActions.Player.LockToTarget.Enable();
	}

    private void DoLockToTarget(InputAction.CallbackContext obj)
    {
		lockOnTarget();
	}

    private void DoBlock(InputAction.CallbackContext obj)
	{
		//Debug.Log("Blocking");
		block();
	}

	private void DoAttack(InputAction.CallbackContext obj)
	{
        //Debug.Log("Attacking");
        
		attack();

	}

	private void DoDodge(InputAction.CallbackContext obj)
	{
		//Debug.Log("Dodging");
		dodge();
	}

	private void DoJump(InputAction.CallbackContext obj)
	{
		//throw new NotImplementedException();
		//Debug.Log("Jumping");
		//GroundedCheck();
		charAni.SetTrigger("Jump");
		JumpForce();
	}

	

	private void OnDisable()
	{
		Move.Disable();
		customCharActions.Player.Jump.Disable();
		Look.Disable();
		customCharActions.Player.Dodge.Disable();
		customCharActions.Player.Attack.Disable();
		customCharActions.Player.Block.Disable();
	}

	private void FixedUpdate()
	{
		//Debug.Log("Movement Values " + Move.ReadValue<Vector2>());
		//Debug.Log("Look Val " + Look.ReadValue<Vector2>());
		GroundedCheck();
		CharGravity();
        //JumpAndGravity();
        if (!blocking)
        {
			MoveValues = Move.ReadValue<Vector2>();
			Movement();
		}
		
		if(blockTimeoutDelta > 0f)
        {
			blockTimeoutDelta -= Time.deltaTime;
        }

		if (dodgeTimeoutDelta > 0f)
		{
			dodgeTimeoutDelta -= Time.deltaTime;
		}
        if (!inAttack1 && !inAttack2 && !inAttack3)
        {
			canAttack = true;
        }
        else
        {
			canAttack = false;
        }
        
	}

	private void Update()
	{
		charAni.SetFloat("isRunning", _speed);
		PlayerInvincibilityDetection();
	}

	private void LateUpdate()
	{
		Lookvalues = Look.ReadValue<Vector2>();
		CameraRotation();
		CameraRotation();
	}

	private void JumpForce()
	{
		// Jump
		if (Grounded && _jumpTimeoutDelta <= 0.0f)
		{

			// the square root of H * -2 * G = how much velocity needed to reach desired height
			_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDJump, true);
			}
		}
	}

	private void CharGravity()
	{

        if (!canFall)
        {
			_verticalVelocity = 0f;
        }
        else
        {
			if (Grounded)
			{

				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					//default val is -2f
					_verticalVelocity = -20f;
				}



				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				//customCharActions.Player.Jump.Disable();
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}
		
	}

	private void GroundedCheck()
	{
		// set sphere position, with offset
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
		Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

		// update animator if using character

	}
	private void OnDrawGizmosSelected()
	{
		Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

		if (Grounded) Gizmos.color = transparentGreen;
		else Gizmos.color = transparentRed;

		// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
		Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
	}
	private void CameraRotation()
	{
		// if there is an input and camera position is not fixed
		_cinemachineTargetYaw += Lookvalues.x * Time.deltaTime;
		_cinemachineTargetPitch += Lookvalues.y * Time.deltaTime;


		// clamp our rotations so our values are limited 360 degrees
		_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

		// Cinemachine will follow this target
		CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
	}
	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}

	private void Movement()
	{

        
		// set target speed based on move speed, sprint speed and if sprint is pressed
		targetSpeed = MoveSpeed;

		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (MoveValues == Vector2.zero) targetSpeed = 0.0f;

		// a reference to the players current horizontal velocity
		float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

		float speedOffset = 0.1f;
		float inputMagnitude = 1f;

		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			// creates curved result rather than a linear one giving a more organic speed change
			// note T in Lerp is clamped, so we don't need to clamp our speed
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

			// round speed to 3 decimal places
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
		}
		else
		{
			_speed = targetSpeed;
		}
		_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

		// normalise input direction
		Vector3 inputDirection = new Vector3(MoveValues.x, 0.0f, MoveValues.y).normalized;

		// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is a move input rotate player when the player is moving
		if (MoveValues != Vector2.zero)
		{
			_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

			// rotate to face input direction relative to camera position
			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}


		Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

		// move the player
		_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

		// update animator if using character
		if (_hasAnimator)
		{
			_animator.SetFloat(_animIDSpeed, _animationBlend);
			_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
		}
		

	}
	private void attack()
	{
		/*Re Enable when GM is integrated
		if (gm.playerstagger == false)
		{
			weapon.enabled = true;
			aniweap.SetTrigger("attack");
		}
		else
		{
			Debug.Log("you are staggered");
		}
		*/
		//weapon.enabled = true;

		if(canAttack)
        {
			charAni.SetTrigger("Attack");
			canAttack = false;
        }

        if (inAttack1)
        {
			charAni.SetTrigger("Attack2");
        }

		if (inAttack2)
		{
			charAni.SetTrigger("Attack3");
		}

	}
	void block()
	{
		/*Re Enable when GM is integrated
		if (gm.playerstagger == false)
		{
			blocking = true;
			aniweap.SetBool("block", true);
		}
		else
		{
			blocking = false;
			aniweap.SetBool("block", false);
		}
		*/
		if(blockTimeoutDelta <= 0.0f)
        {
			charAni.SetTrigger("Block");
			blockTimeoutDelta = blockTimeout;
		}
		

	}

	void dodge()
	{
		if (dodgeTimeoutDelta <= 0.0f)
        {
			//GameMaster.instance.playerDodgePosition.position = gameObject.transform.position;
			dodgeBox.SetActive(true);

			charAni.SetTrigger("Dodge");
			dodgeTimeoutDelta = dodgeTimeout;
		}
	}

	void lockOnTarget()
    {
        if (!lockedOn)
        {
			lockedOn = true;
			Debug.Log("locked to boss");
        }
        else
        {
			lockedOn = false;
			Debug.Log("free Cam");
		}
		
		
	}

	public void playerTakeDamage()
	{
		health -= 5;
		healthBar.value = health;
		//Debug.Log(health);
		if (health <= 0 && !dead)
        {
			loser.text = "YOU SUCK";
			dead = true;
        }
	}

	public void PlayerPickupWeapon()
    {
		obsWep = FindObjectOfType<obsidianWeaponPickup>();
		//obsWep.weprb.useGravity = false;
		obsWep.transform.parent = bossWeaponPosition.transform;
		obsWep.transform.localPosition = obsWep.itemPosition;
		obsWep.transform.localEulerAngles = obsWep.itemRotation;
		obsWep.transform.localScale = obsWep.itemScale;
		playerHasBossWeapon = true;
	}

	public void PlayerInvincibilityDetection()
    {
		if (playerHitTick && InvincibleFrameTimerPlayer > 0)
        {
			InvincibleFrameTimerPlayer -= Time.deltaTime;
			foreach (var hurtbox in hurtboxesPlayer)
            {
				hurtbox.SetActive(false);
            }
        }
		if (playerHitTick && InvincibleFrameTimerPlayer <= 0)
        {
			playerHitTick = false;
			InvincibleFrameTimerPlayer = 1f;
			foreach (var hurtbox in hurtboxesPlayer)
            {
				hurtbox.SetActive(true);
            }
        }
    }

}
