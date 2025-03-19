// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using MySqlConnector;


namespace NetPasswordSDK
{
  
    class Program
    {
        static async Task Main(string[] args)
        {
            const string SDK_FOLDER_PATH = "./dll/";

            // Load the dll dynamically
            var sdkDll = Assembly.LoadFrom(Path.Combine(SDK_FOLDER_PATH, "NetStandardPasswordSDK.dll"));
            
            // get the types from the dll
            Type PasswordSDKType = sdkDll.GetType("CyberArk.AAM.NetStandardPasswordSDK.PasswordSDK");
            Type PasswordRequestType = sdkDll.GetType("CyberArk.AAM.NetStandardPasswordSDK.PSDKPasswordRequest");
            Type PasswordSDKExceptionType = sdkDll.GetType("CyberArk.AAM.NetStandardPasswordSDK.Exceptions.PSDKException");
            Type PasswordResponseType = sdkDll.GetType("CyberArk.AAM.NetStandardPasswordSDK.PSDKPassword");


            object passwordResponse = null;
            

            try
            {
                // Create Password Request
                ConstructorInfo ctor = PasswordRequestType.GetConstructor(System.Type.EmptyTypes);
                if (ctor == null)
                {
                    throw new Exception("Couldent create Password Request");
                }

                object passRequest;
                passRequest = ctor.Invoke(null);

                PropertyInfo propertyInfo = PasswordRequestType.GetProperty("ConnectionPort");
                propertyInfo.SetValue(passRequest, 18923, null);
                
                propertyInfo = PasswordRequestType.GetProperty("ConnectionTimeout");
                propertyInfo.SetValue(passRequest, 30, null);
                
                // Query propreties
                propertyInfo = PasswordRequestType.GetProperty("AppID");
                propertyInfo.SetValue(passRequest,"rpa01", null);
                
                // Was replaced by Query instead of passing the parameters, why do I need to place the object of type VirtualUserName
                propertyInfo = PasswordRequestType.GetProperty("Query");
                propertyInfo.SetValue(passRequest, "Safe=k8s-demo;Folder=root;VirtualUsername=demoaslan", null);

            ////  Uncomment these if you want to use a default query, as UserName or other object type.
                // propertyInfo = PasswordRequestType.GetProperty("Safe");
                // propertyInfo.SetValue(passRequest, "k8s-demo", null);
            
                //    Not necessary    
                // propertyInfo = PasswordRequestType.GetProperty("Folder");
                // propertyInfo.SetValue(passRequest, "root", null);
                
                // propertyInfo = PasswordRequestType.GetProperty("Object");
                // propertyInfo.SetValue(passRequest, "UserName=demoaslan", null);
            ////
                
                propertyInfo = PasswordRequestType.GetProperty("Reason");
                propertyInfo.SetValue(passRequest, "DotNet accessing", null);


                // Sending the request to get the password
                passwordResponse = PasswordSDKType.GetMethod("GetPassword").Invoke(null, new object[] { passRequest });


                propertyInfo = PasswordResponseType.GetProperty("UserName");
                var username = propertyInfo.GetValue(passwordResponse, null);
                //Console.WriteLine(username);
               
                
                propertyInfo = PasswordResponseType.GetProperty("Address");
                var address = propertyInfo.GetValue(passwordResponse, null);
                //use the address here
                //Console.WriteLine(address);

                // Analyzing the response
                propertyInfo = PasswordResponseType.GetProperty("Content");
                var password = (char[])propertyInfo.GetValue(passwordResponse, null);
                //Console.WriteLine(password);
                string passwd = new string(password);

                // use the password here
                 //using var connection = new MySqlConnection("Server={address};User ID={username};Password={password};Database=mydatabase");
                var str = $"Server={address};User ID={username};Password={passwd}";
                //Console.WriteLine(str);
                          
                using var connection = new MySqlConnection(str);
                await connection.OpenAsync();

                //using var command = new MySqlCommand("SELECT field FROM table;", connection);
                using var command = new MySqlCommand("SHOW FULL PROCESSLIST;", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id = reader.GetValue(0);
                    var user = reader.GetValue(1);
                    var host = reader.GetValue(2);
                    var db = reader.GetValue(3);
                    var cmd = reader.GetValue(4);
                    var time = reader.GetValue(5);
                    var state = reader.GetValue(6);
                    var info = reader.GetValue(7);
                    // do something with 'value'
                    //string top = "Id,User,Host,db,Command,Time,State,Info";
                    //Console.WriteLine(top);
                    string output_a = $"{id},{user},{host},{db},{cmd},{time},{state},{info}";
                    Console.WriteLine(output_a);
                }    
    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                // Always clear password form the memory
                passwordResponse = null;

            }
        }
    }
}
