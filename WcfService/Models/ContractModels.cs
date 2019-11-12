using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string SendAuthData(AuthData data);

        [OperationContract]
        IEnumerable<StatEntity> GetPlayerList();

        [OperationContract]
        IEnumerable<BaseEntity> GetBaseList();

        [OperationContract]
        string BaseAction(BaseAction msg);

        [OperationContract]
        IEnumerable<StructureEntity> GetBaseStructures(BaseAction msg);

        [OperationContract]
        BaseEntity GetBaseInfo(BaseAction msg);

        [OperationContract]
        SquadEntity GetBaseSquad(SquadAction obj);

        [OperationContract]
        IEnumerable<SquadEntity> GetSquads(SquadAction obj);

        [OperationContract]
        string SquadAction(SquadAction obj);

        [OperationContract]
        string DbStatus();
    }

    // дата контракты V
    [DataContract]
    public abstract class WithToken
    {
        [DataMember]
        public string token { get; set; }
    }

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
    public class BaseEntity
    {
        [DataMember]
        public int BaseID { get; set; }
        [DataMember]
        public string Basename { get; set; }
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int OwnerID { get; set; }
        [DataMember]
        public IEnumerable<StructureEntity> Structures { get; set; }
        [DataMember]
        public object[] Resources { get; set; }
        [DataMember]
        public object[] Units { get; set; }
    }
    [DataContract]
    public class StructureEntity
    {
        [DataMember]
        public int BaseID { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public int Level { get; set; }
    }
    [DataContract]
    public class SquadEntity
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public int OwnerID { get; set; }
        [DataMember]
        public int MoveFrom { get; set; }
        [DataMember]
        public int StartTime { get; set; }
        [DataMember]
        public int MoveTo { get; set; }
        [DataMember]
        public int FinishTime { get; set; }
        [DataMember]
        public object[] Units { get; set; }
    }
    [DataContract]
    public class BaseAction : WithToken
    {
        [DataMember]
        public int baseid { get; set; }
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public string result { get; set; }
    }
    [DataContract]
    public class SquadAction : WithToken
    {
        [DataMember]
        public string key { get; set; }
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public int to { get; set; }
    }
}