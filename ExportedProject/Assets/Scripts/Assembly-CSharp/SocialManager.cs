using UnityEngine;

public class SocialManager : MonoBehaviour
{
	public string url_RateUsGooglePlay;

	public string url_RateUsAppStore;

	public string url_MoreGamesUsGooglePlay;

	public string url_MoreGamesUsAppStore;

	public string url_JoinUsToFacebook;

	public static SocialManager This { get; private set; }

	private void Awake()
	{
		This = this;
	}
}
