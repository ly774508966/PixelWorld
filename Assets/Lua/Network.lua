
require "protocol"
--Event = require 'events'

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
        local id = data:ReadInt()
        local name = data:ReadString()
        local f = data:ReadFloat()
        print(id, name, f)
        sceneMgr:GotoScene(SceneID.Main)
    elseif key == Protocol.ACK_ENTER then
    end
end

-- 登录
function Network.login(name, pwd)
    local buffer = ByteBuffer.New()
    buffer:WriteShort(Protocol.REQ_LOGIN)
    buffer:WriteString(name)
    buffer:WriteString(pwd)
    networkMgr:SendMessage(buffer)
end

--二进制登录--
function Network.TestLoginBinary(buffer)
	local protocal = buffer:ReadByte()
	local str = buffer:ReadString()
	log('TestLoginBinary: protocal:>'..protocal..' str:>'..str)
end

--PBLUA登录--
function Network.TestLoginPblua(buffer)
	local protocal = buffer:ReadByte()
	local data = buffer:ReadBuffer()

    local msg = login_pb.LoginResponse()
    msg:ParseFromString(data)
	log('TestLoginPblua: protocal:>'..protocal..' msg:>'..msg.id)
end

--卸载网络监听--
function Network.Unload()
    logWarn('Unload Network...')
end