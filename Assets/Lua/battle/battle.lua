--[[
	战斗管理器
--]]
require "battle/effect_mgr"

local GameObject = UnityEngine.GameObject

battle = {}
local TAG = "battle"
this = battle

local gameObject
local transform

function battle.init(obj)
	gameObject = obj
	transform = obj.transform


	this.canvas = GameObject.Find("Canvas Battle").transform
	this.canvas_top = GameObject.Find("Canvas Battle Top").transform
	local go_camera = GameObject.Find("Camera")
	this.camera = go_camera:GetComponent('Camera')
	local lockview = go_camera:GetComponent('LockViewCameraController')

	this.UID = 0
	this.player = nil
	this.characters = {}

	this.init_scene()
	this.init_player()
	this.init_monsters()

	lockview:SetTarget(this.player)
end

function battle.init_scene()
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

function battle.init_player()
	local prefab = resMgr:LoadAsset('Prefabs/Character/king')

    local go = GameObject.Instantiate(prefab)
	go.transform.localScale = Vector3.one
	go.transform.localPosition = Vector3.New(0, 0, 0)
	player = go:GetComponent("Player")
	player.ID = this.UID
	this.UID = this.UID + 1

	this.player = go.transform

	this.characters[player.ID] = {go, player}
end

function battle.init_monsters() 

	local prefab = resMgr:LoadAsset('Prefabs/Monster/1001')
	local prefab_bar = resMgr:LoadAsset('UI/Widget/HealthBar')
	for i = 0, 0 do
	    local go = GameObject.Instantiate(prefab)
		go.transform.localScale = Vector3.one
		go.transform.localPosition = Vector3.New(math.random(-5, 5), 0, math.random(-5, 5))
		monster = go:GetComponent("Monster")
		monster.ID = this.UID
		this.UID = this.UID + 1

		local bar = GameObject.Instantiate(prefab_bar)
		bar.transform:SetParent(this.canvas)
		bar.transform.localScale = Vector3.one
		follow = bar:GetComponent('Follow')
		follow.target = go.transform
		follow.offset = Vector3.New(0, 1, 0)

		this.characters[monster.ID] = {go, monster, bar}
	end
end



function battle.hit(id, attackid)
	print("hit", id, attackid)
	local character = this.characters[id]
	print(character)
	local pos = character[1].transform.position
	effect_mgr.create_hit(pos, math.random(-100, -10))
end

function battle.destroy()
	-- body
end