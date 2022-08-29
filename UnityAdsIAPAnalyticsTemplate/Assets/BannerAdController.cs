using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class BannerAdController : MonoBehaviour
{
	[Header("Banner Buttons")]
	[SerializeField] private Button loadBannerButton;

	[SerializeField] private Button showBannerButton;

	[SerializeField] private Button hideBannerButton;

	[Header("Banner Ad properties")]
	[SerializeField] private BannerPosition bannerPosition;

	[SerializeField] private string androidBannerId;

	[SerializeField] private string iosBannerId;

	private string currentPlatformBannerId;

	private void Start()
	{
#if UNITY_ANDROID || UNITY_EDITOR
		currentPlatformBannerId = androidBannerId;
#elif UNITY_IOS
		currentPlatformBannerId=iosBannerId;
#endif
		if (Advertisement.isInitialized)
		{
			loadBannerButton.interactable = true;
		}
		Advertisement.Banner.SetPosition(bannerPosition);
	}

	#region Banner Load Callbacks

	private void OnBannerLoaded()
	{
		showBannerButton.interactable = true;
		hideBannerButton.interactable = true;
	}

	private void OnBannerLoadFailed(string errorMessage)
	{
		Debug.Log(errorMessage);
	}

	#endregion

	#region Banner Show Callbacks

	private void OnBannerClicked()
	{
		print("Banner clicked");
	}
	private void OnBannerHide()
	{
		print("Banner is hide");
	}

	private void OnBannerShow()
	{
		print("Banner is shown");
	}

	#endregion

	#region Public Button Methods

	public void LoadBannerButtonPressed()
	{
		BannerLoadOptions bannerLoadOptions = new BannerLoadOptions
		{
			loadCallback = OnBannerLoaded,
			errorCallback = OnBannerLoadFailed
		};

		Advertisement.Banner.Load(currentPlatformBannerId, bannerLoadOptions);
	}

	public void OnShowBannerButtonPressed()
	{
		BannerOptions bannerOptions = new BannerOptions 
		{
			clickCallback = OnBannerClicked,
			hideCallback = OnBannerHide,
			showCallback = OnBannerShow
		};
		Advertisement.Banner.Show(currentPlatformBannerId,bannerOptions);
	}

	public void OnHideBannerButtonPressed()
	{
		Advertisement.Banner.Hide();
	}
	#endregion
}
