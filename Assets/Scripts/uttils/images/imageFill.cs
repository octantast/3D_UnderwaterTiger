using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace uttils.images
{
    public class imageFill : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private void Start()
        {
            Sequence bcjiAe = DOTween.Sequence();
            bcjiAe.Append(_image.DOFillAmount(0.1f, 3f));
            bcjiAe.Append(_image.DOFillAmount(0.4f, 1.2f));
            bcjiAe.Append(_image.DOFillAmount(0.55f, 0.3f));
            bcjiAe.Append(_image.DOFillAmount(0.8f, 1.2f));
            bcjiAe.Append(_image.DOFillAmount(1f, 0.8f));
        }
    }
}