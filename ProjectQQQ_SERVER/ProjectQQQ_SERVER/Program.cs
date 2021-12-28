using System;
using System.Collections.Generic;
using Nettention.Proud;
using System.Linq;
using SERVER;

class Program
{
    public static NetServer netServer = new NetServer();
    public static S2C.Proxy proxy = new S2C.Proxy();
    public static C2S.Stub stub = new C2S.Stub();

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
        stub.GameReady = OnGameReady;
        stub.GameStart = OnGameStart;
        stub.RecordPosition = OnRecordPosition;

        netServer.ClientJoinHandler = OnJoinServer;
        netServer.ClientLeaveHandler = OnLeaveServer;

        netServer.AttachProxy(proxy);
        netServer.AttachStub(stub);
        netServer.Start(param);

        Console.WriteLine("Server running...");
        var admin1 = new Client(HostID.HostID_None, "admin1", "1234");
        var admin2 = new Client(HostID.HostID_None, "admin2", "1234");
        K.clients.Add(admin1);
        K.clients.Add(admin2);

        var room = new Room("admins", "1234");
        room.clients.Add(admin1);
        room.clients.Add(admin2);
        admin1.roomNum = admin2.roomNum = room.num = -1;
        K.rooms.Add(room);

        // TODO: 여기서 DB읽어서 위와같이 클라이언트 추가해줘야함

        while (true)
        {

        }
    }

    private static bool OnSignUp(HostID remote, RmiContext rmiContext, string id, string nickName, string pw, string confirmPw)
    {
        var find = K.clients.Find(x => x.ID == id);
        bool isSuccess = false;
        if (find == null)
        {
            Client client = new Client(remote, id, pw);
            isSuccess = pw == confirmPw;
            if (isSuccess) K.clients.Add(client);
        }
        proxy.SignUpResult(remote, rmiContext, id, isSuccess);
        return true;
    }

    private static bool OnLogIn(HostID remote, RmiContext rmiContext, string id, string pw)
    {
        var find = K.clients.Find(x => x.ID == id);
        bool isSuccess = false;
        if (find! != null)
        {
            find.hostID = remote;
            isSuccess = find!.ID == id && find.PW == pw;
        }
        proxy.LoginResult(remote, rmiContext, id, isSuccess);
        return true;
    }

    private static bool OnChatToAll(HostID remote, RmiContext rmiContext, string id, string chat)
    {
        var find = K.clients.Find(x => x.ID == id);
        Console.WriteLine($"( ALL )[ {find!.ID} ] : {chat}");
        foreach (var client in K.clients)
            proxy.EchoToAll(client.hostID, rmiContext, id, chat);
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
        return true;
    }

    private static bool OnGameReady(HostID remote, RmiContext rmiContext, string id, string roomName, bool isReady)
    {
        return true;
    }

    private static bool OnGameStart(HostID remote, RmiContext rmiContext, string id, string roomName)
    {
        return true;
    }

    private static bool OnRecordPosition(HostID remote, RmiContext rmiContext, string id, float x, float y, float z)
    {

        return true;
    }

    private static void OnJoinServer(NetClientInfo clientInfo)
    {
        Console.WriteLine("JOIN");
        return;
    }

    private static void OnLeaveServer(NetClientInfo clientInfo, ErrorInfo errorinfo, ByteArray comment)
    {
        return;
    }
}