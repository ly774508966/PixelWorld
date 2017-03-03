--[[
	战斗场景脚本
--]]

SceneBattle = {}
local TAG = "SceneBattle"

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function SceneBattle.Awake(obj)
	print(TAG, 'Awake')
    facade:sendNotification(OPEN_WINDOW, {name="PanelBattle"})
end

function SceneBattle.OnDestroy()
	print(TAG, 'OnDestroy')
end