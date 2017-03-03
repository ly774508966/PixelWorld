--[[
	主场景脚本
--]]

SceneMain = {}
local TAG = "SceneMain"

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function SceneMain.Awake(obj)
	print(TAG, 'Awake')
    facade:sendNotification(OPEN_WINDOW, {name="PanelMain"})
    facade:sendNotification(OPEN_WINDOW, {name="PanelMenu"})
end

function SceneMain.OnDestroy()
	print(TAG, 'OnDestroy')
end