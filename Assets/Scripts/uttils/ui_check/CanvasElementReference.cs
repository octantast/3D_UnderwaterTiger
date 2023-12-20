using DG.Tweening;
using UnityEngine;

namespace uttils.ui_check
{
    public class CanvasElementReference : MonoBehaviour
    {
        private Transform textTransform;

        public void ShowAndScale()
        {
            CanvasHelper.FadeCanvasGroup(gameObject, true);
            ScaleElement();
        }

        private void ScaleElement()
        {
            textTransform.localScale = new Vector3(1, 0.8f, 1);
            textTransform.DOScale(Vector3.one, 0.5f);
        }

        private void Awake()
        {

        }

        private void DisableCanvas()
        {
            CanvasHelper.FadeCanvasGroup(gameObject, false);
            FindObjectOfType<CanvasHelper>().ActivateNextCanvasElement();
        }
    }
}

