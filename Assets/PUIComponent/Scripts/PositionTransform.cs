using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public struct TransformData
{
    public Vector3 target;
    public float time;
}

[System.Serializable]
public enum PositionMoveType
{
    PositionMoveType_X,
    PositionMoveType_Y,
    PositionMoveType_Z,
    PositionMoveType_Vector3,
}

public class PositionTransform:MonoBehaviour
{
    public Image m_box;
    public Image m_box2;
    public PositionMoveType m_type = PositionMoveType.PositionMoveType_Z;
    public TransformData m_hoverData;
    public TransformData m_pressedData;
    public TransformData m_selectData;
    public bool isSelect;

    private bool isSelected = false;

    private TransformData m_normalData;

    private Tween m_hoverTween;
    private Tween m_pressedTween;

    private void Awake()
    {
        m_normalData = new TransformData();
        m_normalData.target = this.transform.localPosition;
        m_normalData.time = 0.1f;
        PUIEventListener.Get(m_box.gameObject).onHover += OnHover;
//        PUIEventListener.Get(m_box.gameObject).onPress += OnPress;
        PUIEventListener.Get(m_box.gameObject).onClick += OnClick;
        if( null != m_box2)
        {
            PUIEventListener.Get(m_box2.gameObject).onHover += OnHover;
            PUIEventListener.Get(m_box2.gameObject).onPress += OnPress;
            PUIEventListener.Get(m_box2.gameObject).onClick += OnClick;
        }

		Debug.Log (">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>Awake");

    }

    public virtual void OnHover(GameObject obj, bool isHover)
    {
        if (isSelected == true)
        {
            return;
        }
		Debug.Log (">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>OnHover---"+isHover);
        m_hoverTween.Kill();
        if (isHover)
        {
            if(m_type == PositionMoveType.PositionMoveType_Z)
            {
                m_hoverTween = this.transform.DOLocalMoveZ(m_hoverData.target.z, m_hoverData.time);
            }
            else if(m_type == PositionMoveType.PositionMoveType_X)
            {
                m_hoverTween = this.transform.DOLocalMoveX(m_hoverData.target.x, m_hoverData.time);
            }
            else if (m_type == PositionMoveType.PositionMoveType_Y)
            {
                m_hoverTween = this.transform.DOLocalMoveY(m_hoverData.target.y, m_hoverData.time);
            }
            else
            {
                m_hoverTween = this.transform.DOLocalMove(m_hoverData.target, m_hoverData.time);
            }
        }
        else if (isSelect == false)
        {
            if (m_type == PositionMoveType.PositionMoveType_Z)
            {
                m_hoverTween = this.transform.DOLocalMoveZ(m_normalData.target.z, m_hoverData.time);
            }
            else if (m_type == PositionMoveType.PositionMoveType_X)
            {
                m_hoverTween = this.transform.DOLocalMoveX(m_normalData.target.x, m_hoverData.time);
            }
            else if (m_type == PositionMoveType.PositionMoveType_Y)
            {
                m_hoverTween = this.transform.DOLocalMoveY(m_normalData.target.y, m_hoverData.time);
            }
            else
            {
                m_hoverTween = this.transform.DOLocalMove(m_normalData.target, m_hoverData.time);
            }
        }
    }

    public virtual void OnHoverEnter(GameObject obj)
    {
        m_hoverTween.Kill();
        m_hoverTween = this.transform.DOLocalMove(m_hoverData.target, m_hoverData.time);
    }

    public virtual void OnPress(GameObject obj, bool isPressed)
    {
        if (isSelected == true)
        {
            return;
        }
		Debug.Log (">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>OnPress---"+isPressed);
        if (isPressed)
        {
            if (m_type == PositionMoveType.PositionMoveType_Z)
            {
				Debug.Log ("111111111111111111111111111111");
                m_pressedTween = this.transform.DOLocalMoveZ(m_pressedData.target.z, m_pressedData.time);
            }
            else if(m_type == PositionMoveType.PositionMoveType_X)
            {
                m_pressedTween = this.transform.DOLocalMoveX(m_pressedData.target.x, m_pressedData.time);
            }
            else if (m_type == PositionMoveType.PositionMoveType_Y)
            {
                m_pressedTween = this.transform.DOLocalMoveY(m_pressedData.target.y, m_pressedData.time);
            }
            else
            {
                m_pressedTween = this.transform.DOLocalMove(m_pressedData.target, m_pressedData.time);
            }
        }
        else if (isSelect == false)
        {
            if (m_type == PositionMoveType.PositionMoveType_Z)
            {
				Debug.Log ("22222222222222222222222222222222");
                m_pressedTween = this.transform.DOLocalMoveZ(m_hoverData.target.z, m_pressedData.time);
            }
            else if (m_type == PositionMoveType.PositionMoveType_X)
            {
                m_pressedTween = this.transform.DOLocalMoveZ(m_hoverData.target.x, m_pressedData.time);
            }
            else if (m_type == PositionMoveType.PositionMoveType_Y)
            {
                m_pressedTween = this.transform.DOLocalMoveY(m_hoverData.target.y, m_pressedData.time);
            }
            else
            {
                m_pressedTween = this.transform.DOLocalMove(m_hoverData.target, m_pressedData.time);
            }
        }
    }

    public virtual void OnClick(GameObject obj)
    {
		Debug.Log (">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>OnClick");
        if (isSelect == true)
        {
            isSelected = true;
            this.transform.localPosition = m_selectData.target;
        }
        else
        {
            this.transform.localPosition = m_normalData.target;
        }
    }
}
