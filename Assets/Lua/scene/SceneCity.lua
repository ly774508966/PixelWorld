--[[
	city场景脚本
--]]
local GameObject = UnityEngine.GameObject

SceneCity = {}
local TAG = "SceneCity"

local gameObject
local transform

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function SceneCity.Awake(obj)
	print(TAG, 'Awake')

	gameObject = obj
	transform = obj.transform

    facade:sendNotification(OPEN_WINDOW, {name="PanelBattle"})

end

function SceneCity.OnDestroy()
	print(TAG, 'OnDestroy')
end