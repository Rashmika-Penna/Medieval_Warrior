using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Scene_Management
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvas_grp;
        Coroutine active_fade_coroutine = null;

        private void Start()
        {
            canvas_grp = GetComponent<CanvasGroup>();
        }

        public void Immediate_Fade_Out()
        {
            canvas_grp.alpha = 1;
        }

        //public IEnumerator Fade_Coroutine(float alpha_value, float time)
        //{
        //    while (!Mathf.Approximately(canvas_grp.alpha, alpha_value))
        //    {
        //        canvas_grp.alpha += Mathf.MoveTowards(canvas_grp.alpha, alpha_value, Time.deltaTime / time);
        //        yield return null;
        //    }
        //}

        //public Coroutine Fade(float alpha_value, float time)
        //{
        //    if (active_fade_coroutine != null)
        //    {
        //        StopCoroutine(active_fade_coroutine);
        //    }

        //    active_fade_coroutine = StartCoroutine(Fade_Coroutine(alpha_value, time));
        //    return active_fade_coroutine;
        //}

        //public Coroutine Fade_Out(float time)
        //{
        //    return Fade(1, time);
        //}       

        //public Coroutine Fade_In(float time)
        //{
        //    return Fade(0, time);
        //}

        //***********************************************

        //public IEnumerator Fade_Out(float time)
        //{
        //    while (canvas_grp.alpha < 1)
        //    {
        //        canvas_grp.alpha += Time.deltaTime / time;
        //        yield return null;
        //    }
        //}

        //public IEnumerator Fade_In(float time)
        //{
        //    while (canvas_grp.alpha > 0)
        //    {
        //        canvas_grp.alpha -= Time.deltaTime / time;
        //        yield return null;
        //    }
        //}

        private IEnumerator Fade_Coroutine(float alpha_value, float time)
        {
            while (!Mathf.Approximately(canvas_grp.alpha, alpha_value))
            {
                canvas_grp.alpha = Mathf.MoveTowards(canvas_grp.alpha, alpha_value, Time.deltaTime / time);
                yield return null;
            }
        }

        public Coroutine Fade(float alpha_value, float time)
        {
            if (active_fade_coroutine != null)
            {
                StopCoroutine(active_fade_coroutine);
            }

            active_fade_coroutine = StartCoroutine(Fade_Coroutine(alpha_value, time));
            return active_fade_coroutine;
        }

        public Coroutine Fade_Out(float time)
        {
            return Fade(1, time);
        }        

        public Coroutine Fade_In(float time)
        {
            return Fade(0, time);
        }

    }
}