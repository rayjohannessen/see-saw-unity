﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    bool m_bHitSpikes, m_bHitWater;
    bool m_bDied;

    public float AdditionalForceTime = 1.0f;
    public float ClimbForce = 50.0f;
    public float BoostForce = 300.0f;
    public float BoostDuration = 0.5f;
    public float HandOffset = 3.0f;

    float m_fBoostTimer;
    float m_fAddForceTimer;
    // need some leeway for resetting the player if they're not moving fast
    // this is used so no reset occurs immediately after launch
    float m_fResetableTimer;    

    Vector2 m_vDefaultForceCharControl = new Vector2(200, 200);

    // when the player's velocity becomes lower than this
    // the seesaw is moved to that location if they have enough health
    public Vector2 ResetVelocityThreshold = new Vector2(15.0f, 5.0f);

    public Vector3 AdditionalForceOnLaunch = new Vector2(100, 0);

    Vector3 m_vOrigPos;

    Quaternion m_vOrigRot;

    GUIText m_DiedTxt;

    Trampoline_OneWay m_HitTramp;



    void Start()
    {
        m_fBoostTimer = m_fResetableTimer = m_fAddForceTimer = 0.0f;
    }

    void Update()
    {

        if (Game.Instance.LaunchStarted)
        {
// 	        if (Input.GetButton("Character Control Up"))
// 	        {
// 	            rigidbody.AddForce(m_vDefaultForceCharControl.x * 0.0f, m_vDefaultForceCharControl.y, 0.0f);
// 	        }
// 	        else
//             if (Input.GetButton("Character Control Down"))
// 	        {
// 	            rigidbody.AddForce(m_vDefaultForceCharControl.x * 0.0f, -m_vDefaultForceCharControl.y, 0.0f);
// 	        }
// 	        else 
            if (Input.GetButton("Character Control Right"))
	        {
	            rigidbody.AddForce(m_vDefaultForceCharControl.x, m_vDefaultForceCharControl.y * 0.0f, 0.0f);
	        }
	        else if (Input.GetButton("Character Control Left"))
	        {
	            rigidbody.AddForce(-m_vDefaultForceCharControl.x, m_vDefaultForceCharControl.y * 0.0f, 0.0f);
            }

//             if (m_fResetableTimer < 0.0f && !m_bHealthDecremented && !m_bDied &&
//                 Mathf.Abs(rigidbody.velocity.x) < ResetVelocityThreshold.x && 
//                 Mathf.Abs(rigidbody.velocity.y) < ResetVelocityThreshold.y)
//             {
//                 m_bHealthDecremented = true;
//                 rigidbody.Sleep();
//                 // TODO:: check if the see-saw can be moved to new location, or if it's a complete restart
//                 Game.Instance.PlayerInfo.Health--;
//                 if (Game.Instance.PlayerInfo.Health > 0)
//                 {
//                     Game.Instance.CurrLevel.ResetLevel(false, true);
//                 }
//                 else
//                 {
//                     Game.Instance.OnCharacterDied();
//                 }
//             }
            m_fResetableTimer -= Time.deltaTime;
            m_fAddForceTimer -= Time.deltaTime;
            m_fBoostTimer -= Time.deltaTime;
        } 
        else
        {
        }
    }

    void FixedUpdate()
    {
        if (m_HitTramp != null && m_HitTramp.ContinueToApplyForce())
        {
            //Debug.Log("Player Vel:" + rigidbody.velocity.ToString());
            // slow down movement
            rigidbody.AddForce(m_HitTramp.Force);
        }
        if (m_bDied/*m_bHitSpikes*/)
        {
            // TODO:: develop to work with ragdoll, etc
            rigidbody.Sleep();
        }
        if (m_fAddForceTimer > 0.0f)
            rigidbody.AddForce(AdditionalForceOnLaunch);
        if (Input.GetButtonDown("Action Btn 1"))
            m_fBoostTimer = BoostDuration;
        if (m_fBoostTimer > 0.0f)
            rigidbody.AddForce(Vector3.up * BoostForce);
    }

    void OnGUI()
    {
    }

    public void OnReset()
    {
        if (hingeJoint != null)
            Destroy(hingeJoint);
        m_vOrigPos = Game.Instance.CurrLevel.GetPlayerPlacement();
        m_fAddForceTimer = 0.0f;
        m_fResetableTimer = 1.0f;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        rigidbody.isKinematic = true;
        //rigidbody.Sleep();
        //rigidbody.velocity = Vector3.zero;
        m_bHitSpikes = m_bHitWater = m_bDied = false;
        m_DiedTxt.enabled = false;
        rigidbody.transform.rotation = m_vOrigRot;
        rigidbody.transform.position = m_vOrigPos;
    }

    public void OnLaunchStarted()
    {
        m_fAddForceTimer = AdditionalForceTime;
        rigidbody.isKinematic = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX;
        rigidbody.freezeRotation = true;
    }
    public void OnTrampEnter(Trampoline_OneWay _tramp)
    {
        m_HitTramp = _tramp;
    }
    public void OnHitSpikes()
    {
        m_bHitSpikes = m_bDied = true;
        m_DiedTxt.enabled = true;
    }
    public void OnHitWater()
    {
        m_bHitWater = m_bDied = true;
        m_DiedTxt.enabled = true;
    }

    public void Init()
    {
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        m_vOrigRot = rigidbody.rotation;
        //m_vOrigPos = rigidbody.position;

        m_DiedTxt = GameObject.Find("OnDeath").guiText;
        m_DiedTxt.enabled = m_bHitSpikes = m_bHitWater = m_bDied = false;

        OnReset();
    }

    /// <summary>
    /// ACCESSORS/MUTATORS
    /// </summary>
    /// 
}