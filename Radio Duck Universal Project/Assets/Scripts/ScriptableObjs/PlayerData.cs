using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")] 
public class PlayerData : ScriptableObject
{
	[Header("Gravity")]
	[HideInInspector] public float gravityStrength; 
	[HideInInspector] public float gravityScale; 
												 
	[Space(5)]
	public float fallGravityMult; 
	public float maxFallSpeed; 
	[Space(5)]
	public float fastFallGravityMult; 
									  
	public float maxFastFallSpeed; 

	[Space(20)]

	[Header("Run")]
	public float runMaxSpeed; //Target speed we want the player to reach.
	public float runAcceleration; 
	[HideInInspector] public float runAccelAmount; 
	public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
	[HideInInspector] public float runDeccelAmount; 
	[Space(5)]
	[Range(0f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
	[Range(0f, 1)] public float deccelInAir;
	[Space(5)]
	public bool doConserveMomentum = true;

	[Space(20)]

	[Header("Jump")]
	public float jumpHeight; //Height of the player's jump
	public float jumpTimeToApex; 
	[HideInInspector] public float jumpForce; 

	[Header("Both Jumps")]
	public float jumpCutGravityMult; //Multiplier to increase gravity if the player releases thje jump button while still jumping
	[Range(0f, 1)] public float jumpHangGravityMult; 
	public float jumpHangTimeThreshold; 
	[Space(0.5f)]
	public float jumpHangAccelerationMult;
	public float jumpHangMaxSpeedMult;

	[Header("Assists")]
	[Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
	[Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.


	//Unity Callback, called when the inspector updates
	private void OnValidate()
	{
		//Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
		gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

		//Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
		gravityScale = gravityStrength / Physics2D.gravity.y;

		//Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
		runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
		runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

		//Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
		jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

		#region Variable Ranges
		runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
		#endregion
	}
}