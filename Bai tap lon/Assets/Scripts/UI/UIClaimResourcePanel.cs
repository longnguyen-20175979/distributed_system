using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.UI;

public class UIClaimResourcePanel : BaseBox {

    private static UIClaimResourcePanel instance;

    [SerializeField]
    private GameObject horizontalClaimItems;

    [SerializeField]
    private GameObject claimResourceItemPrefab;
    [SerializeField]
    private GameObject fxConfetti;

    [SoundGroup]
    [SerializeField]
    private string greetingSound;

    [SerializeField] private Text characterQuote;

    private List<GameObject> resourceItemObjects = new List<GameObject>();


    public static UIClaimResourcePanel Setup(params ResourceItem[] items) {
        if (instance == null) {
            instance = Instantiate(Resources.Load<UIClaimResourcePanel>(Constants.PathPrefabs.CLAIM_RESOURCE_BOX));
        }
        instance.Init(items);
        return instance;
    }
    public override void Close() {
        UIDailyRewardPanel daily = FindObjectOfType<UIDailyRewardPanel>();
        if (daily != null)
        {
            daily.Close();
        }
        base.Close();             
    }
    public static UIClaimResourcePanel Setup(List<ResourceItem> items) {
        return Setup(items.ToArray());
    }
    protected override void OnStart() {
        base.OnStart();
        GetComponent<Canvas>().sortingOrder = 30;
    }

    public void Init(ResourceItem[] items) {
        RemovePreviousChildren();
        foreach (ResourceItem item in items) {
            GameObject obj = Instantiate(claimResourceItemPrefab);
            obj.transform.SetParent(horizontalClaimItems.transform, false);
            obj.GetComponent<UIClaimResourceItem>().AssignClaimResourceItem(item);
            resourceItemObjects.Add(obj);
        }
        MasterAudio.PlaySound(Constants.Audio.SOUND_PURCHASE_SUCCESS);
        if (items[0].type == ResourceType.Character)
        {
            characterQuote.gameObject.SetActive(true);
            if (items[0].detail == ResourceDetail.CharacterRussell)
            {
                characterQuote.text = "Follow the wind, but watch your back!";
            }
            else if (items[0].detail == ResourceDetail.CharacterChloe)
            {
                characterQuote.text = "Never underestimate girl's power!";
            }
            else if (items[0].detail == ResourceDetail.CharacterPopeye)
            {
                characterQuote.text = "I never miss my target!";
            }
            else if (items[0].detail == ResourceDetail.CharacterCharlotte)
            {
                characterQuote.text = "Money is nothing to me!";
            }
            else if (items[0].detail == ResourceDetail.CharacterPinkPoppy)
            {
                characterQuote.text = "I love you so much!";
            }
            else if (items[0].detail == ResourceDetail.CharacterBluePoppy)
            {
                characterQuote.text = "Yayyy, i was born to be yours!";
            }
        }
        else characterQuote.gameObject.SetActive(false);
    }

    private void RemovePreviousChildren() {
        horizontalClaimItems.transform.DetachChildren();
        if (resourceItemObjects.Count > 0) {
            foreach (GameObject go in resourceItemObjects) {
                Destroy(go);
            }
            resourceItemObjects.Clear();
        }
    }

    public override void Show() {
        base.Show();
        MasterAudio.PlaySound(greetingSound);
    }
    protected override void OnEnable() {
        base.OnEnable();
        fxConfetti.SetActive(true);
    }
    protected override void OnDisable() {
        base.OnDisable();
        fxConfetti.SetActive(false);
    }
}