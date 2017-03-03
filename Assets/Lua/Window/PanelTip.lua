local GameObject = UnityEngine.GameObject
local Sequence = DG.Tweening.Sequence
local Tweener = DG.Tweening.Tweener
local DOTween = DG.Tweening.DOTween
local gameObject
local transform
local sequence

PanelTip = {}
local this = PanelTip
this._name = 'PanelTip'

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelTip.Awake(obj)
	gameObject = obj
	transform = obj.transform
end

function PanelTip.OnDestroy()
	gameObject = nil
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelTip:listNotificationInterests()
	return {}
end
function PanelTip:handleNotification(notification)
end

function PanelTip:show_tip(...)
	local msg = ...
	print("show_tip", msg)
	if gameObject == nil then
    	facade:sendNotification(OPEN_WINDOW, {name="PanelTip"})
	end

	local go = GameObject('tip', typeof(UnityEngine.UI.Text))
	go.transform:SetParent(transform)
	go.transform.localScale = Vector3.one
	go.transform.localPosition = Vector3.zero
	local text = go:GetComponent("Text")
	text.font = fontArial
	text.fontSize = 30
	text.alignment = UnityEngine.TextAnchor.MiddleCenter
	text.text = msg
	text.raycastTarget = false
	local rt = go:GetComponent('RectTransform')
	rt.sizeDelta = Vector2.New(500, 40)

	-- move
	local sequence = DOTween.Sequence()
	move = rt:DOMoveY(400, 2, false)
	sequence:Append(move)
	sequence:AppendCallback(DG.Tweening.TweenCallback(function ()
		-- remove
		GameObject.Destroy(go)
	end))
	sequence:Play()

	-- alpha
	text:CrossFadeAlpha(0, 2, true)
end


return PanelTip