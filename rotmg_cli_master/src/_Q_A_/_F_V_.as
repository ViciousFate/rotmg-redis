// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_Q_A_._F_V_

package _Q_A_{
    import _0D_d.Frame;
    import _0D_d.TextInput;
    import com.company.assembleegameclient.ui.TextButton;
    import flash.events.MouseEvent;
    import com.company.assembleegameclient.appengine._0B_u;
    import com.company.assembleegameclient.parameters.Parameters;
    import _zo._8C_;
    import _zo._mS_;

    class _F_V_ extends Frame {

        public var _xb:TextInput;
        public var _static:TextButton;

        public function _F_V_(){
            super("Forgot your password?  We'll email it.", "Cancel", "Submit", "/forgotPassword");
            this._xb = new TextInput("Email", false, "");
            _vO_(this._xb);
            this._static = new TextButton(12, false, "New user?  Click here to Register");
            __true(this._static);
            Button1.addEventListener(MouseEvent.CLICK, this.onCancel);
            Button2.addEventListener(MouseEvent.CLICK, this._08Y_);
            this._static.addEventListener(MouseEvent.CLICK, this._mO_);
        }
        private function onCancel(_arg1:MouseEvent):void{
            dispatchEvent(new _nJ_(_nJ_._2K_));
        }
        private function _08Y_(_arg1:MouseEvent):void{
            if (this._xb.text() == "")
            {
                this._xb._0B_T_("Not a valid email address");
                return;
            };
            var _local2:_0B_u = new _0B_u(Parameters._fK_(), "/account", true);
            _local2.addEventListener(_8C_.GENERIC_DATA, this._01v);
            _local2.addEventListener(_mS_.TEXT_ERROR, this._Q_9);
            _local2.sendRequest("forgotPassword", {"guid":this._xb.text()});
            _pW_();
        }
        private function _01v(_arg1:_8C_):void{
            dispatchEvent(new _nJ_(_nJ_._2K_));
        }
        private function _Q_9(_arg1:_mS_):void{
            this._xb._0B_T_(_arg1.text_);
            _for();
        }
        private function _mO_(_arg1:MouseEvent):void{
            dispatchEvent(new _nJ_(_nJ_._G__));
        }

    }
}//package _Q_A_

