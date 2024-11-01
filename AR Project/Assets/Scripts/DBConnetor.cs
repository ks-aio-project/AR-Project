//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using UnityEngine;
//using Newtonsoft.Json;

//using MySql.Data;
//using MySql.Data.MySqlClient;

//public class DBConnetor : MonoBehaviour
//{
//    private MySqlConnection connection;

//    // MySQL 서버 정보 설정
//    private string server = "bola.iptime.org";       // 서버 주소
//    private string database = "metabus"; // 데이터베이스 이름
//    private string user = "allione";     // 사용자 이름
//    private string password = "allione1234"; // 비밀번호
//    private string port = "33306";              // 포트번호 (기본 3306)

//    // 데이터베이스 연결 열기
//    public void OpenConnection()
//    {
//        if (connection == null || connection.State == System.Data.ConnectionState.Closed)
//        {
//            try
//            {
//                string connectionString = $"Server={server};Database={database};User ID={user};Password={password};Port={port};";
//                connection = new MySqlConnection(connectionString);

//                connection.Open();
//                Debug.Log("MySQL connection opened.");
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError("Error opening MySQL connection: " + ex.ToString());
//            }
//        }
//    }

//    // 데이터베이스 연결 닫기
//    public void CloseConnection()
//    {
//        if (connection != null && connection.State == System.Data.ConnectionState.Open)
//        {
//            try
//            {
//                connection.Close();
//                Debug.Log("MySQL connection closed.");
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError("Error closing MySQL connection: " + ex.Message);
//            }
//        }
//    }

//    // SELECT 예시 (데이터 읽기)
//    public void ExecuteSelectQuery(string query)
//    {
//        if (connection != null && connection.State == System.Data.ConnectionState.Open)
//        {
//            MySqlCommand cmd = new MySqlCommand(query, connection);
//            Debug.Log($"쿼리 : {query}");

//            try
//            {
//                MySqlDataReader dataReader = cmd.ExecuteReader();

//                // 결과를 저장할 리스트 선언 (각 행을 Dictionary 형태로 저장)
//                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();

//                // 결과 집합의 각 행을 반복 처리
//                while (dataReader.Read())
//                {
//                    Dictionary<string, object> row = new Dictionary<string, object>();

//                    // 현재 행의 각 열을 반복 처리
//                    for (int i = 0; i < dataReader.FieldCount; i++)
//                    {
//                        string columnName = dataReader.GetName(i);
//                        object columnValue = dataReader.IsDBNull(i) ? null : dataReader.GetValue(i);

//                        // Dictionary에 열 이름과 값 추가
//                        row.Add(columnName, columnValue);
//                    }

//                    // 각 행을 리스트에 추가
//                    rows.Add(row);
//                }

//                // MySqlDataReader 닫기
//                dataReader.Close();

//                // 리스트를 JSON 문자열로 변환
//                string jsonResult = JsonConvert.SerializeObject(rows, Formatting.Indented);

//                // JSON 결과 출력 (로그에 출력)
//                Debug.Log("JSON Result: " + jsonResult);
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError("Error executing SELECT query: " + ex.Message);
//            }
//        }
//        else
//        {
//            Debug.LogError("Connection is not open.");
//        }
//    }

//    // INSERT/UPDATE/DELETE 예시 (데이터 수정)
//    public void ExecuteNonQuery(string query)
//    {
//        if (connection != null && connection.State == System.Data.ConnectionState.Open)
//        {
//            MySqlCommand cmd = new MySqlCommand(query, connection);

//            try
//            {
//                int rowsAffected = cmd.ExecuteNonQuery();
//                Debug.Log("Query executed. Rows affected: " + rowsAffected);
//            }
//            catch (Exception ex)
//            {
//                Debug.LogError("Error executing query: " + ex.Message);
//            }
//        }
//        else
//        {
//            Debug.LogError("Connection is not open.");
//        }
//    }

//    // 예시 쿼리 호출
//    private void Start()
//    {
//        // 데이터베이스 연결
//        OpenConnection();

//        // 예시로 SELECT 쿼리 실행 (필요에 맞게 쿼리 수정)
//        string selectQuery = "SELECT * FROM device";
//        ExecuteSelectQuery(selectQuery);

//        // 예시로 INSERT 쿼리 실행 (필요에 맞게 쿼리 수정)
//        //string insertQuery = "INSERT INTO your_table_name (column1, column2) VALUES ('value1', 'value2')";
//        //ExecuteNonQuery(insertQuery);

//        // 데이터베이스 연결 닫기
//        CloseConnection();
//    }
//}
