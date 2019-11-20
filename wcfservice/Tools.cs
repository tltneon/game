using AutoMapper;
using gamelogic;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace wcfservice
{

    /// <summary>
    /// Умные функции для работы с кодом
    /// </summary>
    class Tools
    {
        /// <summary>
        /// Маппер для самостоятельных энтити
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TDestination SmartMapper<TSource, TDestination>(TSource obj) // мульти-маппер-функция на полную ставку
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<TSource, TDestination>(obj);
        }

        /// <summary>
        /// Маппер для энтити в коллекции
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<TDestination> EnumSmartMapper<TSource, TDestination>(IEnumerable<TSource> obj)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(obj);
        }

        /// <summary>
        /// Проверяет пользовательствий ввод на нуль + валидный токен
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string CheckAuthedInput(dynamic obj)
        {
            if (obj == null)
            {
                return "nodatareceived";
            }
            if (obj.token == null)
            {
                return "notokenreceived";
            }
            const string pattern = @"[^a-zA-ZА-Яа-я0-9!№%*@#$^]";
            if (Regex.IsMatch(obj.token, pattern))
            {
                return "wrongdatareceived";
            }
            if (!AccountManager.CheckToken(obj.token))
            {
                return "wrongtoken";
            }
            return "passed";
        }
    }
}