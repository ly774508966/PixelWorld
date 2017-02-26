
require "network/protocol"
--Event = require 'events'

-- pb
require "proto/logic_pb"
require "proto/main_pb"

Network = {}
local this = Network

local transform
local gameObject
local islogging = false

function Network.Start() 
    print("Network.Start!")
end

--当连接建立时--
function Network.OnConnect() 
    Util.LogWarning("Network.OnConnect!")
end

--异常断线--
function Network.OnException() 
    islogging = false
    networkMgr:SendConnect()
   	Util.LogError("OnException------->>>>")
end

--连接中断，或者被踢掉--
function Network.OnDisconnect() 
    islogging = false
    Util.LogError("OnDisconnect------->>>>")
end

--Socket消息--
function Network.OnMessage(key, data)
    print(key, data)
    
    if key == Protocol.ACK_LOGIN then
        local user = main_pb.User()
        user:ParseFromString(data:ReadBuffer())
        print(user.id, user.name, user.lv, user.exp, user.coin)
        sceneMgr:GotoScene(SceneID.Main)
    elseif key == Protocol.ACK_ENTER then
    elseif key == Protocol.ACK_SELL then
        facade:sendNotification(BAG_SELL_OK, data:ReadInt())
    end
end

-- 登录
function Network.login(name, pwd)
    local login = logic_pb.LoginRequest()
    login.name = name
    login.pwd = pwd
    
    local msg = login:SerializeToString()

    local buffer = ByteBuffer.New()
    buffer:WriteShort(Protocol.REQ_LOGIN)
    buffer:WriteBuffer(msg)
    networkMgr:SendMessage(buffer)
end
-- 出售
function Network.sell(id)
    local buffer = ByteBuffer.New()
    buffer:WriteShort(Protocol.REQ_SELL)
    buffer:WriteInt(id)
    networkMgr:SendMessage(buffer)
end


--卸载网络监听--
function Network.Unload()
    logWarn('Unload Network...')
end