using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public void WonGame(Component sender, object obj)
    {
        SceneManager.LoadScene("GameWon");
    }

    public void OutOfMoney(Component sender, object obj)
    {
        SceneManager.LoadScene("OutOfMoney");
    }

    public void GotFired(Component sender, object obj)
    {
        SceneManager.LoadScene("GotFired");
    }

    public void YouDied(Component sender, object obj)
    {
        SceneManager.LoadScene("YouDied");
    }
}
