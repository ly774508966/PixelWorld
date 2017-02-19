local lpeg = require "lpeg"

json = require "cjson"
     

-- register windows
require "Window/PanelLogin"
require "Window/PanelMenu"
require "Window/PanelAlert"
require "Window/PanelBag"
require "Window/PanelEquip"


WWW = UnityEngine.WWW
GameObject = UnityEngine.GameObject


-- manager
guiMgr = GUIManager.GetInstance()
resMgr = ResourceManager.GetInstance()
lanMgr = LanguageManager.GetInstance()
sceneMgr = SceneManager.GetInstance()

cfgMgr = require 'CfgManager'

--管理器--
Game = {}
local this = Game

local game
local transform
local gameObject
local WWW = UnityEngine.WWW


--初始化完成(自更新)
function Game.OnInitOK()
    print('Game Init OK ...')
    GameConfig.SocketPort = 2012
    GameConfig.SocketAddress = "127.0.0.1"
    --networkMgr:SendConnect()

    cfgMgr.Init()

    guiMgr:ShowWindow("PanelLogin", nil)
end

--销毁--
function Game.OnDestroy()
	print('OnDestroy--->>>')
end
