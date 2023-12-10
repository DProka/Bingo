
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DesignManager : MonoBehaviour
{
    [Header("Main Settings")]

    [SerializeField] GeneralSave generalSave;
    [SerializeField] RoomSave roomSave;
    [SerializeField] RoomDesignBase roomBase;
    [SerializeField] Transform designManagerObject;

    private UIController uiController;
    private bool isActive;
    private Vector3 managerStartPosition;

    [Header("Room")]

    [SerializeField] float zoomSpeed = 0.4f;
    [SerializeField] RoomPrefabScript[] roomPrefabArray;

    public int activeRoomNumber;
    public int roomTierProgress;

    public int[] openedRoomObjects;

    [Header("Object Menu")]

    [SerializeField] RoomObjectMenu objectMenu;
    private string lastKey;
    private RoomObjectScript lastRoomObject;

    [Header("Buy Animation")]

    [SerializeField] UIPileReward buyPile;

    [Header("Tutorial")]

    [SerializeField] TutorialBuyButton[] tutorButtonsArray;
    private int tutorialProgress = 0;

    public void Init(UIController controller)
    {
        uiController = controller;
        roomTierProgress = 0;

        managerStartPosition = designManagerObject.position;

        LoadRoomProgress();
        LoadRoomPrefab();
        
        objectMenu.Init(this);
        buyPile.Init();

        tutorialProgress = 0;

        CheckGeneralProgress();
    }

    public void SwitchManagerActive(bool _isActive) 
    { 
        isActive = _isActive;
        SwitchBuyButtons(isActive);

        Debug.Log($"Design manager {isActive}");
    }

    public void SwitchBuyButtons(bool isActive)
    {
        roomPrefabArray[activeRoomNumber].SwitchActiveButtons(isActive);

        foreach(TutorialBuyButton button in tutorButtonsArray)
        {
            button.gameObject.SetActive(isActive);
        }
    }

    public bool GetManagerStatus() { return isActive; }

    private void LoadRoomPrefab()
    {
        //LoadRoomProgress();

        if (GameController.tutorialIsActive)
        {
            activeRoomNumber = 0;
            roomTierProgress = 0;
        }

        roomPrefabArray[activeRoomNumber].Init(this, roomSave.GetTierProgress());
        
        if(openedRoomObjects.Length == 1)
            openedRoomObjects = new int[roomPrefabArray[activeRoomNumber].GetObjectsCount()];
        
        roomPrefabArray[activeRoomNumber].LoadRoomPrefab(roomTierProgress, openedRoomObjects);
    }

    private void SwitchRoomZoom(bool zoomIn, Transform objectPos)
    {
        managerStartPosition = designManagerObject.position;

        if (zoomIn)
        {
            designManagerObject.DOScale(1.2f, zoomSpeed);

            if (objectPos.position.x < -1)
                designManagerObject.DOMoveX(1, zoomSpeed);

            if (objectPos.position.x > 1)
                designManagerObject.DOMoveX(-1, zoomSpeed);

            if (objectPos.position.y < -1)
                designManagerObject.DOMoveY(1, zoomSpeed);

            if (objectPos.position.y > 1)
                designManagerObject.DOMoveY(-1, zoomSpeed);
        }
        else
        {
            designManagerObject.DOScale(1f, zoomSpeed);
            designManagerObject.DOMove(new Vector3(0, 0, 0), zoomSpeed);
        }
    }

    #region Progress

    public void CheckGeneralProgress() 
    {
        roomPrefabArray[activeRoomNumber].ActivateTier(roomTierProgress + 1);
    }

    private void CheckTierProgress(int tier)
    {
        if(roomPrefabArray[activeRoomNumber].GetTierOpenStatus(roomTierProgress))
        {
            roomTierProgress = tier;
            CheckGeneralProgress();
        }
    }
    #endregion

    #region Objects

    public void BuyObject(RoomObjectScript currentObj, int price)
    {
        if (!objectMenu.GetStatus() && isActive)
        {
            if(generalSave.GetMoney() >= price)
            {
                generalSave.CalculateMoney(false, price);
                roomTierProgress = currentObj.GetTier();
                UpdateRoomProgress();

                uiController.UpdateMainUI();
                StartCoroutine(OpenNewObject(currentObj));
            }
            else
            {
                uiController.OpenNoMoney();
            }
        }
    }

    private IEnumerator OpenNewObject(RoomObjectScript currentObj)
    {
        SetLastRoomObj(currentObj);
        buyPile.SetFinishPosition(currentObj.GetBuyButtonPosition());
        buyPile.StartBuyPileAnimation();

        yield return new WaitForSeconds(2f);

        currentObj.SetIsOpen(true);
        CheckTierProgress(currentObj.GetTier());
        CallObjectMenu(currentObj);
    }

    public void CallObjectMenu(RoomObjectScript currentObj)
    {
        if(lastKey != currentObj.GetKey() && isActive)
        {
            if (!objectMenu.GetStatus())
            {
                lastKey = currentObj.GetKey();
                objectMenu.SetSprites(roomBase.GetArrayByKey(lastKey));
                objectMenu.SetKey(lastKey);
                objectMenu.OpenMain();
                uiController.HideMainMenuInterface(true);
                SwitchRoomZoom(true, currentObj.transform);
                SwitchBuyButtons(false);
            }
        }
    }

    public void UpdateObjectSprite(string key, int spriteNum, Sprite sprite)
    {
        if (isActive)
        {
            roomPrefabArray[activeRoomNumber].UpdateObjectSprite(key, spriteNum, sprite);
            lastRoomObject = roomPrefabArray[activeRoomNumber].GetRoomObject(key);
        }
    }

    public void UpdateRoomObjectsArray(int tier, int spritenum)
    {
        openedRoomObjects[tier - 1] = spritenum;
    }

    public void ConfirmSprite()
    {
        uiController.HideMainMenuInterface(false);
        StartCoroutine(objectMenu.CloseWindow());
        SwitchRoomZoom(false, lastRoomObject.transform);
        SwitchBuyButtons(true);
        lastKey = " ";

        if (GameController.tutorialIsActive)
        {
            if (tutorialProgress == 0)
                GameController.tutorialManager.UpdateTutorialProgress(10);

            else if (tutorialProgress == 1)
                GameController.tutorialManager.UpdateTutorialProgress(11);

            tutorialProgress++;
        }

        UpdateRoomProgress();
    }

    public void SetLastRoomObj(RoomObjectScript obj) { lastRoomObject = obj; }
    #endregion

    #region Save/Load

    private void UpdateRoomProgress()
    {
        roomSave.SetActiveRoom(activeRoomNumber);
        roomSave.SetTierProgress(roomTierProgress);
        roomSave.SetRoomProgress(activeRoomNumber, openedRoomObjects);
    }

    private void LoadRoomProgress() 
    {
        activeRoomNumber = roomSave.GetActiveRoom();
        roomTierProgress = roomSave.GetTierProgress();
        openedRoomObjects = roomSave.GetRoomProgress(activeRoomNumber);

        //if(openedRoomObjects.Length == 0)
        //    openedRoomObjects = new int[roomPrefabArray[activeRoomNumber].GetObjectsCount()];
    }

    #endregion

    #region Main Window

    public void OpenMain() { gameObject.SetActive(true); }
    
    public void CloseMain() { gameObject.SetActive(false); }
    #endregion
}
