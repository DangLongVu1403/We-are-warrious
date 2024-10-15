using UnityEngine;
public class UprageManager : MonoBehaviour
{
    private DatabaseManager dbManager;
    public int id;
    void Awake()
    {
        dbManager = FindObjectOfType<DatabaseManager>();
    }

    public void UprageTimeCooldownFood(){
        DatabaseManager.Uprage dataUprage = dbManager.GetUprageById(id);
        DatabaseManager.GameData dataGold = dbManager.GetGold();
        int goldUprageTime = dataUprage.GoldUprageTime;
        if (goldUprageTime <= dataGold.Gold){
            float timeCooldown = float.Parse(dataUprage.TimeCooldownFood);
            timeCooldown -= 0.05f;
            string newTimeCooldown = timeCooldown.ToString();
            dbManager.UpdateTimeCooldownFood(id,newTimeCooldown);
            int jumpGoldUprageTime = dataUprage.JumpGoldTime;
            int newGoldUprageTime = goldUprageTime + jumpGoldUprageTime;
            int newJumpGoldUprageTime = jumpGoldUprageTime +1;
            dbManager.UpdateGoldUprageTime(id,newGoldUprageTime);
            dbManager.UpdateJumpGoldTime(id,newJumpGoldUprageTime);
            int newGold = dataGold.Gold - goldUprageTime;
            dbManager.UpdateGold(newGold);
        }
    }

    public void UprageHealthHall(){
        DatabaseManager.Uprage dataUprage = dbManager.GetUprageById(id);
        DatabaseManager.Hall dataHall = dbManager.GetHallById(id);
        DatabaseManager.GameData dataGold = dbManager.GetGold();
        int GoldUprageHealthHall = dataUprage.GoldUprageHealthHall;
        int JumpGoldUprageHealthHall = dataUprage.JumpGoldHealth;
        if (GoldUprageHealthHall <= dataGold.Gold){
            int newHealthHall = dataHall.AllyHall + 2;
            int newGoldUprageHealthHall = GoldUprageHealthHall +  JumpGoldUprageHealthHall;
            int newJumpGoldUprageHealth = dataUprage.JumpGoldHealth + 1; 
            dbManager.UpdateAllyHallHealth(newHealthHall, id);
            dbManager.UpdateGoldUprageHealthHall(id, newGoldUprageHealthHall);
            dbManager.UpdateJumpGoldHealth(id,newJumpGoldUprageHealth);
            int newGold = dataGold.Gold - GoldUprageHealthHall;
            dbManager.UpdateGold(newGold);
        }
    }

    public void unlockSoldier2(){
        DatabaseManager.GameData dataGold = dbManager.GetGold();
        DatabaseManager.Uprage dataUprage = dbManager.GetUprageById(id);
        if(dataGold.Gold >= dataUprage.GoldUprageAlly2){
            int newGold = dataGold.Gold - dataUprage.GoldUprageAlly2;
            dbManager.UpdateGold(newGold);
            dbManager.UpdateLockAlly2(id,1);
        }
    }
    public void unlockSoldier3(){
        DatabaseManager.GameData dataGold = dbManager.GetGold();
        DatabaseManager.Uprage dataUprage = dbManager.GetUprageById(id);
        if(dataGold.Gold >= dataUprage.GoldUprageAlly3){
            int newGold = dataGold.Gold - dataUprage.GoldUprageAlly3;
            dbManager.UpdateGold(newGold);
            dbManager.UpdateLockAlly3(id,1);
        }
    }
}
