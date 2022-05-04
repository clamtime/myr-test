using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float MaxForwardSpeed = 8.0f;   // Speed when walking forward
            public float ForwardAccelerationRate = 1.0f;
            public float MaxBackwardSpeed = 4.0f;  // Speed when walking backwards
            public float BackwardAccelerationRate = 0.5f;
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
            public float SpeedInAir = 8.0f;   // Speed when onair
            public float JumpForce = 30f;

            public float CurrentTargetSpeed = 8f;

            public bool canMove = true;
            

#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
                if (input == Vector2.zero)
                {
                    CurrentTargetSpeed = 0;
                    return;
                }
				if (input.y < 0)
				{
					//backwards
					CurrentTargetSpeed += -BackwardAccelerationRate;
                    if (CurrentTargetSpeed < -MaxBackwardSpeed)
                        CurrentTargetSpeed = -MaxBackwardSpeed;
				}
				if (input.y > 0)
				{
                    //forwards
                    //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                    CurrentTargetSpeed += ForwardAccelerationRate;
                    if (CurrentTargetSpeed > MaxForwardSpeed)
                        CurrentTargetSpeed = MaxForwardSpeed;

                }
            }

        }

        [Header("Gravity Settings")]
        public bool useCustomGravity = false;
        public float customGravityValue = 1;

        [Header("Camera Settings")]
        public bool canrotate;
        public bool detachCameraRotation;
        public Camera cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public Vector3 relativevelocity;

        public Detection detectGround;

        [HideInInspector] public Vector2 input;

        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        public bool  m_IsGrounded;

        private float lastJumpTime = 0.0f;


        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }


        private void Awake()
        {
            canrotate = true;
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init (transform, cam.transform);
        }

        private void Start()
        {
        }

        private void Update()
        {
            relativevelocity = transform.InverseTransformDirection(m_RigidBody.velocity);
            if (m_IsGrounded)
            {

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    NormalJump();
                }

            }

        }


        private void LateUpdate()
        {
            if (canrotate)
            {
                RotateView(detachCameraRotation);
            }
            else
            {
                mouseLook.LookOveride(transform, cam.transform);
            }
         

        }
        public void CamGoBack(float speed)
        {
            mouseLook.CamGoBack(transform, cam.transform, speed);

        }
        public void CamGoBackAll ()
        {
            mouseLook.CamGoBackAll(transform, cam.transform);

        }
        private void FixedUpdate()
        {
            GroundCheck();
            input = GetInput();

            float h = input.x;
            float v = input.y;
            Vector3 inputVector = new Vector3(h, 0, v);
            inputVector = Vector3.ClampMagnitude(inputVector, 1);

            if (movementSettings.canMove)
            {
                //grounded
                if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && m_IsGrounded)
                {
                    if (Input.GetAxisRaw("Vertical") > 0.3f || (Input.GetAxisRaw("Vertical") < -0.3f))
                    {
                        m_RigidBody.AddRelativeForce(0, 0, Time.deltaTime * 1000f * movementSettings.CurrentTargetSpeed * Mathf.Abs(inputVector.z));
                        //if (m_RigidBody.velocity.z / 1000f / Mathf.Abs(inputVector.z) < movementSettings.MaxForwardSpeed)
                        //    m_RigidBody.velocity = new Vector3(0, 0, Time.deltaTime * 1000f * movementSettings.MaxForwardSpeed * Mathf.Abs(inputVector.z));

                    }
                    if (Input.GetAxisRaw("Horizontal") > 0.5f)
                    {
                        m_RigidBody.AddRelativeForce(Time.deltaTime * 1000f * movementSettings.StrafeSpeed * Mathf.Abs(inputVector.x), 0, 0);
                    }
                    if (Input.GetAxisRaw("Horizontal") < -0.5f)
                    {
                        m_RigidBody.AddRelativeForce(Time.deltaTime * 1000f * -movementSettings.StrafeSpeed * Mathf.Abs(inputVector.x), 0, 0);
                    }

                }
                //inair
                if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && !m_IsGrounded)
                {
                    if (Input.GetAxisRaw("Vertical") > 0.3f || (Input.GetAxisRaw("Vertical") < -0.3f))
                    {
                        m_RigidBody.AddRelativeForce(0, 0, Time.deltaTime * 1000f * (movementSettings.CurrentTargetSpeed * 0.1f) * Mathf.Abs(inputVector.z));
                    }
                    //if (Input.GetAxisRaw("Vertical") < -0.3f)
                    //{
                    //    m_RigidBody.AddRelativeForce(0, 0, Time.deltaTime * 1000f * -movementSettings.SpeedInAir * Mathf.Abs(inputVector.z));
                    //}
                    if (Input.GetAxisRaw("Horizontal") > 0.5f)
                    {
                        m_RigidBody.AddRelativeForce(Time.deltaTime * 1000f * movementSettings.SpeedInAir * Mathf.Abs(inputVector.x), 0, 0);
                    }
                    if (Input.GetAxisRaw("Horizontal") < -0.5f)
                    {
                        m_RigidBody.AddRelativeForce(Time.deltaTime * 1000f * -movementSettings.SpeedInAir * Mathf.Abs(inputVector.x), 0, 0);
                    }

                }
            }     

            // custom gravity
            if (useCustomGravity)
            {
                if (m_RigidBody.useGravity == true)
                    m_RigidBody.useGravity = false;

                m_RigidBody.AddForce(Physics.gravity * m_RigidBody.mass * customGravityValue);
            }
            //else
            //{
            //    if (m_RigidBody.useGravity == false)
            //        m_RigidBody.useGravity = true;
            //}
        }

        public void NormalJump(bool bypassSlide = false)
        {
            if (Time.time != lastJumpTime)
            {
                lastJumpTime = Time.time;
                m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
            }
        }
        public void SwitchDirectionJump()
        {
            m_RigidBody.velocity = transform.forward * m_RigidBody.velocity.magnitude;
            m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
        }
  
        public void DirectionJump(Vector3 dir)
        {
            //if ((dir.x != 0 && dir.x != 1) || (dir.y != 0 && dir.y != 1) || (dir.z != 0 && dir.z != 1))
            //    Debug.LogError("DirectionalJump() vec3 argument must be comrpised of either 0s or 1s only.");


            m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
            m_RigidBody.AddForce(new Vector3(
                Mathf.Sign(dir.x) * movementSettings.JumpForce,
                Mathf.Abs(Mathf.Sign(dir.y) * movementSettings.JumpForce),
                Mathf.Sign(dir.z) * movementSettings.JumpForce), 
                ForceMode.Impulse);
        }
      


        private Vector2 GetInput()
        {
            
            Vector2 input = new Vector2
                {
                    x = Input.GetAxisRaw("Horizontal"),
                    y = Input.GetAxisRaw("Vertical")
                };
			movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }


        private void RotateView(bool detached = false)
        {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation (transform, cam.transform);

       
        }


        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck()
        {
          if(detectGround.isColliding)
            {
                m_IsGrounded = true;
            }
          else
            {
                m_IsGrounded = false;
            }
        }
    }
}
