--[[
	战斗场景脚本
--]]
local GameObject = UnityEngine.GameObject

SceneBattle = {}
local TAG = "SceneBattle"

local gameObject
local transform

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function SceneBattle.Awake(obj)
	print(TAG, 'Awake')

	gameObject = obj
	transform = obj.transform

	battle.init(obj)

    facade:sendNotification(OPEN_WINDOW, {name="PanelBattle"})

end

function SceneBattle.OnDestroy()
	print(TAG, 'OnDestroy')
	battle.destroy()
end