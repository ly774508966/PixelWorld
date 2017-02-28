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
require "notifyname"

-- register windows
facade = Facade:getInstance()
facade:registerProxy(require("model/bag_proxy").new())
facade:registerMediator(require("window/PanelLogin"))
facade:registerMediator(require("window/PanelMenu"))
facade:registerMediator(require("window/PanelBag"))
facade:registerMediator(require("window/PanelEquip"))
facade:registerMediator(require("window/PanelAlert"))
facade:registerMediator(require("window/PanelItemDetail"))

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
    CFG.items = json.decode(data)
    print ('items count ', #CFG.items)
    print (inspect(CFG.items))

    local data = resMgr:LoadAsset('Cfg/equip'):ToString()
    CFG.equips = json.decode(data)
    print ('equips count ', #CFG.equips)

    guiMgr:ShowWindow("PanelLogin", nil)
end

--销毁--
function Game.OnDestroy()
	print('OnDestroy--->>>')
end
