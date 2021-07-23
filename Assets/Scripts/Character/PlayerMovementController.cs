using UnityEngine;

namespace Player.Movement
{
    [System.Serializable]
    public class PlayerMovementConfiguration
    {
        public float walkSpeed = 1.0F;
        public float runSpeed = 2.0F;
        public float rocketPower = 2.0F;
        public float moveDamping = 2.0F;
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

        public Vector3 groundPointOffset;
        public Vector3 groundBoxScale = Vector3.one;
        public PlayerMovementConfiguration movementConfiguration;

        private Vector3 direction;

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

            if (jumpContext.IsHold)
            {
                moveForce += transform.up * movementConfiguration.rocketPower;
            }
            else
            {
                moveForce.y = m_rigidbody.velocity.y;
            }

            m_rigidbody.velocity = moveForce;

        }
    }
}