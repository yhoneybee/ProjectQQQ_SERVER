using System;
using System.Collections.Generic;
using Nettention.Proud;
using System.Linq;
using Newtonsoft.Json;
using ProjectQQQ_SERVER;
using MySql.Data.MySqlClient;

class Program
{
    public static NetServer netServer = new NetServer();
    public static S2C.Proxy proxy = new S2C.Proxy();
    public static C2S.Stub stub = new C2S.Stub();
    public static MySqlHandler mySql = new MySqlHandler("119.196.245.41", "3306", "test", "kkulbeol", "Rnfqjf2671!@#");

    private static void Main(string[] _)
    {
        var param = new StartServerParameter();
        param.tcpPorts.Add(6475);
        param.udpPorts.Add(6475);
        param.protocolVersion = new Nettention.Proud.Guid("{2256FFC1-99F9-48DA-8A27-E18D61954A00}");

        stub.SignUp = OnSignUp;
        stub.LogIn = OnLogIn;
        stub.ChatToAll = OnChatToAll;
        stub.ChatToRoom = OnChatToRoom;
        stub.ChatToPerson = OnChatToPerson;
        stub.CreateRoom = OnCreateRoom;
        stub.EnterRoom = OnEnterRoom;
        stub.ExitRoom = OnExitRoom;
        stub.GameReady = OnGameReady;
        stub.GameStart = OnGameStart;
        stub.RecordPosition = OnRecordPosition;
        stub.RecordRotation = OnRecordRotation;
        stub.GetRoomDatas = OnGetRoomDatas;
        stub.GetClientDatas = OnGetClientDatas;

        netServer.ClientJoinHandler = OnJoinServer;
        netServer.ClientLeaveHandler = OnLeaveServer;

        netServer.AttachProxy(proxy);
        netServer.AttachStub(stub);
        netServer.Start(param);

        Console.WriteLine("Server running...");

        //var admin1 = new Client(HostID.HostID_None, "admin1", "1234");
        //var admin2 = new Client(HostID.HostID_None, "admin2", "1234");
        //K.clients.Add(admin1);
        //K.clients.Add(admin2);

        //var room = new Room("admins", "1234");
        //room.clients.Add(admin1);
        //room.clients.Add(admin2);
        //admin1.roomNum = admin2.roomNum = room.id = -1;
        //K.rooms.Add(room);

        // TODO: ���⼭ DB�о ���Ͱ��� Ŭ���̾�Ʈ �߰��������

        //mySql.InsertUser("kkulbeol", "123", HostID.HostID_None);

        K.roomIDs = K.GetRandomInt(100, 0, 100);

        K.users = mySql.SelectUser();
        Console.WriteLine($"------------------------------------------");
        Console.WriteLine($"- Users ({K.users.Count}) ------------------------------");
        foreach (var user in K.users)
        {
            Console.WriteLine($"user : {user.ID}");
        }
        K.rooms = mySql.SelectRoom();
        Console.WriteLine($"- Rooms ({K.rooms.Count}) ------------------------------");
        foreach (var room in K.rooms)
        {
            Console.WriteLine($"room : {room.name}");
            K.roomIDs.Remove(room.id);
        }
        foreach (var ru in mySql.SelectRoomUser())
        {
            var user = K.users.Find(x => x.ID == ru.userID);
            var room = K.rooms.Find(x => x.id == ru.roomID);
            if (user != null && room != null)
            {
                user.roomID = ru.roomID;
                room.users.Add(user);
                Console.WriteLine($"user({user.ID}) IN --> room({room.name})");
            }
        }

        Console.WriteLine($"------------------------------------------");

        Console.WriteLine("Server is run.");
        while (true)
        {

        }
    }

    private static bool OnSignUp(HostID remote, RmiContext rmiContext, string id, string pw, string confirmPw)
    {
        var find = K.users.Find(x => x.ID == id);
        bool isSuccess = false;
        if (find == null)
        {
            User user = new User(remote, id, pw);
            isSuccess = pw == confirmPw;
            if (isSuccess)
            {
                mySql.InsertUser(id, pw, remote);
                user.PW = mySql.SelectUser($"ID = {id} and PW = md5('{pw}')")[0].PW;
                K.users.Add(user);
                Console.WriteLine($"signup : {user.ID}");
            }
        }
        proxy.SignUpResult(remote, rmiContext, id, isSuccess);
        return true;
    }

    private static bool OnLogIn(HostID remote, RmiContext rmiContext, string id, string pw)
    {
        var find = K.users.Find(x => x.ID == id);
        bool isSuccess = false;
        if (find! != null)
        {
            var user = mySql.SelectUser($"ID = {id} and PW = md5('{pw}')");
            isSuccess = user.Count == 1;
            if (isSuccess)
            {
                find.hostID = remote;
                mySql.UpdateUser(id, remote);
                Console.WriteLine($"login : {find.ID}");
            }
        }
        proxy.LoginResult(remote, rmiContext, id, isSuccess);
        return true;
    }

