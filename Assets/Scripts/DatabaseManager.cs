using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using SQLite4Unity3d;
using System.Linq;
using UnityEngine.Networking;

public class DatabaseManager : MonoBehaviour
{
    private SQLiteConnection db;

    [Table("SoldierAlly")]
    public class SoldierAlly
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int HealthAlly1 { get; set; }
        public int HealthAlly2 { get; set; }
        public int HealthAlly3 { get; set; }
        public int DamageAlly1 { get; set; }
        public int DamageAlly2 { get; set; }
        public int DamageAlly3 { get; set; }
    }

    [Table("SoldierEnemy")]
    public class SoldierEnemy
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int HealthEnemy1 { get; set; }
        public int HealthEnemy2 { get; set; }
        public int HealthEnemy3 { get; set; }
        public int DamageEnemy1 { get; set; }
        public int DamageEnemy2 { get; set; }
        public int DamageEnemy3 { get; set; }
    }

    [Table("Hall")]
    public class Hall
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int AllyHall { get; set; }
        public int EnemyHall { get; set; }
    }

    [Table("Uprage")]
    public class Uprage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int LockAlly2 { get; set; }
        public int LockAlly3 { get; set; }
        public int GoldUprageAlly2 { get; set; }
        public int GoldUprageAlly3 { get; set; }
        public string TimeCooldownFood { get; set; }
        public int GoldUprageTime { get; set; }
        public int GoldUprageHealthHall { get; set; }
        public int JumpGoldTime { get; set; }
        public int JumpGoldHealth { get; set; }
    }

    [Table("SpawnSoldier1")]
    public class SpawnSoldier1
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Time1 { get; set; }
        public int Quantity1 { get; set; }
        public int Time2 { get; set; }
        public int Quantity2 { get; set; }
        public int Time3 { get; set; }
        public int Quantity3 { get; set; }
    }

    [Table("SpawnSoldier2")]
    public class SpawnSoldier2
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Time1 { get; set; }
        public int Quantity1 { get; set; }
        public int Time2 { get; set; }
        public int Quantity2 { get; set; }
        public int Time3 { get; set; }
        public int Quantity3 { get; set; }
    }


    [Table("SpawnSoldier3")]
    public class SpawnSoldier3
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Time1 { get; set; }
        public int Quantity1 { get; set; }
        public int Time2 { get; set; }
        public int Quantity2 { get; set; }
        public int Time3 { get; set; }
        public int Quantity3 { get; set; }
    }
    [Table("GameData")]
    public class GameData
    {
        [PrimaryKey]
        public int Id { get; set; } // Khóa chính
        public int Gold { get; set; } // Số vàng
        public int GoldRoundCurrent { get; set; }
        public int NewGoldUpdate { get; set; }
    }

    [Table("Level")]
    public class Level
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Khóa chính
        public int Pass { get; set; }
    }
    // void Awake()
    // {
    //     // string dbPath = Application.persistentDataPath + "/mydatabase.db";
    //     string dbPath = Path.Combine(Application.streamingAssetsPath, "mydatabase.db");
    //     // db = new SQLiteConnection(dbPath);
    //     StartCoroutine(LoadDatabase(dbPath));
    // }

    // private IEnumerator LoadDatabase(string path)
    // {
    //     UnityWebRequest request = UnityWebRequest.Get(path);
    //     yield return request.SendWebRequest();

    //     if (request.result != UnityWebRequest.Result.Success)
    //     {
    //         Debug.LogError("Error loading database: " + request.error);
    //     }
    //     else
    //     {
    //         // Lưu file vào Application.persistentDataPath
    //         string destinationPath = Path.Combine(Application.persistentDataPath, "mydatabase.db");
    //         File.WriteAllBytes(destinationPath, request.downloadHandler.data);
            
    //         // Mở cơ sở dữ liệu
    //         db = new SQLiteConnection(destinationPath);
    //     }
    // }
   void Awake()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "mydatabase.db");
        StartCoroutine(LoadDatabase(dbPath));
    }

    private IEnumerator LoadDatabase(string path)
    {
        // Kiểm tra xem tệp có tồn tại không
        if (!File.Exists(path))
        {
            // Nếu không tồn tại, sao chép tệp từ StreamingAssets
            string streamingAssetsPath = Path.Combine(Application.streamingAssetsPath, "mydatabase.db");
            
            // Chỉ sử dụng UnityWebRequest cho tệp ở StreamingAssets
            UnityWebRequest request = UnityWebRequest.Get(streamingAssetsPath);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading database: " + request.error);
                yield break;
            }

            // Lưu tệp vào Application.persistentDataPath
            File.WriteAllBytes(path, request.downloadHandler.data);
            Debug.Log("Database copied to persistentDataPath: " + path);
        }

        // Mở cơ sở dữ liệu
        db = new SQLiteConnection(path);
        Debug.Log("Database loaded successfully: " + path);
    }
    // Lấy dữ liệu từ bảng SoldierAlly theo Id
    public SoldierAlly GetSoldierAllyById(int Id)
    {
        var soldierAllyData = db.Table<SoldierAlly>().FirstOrDefault(x => x.Id == Id);
        return soldierAllyData;
    }

    // Lấy dữ liệu từ bảng SoldierEnemy theo Id
    public SoldierEnemy GetSoldierEnemyById(int Id)
    {
        if(db == null) {
            return null;
        }

        var soldierEnemyData = db.Table<SoldierEnemy>().FirstOrDefault(x => x.Id == Id);
        return soldierEnemyData;
    }

    // Lấy dữ liệu từ bảng Hall theo Id
    public Hall GetHallById(int Id)
    {
        var hallData = db.Table<Hall>().FirstOrDefault(x => x.Id == Id);
        return hallData;
    }

    // Lấy dữ liệu từ bảng Uprage theo Id
    public Uprage GetUprageById(int Id)
    {
        var uprageData = db.Table<Uprage>().FirstOrDefault(x => x.Id == Id);
        return uprageData;
    }
    public SpawnSoldier1 GetSpawnSoldier1ById(int Id)
    {
        var SpawnSoldier1Data = db.Table<SpawnSoldier1>().FirstOrDefault(x => x.Id == Id);
        return SpawnSoldier1Data;
    }
    public SpawnSoldier2 GetSpawnSoldier2ById(int Id)
    {
        var SpawnSoldier2Data = db.Table<SpawnSoldier2>().FirstOrDefault(x => x.Id == Id);
        return SpawnSoldier2Data;
    }
    public SpawnSoldier3 GetSpawnSoldier3ById(int Id)
    {
        var SpawnSoldier3Data = db.Table<SpawnSoldier3>().FirstOrDefault(x => x.Id == Id);
        return SpawnSoldier3Data;
    }

    public GameData GetGold()
    {
        var Data = db.Table<GameData>().FirstOrDefault(x => x.Id == 1);
        return Data;
    }

    public void UpdateGold(int newGold)
    {
        // Lấy dữ liệu GameData đầu tiên
        var data = db.Table<GameData>().FirstOrDefault();
        // Cập nhật lượng vàng
        data.Gold = newGold; 
        db.Update(data);
    }
   public List<Level> GetAllLevels()
    {
        if (db == null)
        {
            Debug.LogError("Cơ sở dữ liệu chưa được khởi tạo!");
            return new List<Level>(); // Trả về danh sách rỗng nếu db chưa được khởi tạo
        }

        try
        {
            // Lấy toàn bộ dữ liệu từ bảng Level
            return db.Table<Level>().ToList();
        }
        catch (Exception ex)
        {
            Debug.LogError("Lỗi khi lấy dữ liệu từ bảng Level: " + ex.Message);
            return new List<Level>(); // Trả về danh sách rỗng nếu có lỗi
        }
    }



    public void UpdateLevel(int id, int Pass)
    {
        // Tìm bản ghi có Id phù hợp trong bảng Level
        var data = db.Table<Level>().FirstOrDefault(x => x.Id == id);

        // Kiểm tra xem bản ghi có tồn tại không
        if (data != null)
        {
            // Cập nhật Level mới
            data.Pass = Pass;

            // Lưu bản ghi đã cập nhật vào cơ sở dữ liệu
            db.Update(data); 
        }
    }


    public void UpdateGoldRoundCurrent(int newGoldRoundCurrent)
    {
        // Lấy dữ liệu GameData đầu tiên
        var data = db.Table<GameData>().FirstOrDefault();
        // Cập nhật lượng vàng
        data.GoldRoundCurrent = newGoldRoundCurrent; 
        db.Update(data);
    }
    public void UpdateNewGoldUpdate(int newGoldUpdate)
    {
        // Lấy dữ liệu GameData đầu tiên
        var data = db.Table<GameData>().FirstOrDefault();
        // Cập nhật lượng vàng
        data.NewGoldUpdate = newGoldUpdate; 
        db.Update(data);
    }
    // Cập nhật dữ liệu trong bảng Uprage
    public void UpdateUprage(Uprage uprageData)
    {
        db.Update(uprageData);
    }

    // Các phương thức cập nhật cụ thể
    public void UpdateField<T>(int Id, System.Action<T> updateAction) where T : class, new()
    {
        var data = db.Table<T>().FirstOrDefault(x => x.GetType().GetProperty("Id").GetValue(x).Equals(Id));
        if (data != null)
        {
            updateAction(data);
            db.Update(data);
        }
    }

    // Ví dụ sử dụng UpdateField cho các thuộc tính của Uprage
    public void UpdateLockAlly2(int Id, int newLockAlly2) => UpdateField<Uprage>(Id, data => data.LockAlly2 = newLockAlly2);
    public void UpdateLockAlly3(int Id, int newLockAlly3) => UpdateField<Uprage>(Id, data => data.LockAlly3 = newLockAlly3);
    public void UpdateGoldUprageAlly2(int Id, int newGoldUprageAlly2) => UpdateField<Uprage>(Id, data => data.GoldUprageAlly2 = newGoldUprageAlly2);
    public void UpdateGoldUprageAlly3(int Id, int newGoldUprageAlly3) => UpdateField<Uprage>(Id, data => data.GoldUprageAlly3 = newGoldUprageAlly3);
    public void UpdateTimeCooldownFood(int Id, string newTimeCooldownFood) => UpdateField<Uprage>(Id, data => data.TimeCooldownFood = newTimeCooldownFood);
    public void UpdateGoldUprageTime(int Id, int newGoldUprageTime) => UpdateField<Uprage>(Id, data => data.GoldUprageTime = newGoldUprageTime);
    public void UpdateGoldUprageHealthHall(int Id, int newGoldUprageHealthHall) => UpdateField<Uprage>(Id, data => data.GoldUprageHealthHall = newGoldUprageHealthHall);
    public void UpdateJumpGoldTime(int Id, int newJumpGoldTime) => UpdateField<Uprage>(Id, data => data.JumpGoldTime = newJumpGoldTime);
    public void UpdateJumpGoldHealth(int Id, int newJumpGoldHealth) => UpdateField<Uprage>(Id, data => data.JumpGoldHealth = newJumpGoldHealth);

    // Cập nhật máu của pháo đài đồng minh
    public void UpdateAllyHallHealth(int newHealth, int Id)
    {
        var hallData = GetHallById(Id);
        if (hallData != null)
        {
            hallData.AllyHall = newHealth;
            db.Update(hallData);
        }
    }
}
