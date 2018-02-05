using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System;
using UnityEngine.SceneManagement;

public class Datalogic
{
    private readonly int TestingPeriod = 1;

    public void Initialize()
    {
        //using (DataContext context = GetDataContext())
        //{
        //    context.ExecuteCommand("INSERT INTO LevelScoreStats(UserId, DateTime, LevelNumber, Score)  VALUES(1, '20120618 10:34:09 AM', 2, 350);");
        //}
        //SendLevelInfo(1, 1, "1");
    }

    //public IEnumerator RunTask(int pLevel, int pScore, string pUserID)
    //{
    //    yield return Task.Run(() => SendLevelInfo(pLevel, pScore, pUserID));
    //}

    //public IEnumerator SendLevelInfo(string pUserID, int pLevel, int pScore, int pStars, int pPatientsHealed, int pPatientsKilled)
    //{
    //    string sqlFormattedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
    //    string command = $"INSERT INTO LevelScoreStats(UserId, DateTime, LevelNumber, Score, Stars, PatientsHealed, PatientsKilled, TestingPeriod)  VALUES('{pUserID}', '{sqlFormattedDate}', {pLevel}, {pScore}, {pStars}, {pPatientsHealed}, {pPatientsKilled}, {TestingPeriod});";
    //    Debug.Log("About to start async task..");
    //    //yield return Task.Run(() => SendCommandToServer(command));
    //}

    //public async Task SendCommandToServer(string pCommand)
    //{
    //    SqlConnection connection = new SqlConnection("Data Source=mssql5.gear.host;Integrated Security=False;User ID=fieldhospitaldb;Password=Ux8f3t2?buo?;Connect Timeout=30;Encrypt=False;");
    //    Debug.Log("Opening Async...");
    //    await connection.OpenAsync();
    //    using (DataContext context = new DataContext(connection))
    //    {
    //        Debug.Log("Executing command..");
    //        context.ExecuteCommand(pCommand);
    //        Debug.Log("Done executing..");
    //    }
    //    connection.Close();
    //}

}
