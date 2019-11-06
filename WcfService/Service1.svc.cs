using System;
using System.Collections.Generic;
using AutoMapper;
using gamelogic;

namespace WcfService
{
    public class Service1 : IService1
    {
        public string SendData(string username, string password)
        {
            bool test = false;
            try
            {
                test = TestLogic.CreateUser(username, password);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("damn err");
            }
            return string.Format("Registering user {0} with pass {1}. Result:" + test, username, password);
        }
        public string SendAuthData(AuthData data)
        {
            bool test = false;
            try
            {
                test = TestLogic.AuthClient(Mapper.Map<EditUserViewModel, User>(model));
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("damn err");
            }
            return string.Format("Registering user {0} with pass {1}. Result:" + test, data.username, data.password);
        }
        public AuthData GetDummyUserData() { 
            return new AuthData { username = "testuser", password = "testpass" };
        }
        public string GetData(int value)
        {
            bool test = false;
            try
            {
                test = TestLogic.CreateUser("noshit","bull");
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("damn err");
            }
            return string.Format("Registering user {0} with pass {1}. Salt: {2}. Result:" + test, "noshit", "bull", value);
        }
        public int UpgradeBase(int baseid)
        {
            int test = 0;
            try
            {
                test = TestLogic.UpgradeBase(baseid);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("damn err");
            }
            return test;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}