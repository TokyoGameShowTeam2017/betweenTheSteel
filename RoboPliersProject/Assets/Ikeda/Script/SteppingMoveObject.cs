using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppingMoveObject : MonoBehaviour
{

    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Front,
        Back,

        None
    }
    [SerializeField]
    private Direction m_Direction;

    private bool m_MoveEnd;

    [SerializeField, Tooltip("速さを設定")]
    private float m_Speed;

    [SerializeField, Tooltip("どれくらいポジションを移動させるか")]
    private float m_MovePosition;

    private Vector3 m_StartPosition;

    [SerializeField]
    private GameObject m_Switch;

    private float m_Rate;
    // Use this for initialization
    void Start()
    {
        m_MoveEnd = false;
        m_StartPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        switch (m_Direction)
        {
            case Direction.Up:
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsEnter())
                {
                    if (m_Speed <= 0) m_Speed *= -1;

                    if (m_Rate <= 1) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, m_StartPosition.y + m_MovePosition, transform.localPosition.z), m_Rate);
                }
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsExit())
                {
                    if (m_Speed >= 0) m_Speed *= -1;
                    if (m_Rate >= 0) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, m_StartPosition.y + m_MovePosition, transform.localPosition.z), m_Rate);
                }
                break;

            case Direction.Down:
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsEnter())
                {
                    if (m_Speed <= 0) m_Speed *= -1;

                    if (m_Rate <= 1) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, m_StartPosition.y - m_MovePosition, transform.localPosition.z), m_Rate);
                }
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsExit())
                {
                    if (m_Speed >= 0) m_Speed *= -1;
                    if (m_Rate >= 0) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, m_StartPosition.y - m_MovePosition, transform.localPosition.z), m_Rate);
                }
                break;

            case Direction.Right:
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsEnter())
                {
                    if (m_Speed <= 0) m_Speed *= -1;

                    if (m_Rate <= 1) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(m_StartPosition.x + m_MovePosition, transform.localPosition.y , transform.localPosition.z), m_Rate);
                }
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsExit())
                {
                    if (m_Speed >= 0) m_Speed *= -1;
                    if (m_Rate >= 0) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(m_StartPosition.x + m_MovePosition, transform.localPosition.y, transform.localPosition.z), m_Rate);
                }
                break;

            case Direction.Left:
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsEnter())
                {
                    if (m_Speed <= 0) m_Speed *= -1;

                    if (m_Rate <= 1) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(m_StartPosition.x - m_MovePosition, transform.localPosition.y, transform.localPosition.z), m_Rate);
                }
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsExit())
                {
                    if (m_Speed >= 0) m_Speed *= -1;
                    if (m_Rate >= 0) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(m_StartPosition.x - m_MovePosition, transform.localPosition.y, transform.localPosition.z), m_Rate);
                }
                break;

            case Direction.Front:
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsEnter())
                {
                    if (m_Speed <= 0) m_Speed *= -1;

                    if (m_Rate <= 1) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, m_StartPosition.z + m_MovePosition), m_Rate);
                }
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsExit())
                {
                    if (m_Speed >= 0) m_Speed *= -1;
                    if (m_Rate >= 0) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, m_StartPosition.z + m_MovePosition), m_Rate);
                }
                break;

            case Direction.Back:
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsEnter())
                {
                    if (m_Speed <= 0) m_Speed *= -1;

                    if (m_Rate <= 1) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, m_StartPosition.z - m_MovePosition), m_Rate);
                }
                if (m_Switch.GetComponent<SteppingOnSwitch>().GetIsExit())
                {
                    if (m_Speed >= 0) m_Speed *= -1;
                    if (m_Rate >= 0) m_Rate += m_Speed * Time.deltaTime * 60;
                    transform.localPosition = Vector3.Lerp(m_StartPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, m_StartPosition.z - m_MovePosition), m_Rate);
                }
                break;
        }
    }
}
