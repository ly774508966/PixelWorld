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

    canvas = GameObject.Find("Canvas Battle")

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

	local prefab = resMgr:LoadAsset('Prefabs/Monster/1001')
	local prefab_bar = resMgr:LoadAsset('UI/Widget/HealthBar')
	for i = 0, 0 do
	    local go = GameObject.Instantiate(prefab)
		go.transform:SetParent(transform)
		go.transform.localScale = Vector3.one
		go.transform.localPosition = Vector3.New(math.random(-5, 5), 0, math.random(-5, 5))
		monster = go:GetComponent("Monster")
		monster.ID = i

		local bar = GameObject.Instantiate(prefab_bar)
		bar.transform:SetParent(canvas.transform)
		bar.transform.localScale = Vector3.one
		follow = bar:GetComponent('Follow')
		follow.target = go.transform
		follow.offset = Vector3.New(0, 1, 0)
	end

end

function SceneBattle.OnDestroy()
	print(TAG, 'OnDestroy')
end