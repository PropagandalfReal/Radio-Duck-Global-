using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	//Scriptable object which holds all the player's movement parameters. 

	public PlayerData Data;
	public Animator Animator;

	#region Variables
	//Components
	public Rigidbody2D RB { get; private set; }

	//Variables

	public bool IsFacingRight { get; private set; }
	public bool IsJumping { get; private set; }

	//Timers
	public float LastOnGroundTime { get; private set; }

	//Which input to take
	[SerializeField] string HorizontalLocal;
	[SerializeField] string VerticalLocal;

	//Jump
	private bool _isJumpCut;
	private bool _isJumpFalling;
	private bool _CanFlip;

	private Vector2 _moveInput;
	public float LastPressedJumpTime { get; private set; }

	//Inspector Serial Fields
	[Header("Checks")]
	[SerializeField] private Transform _groundCheckPoint;

	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);

	[Header("Layers & Tags")]
	[SerializeField] private LayerMask _groundLayer;
	#endregion

	//Get Components

	private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;
	}

	//Update to find Inputs and Elapsed Time

	private void Update()
	{
		#region TIMERS
		LastOnGroundTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;
		#endregion

		#region INPUT HANDLER
		_moveInput.x = Input.GetAxisRaw(HorizontalLocal);
		_moveInput.y = Input.GetAxisRaw(VerticalLocal);

		if (_moveInput.x != 0)
		{
			Animator.SetFloat("Speed", Mathf.Abs(_moveInput.x * 10));
			CheckDirectionToFace(_moveInput.x > 0);
		}
		else
		{
			Animator.SetFloat("Speed", Mathf.Abs(0));
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			OnJumpInput();
			Debug.Log("E key was pressed.");
		}

		if (Input.GetKeyUp(KeyCode.E))
		{
			OnJumpUpInput();
			Debug.Log("E key was released.");
		}
		#endregion

		//Collisions

		#region COLLISION CHECKS
		if (!IsJumping)
		{
			//Ground Check
			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping)
			{
				Animator.SetBool("Jumping", false);
				_CanFlip = true;
				LastOnGroundTime = Data.coyoteTime;
			}
		}
		#endregion

		//Jumping logic

		#region JUMP CHECKS
		if (IsJumping && RB.velocity.y < 0)
		{
			IsJumping = false;
		}

		if (LastOnGroundTime > 0 && !IsJumping)
		{
			_isJumpCut = false;

			if (!IsJumping)
				_isJumpFalling = false;
		}

		//Jump execute
		if (CanJump() && LastPressedJumpTime > 0)
		{
			_CanFlip = false;
			IsJumping = true;
			Animator.SetBool("Jumping", IsJumping);
			_isJumpCut = false;
			_isJumpFalling = false;
			Jump();
		}

		#endregion

		#region GRAVITY
		//Higher gravity if we've released the jump input or are falling

		if (RB.velocity.y < 0 && _moveInput.y < 0)
		{
			SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
		}
		else if (_isJumpCut)
		{
			SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
		}
		else if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
		}
		else if (RB.velocity.y < 0)
		{
			SetGravityScale(Data.gravityScale * Data.fallGravityMult);
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
		}
		else
		{
			SetGravityScale(Data.gravityScale);
		}
		#endregion
	}

	private void FixedUpdate()
	{
		//Handle Run
		Run(1);

	}

	#region INPUT CALLBACKS
	public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	public void OnJumpUpInput()
	{
		if (CanJumpCut())
			_isJumpCut = true;
	}
	#endregion

	#region GENERAL METHODS
	public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}
	#endregion

	//Movement
	#region RUN METHODS
	private void Run(float lerpAmount)
	{
		//Calculate the direction
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

		#region Calculate AccelRate
		float accelRate;

		//Gets an acceleration value
		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
		#endregion

		#region Add Bonus Jump Apex Acceleration
		//Increase Force
		if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion

		//Conserve momentum
		#region Conserve Momentum
		//Likely used for final project, not necessary here
		if (Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			accelRate = 0;
		}
		#endregion

		//Calculate difference 
		float speedDif = targetSpeed - RB.velocity.x;
		//Calculate force along x-axis

		float movement = speedDif * accelRate;

		//Convert this to a vector
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
	}

	//Flips player's sprite
	private void Turn()
	{
		if (_CanFlip)
		{
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;

			IsFacingRight = !IsFacingRight;
		}
	}
	#endregion

	#region JUMP METHODS
	private void Jump()
	{
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;

		#region Perform Jump
		float force = Data.jumpForce;
		if (RB.velocity.y < 0)
			force -= RB.velocity.y;

		RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
		#endregion
	}

	#endregion

	#region CHECK METHODS
	public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

	private bool CanJump()
	{
		return LastOnGroundTime > 0 && !IsJumping;
	}

	private bool CanJumpCut()
	{
		return IsJumping && RB.velocity.y > 0;
	}
	#endregion


	#region EDITOR METHODS
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
	}
	#endregion
}

// Heavily edited script by Dawnosaur on YT and Github