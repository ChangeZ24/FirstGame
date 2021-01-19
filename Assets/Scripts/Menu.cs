using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer audioMixer;
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        //弹出暂停菜单
        pauseMenu.SetActive(true);
        //暂停游戏界面（若想做慢动作，将其设置为0.5f）
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        //重启游戏界面
        Time.timeScale = 1f;
    }
    //滑动进度条调节声音
    public void SetVolume(float value)
    {
        audioMixer.SetFloat("MainVolume", value);
    }
}
