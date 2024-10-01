using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Horror
{
    public class HorrorPlayer_Base : State<HorrorPlayer>
    {
        protected HorrorPlayer m_player = null;
        protected UIWorldHint m_interactionUI = null;
        protected NoteItem m_noteItem = null;

        protected bool m_recoverStamina = false;

        protected bool m_conversion = false;
        protected string m_triggerName = "";

        private float m_gravity = -9.81f; // �߷� ���ӵ� ��
        private Vector3 m_velocity;

        protected Transform m_transform;
        protected Transform m_rotationTransform;
        protected Rigidbody m_rigidbody;
        protected Animator m_animator;
        protected AudioSource m_audioSource;

        public HorrorPlayer_Base(StateMachine<HorrorPlayer> stateMachine) : base(stateMachine)
        {
            m_transform = m_stateMachine.Owner.GetComponent<Transform>();
            m_rigidbody = m_stateMachine.Owner.GetComponent<Rigidbody>();
            m_audioSource = m_stateMachine.Owner.GetComponent<AudioSource>();
            m_player = m_stateMachine.Owner.GetComponent<HorrorPlayer>();

            m_rotationTransform = m_transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).transform;
            m_animator = m_transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Animator>();
        }

        public override void Enter_State()
        {
        }

        public override void Update_State()
        {
        }

        public override void Exit_State()
        {
        }

        protected bool Input_Move()
        {
            float yRotate = Input_Rotation();

            Vector3 forwardDir = Quaternion.Euler(0, yRotate, 0) * Vector3.forward;
            Vector3 rightDir = Quaternion.Euler(0, yRotate, 0) * Vector3.right;

            Vector3 velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                velocity += forwardDir * m_player.MoveSpeed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.S))
                velocity += -forwardDir * m_player.MoveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
                velocity += rightDir * m_player.MoveSpeed * Time.deltaTime;
            else if (Input.GetKey(KeyCode.A))
                velocity += -rightDir * m_player.MoveSpeed * Time.deltaTime;

            if (velocity == Vector3.zero)
            {
                m_rigidbody.isKinematic = true;
            }
            else
            {
                m_rigidbody.isKinematic = false;
                m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, velocity, Time.deltaTime * m_player.LerpSpeed);
                return true;
            }

            return false;
        }

        protected float Input_Rotation()
        {
            if (m_player == null)
                return 0f;

            // Y�� �¿� ȸ��
            float yRotateSize = Input.GetAxis("Mouse X") * m_player.RotationSpeed * Time.deltaTime;
            float yRotate = m_transform.eulerAngles.y + yRotateSize;
            m_transform.eulerAngles = new Vector3(m_transform.eulerAngles.x, yRotate, 0);

            // X�� ���� ȸ��
            float xRotateSize = -Input.GetAxis("Mouse Y") * m_player.RotationSpeed * Time.deltaTime;
            m_player.XRotate = Mathf.Clamp(m_player.XRotate + xRotateSize, m_player.RotationLimit.x, m_player.RotationLimit.y); // ���� ����(-45, 80)
            m_rotationTransform.eulerAngles = new Vector3(m_player.XRotate, m_rotationTransform.eulerAngles.y, 0);

            return yRotate;
        }

        protected bool Input_Weapon()
        {
            // Ű���� ctrl
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                m_player.WeaponManagement.Next_Weapon(1);
                return true;
            }
            else
            {
                // ���콺 ��
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll > 0f)
                {
                    m_player.WeaponManagement.Next_Weapon(1);
                    return true;
                }
                else if (scroll < 0f)
                {
                    m_player.WeaponManagement.Next_Weapon(-1);
                    return true;
                }
            }

            return false;
        }

        protected void Input_Interaction()
        {
            // �ǽð����� �ٶ󺸰� �ִ� UI�� Ȱ��ȭ
            RaycastHit interactionHit = GameManager.Ins.Start_Raycast(Camera.main.transform.position, Camera.main.transform.forward, 5f, LayerMask.GetMask("Interaction"));
            if (interactionHit.collider == null) // ���� ����� �ݶ��̴� ��ȯ
            {
                Reset_Interaction();
                return;
            }

            Interaction interaction = interactionHit.collider.gameObject.transform.GetComponent<Interaction>();
            if (interaction == null)
                interaction = interactionHit.collider.gameObject.transform.parent.GetComponent<Interaction>();
            if (interaction == null) // �� 2�� Ÿ��
                interaction = interactionHit.collider.gameObject.transform.parent.parent.GetComponent<Interaction>();
            if (interaction == null)
            {
                Reset_Interaction();
                return;
            }

            // ��ȣ�ۿ� ���� �����ΰ�
            if (interaction.Possible == false)
            {
                Reset_Interaction();
                return;
            }

            // ��ȣ�ۿ� ������ �Ÿ��ΰ�(���� ����)
            float distance = Vector3.Distance(m_player.gameObject.transform.position, interaction.gameObject.transform.position);
            if (distance >= interaction.Dist)
            {
                Reset_Interaction();
                return;
            }

            // ���� ���� �ִ°�
            RaycastHit wallHit = GameManager.Ins.Start_Raycast(Camera.main.transform.position, Camera.main.transform.forward, 5f, LayerMask.GetMask("Wall"));
            if (wallHit.collider != null)
            {
                if (wallHit.distance < interactionHit.distance) // ���� ��ȣ�ۿ� ��Һ��� �տ� �ִٸ� ��ȣ�ۿ�X
                {
                    Reset_Interaction();
                    return;
                }
            }

            if (m_interactionUI != null)
            {
                if (m_interactionUI != interaction.InteractionUI)
                    m_interactionUI.gameObject.SetActive(false);
            }

            m_interactionUI = interaction.InteractionUI;
            if (m_interactionUI != null)
            {
                m_interactionUI.Update_Transform();
                m_interactionUI.gameObject.SetActive(true);
            }

            // ��ȣ�ۿ�
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.Ins.Sound.Play_ManagerAudioSource("Horror_ClickInteraction", false, 1f);
                interaction.Click_Interaction();
            }
        }

        private void Reset_Interaction()
        {
            if (m_interactionUI == null)
                return;

            m_interactionUI.gameObject.SetActive(false);
            m_interactionUI = null;
        }

        protected void Check_Stamina()
        {
            if (m_player.Stamina < m_player.StaminaMax)
                m_recoverStamina = true;
            else
                m_recoverStamina = false;
        }

        protected void Recover_Stamina()
        {
            if (m_recoverStamina == false)
                return;

            m_recoverStamina = m_player.Set_Stamina(0.5f * Time.deltaTime);
        }

        protected void Change_Animation(string stateName, bool play = false)
        {
            if (m_animator.gameObject.activeSelf == false)
                return;
            AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
            m_triggerName = Get_AnimationName(stateName); // ���� ���� üũ

            if (stateInfo.IsName(m_triggerName) == true || play == true)
                m_animator.Play(m_triggerName, 0, 0f); // Ʈ������ ���� ����
            else
                m_animator.SetBool(m_triggerName, m_conversion);
        }

        protected string Get_AnimationName(string stateName)
        {
            string weaponName = "Bbaru"; // �⺻ ����

            m_noteItem = m_player.WeaponManagement.Get_CurrentWeaoponType();
            if (m_noteItem != null)
            {
                switch (m_noteItem.m_itemType)
                {
                    case NoteItem.ITEMTYPE.TYPE_PIPE:
                        weaponName = "Bbaru";
                        break;
                    case NoteItem.ITEMTYPE.TYPE_GUN:
                        weaponName = "Gun";
                        break;
                    case NoteItem.ITEMTYPE.TYPE_FLASHLIGHT:
                        weaponName = "Handlight";
                        break;
                }
            }

            m_conversion = true;
            return weaponName + "_" + stateName;
        }

        protected void Reset_Animation()
        {
            if (m_conversion == false)
                return;

            m_conversion = false;
            m_animator.SetBool(m_triggerName, m_conversion);
        }

        protected void Play_WalkSound(ref float soundTime, float retryTime, float speed)
        {
            soundTime += Time.deltaTime;
            if (soundTime >= retryTime)
            {
                soundTime = 0f;

                int Index = Random.Range(0, 18);
                GameManager.Ins.Sound.Play_AudioSource(m_audioSource, "Horror_Player_Walk_" + Index.ToString(), false, speed);
            }
        }

        protected void Check_PreStateWalk(ref bool preWalk)
        {
            if (m_player.StateMachine.PreState == (int)HorrorPlayer.State.ST_WALK) // �ȱ� ����
            {
                preWalk = true;
                Check_Stamina(); // ���׹̳� ȸ�� ���� �Ǻ�
            }
            else // �޸��� ����
            {
                preWalk = false;
            }
        }

        protected void Update_PreStateWalk(ref bool preWalk, ref float soundTime)
        {
            if (Input_Move() == true) // �̵� �Է� ���� ���� ��
            {
                if (preWalk == true) // �ȱ� ����
                {
                    Play_WalkSound(ref soundTime, 0.6f, 1f);
                    Recover_Stamina();
                }
                else // �޸��� ����
                {
                    Play_WalkSound(ref soundTime, 0.4f, 1f);
                    m_player.Set_Stamina(-1f * Time.deltaTime); // ���׹̳� ���

                    if (m_player.Stamina <= 0)
                    {
                        preWalk = true;
                        Check_Stamina();
                    }
                }
            }
        }

        protected void Update_Gravity()
        {
            //RaycastHit ray;
            //if (Physics.Raycast(m_transform.position, Vector3.down, out ray, Mathf.Infinity, LayerMask.GetMask("Ground")))
            //{
            //    m_velocity.y += m_gravity * Time.deltaTime;
            //    m_transform.position += m_velocity * Time.deltaTime;
            //    //Debug.Log("�߷� ������");
            //}
            //else
            //{
            //    m_velocity.y = 0;
            //    //Debug.Log("�ٴ� ������");
            //}
        }
    }
}

