
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

public class DesignManager : MonoBehaviour
{
    [Header("Main Settings")]

    [SerializeField] Transform designManagerObject;
    [SerializeField] Canvas mainCanvas;

    [Header("Room")]

    [SerializeField] float zoomSpeed = 0.4f;
    [SerializeField] RoomPrefabScript[] roomPrefabArray;
    [SerializeField] RoomPrefabSettings[] roomSettingsArray;

    [Header("Object Menu")]

    [SerializeField] RoomObjectMenu objectMenu;

    //[Header("Buy Animation")]

    //[SerializeField] Transform moneyPileStartPos;

    [Header("Tutorial")]

    [SerializeField] TutorialBuyButton[] tutorButtonsArray;

    private UIController uiController;
    private bool isActive;

    private int roomsOrder;

    public static Action<int, int> updateRoomCash;

    private RoomPrefabScript currentRoom;
    private int activeRoomNumber;
    public int[] openedRoomObjects;

    private int lastID;

    private Transform lastRoomObjectPos;
    private int objMenuCallCount;

    public void Init(UIController controller)
    {
        uiController = controller;
        objMenuCallCount = 0;

        updateRoomCash += UpdateCurrentRoomCash;
        EventBus.onRewardedADClosed += GetRewardObject;

        CheckVersion();
        LoadRoom();

        objectMenu.Init(this);
    }

    public void SwitchManagerActive(bool _isActive)
    {
        isActive = _isActive;
        SwitchBuyButtons(isActive);

        //Debug.Log($"Design manager {isActive}");
    }

    public void SwitchBuyButtons(bool isActive)
    {
        currentRoom.SwitchActiveButtons(isActive);

        foreach (TutorialBuyButton button in tutorButtonsArray)
        {
            button.SetActive(isActive);
        }
    }

    public bool GetManagerStatus() { return isActive; }

    public void SwitchCurrentRoom()
    {
        if (activeRoomNumber < roomPrefabArray.Length - 1)
            activeRoomNumber += 1;

        GeneralSave.Instance.UpdateRoomNum(activeRoomNumber);
        LoadRoom();
    }

