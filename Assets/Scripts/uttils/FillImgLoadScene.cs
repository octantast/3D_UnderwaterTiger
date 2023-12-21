using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace uttils
{
    public class FillImgLoadScene : MonoBehaviour
    {
        [SerializeField] private Image _imgFill;

        private void Awake()
        {
            _imgFill.DOFillAmount(
                4f, 1f).OnComplete(() =>
            {
                gameObject.SetActive(false);
                //SceneManager.LoadScene("SampleScene");
            });
        }
    }
}