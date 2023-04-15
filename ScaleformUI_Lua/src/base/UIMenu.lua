UIMenu = setmetatable({}, UIMenu)
UIMenu.__index = UIMenu
UIMenu.__call = function()
    return "UIMenu"
end

---@class UIMenu
---@field public Title string -- Sets the menu title
---@field public Subtitle string -- Sets the menu subtitle
---@field public AlternativeTitle boolean -- Enable or disable the alternative title (default: false)
---@field public Position vector2 -- Sets the menu position (default: { X = 0.0, Y = 0.0 })
---@field public Pagination table -- Menu pagination settings (default: { Min = 0, Max = 7, Total = 7 })
---@field public Banner table -- Menu banner settings [Setting not fully understood or possibly not used]
---@field public Extra table -- {}
---@field public Description table -- {}
---@field public Items table -- {}
---@field public Windows table -- {}
---@field public Children table -- {}
---@field public Glare boolean -- Sets if the glare animation is enabled or disabled (default: false)
---@field public Controls table -- { Back = { Enabled = true }, Select = { Enabled = true }, Up = { Enabled = true }, Down = { Enabled = true }, Left = { Enabled = true }, Right = { Enabled = true } }
---@field public TxtDictionary string -- Texture dictionary for the menu banner background (default: commonmenu)
---@field public TxtName string -- Texture name for the menu banner background (default: interaction_bgd)
---@field public Logo Sprite -- nil
---@field public Settings table -- Defines the menus settings
---@field public MaxItemsOnScreen fun(max: number) -- Sets the maximum number of items that can be displayed (default: 7)
---@field public AddItem fun(item: UIMenuItem)
---@field public SetParentMenu fun(menu: UIMenu)
---@field public OnIndexChange fun(menu: UIMenu, newindex: number)
---@field public OnListChange fun(menu: UIMenu, list: UIMenuListItem, newindex: number)
---@field public OnSliderChange fun(menu: UIMenu, slider: UIMenuSliderItem, newindex: number)
---@field public OnProgressChange fun(menu: UIMenu, progress: UIMenuProgressItem, newindex: number)
---@field public OnCheckboxChange fun(menu: UIMenu, checkbox: UIMenuCheckboxItem, checked: boolean)
---@field public OnListSelect fun(menu: UIMenu, item: UIMenuItem, index: number)
---@field public OnSliderSelect fun(menu: UIMenu, item: UIMenuItem, index: number)
---@field public OnStatsSelect fun(menu: UIMenu, item: UIMenuItem, index: number)
---@field public OnItemSelect fun(menu: UIMenu, item: UIMenuItem, checked: boolean)
---@field public OnMenuChanged fun(oldmenu: UIMenu, newmenu: UIMenu, change: any)
---@field public BuildAsync fun(enabled: boolean) -- Sets if the menu should be built async (default: false)
---@field public AnimationEnabled fun(enabled: boolean) -- Sets if the menu animation is enabled or disabled (default: true)
---@field public AnimationType fun(type: MenuAnimationType) -- Sets the animation type for the menu (default: MenuAnimationType.LINEAR)
---@field public BuildingAnimation fun(type: MenuBuildingAnimation) -- Sets the build animation type for the menu (default: MenuBuildingAnimation.LEFT)
---@field private counterColor Colours -- Set the counter color (default: Colours.HUD_COLOUR_FREEMODE)
---@field private enableAnimation boolean -- Enable or disable the menu animation (default: true)
---@field private animationType MenuAnimationType -- Sets the menu animation type (default: MenuAnimationType.LINEAR)
---@field private buildingAnimation MenuBuildingAnimation -- Sets the menu building animation type (default: MenuBuildingAnimation.NONE)
---@field private descFont table -- { "$Font2", 0 }

---Creates a new UIMenu.
---@param title string -- Menu title
---@param subTitle string -- Menu subtitle
---@param x number|nil -- Menu Offset X position
---@param y number|nil -- Menu Offset Y position
---@param glare boolean|nil -- Menu glare effect
---@param txtDictionary string|nil -- Custom texture dictionary for the menu banner background (default: commonmenu)
---@param txtName string|nil -- Custom texture name for the menu banner background (default: interaction_bgd)
---@param alternativeTitleStyle boolean|nil -- Use alternative title style (default: false)
function UIMenu.New(title, subTitle, x, y, glare, txtDictionary, txtName, alternativeTitleStyle)
    local X, Y = tonumber(x) or 0, tonumber(y) or 0
    if title ~= nil then
        title = tostring(title) or ""
    else
        title = ""
    end
    if subTitle ~= nil then
        subTitle = tostring(subTitle) or ""
    else
        subTitle = ""
    end
    if txtDictionary ~= nil then
        txtDictionary = tostring(txtDictionary) or "commonmenu"
    else
        txtDictionary = "commonmenu"
    end
    if txtName ~= nil then
        txtName = tostring(txtName) or "interaction_bgd"
    else
        txtName = "interaction_bgd"
    end
    if alternativeTitleStyle == nil then
        alternativeTitleStyle = false
    end
    local _UIMenu = {
        Title = title,
        Subtitle = subTitle,
        AlternativeTitle = alternativeTitleStyle,
        counterColor = Colours.HUD_COLOUR_FREEMODE,
        Position = { x = X, y = Y },
        Pagination = { Min = 0, Max = 7, Total = 7 },
        enableAnimation = true,
        animationType = MenuAnimationType.LINEAR,
        buildingAnimation = MenuBuildingAnimation.NONE,
        descFont = { "$Font2", 0 },
        Extra = {},
        Description = {},
        Items = {},
        Windows = {},
        Children = {},
        TxtDictionary = txtDictionary,
        TxtName = txtName,
        Glare = glare or false,
        Logo = nil,
        _keyboard = false,
        _changed = false,
        _maxItem = 7,
        _menuGlare = 0,
        _canBuild = true,
        _buildAsync = true,
        _isBuilding = false,
        _time = 0,
        _times = 0,
        _delay = 150,
        _canHe = true,
        _scaledWidth = (720 * GetAspectRatio(false)),
        Controls = {
            Back = {
                Enabled = true,
            },
            Select = {
                Enabled = true,
            },
            Left = {
                Enabled = true,
            },
            Right = {
                Enabled = true,
            },
            Up = {
                Enabled = true,
            },
            Down = {
                Enabled = true,
            },
        },
        ParentMenu = nil,
        ParentPool = nil,
        ParentItem = nil,
        _Visible = false,
        ActiveItem = 0,
        Dirty = false,
        ReDraw = true,
        InstructionalButtons = {
            InstructionalButton.New(GetLabelText("HUD_INPUT2"), -1, 176, 176, -1),
            InstructionalButton.New(GetLabelText("HUD_INPUT3"), -1, 177, 177, -1)
        },
        OnIndexChange = function(menu, newindex)
        end,
        OnListChange = function(menu, list, newindex)
        end,
        OnSliderChange = function(menu, slider, newindex)
        end,
        OnProgressChange = function(menu, progress, newindex)
        end,
        OnCheckboxChange = function(menu, item, checked)
        end,
        OnListSelect = function(menu, list, index)
        end,
        OnSliderSelect = function(menu, slider, index)
        end,
        OnProgressSelect = function(menu, progress, index)
        end,
        OnStatsSelect = function(menu, progress, index)
        end,
        OnItemSelect = function(menu, item, index)
        end,
        OnMenuChanged = function(oldmenu, newmenu, change)
        end,
        OnColorPanelChanged = function(menu, item, index)
        end,
        OnPercentagePanelChanged = function(menu, item, index)
        end,
        OnGridPanelChanged = function(menu, item, index)
        end,
        Settings = {
            InstructionalButtons = true,
            MultilineFormats = true,
            ScaleWithSafezone = true,
            ResetCursorOnOpen = true,
            MouseControlsEnabled = true,
            MouseEdgeEnabled = true,
            ControlDisablingEnabled = true,
            Audio = {
                Library = "HUD_FRONTEND_DEFAULT_SOUNDSET",
                UpDown = "NAV_UP_DOWN",
                LeftRight = "NAV_LEFT_RIGHT",
                Select = "SELECT",
                Back = "BACK",
                Error = "ERROR",
            },
            EnabledControls = {
                Controller = {
                    { 0, 2 },  -- Look Up and Down
                    { 0, 1 },  -- Look Left and Right
                    { 0, 25 }, -- Aim
                    { 0, 24 }, -- Attack
                    { 0, 71 }, -- Accelerate Vehicle
                    { 0, 72 }, -- Vehicle Brake
                    { 0, 30 }, -- Move Left and Right
                    { 0, 31 }, -- Move Up and Down
                    { 0, 59 }, -- Move Vehicle Left and Right
                    { 0, 75 }, -- Exit Vehicle
                },
                Keyboard = {
                    { 0, 2 },   -- Look Up and Down
                    { 0, 1 },   -- Look Left and Right
                    { 0, 201 }, -- Select
                    { 0, 195 }, -- X axis
                    { 0, 196 }, -- Y axis
                    { 0, 187 }, -- Down
                    { 0, 188 }, -- Up
                    { 0, 189 }, -- Left
                    { 0, 190 }, -- Right
                    { 0, 202 }, -- Back
                    { 0, 217 }, -- Select
                    { 0, 242 }, -- Scroll down
                    { 0, 241 }, -- Scroll up
                    { 0, 239 }, -- Cursor X
                    { 0, 240 }, -- Cursor Y
                    { 0, 237 },
                    { 0, 238 },
                    { 0, 31 }, -- Move Up and Down
                    { 0, 30 }, -- Move Left and Right
                    { 0, 21 }, -- Sprint
                    { 0, 22 }, -- Jump
                    { 0, 23 }, -- Enter
                    { 0, 75 }, -- Exit Vehicle
                    { 0, 71 }, -- Accelerate Vehicle
                    { 0, 72 }, -- Vehicle Brake
                    { 0, 59 }, -- Move Vehicle Left and Right
                    { 0, 89 }, -- Fly Yaw Left
                    { 0, 9 },  -- Fly Left and Right
                    { 0, 8 },  -- Fly Up and Down
                    { 0, 90 }, -- Fly Yaw Right
                    { 0, 76 }, -- Vehicle Handbrake
                },
            },
        }
    }

    if subTitle ~= "" and subTitle ~= nil then
        _UIMenu.Subtitle = subTitle
    end
    if (_UIMenu._menuGlare == 0) then
        _UIMenu._menuGlare = Scaleform.Request("mp_menu_glare")
    end
    return setmetatable(_UIMenu, UIMenu)
