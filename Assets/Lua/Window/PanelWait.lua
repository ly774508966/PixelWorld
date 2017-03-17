local Sequence = DG.Tweening.Sequence
local Tweener = DG.Tweening.Tweener
local DOTween = DG.Tweening.DOTween
local gameObject
local transform
local sequence

PanelWait = {}
local this = PanelWait
this._name = 'PanelWait'

-- --------------------------------------------------------------------
--	c# callback
-----------------------------------------------------------------------
function PanelWait.Awake(obj)
	gameObject = obj
	transform = obj.transform
end

function PanelWait.OnDestroy()
	gameObject = nil
end

-- --------------------------------------------------------------------
--	mvc notication
-----------------------------------------------------------------------
function PanelWait:listNotificationInterests()
	return {}
end
function PanelWait:handleNotification(notification)
end

function PanelWait:show()
	if gameObject then
		gameObject:SetActive(true)
	else 
    	facade:sendNotification(OPEN_WINDOW, {name="PanelWait"})
	end

	local img = transform:Find("Image")
	rot = img:DORotate(Vector3.New(0, 0, -360), 1, DG.Tweening.RotateMode.FastBeyond360)
	rot:SetLoops(-1)

	-- 10s timer
	sequence = DOTween.Sequence()
	sequence:AppendInterval(2)
	sequence:AppendCallback(DG.Tweening.TweenCallback(function ()
		this.hide()
		
    	--facade:sendNotification(OPEN_WINDOW, {name="PanelAlert", data={lanMgr:GetValue('TITLE_TIP'), lanMgr:GetValue('NETWORK_TIMEOUT')}})
	end))
	sequence:Play()
end

function PanelWait:hide()
	if gameObject then gameObject:SetActive(false) end
	if sequence then sequence:Kill(true) end
end

return PanelWait