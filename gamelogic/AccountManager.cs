using gamelogic.Models;
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
            using (Entities db = new Entities())
            {
                var us = db.Accounts.FirstOrDefault(o => o.Username == data.username);
                if (us != null)
                {
                    if (us.Password == data.password)
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
            using (Entities db = new Entities())
                try
                {
                    // пока без шифрации пароля
                    Account user = new Account { Username = username, Password = password, Role = 0, Token = "Token=потом придумаю как шифровать токен для " + username };
                    db.Accounts.Add(user);
                    db.SaveChanges();

                    int newIdentityValue = user.UserID;
                    db.Players.Add(new Player { UserID = newIdentityValue, Playername = username });
                    db.Bases.Add(new Base { Basename = username + "Base", OwnerID = newIdentityValue, CoordX = 1, CoordY = 1, Level = 0, IsActive = true });
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
            using (Entities db = new Entities())
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
    }
}