// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_W_D_._B_d

package _W_D_{
    import flash.display.DisplayObjectContainer;
    import flash.system.Capabilities;
    import flash.display.LoaderInfo;

    public class _B_d {

        private static var platform:_0I_z;

        private const _C_J_:String = "Desktop";

        [Inject]
        public var root:DisplayObjectContainer;

        public function _0B_z():Boolean{
            return (!((Capabilities.playerType == this._C_J_)));
        }
        public function _H_z():Boolean{
            return ((Capabilities.playerType == this._C_J_));
        }
        public function _R_K_():_0I_z{
            return ((platform = ((platform) || (this._gQ_()))));
        }
        private function _gQ_():_0I_z{
            var _local1:Object = LoaderInfo(this.root.stage.root.loaderInfo).parameters;
            if (this._P_R_(_local1))
            {
                return (_0I_z._0C_i);
            };
            if (this._0J_x(_local1))
            {
                return (_0I_z._I_6);
            };
            return (_0I_z._kj);
        }
        private function _P_R_(_arg1:Object):Boolean{
            return (!((_arg1.kongregate_api_path == null)));
        }
        private function _0J_x(_arg1:Object):Boolean{
            return (!((_arg1.steam_api_path == null)));
        }

    }
}//package _W_D_

