using System;
using System.Collections.Generic;
using System.Linq;
using Based.ResourceManagement;
using DG.Tweening;
using Roro.Scripts.UI;
using UnityCommon.Modules;
using UnityEngine;
using UnityEngine.UI;

namespace Roro.Scripts.Helpers
{
    public class ItemAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image m_Image;

        [SerializeField]
        private Sprite m_ResourceSprite;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private UIImageAnimator m_False;
        [SerializeField]
        private UIImageAnimator m_Semi;
        [SerializeField]
        private UIImageAnimator m_Correct;
        
        private static Dictionary<ResourceType, Stack<Image>> m_ResourceImagesbyTypes = 
            new Dictionary<ResourceType, Stack<Image>>();
        
        private static int m_ImageCount = 25;

        private static Stack<UIImageAnimator> m_CorrectSpriteSheet = new Stack<UIImageAnimator>();
        private static Stack<UIImageAnimator> m_FalseSpriteSheet = new Stack<UIImageAnimator>();
        private static Stack<UIImageAnimator> m_SemiSpriteSheet = new Stack<UIImageAnimator>();

        private static Sprite ResourceSprite;

        private bool m_IsInitialized = false;
        
        
        private void Start()
        {
            if(m_IsInitialized)
                return;
            
            InitializeImages();
        }

