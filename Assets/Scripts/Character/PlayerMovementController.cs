using UnityEngine;

namespace Player.Movement
{
    [System.Serializable]
    public class PlayerMovementConfiguration
    {
        public float walkSpeed = 1.0F;
        public float runSpeed = 2.0F;
        public float jumpPower = 2.0F;
        public float moveDamping = 1.0F;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        public bool IsMove
        {
            get
            {
                return GameInputFocus.IsActive(GameSystemType.PlayerHighPriority) &&
                    (Mathf.Abs(moveContext.Value.y) > inputThreshould ||
                    Mathf.Abs(moveContext.Value.x) > inputThreshould);
            }
        }

        public bool IsGrounded
        {
            get => isGrounded;
        }

        public Vector3 groundPointOffset;
        public PlayerMovementConfiguration movementConfiguration;

        private Vector3 direction;

        private bool isGrounded;
        private float move = 0.0F;
        private float sprint = 0.0F;

        private AxisInputContext moveContext;
        private ButtonInputContext jumpContext;
        private ButtonInputContext sprintContext;

        private Rigidbody m_rigidbody;

        private const float sprintThreshould = 0.25F;
        private const float inputThreshould = 0.05F;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            moveContext = GameInputManager.Instance.GetContext<MoveContext>();
            jumpContext = GameInputManager.Instance.GetContext<JumpContext>();
            sprintContext = GameInputManager.Instance.GetContext<SprintContext>();

            jumpContext.RegisterButtonPressCallback(JumpHandler, GameSystemType.PlayerHighPriority);
            sprintContext.RegisterButtonPressCallback(SprintHandler, GameSystemType.PlayerHighPriority);
        }

        private void SprintHandler()
        {
            if (sprintContext.IsHold && sprint < sprintThreshould)
            {
                sprint = sprintThreshould;
            }
        }

        private void JumpHandler()
        {
            if (jumpContext.IsHold && IsGrounded)
            {
                m_rigidbody.velocity += Physics.gravity.normalized * movementConfiguration.jumpPower;
            }
        }

        private void FixedUpdate()
        {
            if (IsMove)
            {
                direction =
                    transform.forward * moveContext.Value.y +
                    transform.right * moveContext.Value.x;

                move += 3F * Time.fixedDeltaTime;
                sprint += 3F * sprint * Time.fixedDeltaTime;
            }
            else
            {
                move -= movementConfiguration.moveDamping * Time.fixedDeltaTime;
                sprint = 0;
            }

            sprint = Mathf.Clamp01(sprint);
            move = Mathf.Clamp01(move);

            var moveForce = move * direction;// + gravityForce;
            var speed = Mathf.Lerp(movementConfiguration.walkSpeed, movementConfiguration.runSpeed, sprint);

            moveForce *= speed - m_rigidbody.velocity.magnitude;
            moveForce.y = m_rigidbody.velocity.y;

            m_rigidbody.velocity = moveForce;

            isGrounded = Physics.Raycast(transform.localToWorldMatrix.MultiplyPoint(groundPointOffset), Physics.gravity, 0.05F);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.localToWorldMatrix.MultiplyPoint(groundPointOffset), 0.1F);
        }
    }
}