end

function UIMenu:Title(title)
    if title == nil then
        return self.Title
    else
        self.Title = title
        if self:Visible() then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_TITLE_SUBTITLE", false, self.Title, self.Subtitle,
                self.alternativeTitle)
        end
    end
end

function UIMenu:DescriptionFont(fontTable)
    if fontTable == nil then
        return self.descFont
    else
        self.descFont = fontTable
        if self:Visible() then
            ScaleformUI.Scaleforms._ui:CallFunction("SET_DESC_FONT", false, self.descFont[1], self.descFont[2])
        end
    end
end

function UIMenu:Subtitle(sub)
    if sub == nil then
        return self.Subtitle
    else
        self.Subtitle = sub
        if self:Visible() then
            ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_TITLE_SUBTITLE", false, self.Title, self.Subtitle,
                self.alternativeTitle)
        end
    end
end

function UIMenu:CounterColor(color)
    if color == nil then
        return self.counterColor
    else
        self.counterColor = color
        if self:Visible() then
            ScaleformUI.Scaleforms._ui:CallFunction("SET_COUNTER_COLOR", false, self.counterColor)
        end
    end
end

---DisEnableControls
---@param bool boolean
function UIMenu:DisEnableControls(bool)
    if bool then
        EnableAllControlActions(2)
    else
        EnableAllControlActions(2)
    end

    if bool then
        return
    else
        if not IsUsingKeyboard(2) then
            for Index = 1, #self.Settings.EnabledControls.Controller do
                EnableControlAction(self.Settings.EnabledControls.Controller[Index][1],
                    self.Settings.EnabledControls.Controller[Index][2], true)
            end
        else
            for Index = 1, #self.Settings.EnabledControls.Keyboard do
                EnableControlAction(self.Settings.EnabledControls.Keyboard[Index][1],
                    self.Settings.EnabledControls.Keyboard[Index][2], true)
            end
        end
    end
end

--- Set's if the menu can build asynchronously.
---@param enabled boolean|nil
---@return boolean
function UIMenu:BuildAsync(enabled)
    if enabled ~= nil then
        self._buildAsync = enabled
    end
    return self._buildAsync
end

---InstructionalButtons
---@param enabled boolean|nil
---@return boolean
function UIMenu:HasInstructionalButtons(enabled)
    if enabled ~= nil then
        self.Settings.InstructionalButtons = ToBool(enabled)
    end
    return self.Settings.InstructionalButtons
end

--- Sets if the menu can be closed by the player.
---@param playerCanCloseMenu boolean|nil
---@return boolean
function UIMenu:CanPlayerCloseMenu(playerCanCloseMenu)
    if playerCanCloseMenu ~= nil then
        self._canHe = playerCanCloseMenu
    end
    return self._canHe
end

---Sets if controls are disabled when the menu is open. (Default: true) [Documentation requires updating]
---@param enabled boolean|nil
---@return boolean
function UIMenu:ControlDisablingEnabled(enabled)
    if enabled ~= nil then
        self.Settings.ControlDisablingEnabled = ToBool(enabled)
    end
    return self.Settings.ControlDisablingEnabled
end

---Enables or disables mouse controls for the menu.
---@param enabled boolean|nil
---@return boolean
function UIMenu:MouseControlsEnabled(enabled)
    if enabled ~= nil then
        self.Settings.MouseControlsEnabled = ToBool(enabled)
        ScaleformUI.Scaleforms._ui:CallFunction("ENABLE_MOUSE", false, self.Settings.MouseControlsEnabled)
    end
    return self.Settings.MouseControlsEnabled
end

---SetBannerSprite
---@param sprite Sprite
---@param includeChildren boolean
---@see Sprite
function UIMenu:SetBannerSprite(sprite, includeChildren)
    if sprite() == "Sprite" then
        self.Logo = sprite
        self.Logo:Size(431 + self.WidthOffset, 107)
        self.Logo:Position(self.Position.X, self.Position.Y)
        self.Banner = nil
        if includeChildren then
            for item, menu in pairs(self.Children) do
                menu.Logo = sprite
                menu.Logo:Size(431 + self.WidthOffset, 107)
                menu.Logo:Position(self.Position.X, self.Position.Y)
                menu.Banner = nil
            end
        end
    end
end

--- Enables or disabls the menu's animations while the menu is visible.
---@param enable boolean|nil
---@return boolean
function UIMenu:AnimationEnabled(enable)
    if enable ~= nil then
        self.enableAnimation = enable
        if self:Visible() then
            ScaleformUI.Scaleforms._ui:CallFunction("ENABLE_SCROLLING_ANIMATION", false, enable)
        end
    end
    return self.enableAnimation
end

--- Sets the menu's scrolling animationType while the menu is visible.
---@param menuAnimationType MenuAnimationType|nil
---@return number MenuAnimationType
---@see MenuAnimationType
function UIMenu:AnimationType(menuAnimationType)
    if menuAnimationType ~= nil then
        self.animationType = menuAnimationType
        if self:Visible() then
            ScaleformUI.Scaleforms._ui:CallFunction("CHANGE_SCROLLING_ANIMATION_TYPE", false, menuAnimationType)
        end
    end

    return self.animationType
end

