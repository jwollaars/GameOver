using UnityEngine;
using System.Collections;

public class ChildBehaviour : MonoBehaviour
{
    [SerializeField]
    private float m_JumpHeight = 4;
    [SerializeField]
    private float m_TimeToJumpApex = 0.4f;
    private float m_JumpAcceleration;
    [SerializeField]
    private float m_AccelerationTimeAirborn = 0.2f;
    [SerializeField]
    private float m_AccelerationTimeGrounded = 0.1f;
    private float m_MoveSpeed = 1;

    private float m_Gravity;
    private float m_JumpVelocity;
    private float m_VelocityXSmoothing;

    private Vector3 m_Velocity;

    private Controller2D m_Controller;

    [SerializeField]
    private bool m_Special = false;
    public bool Special
    {
        get { return m_Special; }
        set { m_Special = value; }
    }

    private float m_JumpTimer = 0.01f;
    private bool m_Jump = false;

    private GameObject m_Player;
    private Player m_PlayerScript;
    private float m_Distance = 2;

    private GameManager m_Manager;
    private Animator m_Animator;

    public void Start()
    {
        m_Controller = GetComponent<Controller2D>();
        m_Player = GameObject.Find("Player");
        m_PlayerScript = m_Player.GetComponent<Player>();
        m_Animator = GetComponent<Animator>();
        //m_Manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        m_Gravity = -(2 * m_JumpHeight) / Mathf.Pow(m_TimeToJumpApex, 2);
        m_JumpVelocity = Mathf.Abs(m_Gravity) * m_TimeToJumpApex;

        m_Distance = m_PlayerScript.FollowingChildren;
        m_JumpTimer = 0.01f * m_Distance;

        if(m_Special)
        {
            m_Animator.SetBool("Special", true);
        }
    }

    void Update()
    {
        if(m_Player.transform.position.x >= transform.position.x)
        {
            m_MoveSpeed = 1;
        }
        else
        {
            m_MoveSpeed = -1;
        }

        if(Vector2.Distance(m_Player.transform.position, transform.position) < m_Distance)
        {
            m_MoveSpeed = 0;
        }

        if (m_Controller.GetCollisions.above || m_Controller.GetCollisions.below)
        {
            m_Velocity.y = 0;
        }

        if(m_Player.transform.position.y - 1 > transform.position.y && m_Controller.GetCollisions.below && !m_Special)
        {
            m_Jump = true;
        }

        transform.localScale = new Vector3(m_Player.transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        if(m_Jump)
        {            
            m_JumpTimer -= Time.deltaTime;
            if(m_JumpTimer <= 0)
            {
                Jump();
                m_Jump = false;
                m_JumpTimer = 0.01f * m_Distance;
            }
        }

        float targetVelocityX = m_MoveSpeed * 6;
        m_Velocity.x = Mathf.SmoothDamp(m_Velocity.x, targetVelocityX, ref m_VelocityXSmoothing, m_AccelerationTimeGrounded);
        m_Velocity.y += m_Gravity * Time.deltaTime;
        m_Controller.Move(m_Velocity * Time.deltaTime);
    }

    public void Jump()
    {
        m_Velocity.y = m_JumpVelocity;
    }
}