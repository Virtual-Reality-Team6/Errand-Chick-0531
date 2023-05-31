using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public GameObject gameCam;
    public GameObject interactionCam;

    public GameObject[] itemObj;
    public int[] itemPrice;
    public string[] talkData;
    public Text talkText;

    Player enterPlayer;
    public float rotationSpeed = 5f;

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
        RotateTowardsPlayer();
        interactionCam.SetActive(true);
        gameCam.SetActive(false);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 targetDirection = enterPlayer.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = targetRotation;
    }

    // Update is called once per frame
    public void Exit()
    {
        uiGroup.anchoredPosition = Vector3.down * 1000;
        interactionCam.SetActive(false);
        gameCam.SetActive(true);
    }

    public void Buy(int index)
    {
        int price = itemPrice[index];
        if(price > enterPlayer.coin){
            StopCoroutine(Talk());
            StartCoroutine(Talk());
            return;
        }

        enterPlayer.coin -= price;
        
    }

    IEnumerator Talk()
    {
        talkText.text = talkData[1];
        yield return new WaitForSeconds(2f);
        talkText.text = talkData[0];
    }
}
