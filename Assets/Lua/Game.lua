local lpeg = require "lpeg"

local json = require "cjson"
local util = require "cjson/util"
     

-- windows
require "Window/PanelLogin"
require "Window/PanelMenu"
require "Window/PanelAlert"
require "Window/PanelBag"


WWW = UnityEngine.WWW
GameObject = UnityEngine.GameObject


-- manager
guiMgr = GUIManager.GetInstance()
cfgMgr = CfgManager.GetInstance()
resMgr = ResourceManager.GetInstance()
lanMgr = LanguageManager.GetInstance()
sceneMgr = SceneManager.GetInstance()

--管理器--
Game = {}
local this = Game

local game
local transform
local gameObject
local WWW = UnityEngine.WWW


--初始化完成，发送链接服务器信息--
function Game.OnInitOK()
    GameConfig.SocketPort = 2012
    GameConfig.SocketAddress = "127.0.0.1"
    --networkMgr:SendConnect()

    print('Game InitOK--->>>')
end


--销毁--
function Game.OnDestroy()
	print('OnDestroy--->>>')
end