--- Enables or disables the menu's building animationType.
---@param buildingAnimationType MenuBuildingAnimation|nil
---@return MenuBuildingAnimation
---@see MenuBuildingAnimation
function UIMenu:BuildingAnimation(buildingAnimationType)
    if buildingAnimationType ~= nil then
        self.buildingAnimation = buildingAnimationType
        if self:Visible() then
            ScaleformUI.Scaleforms._ui:CallFunction("CHANGE_BUILDING_ANIMATION_TYPE", false, buildingAnimationType)
        end
    end
    return self.buildingAnimation
end

---CurrentSelection
---@param value number|nil
function UIMenu:CurrentSelection(value)
    if value ~= nil then
        if #self.Items == 0 then
            self.ActiveItem = value
            return
        end
        self.Items[self:CurrentSelection()]:Selected(false)
        self.ActiveItem = 1000000 - (1000000 % #self.Items) + tonumber(value)
        self.Items[self:CurrentSelection()]:Selected(true)
        ScaleformUI.Scaleforms._ui:CallFunction("SET_CURRENT_ITEM", false, self:CurrentSelection() - 1)
    else
        if #self.Items == 0 then
            return 1
        else
            if self.ActiveItem % #self.Items == 0 then
                return 1
            else
                return (self.ActiveItem % #self.Items) + 1
            end
        end
    end
end

---AddWindow
---@param window table
function UIMenu:AddWindow(window)
    if window() == "UIMenuWindow" then
        window:SetParentMenu(self)
        self.Windows[#self.Windows + 1] = window
    end
end

---RemoveWindowAt
---@param Index number
function UIMenu:RemoveWindowAt(Index)
    if tonumber(Index) then
        if self.Windows[Index] then
            table.remove(self.Windows, Index)
        end
    end
end

--- Add a new item to the menu.
---@param item UIMenuItem
---@see UIMenuItem
function UIMenu:AddItem(item)
    if item() ~= "UIMenuItem" then
        return
    end
    item:SetParentMenu(self)
    self.Items[#self.Items + 1] = item
end

---RemoveItemAt
---@param Index number
function UIMenu:RemoveItemAt(Index)
    if tonumber(Index) then
        if self.Items[Index] then
            local SelectedItem = self:CurrentSelection()
            table.remove(self.Items, tonumber(Index))
            if self:Visible() then
                ScaleformUI.Scaleforms._ui:CallFunction("REMOVE_ITEM", false, Index - 1) -- scaleform index starts at 0, better remove 1 to the index
            end
            self:CurrentSelection(SelectedItem)
        end
    end
end

---RemoveItem
---@param item table
function UIMenu:RemoveItem(item)
    local idx = 0
    for k, v in pairs(self.Items) do
        if v:Label() == item:Label() then
            idx = k
        end
    end
    if idx > 0 then
        self:RemoveItemAt(idx)
    end
end

---RefreshIndex
function UIMenu:RefreshIndex()
    if #self.Items == 0 then
        self.ActiveItem = 0
        self.Pagination.Max = self.Pagination.Total + 1
        self.Pagination.Min = 0
        return
    end
    self.Items[self:CurrentSelection()]:Selected(false)
    self.ActiveItem = 1000 - (1000 % #self.Items)
    self.Pagination.Max = self.Pagination.Total + 1
    self.Pagination.Min = 0
end

---Clear
function UIMenu:Clear()
    self.Items = {}
end

---MaxItemsOnScreen
---@param max number|nil
function UIMenu:MaxItemsOnScreen(max)
    if max == nil then
        return self._maxItem
    end

    self._maxItem = max
    self:RefreshIndex()
end

---Adds a submenu to the menu, and returns the submenu.
---@param subMenu UIMenu
---@param text string
---@param description string
---@param offset table|nil
---@param KeepBanner boolean|nil
---@return table UIMenu
function UIMenu:AddSubMenu(subMenu, text, description, offset, KeepBanner)
    if subMenu() ~= "UIMenu" then
        print("^1ScaleformUI [ERROR]: You're trying to add a submenu [" ..
            subMenu.Title .. "] to a menu [" .. self.Title .. "] but it's not a menu!^7")
        return subMenu
    end

    assert(subMenu ~= self,
        "^1ScaleformUI [ERROR]: You're can't add a menu [" .. subMenu.Title .. "] as a redundant submenu to itself!")
    for k, v in pairs(self.Children) do
        assert(subMenu ~= v,
            "^1ScaleformUI [ERROR]: You can't add the same submenu [" .. subMenu.Title .. "] more than once!")
    end
    ---@diagnostic disable-next-line: missing-parameter
    local Item = UIMenuItem.New(tostring(text), description or "")
    self:AddItem(Item)
    if offset == nil then
        subMenu.Position = self.Position
    else
        subMenu.Position = offset
    end
    if KeepBanner then
        if self.Logo ~= nil then
            subMenu.Logo = self.Logo
        else
            subMenu.Logo = nil
            subMenu.Banner = self.Banner
        end
    end
    subMenu.Glare = self.Glare

    subMenu.Settings.MouseControlsEnabled = self.Settings.MouseControlsEnabled
    subMenu.Settings.MouseEdgeEnabled = self.Settings.MouseEdgeEnabled
    subMenu.MaxItemsOnScreen(self:MaxItemsOnScreen())
    subMenu.BuildAsync(self:BuildAsync())
    subMenu.AnimationEnabled(self:AnimationEnabled())
    subMenu.AnimationType(self:AnimationType())
    subMenu.BuildingAnimation(self:BuildingAnimation())
    self.ParentPool:Add(subMenu)
    self:BindMenuToItem(subMenu, Item)
    return subMenu
end

---Visible
---@param bool boolean|nil
function UIMenu:Visible(bool)
    if bool ~= nil then
        self._Visible = ToBool(bool)
        self.JustOpened = ToBool(bool)
        self.Dirty = ToBool(bool)

        if self.ParentMenu ~= nil then return end

        if #self.Children > 0 and self.Children[self.Items[self:CurrentSelection()]] ~= nil and self.Children[self.Items[self:CurrentSelection()]]:Visible() then return end
        if bool then
            ScaleformUI.Scaleforms.InstructionalButtons:SetInstructionalButtons(self.InstructionalButtons)
            self.OnMenuChanged(nil, self, "opened")
            if self:BuildAsync() then
                self:BuildUpMenuAsync()
            else
                self:BuildUpMenuSync()
            end
            self.ParentPool.currentMenu = self
            self.ParentPool:ProcessMenus(true)
        else
            self.OnMenuChanged(self, nil, "closed")
            ScaleformUI.Scaleforms._ui:CallFunction("CLEAR_ALL", false)
            self.ParentPool.currentMenu = nil
            self.ParentPool:ProcessMenus(false)
        end
        ScaleformUI.Scaleforms.InstructionalButtons:Enabled(bool)
        if self.Settings.ResetCursorOnOpen then
            local W, H = GetScreenResolution()
            SetCursorLocation(W / 2, H / 2)
        end
    else
        return self._Visible
    end
end

---BuildUpMenu
function UIMenu:BuildUpMenuAsync()
    Citizen.CreateThread(function()
        self._isBuilding = true
        local enab = self:AnimationEnabled()
        self:AnimationEnabled(false)
        while not ScaleformUI.Scaleforms._ui:IsLoaded() do Citizen.Wait(0) end
        ScaleformUI.Scaleforms._ui:CallFunction("CREATE_MENU", false, self.Title, self.Subtitle, 0, 0,
            self.AlternativeTitle, self.TxtDictionary, self.TxtName, self:MaxItemsOnScreen(), self:BuildAsync(),
            self:AnimationType(), self:BuildingAnimation(), self.counterColor, self.descFont[1], self.descFont[2])
        if #self.Windows > 0 then
            for w_id, window in pairs(self.Windows) do
                local Type, SubType = window()
                if SubType == "UIMenuHeritageWindow" then
                    ScaleformUI.Scaleforms._ui:CallFunction("ADD_WINDOW", false, window.id, window.Mom, window.Dad)
                elseif SubType == "UIMenuDetailsWindow" then
                    ScaleformUI.Scaleforms._ui:CallFunction("ADD_WINDOW", false, window.id, window.DetailBottom,
                        window.DetailMid, window.DetailTop, window.DetailLeft.Txd, window.DetailLeft.Txn,
                        window.DetailLeft.Pos.x, window.DetailLeft.Pos.y, window.DetailLeft.Size.x,
                        window.DetailLeft.Size.y)
                    if window.StatWheelEnabled then
                        for key, value in pairs(window.DetailStats) do
                            ScaleformUI.Scaleforms._ui:CallFunction("ADD_STATS_DETAILS_WINDOW_STATWHEEL", false,
                                window.id, value.Percentage, value.HudColor)
                        end
                    end
                end
            end
        end
        local timer = GetGameTimer()
        if #self.Items == 0 then
            while #self.Items == 0 do
                Citizen.Wait(0)
                if GetGameTimer() - timer > 150 then
                    self.ActiveItem = 0
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_CURRENT_ITEM", false, self.ActiveItem)
                    return
                end
            end
        end
        local items = self.Items
        local it = 1
        while it <= #items do
            Citizen.Wait(50)
            if not self:Visible() then return end
            local item = items[it]
            local Type, SubType = item()
            AddTextEntry("desc_{" .. it .. "}", item:Description())

            if SubType == "UIMenuListItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 1, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), table.concat(item.Items, ","), item:Index() - 1,
                    item.Base._mainColor, item.Base._highlightColor, item.Base._textColor,
                    item.Base._highlightedTextColor)
            elseif SubType == "UIMenuDynamicListItem" then -- dynamic list item are handled like list items in the scaleform.. so the type remains 1
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 1, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item:CurrentListItem(), 0, item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor)
            elseif SubType == "UIMenuCheckboxItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 2, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item.CheckBoxStyle, item._Checked, item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor)
            elseif SubType == "UIMenuSliderItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 3, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item._Max, item._Multiplier, item:Index(),
                    item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor, item.SliderColor,
                    item._heritage)
            elseif SubType == "UIMenuProgressItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 4, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item._Max, item._Multiplier, item:Index(),
                    item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor, item.SliderColor)
            elseif SubType == "UIMenuStatsItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 5, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item:Index(), item._Type, item._Color, item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor)
            elseif SubType == "UIMenuSeperatorItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 6, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item.Jumpable, item.Base._mainColor,
                    item.Base._highlightColor,
                    item.Base._textColor, item.Base._highlightedTextColor)
            else
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 0, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item._mainColor, item._highlightColor, item._textColor,
                    item._highlightedTextColor)
                ScaleformUI.Scaleforms._ui:CallFunction("SET_RIGHT_LABEL", false, it - 1, item:RightLabel())
                if item._rightBadge ~= BadgeStyle.NONE then
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_RIGHT_BADGE", false, it - 1, item._rightBadge)
                end
            end

            if (SubType == "UIMenuItem" and item._leftBadge ~= BadgeStyle.NONE) or (SubType ~= "UIMenuItem" and item.Base._leftBadge ~= BadgeStyle.NONE) then
                if SubType ~= "UIMenuItem" then
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_ITEM_LABEL_FONT", false, it - 1, item.Base._labelFont
                        [1], item.Base._labelFont[2])
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_LEFT_BADGE", false, it - 1, item.Base._leftBadge)
                else
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_ITEM_LABEL_FONT", false, it - 1, item._labelFont[1],
                        item._labelFont[2])
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_LEFT_BADGE", false, it - 1, item._leftBadge)
                end
            end
            if #item.Panels > 0 then
                for pan, panel in pairs(item.Panels) do
                    local pType, pSubType = panel()
                    if pSubType == "UIMenuColorPanel" then
                        if panel.CustomColors ~= nil then
                            ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 0, panel.Title,
                                panel.ColorPanelColorType, panel.value, table.concat(panel.CustomColors, ","))
                        else
                            ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 0, panel.Title,
                                panel.ColorPanelColorType, panel.value)
                        end
                    elseif pSubType == "UIMenuPercentagePanel" then
                        ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 1, panel.Title, panel.Min,
                            panel.Max, panel.Percentage)
                    elseif pSubType == "UIMenuGridPanel" then
                        ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 2, panel.TopLabel,
                            panel.RightLabel, panel.LeftLabel, panel.BottomLabel, panel._CirclePosition.x,
                            panel._CirclePosition.y, true, panel.GridType)
                    elseif pSubType == "UIMenuStatisticsPanel" then
                        ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 3)
                        if #panel.Items then
                            for key, stat in pairs(panel.Items) do
                                ScaleformUI.Scaleforms._ui:CallFunction("ADD_STATISTIC_TO_PANEL", false, it - 1, pan - 1,
                                    stat['name'], stat['value'])
                            end
                        end
                    end
                end
            end
            if item.SidePanel ~= nil then
                if item.SidePanel() == "UIMissionDetailsPanel" then
                    ScaleformUI.Scaleforms._ui:CallFunction("ADD_SIDE_PANEL_TO_ITEM", false, it - 1, 0,
                        item.SidePanel.PanelSide, item.SidePanel.TitleType, item.SidePanel.Title,
                        item.SidePanel.TitleColor,
                        item.SidePanel.TextureDict, item.SidePanel.TextureName)
                    for key, value in pairs(item.SidePanel.Items) do
                        ScaleformUI.Scaleforms._ui:CallFunction("ADD_MISSION_DETAILS_DESC_ITEM", false, it - 1,
                            value.Type, value.TextLeft, value.TextRight, value.Icon, value.IconColor, value.Tick)
                    end
                elseif item.SidePanel() == "UIVehicleColorPickerPanel" then
                    ScaleformUI.Scaleforms._ui:CallFunction("ADD_SIDE_PANEL_TO_ITEM", false, it - 1, 1,
                        item.SidePanel.PanelSide, item.SidePanel.TitleType, item.SidePanel.Title,
                        item.SidePanel.TitleColor)
                end
            end
            it = it + 1
        end
        ScaleformUI.Scaleforms._ui:CallFunction("SET_CURRENT_ITEM", false, self:CurrentSelection() - 1)
        local type = self.Items[self.ActiveItem]
        if type == "UIMenuSeparatorItem" then
            if (self.Items[self.ActiveItem].Jumpable) then
                self:GoDown()
            end
        end
        ScaleformUI.Scaleforms._ui:CallFunction("ENABLE_MOUSE", false, self.Settings.MouseControlsEnabled)
        self:AnimationEnabled(enab)
        self._isBuilding = false
    end)
