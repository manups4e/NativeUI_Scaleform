MidMessageInstance = setmetatable({}, MidMessageInstance)
MidMessageInstance.__index = MidMessageInstance
MidMessageInstance.__call = function()
    return "MidMessageInstance"
end

---@class MidMessageInstance
---@field public _sc Scaleform
---@field public _start number
---@field public _timer number
---@field public _hasAnimatedOut boolean
---@field public New fun():MidMessageInstance
---@field public Load fun(self:MidMessageInstance):nil
---@field public Dispose fun(self:MidMessageInstance, force:boolean):nil
---@field public Update fun(self:MidMessageInstance):nil

---Creates a new MidMessageInstance object
---@return MidMessageInstance
function MidMessageInstance.New()
    local _sc = nil --[[@type Scaleform]]
    local _start = 0
    local _timer = 0
    local _hasAnimatedOut = false
    local data = { _sc = _sc, _start = _start, _timer = _timer, _hasAnimatedOut = _hasAnimatedOut }
    return setmetatable(data, MidMessageInstance)
end

---Loads the MIDSIZED_MESSAGE scaleform
function MidMessageInstance:Load()
    if self._sc ~= nil then return end
    self._sc = Scaleform.Request("MIDSIZED_MESSAGE")
    local timeout = 1000
    local start = GlobalGameTimer
    while not self._sc:IsLoaded() and GlobalGameTimer - start < timeout do Citizen.Wait(0) end
end

---Dispose the MIDSIZED_MESSAGE scaleform
function MidMessageInstance:Dispose()
    self._sc:Dispose()
    self._sc = nil
end

---Shows a message on the screen
---@param msg string
---@param desc string
---@param bgColor Colours
---@param useDarkerShard boolean
---@param useCondensedShard boolean
---@param time number|nil - The time in milliseconds the message should be shown for (default: 5000)
function MidMessageInstance:ShowColoredShard(msg, desc, bgColor, useDarkerShard, useCondensedShard, time)
    if time == nil then time = 5000 end
    self:Load()
    self._start = GlobalGameTimer
    self._sc:CallFunction("SHOW_SHARD_MIDSIZED_MESSAGE", false, msg, desc, bgColor, useDarkerShard, useCondensedShard)
    self._timer = time
    self._hasAnimatedOut = false
end

---Shows a message on the screen
function MidMessageInstance:Update()
    self._sc:Render2D()
    if self._start ~= 0 and GlobalGameTimer - self._start > self._timer then
        if not self._hasAnimatedOut then
            self._sc:CallFunction("SHARD_ANIM_OUT", false, 21, 750)
            self._hasAnimatedOut = true
            self._timer = self._timer + 750
        else
            PlaySoundFrontend(-1, "Shard_Disappear", "GTAO_FM_Events_Soundset", true)
            self._start = 0
            self:Dispose()
        end
    end
end
