// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0C_P_.Options

package _0C_P_{
    import flash.display.Sprite;

    import com.company.assembleegameclient.game.GameSprite;
    import com.company.ui.SimpleText;
    import _F_1._H_o;
    import flash.text.TextFieldAutoSize;
    import flash.filters.DropShadowFilter;
    import com.company.rotmg.graphics.ScreenGraphic;
    import flash.events.MouseEvent;
    import flash.events.Event;
    import com.company.assembleegameclient.parameters.Parameters;
    import flash.system.Capabilities;
    import flash.events.KeyboardEvent;
    import com.company.util._H_V_;
    import flash.display.StageDisplayState;
    import _vf._gs;
    import _vf._Q_P_;


    public class Options extends Sprite {

        private static const _2Q_:String = "Controls";
        private static const _A_S_:String = "Hot Keys";
        private static const _uC_:String = "Chat";
        private static const _O_i:String = "Graphics";
        private static const _0D_o:String = "Sound";
        private static const _F_D_:Vector.<String> = new <String>[_2Q_, _A_S_, _uC_, _O_i, _0D_o];

        private var gs_:GameSprite;
        private var _O_k:SimpleText;
        private var _0Q_:_H_o;
        private var _7z:_H_o;
        private var _pc:_H_o;
        private var _C_F_:Vector.<_P_4>;
        private var selected_:_P_4 = null;
        private var _03a:Vector.<Sprite>;
        private var _00s:int = 0;

        public function Options(_arg1:GameSprite){
            var _local4:_P_4;
            this._C_F_ = new Vector.<_P_4>();
            this._03a = new Vector.<Sprite>();
            super();
            this.gs_ = _arg1;
            graphics.clear();
            graphics.beginFill(0x2B2B2B, 0.8);
            graphics.drawRect(0, 0, 800, 600);
            graphics.endFill();
            graphics.lineStyle(1, 0x5E5E5E);
            graphics.moveTo(0, 100);
            graphics.lineTo(800, 100);
            graphics.lineStyle();
            this._O_k = new SimpleText(36, 0xFFFFFF, false, 800, 0, "Myriad Pro");
            this._O_k.setBold(true);
            this._O_k.htmlText = '<p align="center">Options</p>';
            this._O_k.autoSize = TextFieldAutoSize.CENTER;
            this._O_k.filters = [new DropShadowFilter(0, 0, 0)];
            this._O_k.updateMetrics();
            this._O_k.x = ((800 / 2) - (this._O_k.width / 2));
            this._O_k.y = 8;
            addChild(this._O_k);
            addChild(new ScreenGraphic());
            this._0Q_ = new _H_o("continue", 36, false);
            this._0Q_.addEventListener(MouseEvent.CLICK, this._0B_Z_);
            addChild(this._0Q_);
            this._7z = new _H_o("reset to defaults", 22, false);
            this._7z.addEventListener(MouseEvent.CLICK, this._T_8);
            addChild(this._7z);
            this._pc = new _H_o("back to home", 22, false);
            this._pc.addEventListener(MouseEvent.CLICK, this._J_a);
            addChild(this._pc);
            var _local2:int = 14;
            var _local3:int;
            while (_local3 < _F_D_.length)
            {
                _local4 = new _P_4(_F_D_[_local3]);
                _local4.x = _local2;
                _local4.y = 70;
                addChild(_local4);
                _local4.addEventListener(MouseEvent.CLICK, this._ni);
                this._C_F_.push(_local4);
                _local2 = (_local2 + 108);
                _local3++;
            };
            addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
            addEventListener(Event.REMOVED_FROM_STAGE, this.onRemovedFromStage);
        }
        private function _0B_Z_(_arg1:MouseEvent):void{
            this.close();
        }
        private function _T_8(_arg1:MouseEvent):void{
            var _local3:_0_i;
            var _local2:int;
            while (_local2 < this._03a.length)
            {
                _local3 = (this._03a[_local2] as _0_i);
                if (_local3 != null)
                {
                    delete Parameters.data_[_local3._W_Y_];
                };
                _local2++;
            };
            Parameters._fX_();
            Parameters.save();
            this.refresh();
        }
        private function _J_a(_arg1:MouseEvent):void{
            this.gs_.dispatchEvent(new Event(Event.COMPLETE));
        }
        private function _ni(_arg1:MouseEvent):void{
            var _local2:_P_4 = (_arg1.target as _P_4);
            this.setSelected(_local2);
        }
        private function setSelected(_arg1:_P_4):void{
            if (_arg1 == this.selected_)
            {
                return;
            };
            if (this.selected_ != null)
            {
                this.selected_.setSelected(false);
            };
            this.selected_ = _arg1;
            this.selected_.setSelected(true);
            this._H_c();
            switch (this.selected_.text_)
            {
                case _2Q_:
                    this._q9();
                    return;
                case _A_S_:
                    this._wP_();
                    return;
                case _uC_:
                    this._E_j();
                    return;
                case _O_i:
                    this._R_E_();
                    return;
                case _0D_o:
                    this._Y_V_();
                    return;
            };
        }
        private function onAddedToStage(_arg1:Event):void{
            stage;
            this._0Q_.x = ((800 / 2) - (this._0Q_.width / 2));
            this._0Q_.y = 520;
            this._7z.x = 20;
            this._7z.y = 532;
            this._pc.x = 620;
            this._pc.y = 532;
            if (Capabilities.playerType == "Desktop")
            {
                Parameters.data_.fullscreenMode = (stage.displayState == "fullScreenInteractive");
                Parameters.save();
            };
            this.setSelected(this._C_F_[0]);
            stage.addEventListener(KeyboardEvent.KEY_DOWN, this._0A_Y_, false, 1);
            stage.addEventListener(KeyboardEvent.KEY_UP, this._H_H_, false, 1);
        }
        private function onRemovedFromStage(_arg1:Event):void{
            stage.removeEventListener(KeyboardEvent.KEY_DOWN, this._0A_Y_, false);
            stage.removeEventListener(KeyboardEvent.KEY_UP, this._H_H_, false);
        }
        private function _0A_Y_(_arg1:KeyboardEvent):void{
            if ((((Capabilities.playerType == "Desktop")) && ((_arg1.keyCode == _H_V_.ESCAPE))))
            {
                Parameters.data_.fullscreenMode = false;
                Parameters.save();
                this.refresh();
            };
            if (_arg1.keyCode == Parameters.data_.options)
            {
                this.close();
            };
            _arg1.stopImmediatePropagation();
        }
        private function close():void{
            stage.focus = null;
            parent.removeChild(this);
        }
        private function _H_H_(_arg1:KeyboardEvent):void{
            _arg1.stopImmediatePropagation();
        }
        private function _H_c():void{
            var _local1:Sprite;
            for each (_local1 in this._03a)
            {
                removeChild(_local1);
            };
            this._03a.length = 0;
            this._00s = 0;
        }
        private function _q9():void{
            this._yZ_(new _G_X_("moveUp", "Move Up", "Key to will move character up"));
            this._yZ_(new _G_X_("moveLeft", "Move Left", "Key to will move character to the left"));
            this._yZ_(new _G_X_("moveDown", "Move Down", "Key to will move character down"));
            this._yZ_(new _G_X_("moveRight", "Move Right", "Key to will move character to the right"));
            this._yZ_(new _0I_j("allowRotation", new <String>["On", "Off"], [true, false], "Allow Camera Rotation", "Toggles whether to allow for camera rotation", this._F_x));
            this._yZ_(new Sprite());
            this._yZ_(new _G_X_("rotateLeft", "Rotate Left", "Key to will rotate the camera to the left", !(Parameters.data_.allowRotation)));
            this._yZ_(new _G_X_("rotateRight", "Rotate Right", "Key to will rotate the camera to the right", !(Parameters.data_.allowRotation)));
            this._yZ_(new _G_X_("useSpecial", "Use Special Ability", "This key will activate your special ability"));
            this._yZ_(new _G_X_("autofireToggle", "Autofire Toggle", "This key will toggle autofire"));
            this._yZ_(new _G_X_("resetToDefaultCameraAngle", "Reset To Default Camera Angle", ("This key will reset the camera angle to the default " + "position")));
            this._yZ_(new _G_X_("togglePerformanceStats", "Toggle Performance Stats", "This key will toggle a display of fps and memory usage"));
            this._yZ_(new _G_X_("toggleCentering", "Toggle Centering of Player", ("This key will toggle the position between centered and " + "offset")));
            this._yZ_(new _G_X_("interact", "Interact/Buy", "This key will allow you to enter a portal or buy an item"));
            this._yZ_(new _0I_j("contextualClick", new <String>["On", "Off"], [true, false], "Contextual Click", "Toggle the contextual click functionality", null));
            this._yZ_(new _0I_j("clickForGold", new <String>["On", "Off"], [true, false], "Double Click for Gold", "Double clicking on gold/fame while in a Realm will open the payments screen", null));
        }
        private function _F_x():void{
            var _local2:_G_X_;
            var _local1:int;
            while (_local1 < this._03a.length)
            {
                _local2 = (this._03a[_local1] as _G_X_);
                if (_local2 != null)
                {
                    if ((((_local2._W_Y_ == "rotateLeft")) || ((_local2._W_Y_ == "rotateRight"))))
                    {
                        _local2._J_r(!(Parameters.data_.allowRotation));
                    };
                };
                _local1++;
            };
        }
        private function _wP_():void{
            this._yZ_(new _G_X_("useInvSlot1", "Use Inventory Slot 1", "Use item in inventory slot 1"));
            this._yZ_(new _G_X_("useInvSlot2", "Use Inventory Slot 2", "Use item in inventory slot 1"));
            this._yZ_(new _G_X_("useInvSlot3", "Use Inventory Slot 3", "Use item in inventory slot 1"));
            this._yZ_(new _G_X_("useInvSlot4", "Use Inventory Slot 4", "Use item in inventory slot 1"));
            this._yZ_(new _G_X_("useInvSlot5", "Use Inventory Slot 5", "Use item in inventory slot 1"));
            this._yZ_(new _G_X_("useInvSlot6", "Use Inventory Slot 6", "Use item in inventory slot 1"));
            this._yZ_(new _G_X_("useInvSlot7", "Use Inventory Slot 7", "Use item in inventory slot 1"));
            this._yZ_(new _G_X_("useInvSlot8", "Use Inventory Slot 8", "Use item in inventory slot 1"));
            this._yZ_(new _G_X_("miniMapZoomIn", "Mini-Map Zoom In", "This key will zoom in the minimap"));
            this._yZ_(new _G_X_("miniMapZoomOut", "Mini-Map Zoom Out", "This key will zoom out the minimap"));
            this._yZ_(new _G_X_("escapeToNexus", "Escape To Nexus", "This key will instantly escape you to the Nexus"));
            this._yZ_(new _G_X_("options", "Show Options", "This key will bring up the options screen"));
            if (Capabilities.playerType == "Desktop")
            {
                this._yZ_(new _G_X_("toggleFullscreen", "Toggle Fullscreen Mode", ("Toggle whether the game is " + "run in a window or fullscreen")));
            };
        }
        private function _E_j():void{
            this._yZ_(new _G_X_("chat", "Activate Chat", "This key will bring up the chat input box"));
            this._yZ_(new _G_X_("chatCommand", "Start Chat Command", ("This key will bring up the chat with a '/' prepended to " + "allow for commands such as /who, /ignore, etc.")));
            this._yZ_(new _G_X_("tell", "Begin Tell", ("This key will bring up a tell (private message) in the chat" + " input box")));
            this._yZ_(new _G_X_("guildChat", "Begin Guild Chat", ("This key will bring up a guild chat in the chat" + " input box")));
            this._yZ_(new _0I_j("filterLanguage", new <String>["On", "Off"], [true, false], "Filter Offensive Language", ("This toggles whether offensive language be filtering will " + "be attempted"), null));
            this._yZ_(new _G_X_("scrollChatUp", "Scroll Chat Up", ("This key will scroll up to older messages in the chat " + "buffer")));
            this._yZ_(new _G_X_("scrollChatDown", "Scroll Chat Down", ("This key will scroll down to newer messages in the chat " + "buffer")));
        }
        private function _R_E_():void{
            this._yZ_(new _0I_j("defaultCameraAngle", new <String>["45°", "0°"], [((7 * Math.PI) / 4), 0], "Default Camera Angle", "This toggles the default camera angle", this._oD_));
            this._yZ_(new _0I_j("centerOnPlayer", new <String>["On", "Off"], [true, false], "Center On Player", "This toggles whether the player is centered or offset", null));
            this._yZ_(new _0I_j("showQuestPortraits", new <String>["On", "Off"], [true, false], "Show Quest Portraits", "This toggles whether quest portraits are displayed", this._fJ_));
            this._yZ_(new _0I_j("showProtips", new <String>["On", "Off"], [true, false], "Show Tips", ("This toggles whether a tip is displayed when you join a " + "new game"), null));
            this._yZ_(new _0I_j("drawShadows", new <String>["On", "Off"], [true, false], "Draw Shadows", "This toggles whether to draw shadows", null));
            this._yZ_(new _0I_j("textBubbles", new <String>["On", "Off"], [true, false], "Draw Text Bubbles", "This toggles whether to draw text bubbles", null));
            this._yZ_(new _0I_j("showTradePopup", new <String>["On", "Off"], [true, false], "Show Trade Request Panel", ("This toggles whether to show trade requests in the " + "lower-right panel or just in chat."), null));
            this._yZ_(new _0I_j("showGuildInvitePopup", new <String>["On", "Off"], [true, false], "Show Guild Invite Panel", ("This toggles whether to show guild invites in the " + "lower-right panel or just in chat."), null));
            if (Capabilities.playerType == "Desktop")
            {
                this._yZ_(new _0I_j("fullscreenMode", new <String>["On", "Off"], [true, false], "Fullscreen Mode", "This toggles whether the game is run in fullscreen mode.", this._6k));
            };
        }
        private function _oD_():void{
            Parameters.data_.cameraAngle = Parameters.data_.defaultCameraAngle;
            Parameters.save();
        }
        private function _fJ_():void{
            if (((((((!((this.gs_ == null))) && (!((this.gs_.map_ == null))))) && (!((this.gs_.map_.partyOverlay_ == null))))) && (!((this.gs_.map_.partyOverlay_.questArrow_ == null)))))
            {
                this.gs_.map_.partyOverlay_.questArrow_.refreshToolTip();
            };
        }
        private function _6k():void{
            stage.displayState = ((Parameters.data_.fullscreenMode) ? "fullScreenInteractive" : StageDisplayState.NORMAL);
        }
        private function _Y_V_():void{
            this._yZ_(new _0I_j("playMusic", new <String>["On", "Off"], [true, false], "Play Music", "This toggles whether music is played", this._05z));
            this._yZ_(new Sprite());
            this._yZ_(new _0I_j("playSFX", new <String>["On", "Off"], [true, false], "Play Sound Effects", "This toggles whether sound effects are played", this._super));
            this._yZ_(new Sprite());
            this._yZ_(new _0I_j("playPewPew", new <String>["On", "Off"], [true, false], "Play Weapon Sounds", "This toggles whether weapon sounds are played", null));
        }
        private function _05z():void{
            _gs._continue(Parameters.data_.playMusic);
        }
        private function _super():void{
            _Q_P_._2c(Parameters.data_.playSFX);
        }
        private function _yZ_(_arg1:Sprite):void{
            _arg1.x = ((((this._00s % 2) == 0)) ? 20 : 415);
            _arg1.y = ((int((this._00s / 2)) * 44) + 122);
            addChild(_arg1);
            _arg1.addEventListener(Event.CHANGE, this._bR_);
            this._03a.push(_arg1);
            this._00s++;
        }
        private function _bR_(_arg1:Event):void{
            this.refresh();
        }
        private function refresh():void{
            var _local2:_0_i;
            var _local1:int;
            while (_local1 < this._03a.length)
            {
                _local2 = (this._03a[_local1] as _0_i);
                if (_local2 != null)
                {
                    _local2.refresh();
                };
                _local1++;
            };
        }

    }
}//package _0C_P_

