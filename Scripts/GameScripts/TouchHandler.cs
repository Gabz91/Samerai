using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GameScripts;

namespace GenericScripts
{
    public class TouchHandler : MonoBehaviour
    {
        //public LayerMask touchInputMask;

        List<GameObject> touchList = new List<GameObject>();
        GameObject[] touchesOld;
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();

        GraphicRaycaster grRaycaster;

        void Start()
        {
            grRaycaster = GetComponent<GraphicRaycaster>();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                touchesOld = new GameObject[touchList.Count];
                touchList.CopyTo(touchesOld);
                touchList.Clear();


                pointerData.position = Input.mousePosition; // use the position from controller as start of raycast instead of mousePosition.

                grRaycaster.Raycast(pointerData, results);

                if (results.Count > 0)
                {
                    GameObject recipient = results[results.Count - 1].gameObject;
                    ButtonController recipientButton = recipient.GetComponent<ButtonController>();
                    touchList.Add(recipient);
                    if (Input.GetMouseButtonDown(0))
                        recipientButton.GetTouchInput("OnTouchDown");
                    if (Input.GetMouseButtonUp(0))
                        recipientButton.GetTouchInput("OnTouchUp");
                    if (Input.GetMouseButton(0))
                        recipientButton.GetTouchInput("OnTouchStay");
                }
                foreach (GameObject g in touchesOld)
                {
                    g.GetComponent<ButtonController>().GetTouchInput("OnTouchExit");
                }
            }
#endif
            if (Input.touchCount > 0)
            {
                touchesOld = new GameObject[touchList.Count];
                touchList.CopyTo(touchesOld);
                touchList.Clear();
                foreach (Touch touch in Input.touches)
                {
                    pointerData.position = touch.position; // use the position from controller as start of raycast instead of mousePosition.
                    grRaycaster.Raycast(pointerData, results);
                    if (results.Count > 0)
                    {
                        GameObject recipient = results[results.Count - 1].gameObject;
                        ButtonController recipientButton = recipient.GetComponent<ButtonController>();
                        touchList.Add(recipient);

                        if (touch.phase == TouchPhase.Began)
                            recipientButton.GetTouchInput("OnTouchDown");
                        if (touch.phase == TouchPhase.Ended)
                            recipientButton.GetTouchInput("OnTouchUp");
                        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                            recipientButton.GetTouchInput("OnTouchStay");
                        if (touch.phase == TouchPhase.Canceled)
                            recipientButton.GetTouchInput("OnTouchExit");
                    }
                }
                foreach (GameObject g in touchesOld)
                {
                    g.GetComponent<ButtonController>().GetTouchInput("OnTouchExit");
                }
            }
            results.Clear();
        }
    }
}
