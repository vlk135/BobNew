using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	VideoPlayer player;

	[SerializeField]
	AudioSource source;

	[SerializeField]
	GameObject Menu;

	private void Start()
	{
		player.loopPointReached += EndReached;
	}

	public void PlayGame()
	{
		player.Play();
		source.Play();

		Menu.SetActive(false);

		//player.Play();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	void EndReached(UnityEngine.Video.VideoPlayer vp)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}
