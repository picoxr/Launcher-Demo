using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class ImageColorTransform : MonoBehaviour
{
    public Image m_box;
    public Image m_box2;
    public ColorTransformData m_hoverData;
    public ColorTransformData m_pressedData;
    public ColorTransformData m_selectData;
    public bool isSelect = false;

    private bool isSelected = false;
    private ColorTransformData m_normalData;

    private Tween m_hoverTween;
    private Tween m_pressedTween;

    private void Awake()
    {
        m_normalData = new ColorTransformData();
        m_normalData.targetColor = this.transform.GetComponent<Image>().color;
        m_normalData.time = 0.1f;
        PUIEventListener.Get(m_box.gameObject).onHover += OnHover;
        PUIEventListener.Get(m_box.gameObject).onPress += OnPress;
        PUIEventListener.Get(m_box.gameObject).onClick += OnClick;
        if( null != m_box2 )
        {
            PUIEventListener.Get(m_box2.gameObject).onHover += OnHover;
            PUIEventListener.Get(m_box2.gameObject).onPress += OnPress;
            PUIEventListener.Get(m_box2.gameObject).onClick += OnClick;
        }
    }

    public virtual void OnHover(GameObject obj, bool isHover)
    {
        m_hoverTween.Kill();
        if (isHover)
        {
            m_hoverTween = this.transform.GetComponent<Image>().DOColor(m_hoverData.targetColor, m_hoverData.time);
            //if (isSelected == false)
            //{
            //    m_hoverTween = this.transform.GetComponent<Image>().DOColor(m_hoverData.targetColor, m_hoverData.time);
            //}
        }
        else
        {
            m_hoverTween = this.transform.GetComponent<Image>().DOColor(m_normalData.targetColor, m_normalData.time);
            //if (isSelected == false)
            //{
            //    m_hoverTween = this.transform.GetComponent<Image>().DOColor(m_normalData.targetColor, m_normalData.time);
            //}
        }
    }

    public virtual void OnPress(GameObject obj, bool isPressed)
    {
      
        m_pressedTween.Kill();
        if (isPressed)
        {
            m_pressedTween = this.transform.GetComponent<Image>().DOColor(m_pressedData.targetColor, m_pressedData.time);
            //if (isSelected == false)
            //{
            //    m_pressedTween = this.transform.GetComponent<Image>().DOColor(m_pressedData.targetColor, m_pressedData.time);
            //}
           
        }
        else
        {
            m_pressedTween = this.transform.GetComponent<Image>().DOColor(m_hoverData.targetColor, m_hoverData.time);
            //if (isSelected == false)
            //{
                
            //}
        }
    }

    public virtual void OnClick(GameObject obj)
    {
        ////m_hoverTween.Kill();
        ////m_pressedTween.Kill();
        //if (isSelect == true)
        //{
        //    if(isSelected == false)
        //    {
        //        this.transform.GetComponent<Image>().color = m_selectData.targetColor;
        //    }
        //    else
        //    {
        //        this.transform.GetComponent<Image>().color = m_normalData.targetColor;
        //    }
        //    isSelected = !isSelected;
        //}
        //else
        //{
        //    //this.transform.GetComponent<Image>().color = m_normalData.targetColor;
        //}
    }
}
