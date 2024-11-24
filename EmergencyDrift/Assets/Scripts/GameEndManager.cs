using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public void WonGame()
    {
        SceneManager.LoadScene("GameWon");
    }

    public void OutOfMoney()
    {
        SceneManager.LoadScene("OutOfMoney");
    }

    public void GotFired()
    {
        SceneManager.LoadScene("GotFired");
    }

    public void YouDied()
    {
        SceneManager.LoadScene("YouDied");
    }
}
