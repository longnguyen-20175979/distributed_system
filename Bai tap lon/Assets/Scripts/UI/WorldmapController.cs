using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldmapController : MonoBehaviour
{
    [SerializeField] private ElementLevel preEvenLevel;
    [SerializeField] private ElementLevel preOddLevel;
    [SerializeField] private Transform contentTheme;
    [SerializeField] private Transform posPlayer;
    List<Transform> levelTransform;
    private void Start()
    {
        InitTheme();
        LocateCurrentPlayer();
        //GameData.LevelUnlock = 22;
    }
    void InitTheme()
    {
        levelTransform = new List<Transform>();
        float maxLevel = Constants.MAX_LEVEL - 1;
        float levelInWorld = Constants.LEVEL_IN_WORLD;
        int totalTheme = (int)(maxLevel / levelInWorld);
        for (int i = 0; i <= totalTheme; i++)
        {
            GameObject theme = Instantiate(Resources.Load("Theme/Theme" + (i + 1).ToString()) as GameObject, contentTheme);
            int finishLevel = 0;
            if (maxLevel <= levelInWorld)
            {
                finishLevel = (int)maxLevel;
            }
            if (maxLevel > levelInWorld)
            {
                if (i < totalTheme)
                {
                    finishLevel = (int)levelInWorld * (i + 1) - 1;
                }
                else
                    finishLevel = (int)levelInWorld * i + (int)(maxLevel % levelInWorld);
            }
            InitLevel(theme.transform, (int)levelInWorld * i, finishLevel);
        }
        SetPosCharacter();
    }
    
    void InitLevel(Transform content, int startLevel, int finishLevel)
    {
        int index = 0;
        for (int i = startLevel; i < finishLevel + 1; i++)
        {
            if (i % 2 == 0)
            {
                ElementLevel element = Instantiate(preOddLevel, content);
                element.transform.position = content.transform.GetChild(index).position;
                index++;
                int star = GameData.GetLevelStars(i + 1);
                element.InitLevel(i + 1, star);
                levelTransform.Add(element.transform);
            } 
            else 
            {
                ElementLevel element = Instantiate(preEvenLevel, content);
                element.transform.position = content.transform.GetChild(index).position;
                index++;
                int star = GameData.GetLevelStars(i + 1);
                element.InitLevel(i + 1, star);
                levelTransform.Add(element.transform);
            }
        }
    }
    void SetPosCharacter()
    {
        int index = Mathf.Min(GameData.LevelUnlock - 1, Constants.MAX_LEVEL - 1);
        Transform posEnd = levelTransform[index];
        if (GameData.isNextLevel)
        {
            StartCoroutine(SetNextPosCharacter(posEnd, GameData.LevelUnlock));
            GameData.isNextLevel = false;
        }
        else
        {
            posPlayer.transform.position = new Vector2(levelTransform[index].position.x, levelTransform[index].position.y + 0.8f);
            posPlayer.SetParent(posEnd);
        }
    }
    IEnumerator SetNextPosCharacter(Transform posEnd, int level)
    {
        int index = Mathf.Min(GameData.LevelUnlock - 1, Constants.MAX_LEVEL - 1);
        posPlayer.transform.position = new Vector2(levelTransform[index].position.x, levelTransform[index].position.y + 0.8f);
        posPlayer.SetParent(posEnd);
        GameData.levelSelected = level;
        yield return new WaitForSeconds(1);
        PopupStartLevel.Setup().Show();
    }
    public void LocateCurrentPlayer()
    {
        int world = GameData.LevelUnlock / Constants.LEVEL_IN_WORLD;
        contentTheme.DOLocalMove(new Vector2(-2500.62f * world, contentTheme.position.y), 1);
        GameAnalytics.LogButtonClick("navigation_button", "HomeScene");
    }
}
