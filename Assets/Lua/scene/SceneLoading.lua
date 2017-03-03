--[[
	loading场景脚本
--]]

SceneLoading = {}
local TAG = "SceneLoading"

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function SceneLoading.Awake(obj)
	print(TAG, 'Awake')
end

function SceneLoading.OnDestroy()
	print(TAG, 'OnDestroy')
end