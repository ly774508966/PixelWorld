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

    facade:sendNotification(OPEN_WINDOW, {name="PanelBattle"})


    --	new prefab in scene	
	local prefab = resMgr:LoadAsset('Prefabs/Scene/Barrel1')
	for i = 0, 10 do
	    local go = GameObject.Instantiate(prefab)
		go.transform:SetParent(transform)
		go.transform.localScale = Vector3.one
		go.transform.localPosition = Vector3.New(math.random(-5, 5), 0.4, math.random(-5, 5))
	end

	local prefab = resMgr:LoadAsset('Prefabs/Scene/Box2')
	for i = 0, 10 do
	    local go = GameObject.Instantiate(prefab)
		go.transform:SetParent(transform)
		go.transform.localScale = Vector3.one
		go.transform.localPosition = Vector3.New(math.random(-5, 5), 0, math.random(-5, 5))
	end
end

function SceneBattle.OnDestroy()
	print(TAG, 'OnDestroy')
end