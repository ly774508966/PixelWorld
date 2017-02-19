--[[
	CfgManager -配置管理
]]

local CfgManager = {}
local this = CfgManager

function CfgManager.Init()
	print ('CfgManager.Init')

	this.items = {}
	this.equips = {}

	local data = resMgr:LoadAsset('Cfg/item'):ToString()
	local obj = json.decode(data)
	this.items = obj
	print ('items count ', #this.items)

	local data = resMgr:LoadAsset('Cfg/equip'):ToString()
	local obj = json.decode(data)
	this.equips = obj
	print ('equips count ', #this.equips)
end

function CfgManager.Items()
	-- body
end


return CfgManager