end

---BuildUpMenuSync
function UIMenu:BuildUpMenuSync()
    Citizen.CreateThread(function()
        while not ScaleformUI.Scaleforms._ui:IsLoaded() do Citizen.Wait(0) end
        ScaleformUI.Scaleforms._ui:CallFunction("CREATE_MENU", false, self.Title, self.Subtitle, 0, 0,
            self.AlternativeTitle, self.TxtDictionary, self.TxtName, self:MaxItemsOnScreen(), self:BuildAsync(),
            self:AnimationType(), self:BuildingAnimation(), self.counterColor, self.descFont[1], self.descFont[2])
        if #self.Windows > 0 then
            for w_id, window in pairs(self.Windows) do
                local Type, SubType = window()
                if SubType == "UIMenuHeritageWindow" then
                    ScaleformUI.Scaleforms._ui:CallFunction("ADD_WINDOW", false, window.id, window.Mom, window.Dad)
                elseif SubType == "UIMenuDetailsWindow" then
                    ScaleformUI.Scaleforms._ui:CallFunction("ADD_WINDOW", false, window.id, window.DetailBottom,
                        window.DetailMid, window.DetailTop, window.DetailLeft.Txd, window.DetailLeft.Txn,
                        window.DetailLeft.Pos.x, window.DetailLeft.Pos.y, window.DetailLeft.Size.x,
                        window.DetailLeft.Size.y)
                    if window.StatWheelEnabled then
                        for key, value in pairs(window.DetailStats) do
                            ScaleformUI.Scaleforms._ui:CallFunction("ADD_STATS_DETAILS_WINDOW_STATWHEEL", false,
                                window.id, value.Percentage, value.HudColor)
                        end
                    end
                end
            end
        end
        local timer = GetGameTimer()
        if #self.Items == 0 then
            while #self.Items == 0 do
                Citizen.Wait(0)
                if GetGameTimer() - timer > 150 then
                    self.ActiveItem = 0
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_CURRENT_ITEM", false, self.ActiveItem)
                    return
                end
            end
        end
        local items = self.Items

        for it, item in pairs(items) do
            local Type, SubType = item()
            AddTextEntry("desc_{" .. it .. "}", item:Description())

            if SubType == "UIMenuListItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 1, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), table.concat(item.Items, ","), item:Index() - 1,
                    item.Base._mainColor, item.Base._highlightColor, item.Base._textColor,
                    item.Base._highlightedTextColor)
            elseif SubType == "UIMenuDynamicListItem" then -- dynamic list item are handled like list items in the scaleform.. so the type remains 1
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 1, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item:CurrentListItem(), 0, item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor)
            elseif SubType == "UIMenuCheckboxItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 2, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item.CheckBoxStyle, item._Checked, item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor)
            elseif SubType == "UIMenuSliderItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 3, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item._Max, item._Multiplier, item:Index(),
                    item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor, item.SliderColor,
                    item._heritage)
            elseif SubType == "UIMenuProgressItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 4, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item._Max, item._Multiplier, item:Index(),
                    item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor, item.SliderColor)
            elseif SubType == "UIMenuStatsItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 5, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item:Index(), item._Type, item._Color, item.Base._mainColor,
                    item.Base._highlightColor, item.Base._textColor, item.Base._highlightedTextColor)
            elseif SubType == "UIMenuSeperatorItem" then
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 6, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item.Jumpable, item.Base._mainColor,
                    item.Base._highlightColor,
                    item.Base._textColor, item.Base._highlightedTextColor)
            else
                ScaleformUI.Scaleforms._ui:CallFunction("ADD_ITEM", false, 0, item:Label(), "desc_{" .. it .. "}",
                    item:Enabled(), item:BlinkDescription(), item._mainColor, item._highlightColor, item._textColor,
                    item._highlightedTextColor)
                ScaleformUI.Scaleforms._ui:CallFunction("SET_RIGHT_LABEL", false, it - 1, item:RightLabel())
                if item._rightBadge ~= BadgeStyle.NONE then
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_RIGHT_BADGE", false, it - 1, item._rightBadge)
                end
            end

            if (SubType == "UIMenuItem" and item._leftBadge ~= BadgeStyle.NONE) or (SubType ~= "UIMenuItem" and item.Base._leftBadge ~= BadgeStyle.NONE) then
                if SubType ~= "UIMenuItem" then
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_ITEM_LABEL_FONT", false, it - 1, item.Base._labelFont
                        [1], item.Base._labelFont[2])
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_LEFT_BADGE", false, it - 1, item.Base._leftBadge)
                else
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_ITEM_LABEL_FONT", false, it - 1, item._labelFont[1],
                        item._labelFont[2])
                    ScaleformUI.Scaleforms._ui:CallFunction("SET_LEFT_BADGE", false, it - 1, item._leftBadge)
                end
            end
            if #item.Panels > 0 then
                for pan, panel in pairs(item.Panels) do
                    local pType, pSubType = panel()
                    if pSubType == "UIMenuColorPanel" then
                        if panel.CustomColors ~= nil then
                            ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 0, panel.Title,
                                panel.ColorPanelColorType, panel.value, table.concat(panel.CustomColors, ","))
                        else
                            ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 0, panel.Title,
                                panel.ColorPanelColorType, panel.value)
                        end
                    elseif pSubType == "UIMenuPercentagePanel" then
                        ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 1, panel.Title, panel.Min,
                            panel.Max, panel.Percentage)
                    elseif pSubType == "UIMenuGridPanel" then
                        ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 2, panel.TopLabel,
                            panel.RightLabel, panel.LeftLabel, panel.BottomLabel, panel._CirclePosition.x,
                            panel._CirclePosition.y, true, panel.GridType)
                    elseif pSubType == "UIMenuStatisticsPanel" then
                        ScaleformUI.Scaleforms._ui:CallFunction("ADD_PANEL", false, it - 1, 3)
                        if #panel.Items then
                            for key, stat in pairs(panel.Items) do
                                ScaleformUI.Scaleforms._ui:CallFunction("ADD_STATISTIC_TO_PANEL", false, it - 1, pan - 1,
                                    stat['name'], stat['value'])
                            end
                        end
                    end
                end
            end
            if item.SidePanel ~= nil then
                if item.SidePanel() == "UIMissionDetailsPanel" then
                    ScaleformUI.Scaleforms._ui:CallFunction("ADD_SIDE_PANEL_TO_ITEM", false, it - 1, 0,
                        item.SidePanel.PanelSide, item.SidePanel.TitleType, item.SidePanel.Title,
                        item.SidePanel.TitleColor,
                        item.SidePanel.TextureDict, item.SidePanel.TextureName)
                    for key, value in pairs(item.SidePanel.Items) do
                        ScaleformUI.Scaleforms._ui:CallFunction("ADD_MISSION_DETAILS_DESC_ITEM", false, it - 1,
                            value.Type, value.TextLeft, value.TextRight, value.Icon, value.IconColor, value.Tick)
                    end
                elseif item.SidePanel() == "UIVehicleColorPickerPanel" then
                    ScaleformUI.Scaleforms._ui:CallFunction("ADD_SIDE_PANEL_TO_ITEM", false, it - 1, 1,
                        item.SidePanel.PanelSide, item.SidePanel.TitleType, item.SidePanel.Title,
                        item.SidePanel.TitleColor)
                end
            end
        end
        ScaleformUI.Scaleforms._ui:CallFunction("SET_CURRENT_ITEM", false, self:CurrentSelection() - 1)
        local type = self.Items[self.ActiveItem]
        if type == "UIMenuSeparatorItem" then
            if (self.Items[self.ActiveItem].Jumpable) then
                self:GoDown()
            end
        end
        ScaleformUI.Scaleforms._ui:CallFunction("ENABLE_MOUSE", false, self.Settings.MouseControlsEnabled)
    end)