    private void SwitchRoomZoom(bool zoomIn, Transform objectPos)
    {
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

    public void CheckGeneralProgress() { currentRoom.CheckTierIsOpen(); }

    public void ActivateNewRoom()
    {
        SwitchManagerActive(false);
        GameController.tutorialManager.CallStepNewRoom();
    }

    public void CheckRoomPassed() => StartCoroutine(CheckObjectsPurchased());

    private IEnumerator CheckObjectsPurchased()
    {
        if (currentRoom.CheckObjectsPurchased() && activeRoomNumber + 1 < roomPrefabArray.Length)
        {
            yield return new WaitForSeconds(1f);

            ActivateNewRoom();
        }
    }

    private void UpdateCurrentRoomCash(int coins, int money)
    {
        int[] cash = new int[] { coins, money };
        currentRoom.UpdateRoomCash(cash);
    }
    #endregion

    #region Buy Object

    public void BuyObject(RoomObjectScript currentObj, int price)
    {
        if (!objectMenu.isOpen && isActive)
        {
            if (GameController.Instance.playerMoney >= price)
            {
                //GameController.updatePlayerMoney?.Invoke(-price);
                GameController.Instance.CalculateMoney(-price);
                uiController.UpdateMainUI();
                StartCoroutine(OpenNewObject(currentObj));
            }
            else
            {
                UIController.Instance.CallScreen(UIController.Menu.NoMoneyScreen);
            }
        }
    }

    private IEnumerator OpenNewObject(RoomObjectScript currentObj)
    {
        currentObj.SwitchButtonCanBeClicked(false);
        UIAnimationScreen.Instance.StartBuyObjAnimation(currentObj.GetBuyButtonPosition());

        yield return new WaitForSeconds(2f);

        currentObj.SetObjectStatus(RoomObjectScript.Status.isPurchased);
        CheckGeneralProgress();
        CallObjectMenu(currentObj, true);
        currentObj.SwitchButtonCanBeClicked(true);

        if (GameController.tutorialIsActive)
            GameController.tutorialManager.OpenFloorColorScreen();
    }

    public RoomPrefabScript GetCurrentRoom() { return currentRoom; }
    #endregion

    #region Object Menu

    public void CallObjectMenu(RoomObjectScript currentObj, bool isFirstTime)
    {
        if (isActive)
        {
            if (!objectMenu.isOpen)
            {
                SwitchManagerActive(false);
                lastID = currentObj.id;
                lastRoomObjectPos = currentObj.transform;
                objectMenu.OpenMain(isFirstTime, currentObj.GetShortSprites(), currentObj._isAD, roomPrefabArray[activeRoomNumber].spritesArray[currentObj.id]);

                EventBus.onWindowOpened?.Invoke();

                if (GameController.tutorialIsActive && GameController.tutorialManager.GetRoomProgress() == 5)
                    GameController.tutorialManager.CloseWallsScreen();

                SwitchRoomZoom(true, currentObj.transform);
                SwitchBuyButtons(false);
            }
        }
    }

    public void UpdateObjectSprite(int spriteNum) => currentRoom.UpdateObjectSprite("", lastID, spriteNum);

    public void ConfirmSprite()
    {
        SoundController.Instance.PlaySound(SoundController.Sound.SetChest);
        StartCoroutine(objectMenu.CloseWindow());
        SwitchRoomZoom(false, lastRoomObjectPos.transform);
        SwitchManagerActive(true);

        if (GameController.tutorialManager.GetRoomProgress() == 4)
            GameController.tutorialManager.UpdateRoomProgress(5);

        else if (GameController.tutorialManager.GetRoomProgress() == 5)
        {
            if (roomPrefabArray[activeRoomNumber].currentTier >= 3)
                GameController.tutorialManager.UpdateRoomProgress(6);

            if (roomPrefabArray[activeRoomNumber].currentTier == 2)
                GameController.tutorialManager.UpdateRoomProgress(5);
        }

        EventBus.onWindowClosed?.Invoke();

        currentRoom.SavePrefab();

        objMenuCallCount++;
        if (objMenuCallCount >= 10)
        {
            GoogleReviewManager.callRequestReview?.Invoke();
            objMenuCallCount = 0;
        }

        CheckRoomPassed();
    }
    #endregion

    #region Objects

    public int GetCurrentObjectSpriteByNum(int num) { return roomPrefabArray[activeRoomNumber].spritesArray[num]; }

    private void GetRewardObject(string location)
    {
        if (location == "ADPet")
        {
            uiController.UpdateMainUI();
            roomPrefabArray[activeRoomNumber].PurchaseADObject();
            SwitchManagerActive(true);
            CallObjectMenu(roomPrefabArray[activeRoomNumber].GetADObject(), true);
        }
    }

    #endregion

    #region Main Window

    public void OpenMain()
    {
        SwitchManagerActive(true);
        mainCanvas.enabled = true;
    }

    public void CloseMain()
    {
        SwitchManagerActive(false);
        mainCanvas.enabled = false;
    }
    #endregion

    #region Save/Load

    public void SetFirebaseSettings()
    {
        for (int i = 0; i < roomPrefabArray.Length; i++)
        {
            roomPrefabArray[i].SetObjectsPrices(GameController.Instance.firebaseController.roomsPricesArray[i]);
        }
    }

    private void LoadRoom()
    {
        for (int i = 0; i < roomPrefabArray.Length; i++)
        {
            roomPrefabArray[i].Init(this, roomSettingsArray[i].GetPricesArray());
            roomPrefabArray[i].gameObject.SetActive(false);
        }

        activeRoomNumber = GeneralSave.Instance.currentRoomNum;
        currentRoom = roomPrefabArray[activeRoomNumber];
        currentRoom.gameObject.SetActive(true);
    }

    public void ResetSave()
    {
        activeRoomNumber = 0;

        foreach (RoomPrefabScript room in roomPrefabArray)
        {
            room.ResetSave();
        }
    }

    private void CheckVersion()
    {

        if (PlayerPrefs.HasKey("PlayerVersion"))
            roomsOrder = PlayerPrefs.GetInt("PlayerVersion");

        else
        {
            if (GeneralSave.Instance.playerLvl > 0)
                roomsOrder = 0;
            else
                roomsOrder = 1;
        }

        PlayerPrefs.SetInt("PlayerVersion", roomsOrder);

        Debug.Log("Player Design Manager Ver: " + roomsOrder);

        if (roomsOrder == 1)
        {
            RoomPrefabScript[] roomPrefabArray2 = new RoomPrefabScript[] { roomPrefabArray[1], roomPrefabArray[0] };
            roomPrefabArray = roomPrefabArray2;
        }
    }

    #endregion

    private void OnDestroy()
    {
        updateRoomCash -= UpdateCurrentRoomCash;
        EventBus.onRewardedADClosed -= GetRewardObject;
    }
}
