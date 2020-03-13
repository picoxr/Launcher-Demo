using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UMovie : MonoBehaviour
{
    public string movieName;
    public List<Sprite> lSprites;
    public float fSep = 0.05f;

    public float showerWidth
    {
        get
        {
            if (shower == null)
            {
                return 0;
            }
            return shower.rectTransform.rect.width;
        }
    }
    public float showerHeight
    {
        get
        {
            if (shower == null)
            {
                return 0;
            }
            return shower.rectTransform.rect.height;
        }
    }

    void Awake()
    {
        shower = GetComponent<Image>();

        if (string.IsNullOrEmpty(movieName))
        {
            movieName = "movieName";
        }
    }
    void Start()
    {
        Play(curFrame);
    }

    public void Play(int iFrame)
    {
        if (iFrame >= FrameCount)
        {
            iFrame = 0;
        }
        shower.sprite = lSprites[iFrame];
        curFrame = iFrame;
//        shower.SetNativeSize();

        if (dMovieEvents.ContainsKey(iFrame))
        {
            foreach (delegateMovieEvent del in dMovieEvents[iFrame])
            {
                del();
            }
        }
    }

    private Image shower;

    int curFrame = 0;
    public int FrameCount
    {
        get
        {
            return lSprites.Count;
        }
    }

    float fDelta = 0;
    void Update()
    {
        fDelta += Time.deltaTime;
        if (fDelta > fSep)
        {
            fDelta = 0;
            curFrame++;
            Play(curFrame);
        }
    }

    public delegate void delegateMovieEvent();
    private Dictionary<int, List<delegateMovieEvent>> dMovieEvents = new Dictionary<int, List<delegateMovieEvent>>();
    public void RegistMovieEvent(int frame, delegateMovieEvent delEvent)
    {
        if (!dMovieEvents.ContainsKey(frame))
        {
            dMovieEvents.Add(frame, new List<delegateMovieEvent>());
        }
        dMovieEvents[frame].Add(delEvent);
    }
    public void UnregistMovieEvent(int frame, delegateMovieEvent delEvent)
    {
        if (!dMovieEvents.ContainsKey(frame))
        {
            return;
        }
        if (dMovieEvents[frame].Contains(delEvent))
        {
            dMovieEvents[frame].Remove(delEvent);
        }
    }
}