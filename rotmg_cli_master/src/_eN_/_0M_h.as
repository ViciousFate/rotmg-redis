// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_eN_._0M_h

package _eN_{
    import _qN_._9j;
    import com.company.ui.SimpleText;
    import _F_1._H_o;
    import _qN_.Account;
    import _0B_2._1T_;
    import flash.filters.DropShadowFilter;

    public class _0M_h extends _9j {

        private static const _Y_J_:int = 18;

        private var _cY_:SimpleText = null;
        private var _0D_P_:_H_o = null;

        public function _0M_h(){
            this.refresh();
        }
        override public function refresh():void{
            if (this._cY_ != null)
            {
                removeChild(this._cY_);
                this._cY_ = null;
            };
            var _local1:_1T_ = (Account._get() as _1T_);
            this._cY_ = new SimpleText(_Y_J_, 0xB3B3B3, false, 0, 0, "Myriad Pro");
            this._cY_.text = (("logged in as " + _local1._0X_()) + " on Kabam.com");
            this._cY_.updateMetrics();
            this._cY_.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4)];
            addChild(this._cY_);
            if (this._0D_P_ != null)
            {
                this._0D_P_.x = width;
                addChild(this._0D_P_);
            };
        }

    }
}//package _eN_

