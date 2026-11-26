using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rodak.UpgradeSystem.Demos.Shared
{
    [RequireComponent(typeof(RectTransform))]
    public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Config")]
        [SerializeField] public bool Interactable = true;
        [SerializeField] public List<PointerEventData.InputButton> IgnoredButtons;

        [Header("Holding")]
        [SerializeField] public float HoldStartDuration = 0.05f;
        [SerializeField] public float HoldTriggerInterval = 0;

        private readonly Dictionary<PointerEventData.InputButton, float> pointerDownTime = new();
        private readonly Dictionary<PointerEventData.InputButton, float> pointerHoldTimer = new();

        public event Action<CustomButton, PointerEventData.InputButton> OnClick;
        public event Action<CustomButton, PointerEventData.InputButton> OnHold;

        public event Action<CustomButton> OnEnter;
        public event Action<CustomButton> OnExit;

        private void Awake()
        {
            foreach (PointerEventData.InputButton button in Enum.GetValues(typeof(PointerEventData.InputButton)))
            {
                pointerDownTime.Add(button, -1);
                pointerHoldTimer.Add(button, 0);
            }
        }

        private void Update()
        {
            foreach (PointerEventData.InputButton button in pointerDownTime.Keys)
            {
                if (pointerDownTime[button] == -1) continue;
                float downDuration = Time.time - pointerDownTime[button];
                if (downDuration > HoldStartDuration)
                {
                    if (pointerHoldTimer[button] > 0)
                    {
                        pointerHoldTimer[button] -= Time.deltaTime;
                    }
                    else
                    {
                        OnHold?.Invoke(this, button);
                        pointerHoldTimer[button] = HoldTriggerInterval;
                    }
                }
            }
        }

        public bool IsIgnored(PointerEventData.InputButton button)
        {
            return IgnoredButtons.Contains(button);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsIgnored(eventData.button)) return;
            pointerDownTime[eventData.button] = Time.time;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsIgnored(eventData.button)) return;
            OnEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsIgnored(eventData.button)) return;
            OnExit?.Invoke(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerEventData.InputButton button = eventData.button;

            float downTime = pointerDownTime[button];
            pointerDownTime[button] = -1;
            pointerHoldTimer[button] = 0;

            if (IsIgnored(button) || downTime == -1) return;

            // float downDuration = Time.time - downTime;
            // if (downDuration > HoldStartDuration) return;

            OnClick?.Invoke(this, button);
        }
    }
}