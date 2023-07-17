using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager instance;

#if UNITY_IOS
    private string gameID = "5329694";
    private string rewardPlacementID = "Rewarded_iOS";
    private string interstitialPlacementID = "Interstitial_iOS";

#elif UNITY_ANDROID
    private string gameID = "5329695";
    private string rewardPlacementID = "Rewarded_Android";
    private string interstitialPlacementID = "Interstitial_Android";

#elif UNITY_WEBGL
    private string gameID = "5329695";
    private string rewardPlacementID = "Rewarded_Android";
    private string interstitialPlacementID = "Interstitial_Android";
#endif

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        Advertisement.Initialize(gameID, false, this);
    }

    public void ShowRewardAds()
    {
        Advertisement.Show(rewardPlacementID, this);
    }

    public void ShowInterstitialAds()
    {
        Advertisement.Show(interstitialPlacementID, this);
    }

    #region 初始化
    public void OnInitializationComplete()
    {
        Debug.Log("广告初始化成功");
        Advertisement.Load(rewardPlacementID, this);
        Advertisement.Load(interstitialPlacementID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("广告初始化失败" + message);
    }

    #endregion

    #region 广告加载
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("广告" + placementId + "加载成功");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("广告" + placementId + "加载失败" + message);
    }

    #endregion

    #region 广告显示
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        throw new System.NotImplementedException();
    }
    
    public void OnUnityAdsShowStart(string placementId)
    {
        //停止音乐
        AudioManager.instance.bgmMusic.Stop();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //重新开始游戏
        TransitionManager.instance.Transition("GamePlay");
        AudioManager.instance.bgmMusic.Play();
        Advertisement.Load(rewardPlacementID, this);
        Advertisement.Load(interstitialPlacementID, this);
    }

    #endregion
}
