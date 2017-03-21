--[[
	战斗管理器
--]]
require "battle/enemy_mgr"
require "battle/effect_mgr"

local GameObject = UnityEngine.GameObject

battle = {}
local TAG = "battle"
local this = battle

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
	this.player_tf = nil

	this.init_scene()
	this.init_player()

	lockview:SetTarget(this.player_tf)

	-- enemy
	enemy_mgr.init()
end

function battle.init_scene()
	--	new prefab in scene
	local prefab = resMgr:LoadAsset('Prefabs/Scene/Box2')
	for i = 0, 3 do
	    local go = GameObject.Instantiate(prefab)
		go.transform:SetParent(transform)
		go.transform.localScale = Vector3.one
		go.transform.localPosition = Vector3.New(math.random(2, 40), 0, math.random(1, 10))
	end
end

function battle.init_player()
	local prefab = resMgr:LoadAsset('Prefabs/Character/king')

    local go = GameObject.Instantiate(prefab)
	go.transform.localScale = Vector3.one
	go.transform.localPosition = Vector3.New(5, 0, 5)

	this.player = go:GetComponent("Player")
	this.player.ID = this.UID
	this.UID = this.UID + 1

	this.player_tf = go.transform
end

function battle.player_hit(id, attackid)
	print("player_hit", id, attackid)

	if this.player.HP == 0 then return end

	-- calculate attack
	local attack = math.random(1, 10)
	local hp = math.max(0, this.player.HP - attack)
	this.player.HP = hp

	if hp == 0 then 
		-- die, balance
		this.player:ActDie()

		local function balance( ... )
			facade:sendNotification(OPEN_WINDOW, {name="PanelBattleBalance"})
		end
		local timer = Timer.New(balance, 2, 0, true)
		timer:Start()
	end

    facade:sendNotification(BATTLE_HP_CHANGE, {hp=hp})

	-- effect
	local pos = this.player_tf.position + Vector3.New(0, 1, 0)
	effect_mgr.create_hit(pos, -attack)
	
end

function battle.enemy_hit(id, attackid)
	print("enemy_hit", id, attackid)

	-- calculate attack
	local attack = math.random(10, 30)

	local enemy = enemy_mgr.enemy_hit(id, attack)
	if enemy then
		-- effect
		local pos = enemy[3].position + Vector3.New(0, 1, 0)
		effect_mgr.create_hit(pos, -attack)
	end

end

function battle.destroy()
	-- body
end