end

---ProcessControl
function UIMenu:ProcessControl()
    if not self._Visible then
        return
    end

    if self.JustOpened then
        self.JustOpened = false
        return
    end

    if UpdateOnscreenKeyboard() == 0 or IsWarningMessageActive() then return end

    if self.Controls.Back.Enabled and (IsDisabledControlJustReleased(0, 177) or IsDisabledControlJustReleased(1, 177) or IsDisabledControlJustReleased(2, 177) or IsDisabledControlJustReleased(0, 199) or IsDisabledControlJustReleased(1, 199) or IsDisabledControlJustReleased(2, 199)) then
        Citizen.CreateThread(function()
            self:GoBack()
            Citizen.Wait(125)
            return
        end)
    end

    if #self.Items == 0 then
        return
    end

    if self.Controls.Up.Enabled and not self._isBuilding and (IsDisabledControlPressed(0, 172) or IsDisabledControlPressed(1, 172) or IsDisabledControlPressed(2, 172) or IsDisabledControlPressed(0, 241) or IsDisabledControlPressed(1, 241) or IsDisabledControlPressed(2, 241) or IsDisabledControlPressed(2, 241)) then
        if GetGameTimer() - self._time > self._delay then
            self:ButtonDelay()
            Citizen.CreateThread(function()
                self:GoUp()
                return
            end)
        end
    end

    if self.Controls.Down.Enabled and not self._isBuilding and (IsDisabledControlPressed(0, 173) or IsDisabledControlPressed(1, 173) or IsDisabledControlPressed(2, 173) or IsDisabledControlPressed(0, 242) or IsDisabledControlPressed(1, 242) or IsDisabledControlPressed(2, 242)) then
        if GetGameTimer() - self._time > self._delay then
            self:ButtonDelay()
            Citizen.CreateThread(function()
                self:GoDown()
                return
            end)
        end
    end

    if self.Controls.Left.Enabled and not self._isBuilding and (IsDisabledControlPressed(0, 174) or IsDisabledControlPressed(1, 174) or IsDisabledControlPressed(2, 174)) then
        if GetGameTimer() - self._time > self._delay then
            self:ButtonDelay()
            Citizen.CreateThread(function()
                self:GoLeft()
                return
            end)
        end
    end

    if self.Controls.Right.Enabled and not self._isBuilding and (IsDisabledControlPressed(0, 175) or IsDisabledControlPressed(1, 175) or IsDisabledControlPressed(2, 175)) then
        if GetGameTimer() - self._time > self._delay then
            self:ButtonDelay()
            Citizen.CreateThread(function()
                self:GoRight()
                return
            end)
        end
    end

    if self.Controls.Select.Enabled and not self._isBuilding and (IsDisabledControlJustPressed(0, 201) or IsDisabledControlJustPressed(1, 201) or IsDisabledControlJustPressed(2, 201)) then
        Citizen.CreateThread(function()
            self:SelectItem()
            Citizen.Wait(125)
            return
        end)
    end

    if self.Controls.Select.Enabled and not self._isBuilding and (IsDisabledControlJustPressed(0, 24) or IsDisabledControlJustPressed(1, 24) or IsDisabledControlJustPressed(2, 24)) then
        Citizen.CreateThread(function()
            self:SelectItem()
            Citizen.Wait(125)
            return
        end)
    end

    if (IsDisabledControlJustReleased(0, 172) or IsDisabledControlJustReleased(1, 172) or IsDisabledControlJustReleased(2, 172) or IsDisabledControlJustReleased(0, 241) or IsDisabledControlJustReleased(1, 241) or IsDisabledControlJustReleased(2, 241) or IsDisabledControlJustReleased(2, 241)) or
        (IsDisabledControlJustReleased(0, 173) or IsDisabledControlJustReleased(1, 173) or IsDisabledControlJustReleased(2, 173) or IsDisabledControlJustReleased(0, 242) or IsDisabledControlJustReleased(1, 242) or IsDisabledControlJustReleased(2, 242)) or
        (IsDisabledControlJustReleased(0, 174) or IsDisabledControlJustReleased(1, 174) or IsDisabledControlJustReleased(2, 174)) or
        (IsDisabledControlJustReleased(0, 175) or IsDisabledControlJustReleased(1, 175) or IsDisabledControlJustReleased(2, 175))
    then
        self._times = 0
        self._delay = 150
    end
