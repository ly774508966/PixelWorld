--[[
    游戏总入口，自更新完成后加载
    加载各个游戏模块（network, manager, 所有面板等）
]]
json = require "cjson"
inspect = require "core/inspect"

-- manager
guiMgr = GUIManager.GetInstance()
resMgr = ResourceManager.GetInstance()
lanMgr = LanguageManager.GetInstance()
sceneMgr = SceneManager.GetInstance()
networkMgr = NetworkManager.GetInstance()

require "config"
require "network/network"
require "core/fsm"
require "core/mvc"

-- register windows
require "window/PanelLogin"
require "window/PanelMenu"
--require "window/PanelAlert"
require "window/PanelBag"
require "window/PanelEquip"
require "window/PanelItemDetail"


facade = Facade:getInstance()
facade:registerMediator(require("window/PanelAlert"))

-- cfg
CFG = {}

--管理器--
Game = {}
local this = Game

local transform
local gameObject
local WWW = UnityEngine.WWW


--初始化完成(自更新)
function Game.OnInitOK()
    print('Game Init OK ...')
    
    networkMgr:OnInit()
    networkMgr:SendConnect(CONFIG_SOCKET_IP, CONFIG_SOCKET_PORT)

    local data = resMgr:LoadAsset('Cfg/item'):ToString()
    this.items = json.decode(data)
    print ('items count ', #this.items)

    local data = resMgr:LoadAsset('Cfg/equip'):ToString()
    this.equips = json.decode(data)
    print ('equips count ', #this.equips)

    guiMgr:ShowWindow("PanelLogin", nil)
end

--销毁--
function Game.OnDestroy()
	print('OnDestroy--->>>')
end
