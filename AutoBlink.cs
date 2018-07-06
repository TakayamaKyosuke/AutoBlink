using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBlink : MonoBehaviour
{

    [System.Serializable]
    public class BlendShapes
    {
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public int index;
        public int minBlend = 0;
        public int maxBlend = 100;
    }

    public BlendShapes[] blendShapes;
    [Header("Float")]
    public float blendTime = 0.2f;
    public float interval = 6.0f;

    private float m_elapsedTime = 0.0f;
    private float m_deltaTime = 0.0f;
    private bool isRunning = false;


    // Use this for initialization
    void Start()
    {
        if (blendShapes.Length == 0)
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_elapsedTime += Time.deltaTime;

        if(m_elapsedTime>interval){
            Blink(blendTime);
            m_elapsedTime = 0;
        }

    }

    private void Blink(float time = 2.0f)
    {
        StartCoroutine(Blinking(time));
    }


    private IEnumerator Blinking(float time)
    {

        if (isRunning)
        {
            yield break;
        }

        isRunning = true;


        m_deltaTime = 0;                      // 経過時間のリセット

        float halfTime = time / 2;

        while (m_deltaTime <= halfTime)
        {
            m_deltaTime += Time.deltaTime;

            foreach (BlendShapes bs in blendShapes)
            {
                float value = Mathf.Lerp(bs.minBlend, bs.maxBlend, m_deltaTime / halfTime);
                bs.skinnedMeshRenderer.SetBlendShapeWeight(bs.index, value);
            }

            yield return 0;

        }

        m_deltaTime = 0;                      // 経過時間のリセット

        while (m_deltaTime <= halfTime)
        {
            m_deltaTime += Time.deltaTime;

            foreach (BlendShapes bs in blendShapes)
            {
                float value = Mathf.Lerp(bs.maxBlend, bs.minBlend, m_deltaTime / halfTime);
                bs.skinnedMeshRenderer.SetBlendShapeWeight(bs.index, value);
            }

            yield return 0;

        }


        isRunning = false;

    }

}