end

function UIMenu:ButtonDelay()
    self._times = self._times + 1
    if self._times % 5 == 0 then
        self._delay = self._delay - 10
        if self._delay < 50 then
            self._delay = 50
        end
    end
    self._time = GetGameTimer()
end

---GoUp
function UIMenu:GoUp()
    self.Items[self:CurrentSelection()]:Selected(false)
    local return_value = ScaleformUI.Scaleforms._ui:CallFunction("SET_INPUT_EVENT", true, 8, self._delay)
    while not IsScaleformMovieMethodReturnValueReady(return_value) do
        Citizen.Wait(0)
    end
    self.ActiveItem = GetScaleformMovieMethodReturnValueInt(return_value)
    self.Items[self:CurrentSelection()]:Selected(true)
    PlaySoundFrontend(-1, self.Settings.Audio.UpDown, self.Settings.Audio.Library, true)
    self.OnIndexChange(self, self:CurrentSelection())
end

---GoDown
function UIMenu:GoDown()
    self.Items[self:CurrentSelection()]:Selected(false)
    local return_value = ScaleformUI.Scaleforms._ui:CallFunction("SET_INPUT_EVENT", true, 9, self._delay)
    while not IsScaleformMovieMethodReturnValueReady(return_value) do
        Citizen.Wait(0)
    end
    self.ActiveItem = GetScaleformMovieMethodReturnValueInt(return_value)
    self.Items[self:CurrentSelection()]:Selected(true)
    PlaySoundFrontend(-1, self.Settings.Audio.UpDown, self.Settings.Audio.Library, true)
    self.OnIndexChange(self, self:CurrentSelection())
end

---GoLeft
function UIMenu:GoLeft()
    local Item = self.Items[self:CurrentSelection()]
    local type, subtype = Item()
    if subtype ~= "UIMenuListItem" and subtype ~= "UIMenuDynamicListItem" and subtype ~= "UIMenuSliderItem" and subtype ~= "UIMenuProgressItem" and subtype ~= "UIMenuStatsItem" then
        return
    end

    if not Item:Enabled() then
        PlaySoundFrontend(-1, self.Settings.Audio.Error, self.Settings.Audio.Library, true)
        return
    end

    local return_value = ScaleformUI.Scaleforms._ui:CallFunction("SET_INPUT_EVENT", true, 10)
    while not IsScaleformMovieMethodReturnValueReady(return_value) do
        Citizen.Wait(0)
    end
    local res = GetScaleformMovieMethodReturnValueInt(return_value) + 1

    if subtype == "UIMenuListItem" then
        Item:Index(res)
        self.OnListChange(self, Item, Item._Index)
        Item.OnListChanged(self, Item, Item._Index)
        PlaySoundFrontend(-1, self.Settings.Audio.LeftRight, self.Settings.Audio.Library, true)
    elseif (subtype == "UIMenuDynamicListItem") then
        local result = tostring(Item.Callback(Item, "left"))
        Item:CurrentListItem(result)
        PlaySoundFrontend(-1, self.Settings.Audio.LeftRight, self.Settings.Audio.Library, true)
    elseif subtype == "UIMenuSliderItem" then
        Item:Index(res)
        self.OnSliderChange(self, Item, Item:Index())
        Item.OnSliderChanged(self, Item, Item._Index)
        PlaySoundFrontend(-1, self.Settings.Audio.LeftRight, self.Settings.Audio.Library, true)
    elseif subtype == "UIMenuProgressItem" then
        Item:Index(res)
        self.OnProgressChange(self, Item, Item:Index())
        Item.OnProgressChanged(self, Item, Item:Index())
        PlaySoundFrontend(-1, self.Settings.Audio.LeftRight, self.Settings.Audio.Library, true)
    elseif subtype == "UIMenuStatsItem" then
        Item:Index(res)
        self.OnStatsChanged(self, Item, Item:Index())
        Item.OnStatsChanged(self, Item, Item._Index)
    end
end

---GoRight
function UIMenu:GoRight()
    local Item = self.Items[self:CurrentSelection()]
    local type, subtype = Item()
    if subtype ~= "UIMenuListItem" and subtype ~= "UIMenuDynamicListItem" and subtype ~= "UIMenuSliderItem" and subtype ~= "UIMenuProgressItem" and subtype ~= "UIMenuStatsItem" then
        return
    end
    if not Item:Enabled() then
        PlaySoundFrontend(-1, self.Settings.Audio.Error, self.Settings.Audio.Library, true)
        return
    end

    local return_value = ScaleformUI.Scaleforms._ui:CallFunction("SET_INPUT_EVENT", true, 11)
    while not IsScaleformMovieMethodReturnValueReady(return_value) do
        Citizen.Wait(0)
    end
    local res = GetScaleformMovieMethodReturnValueInt(return_value) + 1

    if subtype == "UIMenuListItem" then
        Item:Index(res)
        self.OnListChange(self, Item, Item._Index)
        Item.OnListChanged(self, Item, Item._Index)
        PlaySoundFrontend(-1, self.Settings.Audio.LeftRight, self.Settings.Audio.Library, true)
    elseif (subtype == "UIMenuDynamicListItem") then
        local result = tostring(Item.Callback(Item, "right"))
        Item:CurrentListItem(result)
        PlaySoundFrontend(-1, self.Settings.Audio.LeftRight, self.Settings.Audio.Library, true)
    elseif subtype == "UIMenuSliderItem" then
        Item:Index(res)
        self.OnSliderChange(self, Item, Item:Index())
        Item.OnSliderChanged(self, Item, Item._Index)
        PlaySoundFrontend(-1, self.Settings.Audio.LeftRight, self.Settings.Audio.Library, true)
    elseif subtype == "UIMenuProgressItem" then
        Item:Index(res)
        self.OnProgressChange(self, Item, Item:Index())
        Item.OnProgressChanged(self, Item, Item:Index())
        PlaySoundFrontend(-1, self.Settings.Audio.LeftRight, self.Settings.Audio.Library, true)
    elseif subtype == "UIMenuStatsItem" then
        Item:Index(res)
        self.OnStatsChanged(self, Item, Item:Index())
        Item.OnStatsChanged(self, Item, Item._Index)
    end
