// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_0B_2._1T_

package _0B_2{
    import _qN_.Account;
    import _0D_n._throw;
    import _0D_n._P_I_;
    import flash.display.Stage;
    import flash.external.ExternalInterface;
    import com.hurlant.util.Base64;
    import com.company.assembleegameclient.appengine._02k;
    import _0L_C_._qM_;
    import com.company.assembleegameclient.appengine._0B_u;
    import flash.display.LoaderInfo;
    import _0L_C_._qO_;
    import com.company.assembleegameclient.parameters.Parameters;
    import _zo._8C_;
    import _zo._mS_;
    import flash.net.navigateToURL;
    import flash.net.URLRequest;
    import _eN_._T_J_;
    import flash.display.Sprite;
    import _eN_._0M_h;
    import _qN_._9j;
    import flash.events.Event;
    import _Q_A_._02R_;

    public class _1T_ extends Account {

        public static const _000:String = "kabam.com";
        private static const _is:String = "kabam.com";
        private static const _N_G_:_throw = _P_I_._dJ_();

        private var callback_:Function;
        private var _8U_:String = "";
        private var guid_:String = null;
        private var _T_x:String = "https://www.kabam.com";
        private var secret_:String = null;
        private var _mV_:String = null;
        public var _cd:Stage;
        private var _a1:Object = null;

        public function _1T_(){
            var _local1:String;
            var _local2:String;
            super();
            try
            {
                _local1 = ExternalInterface.call("rotmg.UrlLib.getParam", "entrypt");
                if (_local1 != null)
                {
                    this._8U_ = _local1;
                };
            } catch(error:Error)
            {
            };
            try
            {
                _local2 = ExternalInterface.call("rotmg.KabamDotComLib.getKabamGamePage");
                if (((!((_local2 == null))) && ((_local2.length > 0))))
                {
                    this._T_x = _local2;
                };
            } catch(error:Error)
            {
            };
        }
        protected static function _0_o(_arg1:String):String{
            var _local2:RegExp = /-/g;
            var _local3:RegExp = /_/g;
            var _local4:int = (4 - (_arg1.length % 4));
            while (_local4--)
            {
                _arg1 = (_arg1 + "=");
            };
            _arg1 = _arg1.replace(_local2, "+").replace(_local3, "/");
            return (Base64.decode(_arg1));
        }
        public static function _0J_5(signedRequest:String):Object{
            var requestDetails:Array;
            var payload:String;
            var userSession:Object;
            try
            {
                requestDetails = signedRequest.split(".", 2);
                if (requestDetails.length != 2)
                {
                    throw (new Error("Invalid signed request"));
                };
                payload = _0_o(requestDetails[1]);
                userSession = _N_G_.parse(payload);
            } catch(error:Error)
            {
                userSession = null;
            };
            return (userSession);
        }

        override public function cacheOffers():void{
            _02k._U_t("/credits", null);
        }
        override public function credentials():Object{
            return ({
                "guid":this.guid(),
                "secret":this.secret(),
                "signedRequest":this._mV_
            });
        }
        override public function entrytag():String{
            return (this._8U_);
        }
        override public function gameNetworkUserId():String{
            if ((((this._a1 == null)) || ((this._a1["kabam_naid"] == null))))
            {
                return ("");
            };
            return (this._a1["kabam_naid"]);
        }
        override public function gameNetwork():String{
            return (_000);
        }
        public function _0X_():String{
            if ((((((this._a1 == null)) || ((this._a1["user"] == null)))) || ((this._a1["user"]["email"] == null))))
            {
                return ("");
            };
            return (this._a1["user"]["email"]);
        }
        override public function guid():String{
            return (this.guid_);
        }
        override protected function internalLoad(_arg1:Stage, _arg2:Function):void{
            var _local5:String;
            var _local6:_qM_;
            var _local7:_0B_u;
            this._cd = _arg1;
            this.callback_ = _arg2;
            var _local3:Object = LoaderInfo(_arg1.root.loaderInfo).parameters;
            var _local4:String = _local3.kabam_signed_request;
            this._a1 = _0J_5(_local4);
            if (this._a1 == null)
            {
                _local5 = "Failed to retrieve valid Kabam request! Click to reload.";
                _local6 = new _qM_(_local5);
                _local6.addEventListener(_qO_.BUTTON1_EVENT, this._T_3);
                this._cd.addChild(_local6);
            } else
            {
                this._mV_ = _local4;
                _local7 = new _0B_u(Parameters._fK_(), "/kabam", true, 2);
                _local7.addEventListener(_8C_.GENERIC_DATA, this._6l);
                _local7.addEventListener(_mS_.TEXT_ERROR, this._T_);
                _local7.sendRequest("getcredentials", {
                    "signedRequest":_local4,
                    "entrytag":this.entrytag()
                });
            };
        }
        public function _aT_():Boolean{
            return (false);
        }
        override public function isRegistered():Boolean{
            return (true);
        }
        public function _N_():void{
            navigateToURL(new URLRequest(this._T_x), "_top");
        }
        override public function modify(_arg1:String, _arg2:String, _arg3:String):void{
            this.guid_ = _arg1;
            this.secret_ = _arg3;
        }
        override public function newAccountManagement():Sprite{
            return (new _T_J_());
        }
        override public function newAccountText():_9j{
            return (new _0M_h());
        }
        private function _6l(_arg1:_8C_):void{
            var _local2:XML = new XML(_arg1.data_);
            this.guid_ = _local2.GUID;
            this.secret_ = _local2.Secret;
            this.callback_();
        }
        private function _T_(_arg1:_mS_):void{
            this._cd.addChild(new _qM_(("Error: " + _arg1.text_)));
        }
        private function _T_3(_arg1:Event):void{
            this._N_();
        }
        override public function playPlatform():String{
            return (_is);
        }
        override public function secret():String{
            return ((((this.secret_)==null) ? "" : this.secret_));
        }
        override public function showMoneyManagement(_arg1:Stage):void{
            _arg1.addChild(new _02R_());
        }

    }
}//package _0B_2

