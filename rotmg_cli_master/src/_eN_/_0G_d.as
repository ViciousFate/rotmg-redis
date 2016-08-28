// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_eN_._0G_d

package _eN_{
    import _0D_d.Frame;
    import com.company.ui.SimpleText;
    import flash.filters.DropShadowFilter;
    import _qN_.Account;
    import _0B_2._1T_;
    import flash.events.MouseEvent;

    class _0G_d extends Frame {

        public var _lv:SimpleText;
        public var _ba:SimpleText;

        public function _0G_d(){
            super("Current account", "", "Continue", "/currentKabamLogin");
            this._lv = new SimpleText(18, 0xB3B3B3, false, 0, 0, "Myriad Pro");
            this._lv.setBold(true);
            this._lv.text = "Currently logged on Kabam.com as:";
            this._lv.updateMetrics();
            this._lv.filters = [new DropShadowFilter(0, 0, 0)];
            this._lv.y = (h_ - 60);
            this._lv.x = 17;
            addChild(this._lv);
            this._ba = new SimpleText(16, 0xB3B3B3, false, 238, 30, "Myriad Pro");
            this._ba.text = (Account._get() as _1T_)._0X_();
            this._ba.updateMetrics();
            this._ba.y = (h_ - 30);
            this._ba.x = 17;
            addChild(this._ba);
            h_ = (h_ + 88);
            w_ = (w_ + 60);
            Button2.addEventListener(MouseEvent.CLICK, this._03t);
        }
        private function _03t(_arg1:MouseEvent):void{
            dispatchEvent(new _1(_1.DONE));
        }

    }
}//package _eN_