end

---SelectItem
---@param play boolean|nil
function UIMenu:SelectItem(play)
    if not self.Items[self:CurrentSelection()]:Enabled() then
        PlaySoundFrontend(-1, self.Settings.Audio.Error, self.Settings.Audio.Library, true)
        return
    end
    if play then
        PlaySoundFrontend(-1, self.Settings.Audio.Select, self.Settings.Audio.Library, true)
    end

    local Item = self.Items[self:CurrentSelection()]
    local type, subtype = Item()
    if subtype == "UIMenuCheckboxItem" then
        Item:Checked(not Item:Checked())
        PlaySoundFrontend(-1, self.Settings.Audio.Select, self.Settings.Audio.Library, true)
        self.OnCheckboxChange(self, Item, Item:Checked())
        Item.OnCheckboxChanged(self, Item, Item:Checked())
    elseif subtype == "UIMenuListItem" then
        PlaySoundFrontend(-1, self.Settings.Audio.Select, self.Settings.Audio.Library, true)
        self.OnListSelect(self, Item, Item._Index)
        Item.OnListSelected(self, Item, Item._Index)
    elseif subtype == "UIMenuDynamicListItem" then
        PlaySoundFrontend(-1, self.Settings.Audio.Select, self.Settings.Audio.Library, true)
        self.OnListSelect(self, Item, Item._currentItem)
        Item.OnListSelected(self, Item, Item._currentItem)
    elseif subtype == "UIMenuSliderItem" then
        PlaySoundFrontend(-1, self.Settings.Audio.Select, self.Settings.Audio.Library, true)
        self.OnSliderSelect(self, Item, Item._Index)
        Item.OnSliderSelected(self, Item, Item._Index)
    elseif subtype == "UIMenuProgressItem" then
        PlaySoundFrontend(-1, self.Settings.Audio.Select, self.Settings.Audio.Library, true)
        self.OnProgressSelect(self, Item, Item._Index)
        Item.OnProgressSelected(self, Item, Item._Index)
    elseif subtype == "UIMenuStatsItem" then
        PlaySoundFrontend(-1, self.Settings.Audio.Select, self.Settings.Audio.Library, true)
        self.OnStatsSelect(self, Item, Item._Index)
        Item.OnStatsSelected(self, Item, Item._Index)
    else
        self.OnItemSelect(self, Item, self:CurrentSelection())
        Item:Activated(self, Item)
        if not self.Children[Item] then
            return
        end
        self._canBuild = false
        self._Visible = false
        self.OnMenuChanged(self, self.Children[self.Items[self:CurrentSelection()]], true)
        ScaleformUI.Scaleforms._ui:CallFunction("CLEAR_ALL", false)
        ScaleformUI.Scaleforms.InstructionalButtons:Enabled(true)
        ScaleformUI.Scaleforms.InstructionalButtons:SetInstructionalButtons(self.Children
            [self.Items[self:CurrentSelection()]].InstructionalButtons)
        self.OnMenuChanged(self, self.Children[Item], "forwards")
        self.Children[Item].OnMenuChanged(self, self.Children[Item], "forwards")
        self.Children[Item]._canBuild = true
        self.Children[Item]:Visible(true)
        if self.Children[Item]:BuildAsync() then
            self.Children[Item]:BuildUpMenuAsync()
        else
            self.Children[Item]:BuildUpMenuSync()
        end
    end
end

---GoBack
function UIMenu:GoBack()
    PlaySoundFrontend(-1, self.Settings.Audio.Back, self.Settings.Audio.Library, true)
    if self.ParentMenu ~= nil then
        self._canBuild = false
        self:Visible(false)
        ScaleformUI.Scaleforms._ui:CallFunction("CLEAR_ALL", false)
        ScaleformUI.Scaleforms.InstructionalButtons:Enabled(true)
        ScaleformUI.Scaleforms.InstructionalButtons:SetInstructionalButtons(self.ParentMenu.InstructionalButtons)
        self.ParentMenu._canBuild = true
        self.ParentMenu._Visible = true
        if self.ParentMenu:BuildAsync() then
            self.ParentMenu:BuildUpMenuAsync()
        else
            self.ParentMenu:BuildUpMenuSync()
        end
        self.OnMenuChanged(self, self.ParentMenu, "backwards")
        self.ParentMenu.OnMenuChanged(self, self.ParentMenu, "backwards")
    else
        if self:CanPlayerCloseMenu() then
            self:Visible(false)
        end
    end
end

---BindMenuToItem
---@param Menu table
---@param Item table
function UIMenu:BindMenuToItem(Menu, Item)
    if Menu() == "UIMenu" and Item() == "UIMenuItem" then
        Menu.ParentMenu = self
        Menu.ParentItem = Item
        self.Children[Item] = Menu
    end
end

---ReleaseMenuFromItem
---@param Item table
function UIMenu:ReleaseMenuFromItem(Item)
    if Item() == "UIMenuItem" then
        if not self.Children[Item] then
            return false
        end
        self.Children[Item].ParentMenu = nil
        self.Children[Item].ParentItem = nil
        self.Children[Item] = nil
        return true
    end
end

function UIMenu:UpdateDescription()
    ScaleformUI.Scaleforms._ui:CallFunction("UPDATE_ITEM_DESCRIPTION", false, self:CurrentSelection() - 1,
        "desc_{" .. self:CurrentSelection() .. "}")
end

---Draw
function UIMenu:Draw()
    if not self._Visible or ScaleformUI.Scaleforms.Warning:IsShowing() then return end
    if not ScaleformUI.Scaleforms._ui:IsLoaded() then
        while not ScaleformUI.Scaleforms._ui:IsLoaded() do Citizen.Wait(0) end
    end

    HideHudComponentThisFrame(19)

    if self.Settings.ControlDisablingEnabled then
        self:DisEnableControls(false)
    end

    local x = self.Position.X / 1280
    local y = self.Position.Y / 720
    local width = 1280 / self._scaledWidth
    local height = 720 / 720
    ScaleformUI.Scaleforms._ui:Render2DNormal(x + (width / 2.0), y + (height / 2.0), width, height)

    if self.Glare then
        self._menuGlare:CallFunction("SET_DATA_SLOT", false, GetGameplayCamRelativeHeading())

        local gx = self.Position.X / 1280 + 0.4499
        local gy = self.Position.Y / 720 + 0.449

        self._menuGlare:Render2DNormal(gx, gy, 1.0, 1.0)
    end

    if not IsUsingKeyboard(2) then
        if self._keyboard then
            self._keyboard = false
            self._changed = true
        end
    else
        if not self._keyboard then
            self._keyboard = true
            self._changed = true
        end
    end
    if self._changed then
        self:UpdateDescription()
        self._changed = false
    end
end

local cursor_pressed = false
local menuSound = -1
local success, event_type, context, item_id

