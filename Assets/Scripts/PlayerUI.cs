using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour, IPlayerUI {

    public Text points;

    public void RefreshUI(IPlayer player, bool playAnim = false)
    {
        points.text = player.GetPoints();

        if (playAnim)
        {
            points.GetComponent<Animation>().Play();
        }
    }
}

public interface IPlayerUI
{
    void RefreshUI(IPlayer player, bool playAnim = false);
}
