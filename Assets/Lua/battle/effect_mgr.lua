--[[
	战斗管理器
--]]
local GameObject = UnityEngine.GameObject
local Sequence = DG.Tweening.Sequence
local Tweener = DG.Tweening.Tweener
local DOTween = DG.Tweening.DOTween
local gameObject

local effect_mgr = {}
local TAG = "effect_mgr"

local gameObject
local transform
local ui_root

function effect_mgr.init()

end

function effect_mgr.create_hit_label(wpos, num)
	local pos = battle.camera:WorldToScreenPoint(wpos)

	local prefab = resMgr:LoadAsset('UI/Widget/CritNum')
    local go = GameObject.Instantiate(prefab)
	go.transform:SetParent(battle['canvas_top'])
	go.transform.localScale = Vector3.one
	go.transform.position = pos
	text = go:GetComponent("Text")
	text.text = num

	-- move
	local rt = go:GetComponent('RectTransform')
	local sequence = DOTween.Sequence()
	move = rt:DOMoveY(pos.y+50, 1, false)
	sequence:Append(move)
	sequence:AppendCallback(DG.Tweening.TweenCallback(function ()
		-- remove
		GameObject.Destroy(go)
	end))
	sequence:Play()

	-- alpha
	text:CrossFadeAlpha(0, 1, true)
end

function effect_mgr.create_hit(parent)
	
	local prefab = resMgr:LoadAsset('Prefabs/Effect/Hit')
    local go = GameObject.Instantiate(prefab)
	go.transform:SetParent(parent)
	go.transform.localScale = Vector3.one
	go.transform.localPosition = Vector3.New(0, 0.5, 0)
	--GameObject.Destroy(go, 0.5)
end

_G['effect_mgr'] = effect_mgr

return effect_mgr