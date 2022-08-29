using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdInitializationController : MonoBehaviour, IUnityAdsInitializationListener
{
	[Header("Ads Scene Properties")]

	[SerializeField] private Button bannerAdSceneButton;

	[SerializeField] private Button interstatialAdSceneButton;

	[SerializeField] private Button rewardedAdSceneButton;

	[SerializeField] private TMP_Text notificationText;

	[SerializeField] private float notificationFadeOutSpeed;

	[Header("Ads Scene IDs")]

	[SerializeField] private string androidId;

	[SerializeField] private string iosId;

	[SerializeField] private bool testMode = true;

	private string gameId;

	#region Unity Methods

	private void Start()
	{
#if UNITY_ANDROID || UNITY_EDITOR
		gameId = androidId;
#elif UNITY_IOS
		gameId=iosId;
#endif
		Debug.Log(gameId);
		Advertisement.Initialize(gameId, testMode,this);
	}

	#endregion


	#region Ads Callbacks
	public void OnInitializationComplete()
	{
		ShowNotificationText("Ad initialization completed");

		bannerAdSceneButton.interactable = true;

		interstatialAdSceneButton.interactable = true;

		rewardedAdSceneButton.interactable = true;

	}

	public void OnInitializationFailed(UnityAdsInitializationError error, string message)
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) 
		{
			StartCoroutine(ShowNotificationText("Internet connection lost"));
			return;
		}


		switch (error)
		{
			case UnityAdsInitializationError.UNKNOWN:
				StartCoroutine(ShowNotificationText("Unknown problem,trying to connect"));
				break;
			case UnityAdsInitializationError.INTERNAL_ERROR:
				StartCoroutine(ShowNotificationText("Internal server error,trying to connect"));
				break;
			case UnityAdsInitializationError.INVALID_ARGUMENT:
				StartCoroutine(ShowNotificationText("Invalid argument,trying to connect"));
				break;
			case UnityAdsInitializationError.AD_BLOCKER_DETECTED:
				StartCoroutine(ShowNotificationText("Ad blocker detected"));
				break;

		}
	}
	#endregion

	#region Private Methods

	IEnumerator ShowNotificationText(string text)
	{
		Color textColor = notificationText.color;

		notificationText.text = text;

		for(float i = 0; i < 1; i += notificationFadeOutSpeed*Time.deltaTime)
		{
			textColor.a = i;

			notificationText.color = textColor;

			yield return new WaitForEndOfFrame();
		}

		for (float i = 1; i >= 0; i -= notificationFadeOutSpeed * Time.deltaTime)
		{
			textColor.a = i;

			notificationText.color = textColor;

			yield return new WaitForEndOfFrame();
		}
	}

	public void LoadSceneButton(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}
	#endregion
}
