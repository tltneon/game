using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {
        // Рабочий код
        [OperationContract]
        string SendAuthData(AuthData data);

        [OperationContract]
        IEnumerable<StatEntity> GetUserList();

        [OperationContract]
        string BaseAction(BaseAction msg);
        // Тестовый код
        [OperationContract]
        string GetData(int value);
        
        [OperationContract]
        string SendData(string username, string password);

        [OperationContract]
        AuthData GetDummyUserData();

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Добавьте здесь операции служб
    }


    // Используйте контракт данных, как показано в примере ниже, чтобы добавить составные типы к операциям служб.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    [DataContract]
    public class AuthData
    {
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; set; }
    }
    [DataContract]
    public class StatEntity
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string Playername { get; set; }
        [DataMember]
        public int Wins { get; set; }
        [DataMember]
        public int Loses { get; set; }
    }
    [DataContract]
    public class BaseAction
    {
        [DataMember]
        public int baseid { get; set; }
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public string result { get; set; }
        [DataMember]
        public string token { get; set; }
    }
}