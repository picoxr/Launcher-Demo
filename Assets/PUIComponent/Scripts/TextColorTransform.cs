using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ColorTransformData
{
    public Color targetColor;
    public float time;
}

public class TextColorTransform : MonoBehaviour
{
    public Image m_box;
    public ColorTransformData m_hoverData;
    public ColorTransformData m_pressedData;
    public ColorTransformData m_selectData;
    public bool isSelect;
    private bool isSelected;

    private ColorTransformData m_normalData;

    private Tween m_hoverTween;
    private Tween m_pressedTween;

    private void Awake()
    {
        m_normalData = new ColorTransformData();
        m_normalData.targetColor = this.transform.GetComponent<Text>().color;
        m_normalData.time = 0.1f;
        PUIEventListener.Get(m_box.gameObject).onHover += OnHover;
        PUIEventListener.Get(m_box.gameObject).onPress += OnPress;
        PUIEventListener.Get(m_box.gameObject).onClick += OnClick;
    }

    public virtual void OnHover(GameObject obj, bool isHover)
    {
        if(isSelected == true)
        {
            return;
        }
        m_hoverTween.Kill();
        if (isHover)
        {
            m_hoverTween = this.transform.GetComponent<Text>().DOColor(m_hoverData.targetColor, m_hoverData.time);
        }
        else if (isSelect == false)
        {
            m_hoverTween = this.transform.GetComponent<Text>().DOColor(m_normalData.targetColor, m_normalData.time);
        }
    }

    public virtual void OnPress(GameObject obj, bool isPressed)
    {
        if (isSelected == true)
        {
            return;
        }
        m_pressedTween.Kill();
        if (isPressed)
        {
            m_pressedTween = this.transform.GetComponent<Text>().DOColor(m_pressedData.targetColor, m_pressedData.time);
        }
        else if(isSelect == false)
        {
            m_pressedTween = this.transform.GetComponent<Text>().DOColor(m_hoverData.targetColor, m_hoverData.time);
        }
    }

    public virtual void OnClick(GameObject obj)
    {
        if (isSelect == true)
        {
            isSelected = true;
            this.transform.GetComponent<Text>().color = m_selectData.targetColor;
        }
        else
        {
            this.transform.GetComponent<Text>().color = m_normalData.targetColor;
        }
    }
}
