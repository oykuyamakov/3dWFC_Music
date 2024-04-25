// using System;
// using System.Collections.Generic;
// using Roro.Scripts.UI.UITemplates;
// using Sirenix.Utilities;
// using UnityCommon.Runtime.UI.Animations;
// using UnityCommon.Variables;
// using UnityEngine;
//
// namespace Roro.Scripts.UI
// {
//     public class HCTopUI : MonoBehaviour
//     {
//
//         [SerializeField]
//         private List<UITemplate<object>> m_Elements;
//         
//         
//         [SerializeField]
//         private BoolVariable m_GameIsRunning;
//
//         private Dictionary<UIElementType, UITemplate<object>> m_TemplatesByTypes =
//             new Dictionary<UIElementType, UITemplate<object>>();
//
//
//         private void Awake()
//         {
//             SetDictionary();
//
//             GetComponentsInChildren<UIAnimation>().ForEach(anim => anim.FadeIn());
//         }
//
//         private void SetDictionary()
//         {
//             for (int i = 0; i < m_Elements.Count; i++)
//             {
//                 if (!m_TemplatesByTypes.ContainsKey(m_Elements[i].GetElementType()))
//                 {
//                     m_TemplatesByTypes.Add(m_Elements[i].GetElementType(), m_Elements[i]);
//                 }
//             }
//         }
//         public bool TrySetTimerText(string time)
//         {
//             return TrySetElementOnType(UIElementType.Timer,time);
//
//         }
//         public bool TrySetLevelText(string level)
//         {
//             return TrySetElementOnType(UIElementType.Level,$"LEVEL {level}");
//         }
//
//         public void TrySetNextButtonAction(Action onNextButton)
//         {
//             m_TemplatesByTypes[UIElementType.Next].Set(onNextButton);
//         }
//         private bool TrySetElementOnType(UIElementType type, object value)
//         {
//             if (!m_TemplatesByTypes.ContainsKey(type)) 
//                 return false;
//             
//             m_TemplatesByTypes[type].Set(value);
//             return true;
//         }
//     }
// }
