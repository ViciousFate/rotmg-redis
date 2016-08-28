// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_D_d._hj

package _D_d{
    import flash.display.Sprite;
    import com.company.assembleegameclient.appengine._0K_R_;
    import com.company.assembleegameclient.game.GameSprite;
    import com.company.assembleegameclient.parameters.Parameters;
    import flash.events.Event;
    import _9R_._j_;
    import _9R_._T_5;
    import _9R_._D_X_;

    public class _hj extends Sprite {

        private var _T_1:_0K_R_;
        private var _xG_:_0K_V_;
        private var _0B_9:GameSprite;

        public function _hj(){
            this._xG_ = new _0K_V_();
            this._xG_.addEventListener(_0C_r._048, this._0D_9);
            addChild(this._xG_);
        }
        public function initialize(_arg1:_0K_R_):void{
            this._T_1 = _arg1;
        }
        private function _0D_9(_arg1:_0C_r):void{
            removeChild(this._xG_);
            this._0B_9 = new GameSprite(this._T_1._04D_(), Parameters.TEST_ID, false, this._T_1.savedChars_[0].charId(), -1, null, this._T_1, _arg1._2B_);
            this._0B_9._3c = true;
            this._0B_9.addEventListener(Event.COMPLETE, this._08x);
            this._0B_9.addEventListener(_j_.RECONNECT, this._08x);
            this._0B_9.addEventListener(_T_5.DEATH, this._08x);
            this._0B_9.addEventListener(_D_X_.CLIENT_UPDATE, this._Y_z);
            addChild(this._0B_9);
        }
        private function _08x(_arg1:Event):void{
            this._K_k();
            addChild(this._xG_);
        }
        private function _Y_z(_arg1:Event):void{
            this._K_k();
            addChild(this._xG_);
        }
        private function _K_k():void{
            this._0B_9.removeEventListener(Event.COMPLETE, this._08x);
            this._0B_9.removeEventListener(_j_.RECONNECT, this._08x);
            this._0B_9.removeEventListener(_T_5.DEATH, this._08x);
            this._0B_9.removeEventListener(_D_X_.CLIENT_UPDATE, this._Y_z);
            removeChild(this._0B_9);
            this._0B_9 = null;
        }

    }
}//package _D_d

