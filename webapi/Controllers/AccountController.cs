﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace webapi.Controllers
{
    /// <summary>
    /// Контроллер, получающий данные об аккаунте
    /// </summary>
    public class AccountController : ApiController
    {
        /// <summary>
        /// Выполняет действия авторизации / регистрации игрока
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Auth(wcfservice.AuthData message)
        {
            if (CheckWrongData(message) || !ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Invalid input");
            }
            var client = new Service1Client();
            var token = await client.SendAuthDataAsync(message);
            client.Close();
            return Request.CreateResponse(HttpStatusCode.OK, token);
        }

        /// <summary>
        /// Возвращает тестовые данные
        /// </summary>
        /// <returns></returns>
        public async Task<string> SetAccountPassword(wcfservice.AuthData msg)
        {
            var client = new Service1Client();
            var result = await client.SetAccountPasswordAsync(msg);
            client.Close();
            return result;
        }

        /// <summary>
        /// Проверяет на некорректность ввода
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static bool CheckWrongData(wcfservice.AuthData message)
        { // прочекаем входные данные на вшивость
            if (message == null)
            {
                return true;
            }
            if (message.username == null || message.password == null)
            {
                return true;
            }
            if (message.username.Length < 3 || message.password.Length < 3)
            {
                return true;
            }
            return false;
        }
    }
}