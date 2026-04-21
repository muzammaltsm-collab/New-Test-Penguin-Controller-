using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// A class used to control a player in a Runner
/// game. Includes logic for player movement as well as 
/// other gameplay logic.
/// </summary>
public class PlayerController : MonoBehaviour
{
   
    public static PlayerController Instance => s_Instance;
    static PlayerController s_Instance;
    

    Vector3 m_LastPosition;
    float m_StartHeight;

    const float k_MinimumScale = 0.1f;
    static readonly string s_Speed = "Speed";

    enum PlayerSpeedPreset
    {
        Slow,
        Medium,
        Fast,
        Custom
    }

    Transform m_Transform;
    Vector3 m_StartPosition;
    bool m_HasInput;
    public float m_MaxXPosition;
    float m_XPos;
    float m_ZPos;
    public float m_TargetPosition;
    public float m_Speed;
    float m_TargetSpeed;
    Vector3 m_Scale;
    Vector3 m_TargetScale;
    Vector3 m_DefaultScale;

    const float k_HalfWidth = 0.5f;

    /// <summary> The player's root Transform component. </summary>
    public Transform Transform => m_Transform;

    /// <summary> The player's current speed. </summary>
    public float Speed => m_Speed;

    /// <summary> The player's target speed. </summary>
    public float TargetSpeed => m_TargetSpeed;

    /// <summary> The player's minimum possible local scale. </summary>
    public float MinimumScale => k_MinimumScale;

    /// <summary> The player's current local scale. </summary>
    public Vector3 Scale => m_Scale;

    /// <summary> The player's target local scale. </summary>
    public Vector3 TargetScale => m_TargetScale;

    /// <summary> The player's default local scale. </summary>
    public Vector3 DefaultScale => m_DefaultScale;

    /// <summary> The player's default local height. </summary>
    public float StartHeight => m_StartHeight;

    /// <summary> The player's default local height. </summary>
    public float TargetPosition => m_TargetPosition;

    /// <summary> The player's maximum X position. </summary>
    public float MaxXPosition => m_MaxXPosition;

    void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;

        Initialize();
    }

    /// <summary>
    /// Set up all necessary values for the PlayerController.
    /// </summary>
    public void Initialize()
    {
        m_Transform = transform;
        m_StartPosition = m_Transform.position;
        m_DefaultScale = m_Transform.localScale;
        m_Scale = m_DefaultScale;
        m_TargetScale = m_Scale;
    }

    

   
   

    /// <summary>
    /// Reset the player's current speed to their default speed
    
   
    /// <summary>
    /// Reset the player's current speed to their default speed
    /// </summary>
   

    public Transform NewTarget;
    float Yposition = 0;
    /// <summary>
    /// Returns the player's transform component
    /// </summary>
    public Vector3 GetPlayerTop()
    {
        if (NewTarget != null)
        {
            if (Yposition == 0)
            {
                Yposition = NewTarget.position.y;
                //Debug.Log("YpOSITION   >>>>>>>>>>> " + Yposition);
            }
            return new Vector3(NewTarget.position.x, Yposition, NewTarget.position.z) + Vector3.up * (m_StartHeight * m_Scale.y - m_StartHeight);
        }
        else
            return m_Transform.position + Vector3.up * (m_StartHeight * m_Scale.y - m_StartHeight);
    }

    /// <summary>
    /// Sets the target X position of the player
    /// </summary>
    public void SetDeltaPosition(float normalizedDeltaPosition)
    {
        if (m_MaxXPosition == 0.0f)
        {
            Debug.LogError("Player cannot move because SetMaxXPosition has never been called or Level Width is 0. If you are in the LevelEditor scene, ensure a level has been loaded in the LevelEditor Window!");
        }

        float fullWidth = m_MaxXPosition * 2.0f;
        m_TargetPosition = m_TargetPosition + fullWidth * normalizedDeltaPosition;
        m_TargetPosition = Mathf.Clamp(m_TargetPosition, -m_MaxXPosition, m_MaxXPosition);
        m_HasInput = true;
    }

    /// <summary>
    /// Stops player movement  
    /// </summary>
   
    
    /// <summary>
    /// Set the level width to keep the player constrained
    /// </summary>
   

    /// <summary>
    /// Returns player to their starting position
    /// </summary>
   
    void Update()
    {
    }

    bool Approximately(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
    }
}