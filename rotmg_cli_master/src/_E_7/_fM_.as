// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_E_7._fM_

package _E_7{
    import _05H_._09A_;
    import _05H_._M_K_;
    import com.company.assembleegameclient.ui.Slot;
    import _05H_._A_Y_;
    import _05H_._zr;
    import _05H_._04E_;
    import _05H_._implements;
    import _05H_._L_m;
    import _05H_._F_o;
    import _05H_._oE_;
    import _05H_._J_5;
    import _05H_._M_;
    import _05H_._X_7;
    import _05H_._vD_;
    import _05H_._5q;
    import _05H_._W_C_;
    import _05H_._U_y;

    public class _fM_ {

        private var hash:Object;

        public function _fM_(){
            var _local1:_09A_ = new _09A_();
            var _local2:_M_K_ = new _M_K_();
            this.hash = {};
            this.hash[Slot._Z_f] = _local1;
            this.hash[Slot._J_S_] = _local1;
            this.hash[Slot._rm] = _local1;
            this.hash[Slot._L_0] = new _A_Y_();
            this.hash[Slot._ca] = new _zr();
            this.hash[Slot._jP_] = _local2;
            this.hash[Slot._05C_] = _local2;
            this.hash[Slot._0J_a] = _local1;
            this.hash[Slot._3n] = new _04E_();
            this.hash[Slot._G_V_] = new _implements();
            this.hash[Slot._06u] = new _L_m();
            this.hash[Slot._5l] = _local2;
            this.hash[Slot._I_f] = new _F_o();
            this.hash[Slot._0M_o] = new _oE_();
            this.hash[Slot._nv] = _local1;
            this.hash[Slot._0C_9] = new _J_5();
            this.hash[Slot._wf] = new _M_();
            this.hash[Slot._xv] = new _X_7();
            this.hash[Slot._hR_] = new _vD_();
            this.hash[Slot._2X_] = new _5q();
            this.hash[Slot._lc] = new _W_C_();
        }
        public function _hS_(_arg1:XML, _arg2:XML):_R_N_{
            var _local3:int = int(_arg1.SlotType);
            var _local4:_U_y = this.hash[_local3];
            var _local5:_R_N_ = new _R_N_();
            if (_local4 != null)
            {
                _local4._N_Q_(_arg1, _arg2);
                _local5.text = _local4._t4;
                _local5._5n = _local4._5n;
                _local5._P_3 = _local4._P_3;
            };
            return (_local5);
        }

    }
}//package _E_7

