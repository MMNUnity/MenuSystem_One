
using UnityEngine;

namespace UnityCore
{
    namespace Menu
    {
        public class TestMenu : MonoBehaviour
        {
            public PageController pageController;

#if UNITY_EDITOR
            private void Update()
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    pageController.TurnPageOn(PageType.Loading);
                }

                if (Input.GetKeyDown(KeyCode.G))
                {
                    pageController.TurnPageOff(PageType.Loading);
                }

                //Animate page completely out before new page loads
                if (Input.GetKeyUp(KeyCode.H))
                {
                    pageController.TurnPageOff(PageType.Loading, PageType.Menu);
                }

                //Animate page out and new page in simultaneously
                if (Input.GetKeyUp(KeyCode.J))
                {
                    pageController.TurnPageOff(PageType.Loading, PageType.Menu, true);
                }

                if (Input.GetKeyUp(KeyCode.P))
                {
                    pageController.TurnPageOff(PageType.Menu, PageType.Loading, true);
                }

            }
#endif
        }
    }
}

