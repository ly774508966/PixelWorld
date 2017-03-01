
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
    print("Network.Start")
end

function Network.Reconnect()
    print("Network.Reconnect")
    -- 可多次尝试
    networkMgr:SendConnect(CONFIG_SOCKET_IP, CONFIG_SOCKET_PORT)
    facade:sendNotification(WAIT, {name="show"})
end

--当连接建立时--
function Network.OnConnect()
    facade:sendNotification(WAIT, {name="hide"})
    islogging = true
end

--当连接失败时--
function Network.OnRefuse()
    facade:sendNotification(WAIT, {name="hide"})

    local data = {lanMgr:GetValue('NETWORK'), lanMgr:GetValue('NETWORK_FAIL'), function (ret) if ret == 1 then this.Reconnect() end end}
    facade:sendNotification(OPEN_WINDOW, {name="PanelAlert", data=data})
end

--异常断线--
function Network.OnException() 
    facade:sendNotification(WAIT, {name="hide"})
    islogging = false
    local data = {lanMgr:GetValue('NETWORK'), lanMgr:GetValue('NETWORK_FAIL'), function (ret) if ret == 1 then this.Reconnect() end end}
    facade:sendNotification(OPEN_WINDOW, {name="PanelAlert", data=data})
end

--连接中断，或者被踢掉--
function Network.OnDisconnect()
    facade:sendNotification(WAIT, {name="hide"})
    islogging = false
    local data = {lanMgr:GetValue('NETWORK'), lanMgr:GetValue('NETWORK_FAIL'), function (ret) if ret == 1 then this.Reconnect() end end}
    facade:sendNotification(OPEN_WINDOW, {name="PanelAlert", data=data})
end

--Socket消息--
function Network.OnMessage(key, data)
    --print(key, data)
    
    if key == Protocol.ACK_MESSAGE then
    elseif key == Protocol.ACK_NOTICE then
    else 
        -- ui response
        facade:sendNotification(WAIT, {name="hide"})

        if key == Protocol.ACK_LOGIN then
            local user = main_pb.User()
            user:ParseFromString(data:ReadBuffer())
            print(user.id, user.name, user.lv, user.exp, user.coin)
            facade:retrieveProxy("BagProxy"):parse(user.item)
            sceneMgr:GotoScene(SceneID.Main)
        elseif key == Protocol.ACK_ENTER then
        elseif key == Protocol.ACK_SELL then
            facade:sendNotification(BAG_SELL_OK, data:ReadInt())
        end
    end
end

-- 登录
function Network.login(name, pwd)
    facade:sendNotification(WAIT, {name="show"})
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
    facade:sendNotification(WAIT, {name="show"})
    local buffer = ByteBuffer.New()
    buffer:WriteShort(Protocol.REQ_SELL)
    buffer:WriteInt(id)
    networkMgr:SendMessage(buffer)
end


--卸载网络监听--
function Network.Unload()
    logWarn('Unload Network...')
end