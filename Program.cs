
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using MySqlConnector;

namespace NetPasswordSDK
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string SDK_FOLDER_PATH = "./dll/";
            string sdkPath = Path.Combine(SDK_FOLDER_PATH, "NetStandardPasswordSDK.dll");

            // Carrega a DLL do SDK
            var sdkDll = Assembly.LoadFrom(sdkPath);
            var PasswordSDKType = sdkDll.GetType("CyberArk.AAM.NetStandardPasswordSDK.PasswordSDK");
            var PasswordRequestType = sdkDll.GetType("CyberArk.AAM.NetStandardPasswordSDK.PSDKPasswordRequest");
            var PasswordResponseType = sdkDll.GetType("CyberArk.AAM.NetStandardPasswordSDK.PSDKPassword");

            if (PasswordSDKType == null || PasswordRequestType == null || PasswordResponseType == null)
            {
                Console.WriteLine("Erro ao carregar tipos da DLL.");
                return;
            }

            object? passwordResponse = null;

            try
            {
                var passRequest = Activator.CreateInstance(PasswordRequestType);
                if (passRequest == null)
                {
                    Console.WriteLine("Falha ao criar instância de PasswordRequest.");
                    return;
                }

                SetProperty(passRequest, "ConnectionPort", 18923);
                SetProperty(passRequest, "ConnectionTimeout", 30);
                SetProperty(passRequest, "AppID", "puertorico"); . // Here is the APPID 
                SetProperty(passRequest, "Query", "Safe=dev-demo-cred;Folder=root;Username=appuser_db"); // HERE is Query to get the account
                SetProperty(passRequest, "Reason", "DotNet accessing");

                passwordResponse = PasswordSDKType.GetMethod("GetPassword")?.Invoke(null, new object[] { passRequest });
                if (passwordResponse == null)
                {
                    Console.WriteLine("Falha ao obter resposta de senha.");
                    return;
                }

                var username = GetProperty(passwordResponse, PasswordResponseType, "UserName")?.ToString() ?? "defaultUser";
                var passwordChars = (char[]?)GetProperty(passwordResponse, PasswordResponseType, "Content") ?? Array.Empty<char>();
                var password = new string(passwordChars);
                var address = "10.78.10.171";

                var connectionString = $"Server={address};User ID={username};Password={password};Database=userdb";
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                using var command = new MySqlCommand("SELECT * FROM users;", connection);
                using var reader = await command.ExecuteReaderAsync();

                Console.WriteLine("Dados da tabela 'users':");
                while (await reader.ReadAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader.GetValue(i)}\t");
                    }
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            finally
            {
                passwordResponse = null;
            }
        }

        static void SetProperty(object? obj, string propertyName, object value)
        {
            if (obj == null) return;
            var prop = obj.GetType().GetProperty(propertyName);
            prop?.SetValue(obj, value);
        }

        static object? GetProperty(object? obj, Type? type, string propertyName)
        {
            if (obj == null || type == null) return null;
            var prop = type.GetProperty(propertyName);
            return prop?.GetValue(obj);
        }
    }
}