        private void InitializeImages()
        {
            var types = (ResourceType[])Enum.GetValues(typeof(ResourceType));
            
            ResourceSprite  = m_ResourceSprite;
            
            for (int i = 0; i < 10; i++)
            {
                var image = Instantiate(m_Correct,canvas.transform);
                image.DisableSelf();
                m_CorrectSpriteSheet.Push(image);
            }
            for (int i = 0; i < 10; i++)
            {
                var image = Instantiate(m_Semi,canvas.transform);
                image.DisableSelf();
                m_SemiSpriteSheet.Push(image);
            }
            for (int i = 0; i < 10; i++)
            {
                var image = Instantiate(m_False,canvas.transform);
                image.DisableSelf();
                m_FalseSpriteSheet.Push(image);
            }

            for (int i = 0; i < types.Length; i++)
            {
                m_ResourceImagesbyTypes.Add(types[i], new Stack<Image>());
                for (int j = 0; j < 40; j++)
                {
                    var spr = types[i] switch
                    {
                        ResourceType.Gold => ResourceSprite,
                        ResourceType.None => ResourceSprite,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    
                    var image = Instantiate(m_Image,canvas.transform);
                    image.enabled = false;
                    image.sprite = spr;
                    if (types[i] == ResourceType.Gold)
                        image.GetComponent<RectTransform>().sizeDelta *= 0.5f;
                    m_ResourceImagesbyTypes[types[i]].Push(image);
                }
            }

            m_IsInitialized = true;
        }
        
        public static void MoveWorldFromTo(ResourceType type, Vector3 initPos, Vector3 wordlPos, Action onComplete = null, Sprite spr = null)
        {
            if (!m_ResourceImagesbyTypes[type].Any())
            {
                onComplete?.Invoke();
                return;
            }

            var finalPos = Camera.main.WorldToScreenPoint(wordlPos);

            var image = m_ResourceImagesbyTypes[type].Pop();
            var t = image.transform;
            t.localScale = Vector3.one / 3;
            t.position = initPos;
            image.enabled = true;

            if (type == ResourceType.None)
                image.sprite = spr;

            var seq = DOTween.Sequence().Append(image.transform.DOMove(finalPos, 1.7f).SetEase(Ease.InOutQuad))
                .Join(image.transform.DOScale(Vector3.zero, 2.7f)).OnComplete(() =>
                {
                    image.transform.localScale = Vector3.one;
                    image.enabled = false;
                    m_ResourceImagesbyTypes[type].Push(image);
                    onComplete?.Invoke();
                });
        }
        public static void MoveFromToBar(ResourceType type, Vector3 initPos, Vector3 finalPos, int count = 3,  Action onComplete = null, Sprite spr = null)
        {
            if (!m_ResourceImagesbyTypes[type].Any())
            {
                onComplete?.Invoke();
                return;
            }

            var animCount = Math.Min(count, 3);
            
            for (int i = 0; i < animCount; i++)
            {
                var image = m_ResourceImagesbyTypes[type].Pop();
                var t = image.transform;
                t.localScale = Vector3.one / 1.5f;
                t.position = initPos;
                image.enabled = true;
                
                if (type == ResourceType.None)
                    image.sprite = spr;

                var i1 = i;
                var seq = DOTween.Sequence().Append(image.transform.DOMove(finalPos, 1f)).SetDelay(i1 * .1f).SetEase(Ease.InQuad)
                    .Join(image.transform.DOScale(Vector3.one / 2f, 1f)).OnComplete(() =>
                    {
                        image.transform.localScale = Vector3.one;
                        image.enabled = false;
                        m_ResourceImagesbyTypes[type].Push(image);
                        
                        if (i1 != m_ImageCount - 1)
                            return;
                            
                        onComplete?.Invoke();
                    });
            }
        }
        public static void AnimateSpriteSheet(int index, Vector3 pos)
        {
            switch (index)
            {
                case 2:
                    AnimateSpriteSheet(m_CorrectSpriteSheet,pos);
                    break;
                case 1:
                    AnimateSpriteSheet(m_SemiSpriteSheet,pos);
                    break;
                case 0:
                    AnimateSpriteSheet(m_FalseSpriteSheet, pos);
                    break;
            }
        }

        public static void AnimateSpriteSheet(Stack<UIImageAnimator> stack, Vector3 pos)
        {
            var s = stack.Pop();
            s.EnableSelf();
            s.transform.position = pos;
            Conditional.For(1f).Do(s.LoopAnim).OnComplete(() =>
            {
                stack.Push(s);
            });
        }

        public static void AnimateSpiral(Sprite sp, Vector3 initPos, Vector3 finalPos, Action onComplete = null, int count = 5)
        {
            AnimateSpiral(ResourceType.None,initPos,finalPos, onComplete,count, sp);
        } 
        public static void RevealSpiral(Sprite sp, Vector3 initPos, Action onComplete = null, float dur = 1.5f)
        {
            RevealSpiral(ResourceType.None,initPos,dur, onComplete, sp);
        }
        
        private static void RevealSpiral(ResourceType type, Vector3 pos, float dur = 10f, Action onComplete = null, Sprite spr = null){
            if (!m_ResourceImagesbyTypes[type].Any())
            {
                onComplete?.Invoke();
                return;
            }

            var im = m_ResourceImagesbyTypes[type].Pop();
            im.enabled = true;
            im.transform.position = pos;
            if (spr != null)
                im.sprite = spr;

            im.fillMethod = Image.FillMethod.Radial360;
            im.type = Image.Type.Filled;
            im.fillAmount = 0;
            DOTween.To((val) => im.fillAmount = val, 0f, 18f, dur).OnComplete((() =>
            {
                m_ResourceImagesbyTypes[type].Push(im);
                im.enabled = false;
                im.transform.localScale = Vector3.one;
                im.type = Image.Type.Simple;
                im.fillAmount = 1;
            }));
        }
        
        private static void AnimateSpiral(ResourceType rType, Vector3 initPos, Vector3 finalPos, Action onComplete = null, int count = 5, Sprite spr = null)
        {
            var angle = 0;
            var increment = 360 / count;
            for (int i = 0; i < count; i++)
            {
                if (!m_ResourceImagesbyTypes[rType].Any())
                {
                    onComplete?.Invoke();
                    return;
                }
                
                var image = m_ResourceImagesbyTypes[rType].Pop();
                image.enabled = true;
                image.transform.position = initPos;
                if (spr != null)
                    image.sprite = spr;
                
                var dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
                var i1 = i;
                
                var seq = DOTween.Sequence()
                    .Append(image.transform.DOMove(initPos + dir * 50, .23f).SetEase(Ease.OutBack))
                    .Join(image.transform.DOScale(Vector3.one / 2f, .3f).SetLoops(1,LoopType.Yoyo))
                    .Append(image.transform.DOMove(finalPos, .5f).SetDelay(.0771f * i).SetEase(Ease.InOutQuad)).OnComplete(
                        () =>
                        {
                            image.enabled = false;
                            image.transform.localScale = Vector3.one;
                            m_ResourceImagesbyTypes[rType].Push(image);
                            
                            if (i1 != count - 1)
                                return;
                            
                            onComplete?.Invoke();
                            
                        }).Join(image.transform.DOScale(Vector3.one / 3.5f, .2f));

                angle += increment;
            }
        }

        public static void AnimateTypeSpiral(ResourceType type, Vector3 initPos, Vector3 finalPos, Action onComplete = null)
        {
            AnimateSpiral(type, initPos, finalPos, onComplete, 5);
        } 
        public static void AnimateTypeSpiralFromMiddle(ResourceType type, Vector3 finalPos, Action onComplete = null)
        {
            var initPos = new Vector2(Screen.width/2f, Screen.height/1.7f);
            
            AnimateTypeSpiral(type,initPos,finalPos,onComplete);
        }
    }
}