function UIMenu:ProcessMouse()
    if not self._Visible or self.JustOpened or #self.Items == 0 or not IsUsingKeyboard(2) or not self.Settings.MouseControlsEnabled then
        EnableControlAction(0, 1, true)
        EnableControlAction(0, 2, true)
        EnableControlAction(1, 1, true)
        EnableControlAction(1, 2, true)
        EnableControlAction(2, 1, true)
        EnableControlAction(2, 2, true)
        if self.Dirty then
            for _, Item in pairs(self.Items) do
                if Item:Hovered() then
                    Item:Hovered(false)
                end
            end
            self.Dirty = false
        end
        return
    end

    SetMouseCursorActiveThisFrame()
    SetInputExclusive(2, 239)
    SetInputExclusive(2, 240)
    SetInputExclusive(2, 237)
    SetInputExclusive(2, 238)

    success, event_type, context, item_id = GetScaleformMovieCursorSelection(ScaleformUI.Scaleforms._ui.handle)

    if success == 1 then
        if event_type == 5 then  --ON CLICK
            if context == 0 then -- normal menu items
                local item = self.Items[(item_id + 1)]
                if (item == nil) then return end
                if item:Selected() then
                    if item.ItemId == 0 or item.ItemId == 2 then
                        self:SelectItem(false)
                    elseif item.ItemId == 1 or item.ItemId == 3 or item.ItemId == 4 then
                        Citizen.CreateThread(function()
                            local return_value = ScaleformUI.Scaleforms._ui:CallFunction("SELECT_ITEM", true, item_id)
                            while not IsScaleformMovieMethodReturnValueReady(return_value) do
                                Citizen.Wait(0)
                            end
                            local value = GetScaleformMovieMethodReturnValueInt(return_value)

                            local curr_select_item = self.Items[self:CurrentSelection()]
                            local item_type_curr, item_subtype_curr = curr_select_item()
                            if item.ItemId == 1 then
                                curr_select_item:Index(value)
                                self.OnListChange(self, curr_select_item, curr_select_item:Index())
                                curr_select_item.OnListChanged(self, curr_select_item, curr_select_item:Index())
                            elseif item.ItemId == 3 then
                                if (value ~= curr_select_item:Index()) then
                                    curr_select_item:Index(value)
                                    curr_select_item.OnSliderChanged(self, curr_select_item, curr_select_item:Index())
                                    self.OnSliderChange(self, curr_select_item, curr_select_item:Index())
                                end
                            elseif item.ItemId == 4 then
                                if (value ~= curr_select_item:Index()) then
                                    curr_select_item:Index(value)
                                    curr_select_item.OnProgressChanged(self, curr_select_item, curr_select_item:Index())
                                    self.OnProgressChange(self, curr_select_item, curr_select_item:Index())
                                end
                            end
                            return
                        end)
                    end
                    return
                end
                if (item.ItemId == 6 and item.Jumpable == true) or not item:Enabled() then
                    PlaySoundFrontend(-1, "ERROR", "HUD_FRONTEND_DEFAULT_SOUNDSET", true)
                    return
                end
                self:CurrentSelection(item_id)
                PlaySoundFrontend(-1, "SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET", true)
            elseif context == 10 then -- panels (10 => context 1, panel_type 0) // ColorPanel
                Citizen.CreateThread(function()
                    local return_value = ScaleformUI.Scaleforms._ui:CallFunction("SELECT_PANEL", true,
                        self:CurrentSelection() - 1)
                    while not IsScaleformMovieMethodReturnValueReady(return_value) do
                        Citizen.Wait(0)
                    end
                    local res = GetScaleformMovieMethodReturnValueString(return_value)
                    local split = Split(res, ",")
                    local panel = self.Items[self:CurrentSelection()].Panels[tonumber(split[1]) + 1]
                    panel.value = tonumber(split[2]) + 1
                    self.OnColorPanelChanged(panel.ParentItem, panel, panel:CurrentSelection())
                    panel.OnColorPanelChanged(panel.ParentItem, panel, panel:CurrentSelection())
                end)
            elseif context == 11 then -- panels (11 => context 1, panel_type 1) // PercentagePanel
                cursor_pressed = true
            elseif context == 12 then -- panels (12 => context 1, panel_type 2) // GridPanel
                cursor_pressed = true
            elseif context == 2 then  -- sidepanel
                local panel = self.Items[self:CurrentSelection()].SidePanel
                if item_id ~= -1 then
                    panel.Value = item_id - 1
                    panel.PickerSelect(panel.ParentItem, panel, panel.Value)
                end
            end
        elseif event_type == 6 then -- ON CLICK RELEASED
            cursor_pressed = false
        elseif event_type == 7 then -- ON CLICK RELEASED OUTSIDE
            cursor_pressed = false
            SetMouseCursorSprite(1)
        elseif event_type == 8 then -- ON NOT HOVER
            cursor_pressed = false
            if context == 0 then
                self.Items[item_id + 1]:Hovered(false)
            end
            SetMouseCursorSprite(1)
        elseif event_type == 9 then -- ON HOVERED
            if context == 0 then
                self.Items[item_id + 1]:Hovered(true)
            end
            SetMouseCursorSprite(5)
        elseif event_type == 0 then -- DRAGGED OUTSIDE
            cursor_pressed = false
        elseif event_type == 1 then -- DRAGGED INSIDE
            cursor_pressed = true
        end
    end

    if cursor_pressed == true then
        if HasSoundFinished(menuSound) then
            menuSound = GetSoundId()
            PlaySoundFrontend(menuSound, "CONTINUOUS_SLIDER", "HUD_FRONTEND_DEFAULT_SOUNDSET", true)
        end

        Citizen.CreateThread(function()
            local return_value = ScaleformUI.Scaleforms._ui:CallFunction("SET_INPUT_MOUSE_EVENT_CONTINUE", true)
            while not IsScaleformMovieMethodReturnValueReady(return_value) do
                Citizen.Wait(0)
            end
            local value = GetScaleformMovieMethodReturnValueString(return_value)

            local split = Split(value, ",")
            local panel = self.Items[self:CurrentSelection()].Panels[tonumber(split[1]) + 1]
            local panel_type, panel_subtype = panel()

            if panel_subtype == "UIMenuGridPanel" then
                panel._CirclePosition = vector2(tonumber(split[2]) or 0, tonumber(split[3]) or 0)
                self.OnGridPanelChanged(panel.ParentItem, panel, panel._CirclePosition)
                panel.OnGridPanelChanged(panel.ParentItem, panel, panel._CirclePosition)
            elseif panel_subtype == "UIMenuPercentagePanel" then
                panel.Percentage = tonumber(split[2])
                self:OnPercentagePanelChanged(panel.ParentItem, panel, panel.Percentage)
                panel.OnPercentagePanelChange(panel.ParentItem, panel, panel.Percentage)
            end
        end)
    else
        if not HasSoundFinished(menuSound) then
            StopSound(menuSound)
            ReleaseSoundId(menuSound)
        end
    end
end

---AddInstructionButton
---@param button table
function UIMenu:AddInstructionButton(button)
    if type(button) == "table" and #button == 2 then
        self.InstructionalButtons[#self.InstructionalButtons + 1] = button
    end
end

---RemoveInstructionButton
---@param button table
function UIMenu:RemoveInstructionButton(button)
    if type(button) == "table" then
        for i = 1, #self.InstructionalButtons do
            if button == self.InstructionalButtons[i] then
                table.remove(self.InstructionalButtons, i)
                break
            end
        end
    else
        if tonumber(button) then
            if self.InstructionalButtons[tonumber(button)] then
                table.remove(self.InstructionalButtons, tonumber(button))
            end
        end
    end
end

---AddEnabledControl
---@param Inputgroup number
---@param Control number
---@param Controller table
function UIMenu:AddEnabledControl(Inputgroup, Control, Controller)
    if tonumber(Inputgroup) and tonumber(Control) then
        table.insert(self.Settings.EnabledControls[(Controller and "Controller" or "Keyboard")], { Inputgroup, Control })
    end
end

---RemoveEnabledControl
---@param Inputgroup number
---@param Control number
---@param Controller table
function UIMenu:RemoveEnabledControl(Inputgroup, Control, Controller)
    local Type = (Controller and "Controller" or "Keyboard")
    for Index = 1, #self.Settings.EnabledControls[Type] do
        if Inputgroup == self.Settings.EnabledControls[Type][Index][1] and Control == self.Settings.EnabledControls[Type][Index][2] then
            table.remove(self.Settings.EnabledControls[Type], Index)
            break
        end
    end
end