    private static bool OnChatToAll(HostID remote, RmiContext rmiContext, string id, string chat)
    {
        var find = K.users.Find(x => x.ID == id);
        Console.WriteLine($"{chat}");
        foreach (var user in K.users)
        {
            if (find!.ID == user.ID) continue;
            proxy.EchoToAll(user.hostID, rmiContext, id, chat);
        }
        return true;
    }

    private static bool OnChatToRoom(HostID remote, RmiContext rmiContext, string id, string roomId, string chat)
    {
        return true;
    }

    private static bool OnChatToPerson(HostID remote, RmiContext rmiContext, string id, string targetId, string chat)
    {
        return true;
    }

    private static bool OnCreateRoom(HostID remote, RmiContext rmiContext, string id, string roomName, string pw)
    {
        int roomId = K.roomIDs[0];
        K.roomIDs.RemoveAt(0);
        var find = K.rooms.Find(x => x.name == roomName);
        proxy.CreateRoomResult(remote, rmiContext, id, roomId.ToString(), find == null);
        if (find != null) return false;
        Room room = new Room(roomName, roomId, pw);
        K.rooms.Add(room);
        mySql.InsertRoom(roomName, pw, roomId);
        room.pw = mySql.SelectRoom($"ID = {roomId} and PW = md5('{pw}')")[0].pw;

        return true;
    }

    private static bool OnEnterRoom(HostID remote, RmiContext rmiContext, string id, string roomName, string pw)
    {
        var room = K.rooms.Find(x => x.id.ToString() == roomName);
        var user = K.users.Find(x => x.ID == id);
        var roomUser = room!.users.Find(x => x.ID == user!.ID);
        proxy.EnterRoomResult(remote, rmiContext, id, roomName, user!.roomID == 0 && room != null && user != null && roomUser == null);
        if (user!.roomID != 0 || roomUser != null || room == null || user == null) return false;
        user!.roomID = room!.id;
        room!.users.Add(user!);
        mySql.InsertRoomUser(room!.id, id);
        return true;
    }

    private static bool OnExitRoom(HostID remote, RmiContext rmiContext, string id, string roomId)
    {
        var user = K.users.Find(x => x.ID == id);

        if (user == null) return false;

        int iRoomID = Convert.ToInt32(roomId);
        mySql.DeleteRoomUser($"UserID = '{id}' and RoomID = {iRoomID}");
        var roomUsers = mySql.SelectRoomUser($"RoomID = {iRoomID}");
        proxy.ExitRoomResult(remote, rmiContext, id, roomId, user != null);
        if (roomUsers.Count > 0) return true;

        user.roomID = 0;
        K.rooms.RemoveAll(x => x.id == iRoomID);
        mySql.DeleteRoom($"ID = {iRoomID}");
        K.roomIDs.Add(iRoomID);

        return true;
    }

    private static bool OnGameReady(HostID remote, RmiContext rmiContext, string id)
    {
        return true;
    }

    private static bool OnGameStart(HostID remote, RmiContext rmiContext, string id)
    {
        return true;
    }

    private static bool OnRecordPosition(HostID remote, RmiContext rmiContext, string id, float x, float y, float z)
    {
        var user = K.users.Find(x => x.ID == id);
        if (user! != null)
        {
            var room = K.rooms.Find(x => x.id == user.roomID);
            user.pX = x;
            user.pY = y;
            user.pZ = z;
            foreach (var roomUser in room!.users)
            {
                if (roomUser.ID == user.ID) continue;
                proxy.NotifyPosition(roomUser.hostID, rmiContext, id, x, y, z);
            }
        }
        return true;
    }

    private static bool OnRecordRotation(HostID remote, RmiContext rmiContext, string id, float x, float y, float z)
    {
        var user = K.users.Find(x => x.ID == id);
        if (user! != null)
        {
            var room = K.rooms.Find(x => x.id == user.roomID);
            user.rX = x;
            user.rY = y;
            user.rZ = z;
            foreach (var roomUser in room!.users)
            {
                if (roomUser.ID == user.ID) continue;
                proxy.NotifyRotation(roomUser.hostID, rmiContext, id, x, y, z);
            }
        }
        return true;
    }

    private static bool OnGetRoomDatas(HostID remote, RmiContext rmiContext, string id)
    {
        string json = JsonConvert.SerializeObject(new Serialization<Room>(K.rooms));
        proxy.GetRoomDatas(remote, rmiContext, json);
        return true;
    }

    private static bool OnGetClientDatas(HostID remote, RmiContext rmiContext, string id)
    {
        string json = JsonConvert.SerializeObject(new Serialization<User>(K.users));
        proxy.GetClientDatas(remote, rmiContext, json);
        return true;
    }

    private static void OnJoinServer(NetClientInfo clientInfo)
    {
        Console.WriteLine("JOIN");
        return;
    }

    private static void OnLeaveServer(NetClientInfo clientInfo, ErrorInfo errorinfo, ByteArray comment)
    {
        Console.WriteLine("LEAVE");
        var find = K.users.Find(x => x.hostID == clientInfo.hostID);
        OnExitRoom(find!.hostID, RmiContext.ReliableSend, find.ID, find.roomID.ToString());
        return;
    }
}
