// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0D_B_._02p

package _0D_B_{
    import com.company.assembleegameclient.game.GameSprite;
    import com.company.assembleegameclient.objects.Player;
    import com.company.assembleegameclient.ui._gV_;
    import flash.events.MouseEvent;
    import com.company.assembleegameclient.util._07E_;
    import com.company.util.AssetLibrary;
    import flash.events.Event;

    public class _02p extends Menu {

        public var gs_:GameSprite;
        public var _9y:String;
        public var player_:Player;
        public var _02y:_gV_;

        public function _02p(_arg1:GameSprite, _arg2:Player){
            var _local3:_0K_G_;
            super(0x363636, 0xFFFFFF);
            this.gs_ = _arg1;
            this._9y = _arg2.name_;
            this.player_ = _arg2;
            this._02y = new _gV_(0xB3B3B3, true, this.player_);
            addChild(this._02y);
            if (((this.gs_.map_.allowPlayerTeleport_) && (this.player_._0D_X_(this.player_))))
            {
                _local3 = new _K_h(this.gs_.map_.player_);
                _local3.addEventListener(MouseEvent.CLICK, this._L_H_);
                _yZ_(_local3);
            };
            if ((((this.gs_.map_.player_.guildRank_ >= _07E_._f3)) && ((((_arg2.guildName_ == null)) || ((_arg2.guildName_.length == 0))))))
            {
                _local3 = new _0K_G_(AssetLibrary._xK_("lofiInterfaceBig", 10), 0xFFFFFF, "Invite");
                _local3.addEventListener(MouseEvent.CLICK, this._N_I_);
                _yZ_(_local3);
            };
            if (!this.player_.starred_)
            {
                _local3 = new _0K_G_(AssetLibrary._xK_("lofiInterface2", 5), 0xFFFFFF, "Lock");
                _local3.addEventListener(MouseEvent.CLICK, this._N_f);
                _yZ_(_local3);
            } else
            {
                _local3 = new _0K_G_(AssetLibrary._xK_("lofiInterface2", 6), 0xFFFFFF, "Unlock");
                _local3.addEventListener(MouseEvent.CLICK, this._qw);
                _yZ_(_local3);
            };
            _local3 = new _0K_G_(AssetLibrary._xK_("lofiInterfaceBig", 7), 0xFFFFFF, "Trade");
            _local3.addEventListener(MouseEvent.CLICK, this._48);
            _yZ_(_local3);
            if (!this.player_._0M_w)
            {
                _local3 = new _0K_G_(AssetLibrary._xK_("lofiInterfaceBig", 8), 0xFFFFFF, "Ignore");
                _local3.addEventListener(MouseEvent.CLICK, this._K_o);
                _yZ_(_local3);
            } else
            {
                _local3 = new _0K_G_(AssetLibrary._xK_("lofiInterfaceBig", 9), 0xFFFFFF, "Unignore");
                _local3.addEventListener(MouseEvent.CLICK, this._4F_);
                _yZ_(_local3);
            };
        }
        private function _L_H_(_arg1:Event):void{
            this.gs_.map_.player_.teleportTo(this.player_);
            remove();
        }
        private function _N_I_(_arg1:Event):void{
            this.gs_.gsc_._H_X_(this._9y);
            remove();
        }
        private function _N_f(_arg1:Event):void{
            this.gs_.map_.party_.lockPlayer(this.player_);
            remove();
        }
        private function _qw(_arg1:Event):void{
            this.gs_.map_.party_.unlockPlayer(this.player_);
            remove();
        }
        private function _48(_arg1:Event):void{
            this.gs_.gsc_.requestTrade(this._9y);
            remove();
        }
        private function _K_o(_arg1:Event):void{
            this.gs_.map_.party_.ignorePlayer(this.player_);
            remove();
        }
        private function _4F_(_arg1:Event):void{
            this.gs_.map_.party_.unignorePlayer(this.player_);
            remove();
        }

    }
}//package _0D_B_

