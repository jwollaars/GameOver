using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlaagGeest : MonoBehaviour
{
    private float m_JumpHeight = 0;
    private float m_TimeToJumpApex = 0;
    private float m_JumpAcceleration;
    private float m_AccelerationTimeAirborn = 0;
    private float m_AccelerationTimeGrounded = 0;
    private float m_MoveSpeed = 2;

    private float m_Gravity;
    private float m_JumpVelocity;
    private float m_VelocityXSmoothing;

    private Vector3 m_Velocity;

    private Controller2D m_Controller;

    [SerializeField]
    private GameObject m_Darkness;
    private Texture2D m_Texture2D;
    private Sprite m_Sprite;
    private Color m_Transparantable;
    [SerializeField]
    private float m_CurrentFade = 1;

    private float m_Timer = 0;

    [SerializeField]
    private float m_BeginHeight;
    public float GetBeginHeight
    {
        get { return m_BeginHeight; }
    }
    [SerializeField]
    private float m_TargetHeight;
    private float m_CurrentHeight;
    public float GetCurrentHeight
    {
        get { return m_CurrentHeight; }
    }
    private float m_Height;

    [SerializeField]
    private float m_DecreaseSpeed;

    [SerializeField]
    private Slider m_HeightSlider;

    [SerializeField]
    private GameObject m_Target;
    public GameObject Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }

    public bool m_GameRunning = true;

    private GameManager m_Manager;
    private bool m_playSound = true;

    private Animator m_Animator;

    void Start()
    {
        m_Manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_Controller = GetComponent<Controller2D>();
        m_Animator = GetComponent<Animator>();

        m_Gravity = -(2 * m_JumpHeight) / Mathf.Pow(m_TimeToJumpApex, 2);
        m_JumpVelocity = Mathf.Abs(m_Gravity) * m_TimeToJumpApex;

        m_CurrentHeight = m_BeginHeight;

        m_Transparantable = new Color(0, 0, 0, m_CurrentFade);
        m_Texture2D = new Texture2D(1, 1);
        m_Texture2D.SetPixel(0, 0, m_Transparantable);
        m_Texture2D.Apply();

        m_Darkness.GetComponent<SpriteRenderer>().sprite = Sprite.Create(m_Texture2D, new Rect(0, 0, m_Texture2D.width, m_Texture2D.height), new Vector2(0.5f, 0.5f));
        m_Darkness.transform.localScale = new Vector2(2600, 1500);
    }

    void Update()
    {
        if (m_GameRunning && m_Target != null)
        {
            SetLight();
            GeestHeightMovement();

            GeestDamage();
            GeestBoost();
            SliderHandler();

            if (m_CurrentHeight <= m_BeginHeight / 2)
            {
                if (m_playSound)
                {
                    m_Manager.PlaySoundOnce(0);
                }
                m_playSound = false;
            }
            else
            {
                m_playSound = true;
            }

            if (m_CurrentHeight <= m_TargetHeight + 0.5f)
            {
                m_Manager.GetComponent<GameManager>().ScoreScreen("Game over");
            }
        }
        else
        {
            m_Height = 500;
            SetLight();
        }
    }

    private void SetLight()
    {
        //float distance = 0;
        //distance = Vector2.Distance(transform.position, m_LadyBlabla.transform.position);

        m_CurrentFade = 1 - (m_Height - m_TargetHeight) / 12;
        m_Transparantable = new Color(0, 0, 0, m_CurrentFade);
        m_Texture2D.SetPixel(0, 0, m_Transparantable);
        m_Texture2D.Apply();
    }

    private void GeestDamage()
    {
        if (m_Timer >= 0)
        {
            m_Timer -= Time.deltaTime;

            if (m_Timer >= 0)
            {
                m_CurrentHeight += (m_DecreaseSpeed * m_Manager.GetKidsSaved * 2) * Time.deltaTime;
                m_Animator.SetBool("Normal", false);
            }
            else
            {
                m_Animator.SetBool("Normal", true);
            }

            if (m_Timer <= 0)
            {
                m_Timer = 0;
            }
        }
    }
    private void GeestBoost()
    {
        if (m_Timer <= 0)
        {
            m_Timer += Time.deltaTime;

            if (m_Timer <= 0)
            {
                m_CurrentHeight += (-m_DecreaseSpeed * 2) * Time.deltaTime;
            }

            if (m_Timer >= 0)
            {
                m_Timer = 0;
            }
        }
    }
    private void GeestHeightMovement()
    {
        m_CurrentHeight -= m_DecreaseSpeed * m_Manager.GetKidsSaved * 0.7f * Time.deltaTime;
        m_Height = m_CurrentHeight + m_Target.transform.position.y;

        if (m_CurrentHeight <= m_TargetHeight)
        {
            m_CurrentHeight = m_BeginHeight;
        }

        transform.position = new Vector3(m_Target.transform.position.x, m_CurrentHeight, transform.position.z);
    }
    private void GeestMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_Target.transform.position, m_MoveSpeed * Time.deltaTime);
    }
    private void GeestMovementCollision()
    {
        float angle = Mathf.Atan2(m_Target.transform.position.y - transform.position.y, m_Target.transform.position.x - transform.position.x) * (180 / Mathf.PI) + 90;
        Vector2 direction = new Vector2(0, 0);
        direction.x = Mathf.Cos((angle - 90) * (Mathf.PI / 180));
        direction.y = Mathf.Sin((angle - 90) * (Mathf.PI / 180));
        Debug.Log(direction.x);
        Debug.Log(direction.y);

        float targetVelocityX = direction.x * m_MoveSpeed;
        m_Velocity.x = Mathf.SmoothDamp(m_Velocity.x, targetVelocityX, ref m_VelocityXSmoothing, m_AccelerationTimeGrounded);
        float targetVelocityY = direction.y * m_MoveSpeed;
        m_Velocity.y = Mathf.SmoothDamp(m_Velocity.y, targetVelocityY, ref m_VelocityXSmoothing, m_AccelerationTimeGrounded);

        m_Controller.Move(m_Velocity * Time.deltaTime);
    }

    private void SliderHandler()
    {
        float value = (m_CurrentHeight - m_BeginHeight) / 100;

        m_HeightSlider.value = value;
    }

    public void AddCount(float valueToAdd)
    {
        m_Timer += valueToAdd;
    }
}