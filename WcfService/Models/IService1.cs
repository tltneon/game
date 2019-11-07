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
        // Тестовый код
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        int UpgradeBase(int value);

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
        public string username { get; set; }
        [DataMember]
        public string wins { get; set; }
        [DataMember]
        public string loses { get; set; }
        [DataMember]
        public string basename { get; set; }
        [DataMember]
        public string baselevel { get; set; }
    }
}