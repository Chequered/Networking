using UnityEngine;
using System.Collections;

public enum AnimationState
{
    None,
    Normal,
    Attack,
    Bomb,
	Death
}

public class PlayerGraphics : MonoBehaviour {

    //vars
    [SerializeField] int horAttackFrame;
    [SerializeField] int verAttackFrame;

    //standard
    [SerializeField] private Sprite[] m_idleAnimation;
    [SerializeField] private Sprite[] m_walkAnimation;

    //attack
    [SerializeField] private Sprite[] m_atkHorAnimation;
    [SerializeField] private Sprite[] m_atkUpAnimation;
    [SerializeField] private Sprite[] m_atkDownAnimation;

    //bomb
    [SerializeField] private Sprite[] m_bombAtkAnimation;
    [SerializeField] private Sprite[] m_bombIdleAnimation;
    [SerializeField] private Sprite[] m_bombWalkAnimation;
    [SerializeField] private Sprite[] m_bombSpawnAnimation;

	//Death
	[SerializeField] private Sprite[] m_deathAnimation;

    private AnimationState m_state;
    private PlayerMovement m_movement;
    private SpriteRenderer m_spriteRenderer;
    private Sprite[] m_currentAnimation;
    private int m_currentFrame;
    private int m_spriteDirection = 1;
    private GameObject m_playerName;
    private GameObject m_playerHitzones;
    private bool m_loop;
    private int m_attackDirection;

    private void Start()
    {
        m_state = AnimationState.Normal;
        m_movement = GetComponent<PlayerMovement>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerName = transform.FindChild("PlayerName").gameObject;
        m_playerHitzones = transform.FindChild("Attack Zones").gameObject;
        m_currentFrame = 0;
        m_frameInterval = 0.02f;
        m_previousFrameTime = Time.time;
        standardXScale = transform.localScale.x;
    }

    private float m_frameInterval;
    private float m_previousFrameTime;
    private void LateUpdate()
    {
        if (m_currentAnimation != null)
        {
            if (m_previousFrameTime + m_frameInterval <= Time.time)
            {
                m_currentFrame++;

                if (m_state == AnimationState.Attack)
                {
                    if(m_attackDirection == 1 || m_attackDirection == 2)
                    {
                        if (m_currentFrame == horAttackFrame)
                        {
                            transform.FindChild("Attack Zones").GetComponent<PlayerDmg>().Attack(m_attackDirection);
                        }
                    }
                    else
                    {
                        if (m_currentFrame == verAttackFrame)
                        {
                            transform.FindChild("Attack Zones").GetComponent<PlayerDmg>().Attack(m_attackDirection);
                        }
                    }
                }

                if (m_currentFrame >= m_currentAnimation.Length)
                {
                    m_currentFrame = 0;
                    if(!m_loop)
                    {
                        ChangeAnimationState(StateToID(AnimationState.Normal));
                        Idle();
                    }
                }
                m_spriteRenderer.sprite = m_currentAnimation[m_currentFrame];
                m_previousFrameTime = Time.time + m_frameInterval;
            }
        }
    }

    private void OnGUI()
    {
        if(GetComponent<NetworkView>().isMine)
        {
            GUI.Label(new Rect(50, 50, 500, 50), "f: " + m_currentFrame);
            GUI.Label(new Rect(50, 100, 500, 50), "s: " + m_state);
            GUI.Label(new Rect(50, 150, 500, 50), "d: " + m_attackDirection);
        }
    }

    [RPC]
    public void ChangeAnimationState(int stateID)
    {
        m_state = IDToState(stateID);
    }

    [RPC]
    public void Idle()
    {
        Debug.Log(m_state);
        if(m_state != AnimationState.Attack)
        {
            switch (m_state)
            {
                case AnimationState.None:
                    break;
                case AnimationState.Normal:
                    PlayAnimation(m_idleAnimation);
                    break;
                case AnimationState.Attack:
                    PlayAnimation(m_idleAnimation);
                    break;
                case AnimationState.Bomb:
                    PlayAnimation(m_bombIdleAnimation);
                    break;
				case AnimationState.Death:
					PlayAnimation(m_deathAnimation);
					break;
                default:
                    break;
            }
        }
    }

    [RPC]
    public void SwitchDirection(int dir)
    {
        m_spriteDirection = dir;

        transform.localScale = new Vector2(standardXScale * m_spriteDirection, transform.localScale.y);
        m_playerName.GetComponent<RectTransform>().localScale = transform.localScale.normalized;
        m_playerHitzones.transform.localScale = transform.localScale.normalized;
    }

    private float standardXScale;
    [RPC]
    public void Walk()
    {
        if (m_movement.Velocity.x > 0)
        {
            m_spriteDirection = 1;
        }
        else if (m_movement.Velocity.x < 0)
        {
            m_spriteDirection = -1;
        }

        GetComponent<NetworkView>().RPC("SwitchDirection", RPCMode.AllBuffered, m_spriteDirection);

        transform.localScale = new Vector2(standardXScale * m_spriteDirection, transform.localScale.y);
        m_playerName.GetComponent<RectTransform>().localScale = transform.localScale.normalized;
        m_playerHitzones.transform.localScale = transform.localScale.normalized;

        switch (m_state)
        {
            case AnimationState.None:
                break;
            case AnimationState.Normal:
                PlayAnimation(m_walkAnimation);
                break;
            case AnimationState.Attack:
                PlayAnimation(m_walkAnimation);
                break;
            case AnimationState.Bomb:
                PlayAnimation(m_bombAtkAnimation);
                break;
			case AnimationState.Death:
				PlayAnimation(m_deathAnimation);
				break;
            default:
                break;
        }
    }

    [RPC]
    public void Attack(int dir)
    {
        ChangeAnimationState(StateToID(AnimationState.Attack));
        m_attackDirection = dir;
        switch (dir)
        {
            case 1:
                PlayAnimation(m_atkHorAnimation, false);
                break;
            case 2:
                PlayAnimation(m_atkHorAnimation, false);
                break;
            case 3:
                PlayAnimation(m_atkDownAnimation, false);
                break;
            case 4:
                PlayAnimation(m_atkUpAnimation, false);
                break;
        }
    }

    private void PlayAnimation(Sprite[] spriteSheet, bool loop = true)
    {
        if(m_currentAnimation != spriteSheet)
        {
            m_currentAnimation = spriteSheet;
            m_currentFrame = 0;
        }
        m_loop = loop;
    }

    public AnimationState State
    {
        get
        {
            return m_state;
        }
    }

    #region converters

    public static int StateToID(AnimationState state)
    {
        switch (state)
        {
            case AnimationState.None:
                return 0;
            case AnimationState.Normal:
                return 1;
            case AnimationState.Attack:
                return 2;
            case AnimationState.Bomb:
                return 3;
			case AnimationState.Death:
				return 4;
            default:
                return 0;
        }
    }

    public static AnimationState IDToState(int ID)
    {
        switch (ID)
        {
            case 0:
                return AnimationState.None;
            case 1:
                return AnimationState.Normal;
            case 2:
                return AnimationState.Attack;
            case 3:
                return AnimationState.Bomb;
			case 4:
				return AnimationState.Death;
            default:
                return AnimationState.None;
        }
    }
    #endregion
}
