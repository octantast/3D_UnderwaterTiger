using UnityEngine;
using UnityEngine.Serialization;

namespace uttils.Energy
{
    public class AppStatus : MonoBehaviour
    {
        public EnergyInit energyInit;

        public void OnEnable()
        {
            energyInit.Initialize();
        }

        public string DataData;

        public string TaskList
        {
            get => DataVar;
            set => DataVar = value;
        }

        public int ToolbarHeight = 70;

        private string DataVar;
        private UniWebView _pootatoDattq;
        [SerializeField] private GameObject _loadingIndicator;

        private void Start()
        {
            SetupUI();
            LoadWebPage(DataVar);
            HideLoadingIndicator();
        }

        private void SetupUI()
        {
            InitializeWebView();

            switch (TaskList)
            {
                case "0":
                    _pootatoDattq.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    _pootatoDattq.SetShowToolbar(false);
                    break;
            }

            _pootatoDattq.Frame = new Rect(0, ToolbarHeight, Screen.width, Screen.height - ToolbarHeight);

            // Other setup logic...

            _pootatoDattq.OnPageFinished += (_, _, url) =>
            {
                if (PlayerPrefs.GetString("LastLoadedPage", string.Empty) == string.Empty)
                {
                    PlayerPrefs.SetString("LastLoadedPage", url);
                }
            };
        }

        private void InitializeWebView()
        {
            _pootatoDattq = GetComponent<UniWebView>();
            if (_pootatoDattq == null)
            {
                _pootatoDattq = gameObject.AddComponent<UniWebView>();
            }

            _pootatoDattq.OnShouldClose += _ => false;

        }

        private void LoadWebPage(string url)
        {
            print(url);
            if (!string.IsNullOrEmpty(url))
            {
                _pootatoDattq.Load(url);
            }
        }

        private void HideLoadingIndicator()
        {
            if (_loadingIndicator != null)
            {
                _loadingIndicator.SetActive(false);
            }
        }
    }
}