// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_F_1._C_Q_

package _F_1{
    import _sp._aJ_;
    import com.company.ui.SimpleText;
    import com.company.assembleegameclient.appengine._0K_R_;
    import _02t._R_f;
    import com.company.rotmg.graphics.TitleScreenGraphic;
    import _ke._0M_1;
    import flash.events.MouseEvent;
    import flash.system.Capabilities;
    import com.company.assembleegameclient.parameters.Parameters;
    import flash.filters.DropShadowFilter;

    public class _C_Q_ extends _05p {

        public var _ft:_aJ_;
        private var _qR_:_H_o;
        private var _2z:_H_o;
        private var _2x:_H_o;
        private var _0_W_:_H_o;
        private var _bP_:_H_o;
        private var _04A_:_H_o;
        private var _8k:_H_o;
        private var _T_w:SimpleText;
        private var _06K_:SimpleText;
        private var _T_1:_0K_R_;

        public function _C_Q_(){
            addChild(new _R_f());
            super(_C_Q_);
            addChild(new TitleScreenGraphic());
            this._ft = new _aJ_(String);
        }
        override public function initialize(_arg1:_0K_R_):void{
            super.initialize(_arg1);
            this._T_1 = _arg1;
            this._qR_ = new _H_o(_0M_1.PLAY, 36, true);
            this._qR_.addEventListener(MouseEvent.CLICK, this._021);
            addChild(this._qR_);
            this._2z = new _H_o(_0M_1.SERVERS, 22, false);
            this._2z.addEventListener(MouseEvent.CLICK, this._021);
            addChild(this._2z);
            this._2x = new _H_o(_0M_1.CREDITS, 22, false);
            this._2x.addEventListener(MouseEvent.CLICK, this._021);
            addChild(this._2x);
            this._0_W_ = new _H_o(_0M_1.ACCOUNT, 22, false);
            this._0_W_.addEventListener(MouseEvent.CLICK, this._021);
            addChild(this._0_W_);
            this._bP_ = new _H_o(_0M_1.LEGENDS, 22, false);
            this._bP_.addEventListener(MouseEvent.CLICK, this._021);
            addChild(this._bP_);
            this._04A_ = new _H_o(_0M_1.EDITOR, 22, false);
            this._04A_.addEventListener(MouseEvent.CLICK, this._021);
            this._04A_.visible = _arg1._V_v;
            addChild(this._04A_);
            this._8k = new _H_o(_0M_1.QUIT, 22, false);
            this._8k.addEventListener(MouseEvent.CLICK, this._021);
            this._8k.visible = (Capabilities.playerType == "Desktop");
            addChild(this._8k);
            this._T_w = new SimpleText(12, 0x7F7F7F, false, 0, 0, "Myriad Pro");
            this._T_w.htmlText = ((((Parameters.isTesting) ? ((Parameters._I_O_()) ? "Testing " : "") : '<font color="#ff0000">DEVELOPMENT</font> ') + "v") + Parameters._0A_G_);
            this._T_w.updateMetrics();
            this._T_w.filters = [new DropShadowFilter(0, 0, 0)];
            addChild(this._T_w);
            this._06K_ = new SimpleText(12, 0x7F7F7F, false, 0, 0, "Myriad Pro");
            this._06K_.text = "© 2014 by Nilly! <3.";
            this._06K_.updateMetrics();
            this._06K_.filters = [new DropShadowFilter(0, 0, 0)];
            addChild(this._06K_);
            this._qR_.x = ((800 / 2) - (this._qR_.width / 2));
            this._qR_.y = 520;
            this._2z.x = (((800 / 2) - (this._2z.width / 2)) - 94);
            this._2z.y = 532;
            this._2x.x = 180;
            this._2x.y = 532;
            this._0_W_.x = (((800 / 2) - (this._0_W_.width / 2)) + 96);
            this._0_W_.y = 532;
            this._bP_.x = 550;
            this._bP_.y = 532;
            this._04A_.x = 100;
            this._04A_.y = 532;
            this._8k.x = 645;
            this._8k.y = 532;
            this._T_w.y = (600 - this._T_w.height);
            this._06K_.x = (800 - this._06K_.width);
            this._06K_.y = (600 - this._06K_.height);
        }
        private function _021(_arg1:MouseEvent):void{
            var _local2:_H_o = (_arg1.target as _H_o);
            this._ft.dispatch(_local2.name);
        }

    }
}//package _F_1

