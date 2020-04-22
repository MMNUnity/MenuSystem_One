using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityCore
{
    namespace Menu
    {
        public class PageController : MonoBehaviour
        {

            public static PageController instance;

            public bool debug;
            public PageType entryPage;
            public Page[] pages;

            private Hashtable m_Pages;

#region Unity Functions
            private void Awake()
            {
                if (!instance)
                {
                    instance = this;
                    m_Pages = new Hashtable();
                    RegisterAllPages();

                    if (entryPage != PageType.None)
                    {
                        TurnPageOn(entryPage);
                    }

                    DontDestroyOnLoad(gameObject);
                }

                else 
                {
                    Destroy(gameObject);
                }
            }

#endregion

#region Public Functions
            public void TurnPageOn(PageType _type)
            {
                if (_type == PageType.None) return;

                if (!PageExists(_type))
                {
                    LogWarning("Trying to turn on a page ["+_type+"] that does not exist");
                    return;
                }

                Page _page = GetPage(_type);
                _page.gameObject.SetActive(true);
                _page.Animate(true);
            }

            public void TurnPageOff(PageType _typeOff, PageType _on = PageType.None, bool _waitForExit = false)
            {
                if (_typeOff == PageType.None) return;
                if (!PageExists(_typeOff))
                {
                    LogWarning("Trying to turn off a page [" + _typeOff + "] that does not exist");
                    return;
                }

                Page _offPage = GetPage(_typeOff);
                if (_offPage.gameObject.activeSelf)
                {
                    _offPage.Animate(false);
                }

                if (_on != PageType.None)
                {
                    Page _onPage = GetPage(_on);
                    if (_waitForExit)
                    {
                        StopCoroutine("WaitForPageExit");
                        StartCoroutine(WaitForPageExit(_onPage, _offPage));
                    }
                    else
                    {
                        TurnPageOn(_on);
                    }
                }

            }

            public bool PageIsOn(PageType _type)
            {
                if (!PageExists(_type))
                {
                    LogWarning("You are trying to detect if a page is on [" + _type + "], but it has not been registered ");
                    return false;
                }

                return GetPage(_type).isOn;
            }
            #endregion

            #region Private Functions

            //Wait for 'off' page to finish turn 'off' before turning new page 'on'
            private IEnumerator WaitForPageExit(Page _on, Page _off)
            {
                while (_off.targetState != Page.FLAG_NONE)
                {
                    yield return null;
                }

                TurnPageOn(_on.type);
            }

            private void RegisterAllPages()
            {
                foreach (Page _page in pages)
                {
                    RegisterPage(_page);
                }
            }

            private void RegisterPage(Page _page)
            {
                if (PageExists(_page.type))
                {
                    LogWarning("Trying to register [" + _page.type + "] that has already registered page" + _page.gameObject);
                    return;
                }

                m_Pages.Add(_page.type, _page);

                Log("Registered new page ["+_page.type+"]");
            }

            private Page GetPage(PageType _type)
            {
                if (!PageExists(_type))
                {
                    LogWarning("Trying to get a page ["+_type+"] that has not been registered");
                    return null;
                }

                return (Page)m_Pages[_type];
            }

            private bool PageExists(PageType _type)
            {
                return m_Pages.ContainsKey(_type);
            }

            private void Log(string _msg)
            {
                if (!debug) return;
                Debug.Log("[Page Controller]: " + _msg);
            }

            private void LogWarning(string _msg)
            {
                if (!debug) return;
                Debug.LogWarning("[Page Controller]: " + _msg);
            }
            #endregion
        }

    }
}
