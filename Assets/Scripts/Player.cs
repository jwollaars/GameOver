using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float k_JumpHeight = 4;
    [SerializeField]
    private float k_TimeToJumpApex = 0.4f;
    private float k_JumpAcceleration;
    [SerializeField]
    private float k_AccelerationTimeAirborn = 0.2f;
    [SerializeField]
    private float k_AccelerationTimeGrounded = 0.1f;
    private float k_MoveSpeed = 6;

    private float k_Gravity;
    private float k_JumpVelocity;
    private float k_VelocityXSmoothing;

    private Vector3 k_Velocity;

    private int m_followingChildren;
    public int FollowingChildren
    {
        get { return m_followingChildren; }
        set { m_followingChildren = value; }
    }

    private AudioSource k_AudioSource;
    private Animator k_Animator;
    private SpriteRenderer k_SpriteRenderer;
    private Controller2D k_Controller;
    public Controller2D getController
    {
        get { return k_Controller; }
    }

    private GameObject m_LastCage;
    public GameObject GetLastCage
    {
        get { return m_LastCage; }
    }

    private GameManager m_GameManager;

    void Start()
    {
        k_Animator = GetComponent<Animator>();
        k_SpriteRenderer = GetComponent<SpriteRenderer>();
        k_Controller = GetComponent<Controller2D>();
        m_GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //k_AudioSource = GetComponent<AudioSource>();

        k_Gravity = -(2 * k_JumpHeight) / Mathf.Pow(k_TimeToJumpApex, 2);
        k_JumpVelocity = Mathf.Abs(k_Gravity) * k_TimeToJumpApex;
    }

    void Update()
    {
        if (k_Controller.GetCollisions.above || k_Controller.GetCollisions.below)
        {
            k_Velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        k_Animator.SetFloat("move", Input.GetAxisRaw("Horizontal"));
        if (Input.GetAxisRaw("Horizontal") >= 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetAxisRaw("Horizontal") <= -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && k_Controller.GetCollisions.below)
        {
            k_Velocity.y = k_JumpVelocity;
            if (Random.Range(0, 6) <= 3)
            {
                m_GameManager.PlaySoundOnce(Random.Range(4, 6));
            }
        }

        CollisionChecks(k_Controller.GetCollisions.gAbove);
        CollisionChecks(k_Controller.GetCollisions.gBelow);
        CollisionChecks(k_Controller.GetCollisions.gLeft);
        CollisionChecks(k_Controller.GetCollisions.gRight);

        float targetVelocityX = input.x * k_MoveSpeed;
        k_Velocity.x = Mathf.SmoothDamp(k_Velocity.x, targetVelocityX, ref k_VelocityXSmoothing, k_AccelerationTimeGrounded);
        k_Velocity.y += k_Gravity * Time.deltaTime;
        k_Controller.Move(k_Velocity * Time.deltaTime);
    }

    private void CollisionChecks(GameObject direction)
    {
        if (direction != null)
        {
            if (direction.tag == "Enemy")
            {
                direction.GetComponent<Enemy>().EnemyHit();
            }
        }

        if (k_Controller.GetCollisions.gRight != null)
        {
            if (k_Controller.GetCollisions.gRight.tag == "Finish")
            {
                m_GameManager.ScoreScreen("You Win");
            }
        }

        if (k_Controller.GetCollisions.gAbove != null)
        {
            if (k_Controller.GetCollisions.gAbove.tag == "Cage")
            {
                m_LastCage = k_Controller.GetCollisions.gAbove;
                if (m_LastCage.GetComponent<Cage>().GetCageState == true)
                {
                    m_LastCage.GetComponent<Cage>().ReleaseChild();
                    int value = Random.Range(0, 10);
                    if (value == 3)
                    {
                        m_GameManager.PlaySoundOnce(1);
                    }
                }
                else if (m_LastCage.GetComponent<Cage>().GetCageState == false)
                {
                    m_LastCage.GetComponent<Cage>().ReleaseDarkness();
                }
                //k_Controller.GetCollisions.gAbove.GetComponent<Cage>().ActivateQuestion();
            }
        }

        if (k_Controller.GetCollisions.gBelow != null)
        {
            if (k_Controller.GetCollisions.gBelow.tag == "Platform" || k_Controller.GetCollisions.gBelow.tag == "Cage")
            {
                k_Animator.SetBool("Jump", false);
            }

            if (k_Controller.GetCollisions.gBelow.tag == "Mud")
            {
                k_Animator.SetBool("Jump", false);
                k_Animator.SetBool("InMud", true);
                k_MoveSpeed = 1.5f;
            }
            else
            {
                k_Animator.SetBool("InMud", false);
                k_MoveSpeed = 6;
            }
        }
        else
        {
            k_Animator.SetBool("Jump", true);
        }
    }
}