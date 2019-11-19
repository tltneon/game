﻿using gamelogic.Models;
using System;
using System.Linq;

namespace gamelogic
{
    /// <summary>
    /// Класс менеджера аккаунтов управляет авторизацией, регистрацией и прочим взаимодействием с аккаунтами
    /// </summary>
    public class AccountManager
    {
        /// <summary>
        /// Управляет авторизацией пользователя
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AuthClient(AuthData data)
        {
            using (var db = new Entities())
            {
                var us = db.Accounts.FirstOrDefault(o => o.Username == data.username);
                if (us != null)
                {
                    if (Base64Decode(us.Password) == data.password)
                    {
                        return us.Token;
                    }
                    else
                    {
                        ProceedActions.Log("Event", $"Неудачная попытка авторизоваться под аккаунтом {us.Username}");
                        const string error = "Error#Wrong Password";
                        return error;
                    }
                }
                else
                {
                    return CreateUser(data.username, data.password);
                }
            }
        }

        /// <summary>
        /// Управляет созданием аккаунта
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CreateUser(string username, string password)
        {
            using (var db = new Entities())
                try
                {
                    var user = new Account { Username = username, Password = Base64Encode(password), Role = 0, Token = "Token" + Base64Encode(username + "salt") };
                    db.Accounts.Add(user);
                    db.SaveChanges();

                    int newIdentityValue = user.UserID;
                    db.Players.Add(new Player { UserID = newIdentityValue, Playername = username });
                    db.Bases.Add(new Base { Basename = username + "Base", OwnerID = newIdentityValue, CoordX = 1, CoordY = 1, Level = 0, IsActive = true });
                    db.Resources.Add(new Resource { Instance = "bas" + newIdentityValue, Credits = 200, Energy = 200, Neutrino = 0.0 });
                    db.SaveChanges();
                    return user.Token;
                }
                catch (Exception ex)
                {
                    ProceedActions.Log("Exception", $"Исключение: {ex.Message}, функция AccountManager.CreateUser");
                    return "Error#Exception: " + ex.Message;
                }
        }

        /// <summary>
        /// Возвращает аккаунт по его токену
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Account GetAccountByToken(string token)
        {
            using (var db = new Entities())
            {
                return db.Accounts.FirstOrDefault(o => o.Token == token);
            }
        }

        /// <summary>
        /// Проверяет токен игрока
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool CheckToken(string token)
        {
            return GetAccountByToken(token) != null;
        }

        /// <summary>
        /// Кодирует строку в BASE64
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Декодирует строку из BASE64
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}