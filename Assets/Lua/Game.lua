--[[
    游戏总入口，自更新完成后加载
    加载各个游戏模块（network, manager, 所有面板等）
]]
local lpeg = require "lpeg"

json = require "cjson"

-- manager
guiMgr = GUIManager.GetInstance()
resMgr = ResourceManager.GetInstance()
lanMgr = LanguageManager.GetInstance()
sceneMgr = SceneManager.GetInstance()
networkMgr = NetworkManager.GetInstance()

cfgMgr = require 'CfgManager'

require "protocol"
require "network"

-- register windows
require "Window/PanelLogin"
require "Window/PanelMenu"
require "Window/PanelAlert"
require "Window/PanelBag"
require "Window/PanelEquip"
require "Window/PanelItemDetail"


WWW = UnityEngine.WWW
GameObject = UnityEngine.GameObject


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
    GameConfig.SocketPort = 8001
    GameConfig.SocketAddress = "127.0.0.1"
    
    networkMgr:OnInit()
    networkMgr:SendConnect()

    cfgMgr.Init()

    guiMgr:ShowWindow("PanelLogin", nil)
end

--销毁--
function Game.OnDestroy()
	print('OnDestroy--->>>')
end
