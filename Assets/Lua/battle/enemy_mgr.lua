--[[
	enemy生成、管理、销毁
--]]
local GameObject = UnityEngine.GameObject
local Sequence = DG.Tweening.Sequence
local Tweener = DG.Tweening.Tweener
local DOTween = DG.Tweening.DOTween
local UpdateBeat = UpdateBeat

local enemy_mgr = {}
local TAG = "enemy_mgr"
local this = enemy_mgr

local gameObject
local transform

function enemy_mgr.init()

	this.UID = 0
	this.enemys = {}

	-- update
	UpdateBeat:Add(this.Update, this)
end

function enemy_mgr.create(id)

	local monster = chMgr:AddEnemy(1001, math.random(2, 40), 0, math.random(2, 10))
	monster.ID = this.UID
	this.UID = this.UID + 1

	local transform = monster.transform

	local prefab_bar = resMgr:LoadAsset('UI/Widget/HealthBar')
	local bar = GameObject.Instantiate(prefab_bar)
	bar.transform:SetParent(battle.canvas)
	bar.transform.localScale = Vector3.one
	local follow = bar:GetComponent('Follow')
	follow.target = transform
	follow.offset = Vector3.New(0, 1, 0)

	local slider = bar.transform:Find('Slider'):GetComponent('Slider') 
	slider.value = 1

	this.enemys[monster.ID] = {monster, go, transform, bar, slider}
end

function enemy_mgr.Update()
	local n = 0
	for k in pairs(this.enemys) do
		if this.enemys[k] then n = n + 1 end
	end

	if n < 5 then
		this.create(1001)
	end
end

function enemy_mgr.get_enemy(id)
	local id = tonumber(id)
	return this.enemys[id]
end


function enemy_mgr.enemy_hit(id, attack)
	local id = tonumber(id)
	local enemy = this.enemys[id]
	if enemy == nil then return nil end
	
	if enemy[1].HP == 0 then return nil end

	local hp = math.max(0, enemy[1].HP - attack)
	enemy[1].HP = hp
	enemy[5].value = hp / 100

	if hp == 0 then 
		-- die, balance
		this.enemy_die(id)
	end

	return enemy
end

function enemy_mgr.enemy_die(id)
	local id = tonumber(id)
	local enemy = this.enemys[id]
	if enemy == nil then return end


	enemy[1]:ActDie()		
	
	-- remove bar
	GameObject.Destroy(enemy[4])

	local sequence = DOTween.Sequence()
	sequence:AppendInterval(2)
	sequence:AppendCallback(DG.Tweening.TweenCallback(function ()
		-- remove
		chMgr:Remove(enemy[1])
		this.enemys[id] = nil
	end))
	sequence:Play()
end


_G['enemy_mgr'] = enemy_mgr

return enemy_mgr