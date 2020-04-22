using System.Collections;
using UnityEngine;

namespace UnityCore
{
    namespace Menu
    {
        //** Handles animations and state logic of pages **//
        public class Page : MonoBehaviour
        {
            public static readonly string FLAG_ON = "On";
            public static readonly string FLAG_OFF = "Off";
            public static readonly string FLAG_NONE = "None";

            public PageType type;
            public bool debug;
            public bool useAnimation = false;
            public string targetState { get; private set; }


            //TODO Look at replacing with Tween
            private Animator m_Animator;

            private bool m_IsOn;
            public bool isOn
            {
                get { return m_IsOn; }
                private set { m_IsOn = value; }
            }

            #region Unity Functions
            private void OnEnable()
            {
                CheckAnimatorIntegrity();
            }
            #endregion

            #region Public Functions
            public void Animate(bool _on)
            {
                if (useAnimation)
                {
                    m_Animator.SetBool("on", _on);

                    StopCoroutine("AwaitAnimation");
                    StartCoroutine("AwaitAnimation", _on);
                }
                else 
                {
                    if (!_on)
                    {
                        gameObject.SetActive(false);
                        isOn = false;
                    }
                    else 
                    { 
                        isOn = true;
                    }
                }
            }
            #endregion

            #region Private Functions
            private IEnumerator AwaitAnimation(bool _On)
            {
                targetState = _On ? FLAG_ON : FLAG_OFF;

                // Wait for animator to reach target state
                while(!m_Animator.GetCurrentAnimatorStateInfo(0).IsName(targetState))
                {
                    yield return null;
                }

                //Wait for animator to finish animating
                while (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    yield return null;
                }

                targetState = FLAG_NONE;

                Log("Page: ["+type+"] finished transition to "+ (_On ? "on" : "off"));

                if (!_On)
                {
                    isOn = false;
                    gameObject.SetActive(false);

                }
                else 
                {
                    isOn = true; 
                }
            }

            private void CheckAnimatorIntegrity()
            {
                if (useAnimation)
                {
                    m_Animator = GetComponent<Animator>();
                    if (!m_Animator)
                    {
                        LogWarning("Trying to animate page of type [" + type + "], but no animator exists on object");
                    }
                }
            }

            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[Page ]: " + _msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[Page ]: " + _msg);
            }
            #endregion
        }
    }
}

