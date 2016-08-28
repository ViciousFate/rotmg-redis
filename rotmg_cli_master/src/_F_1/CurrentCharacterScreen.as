// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_F_1.CurrentCharacterScreen

package _F_1{
    import _sp._aJ_;
    import com.company.assembleegameclient.appengine._0K_R_;
    import com.company.ui.SimpleText;
    import com.company.assembleegameclient.ui.TextButton;
    import com.company.assembleegameclient.ui._0B_v;
    import _nA_._ax;
    import flash.display.Shape;
    import com.company.assembleegameclient.ui._0K_B_;
    import _02t._R_f;
    import _S_K_._u3;
    import flash.events.MouseEvent;
    import flash.filters.DropShadowFilter;
    import flash.events.Event;
    import com.company.rotmg.graphics.ScreenGraphic;
    import flash.display.Graphics;
    import com.company.assembleegameclient.screens.charrects.CharacterRect;
    import _0L_C_._4B_;
    import _qN_.Account;
    import _0L_C_._qO_;
    import _0D_d._T_K_;

    public class CurrentCharacterScreen extends _05p {

        public var close:_aJ_;
        public var _n7:_aJ_;
        public var newCharacter:_aJ_;
        public var _1V_:_aJ_;
        public var _O_v:_aJ_;
        public var _D_u:_aJ_;
        private var _T_1:_0K_R_;
        private var nameText_:SimpleText;
        private var _0_9:TextButton;
        private var _H_t:_0B_v;
        private var _A_e:SimpleText;
        private var _Q_f:SimpleText;
        private var _X_0:CharsAndNews;
        public var _lR_:_ax;
        private var _77:Number;
        private var _qR_:_H_o;
        private var _p6:_H_o;
        private var _0G_N_:_H_o;
        private var _dL_:Shape;
        private var _E_k:_0K_B_;

        public function CurrentCharacterScreen(){
            addChild(new _R_f());
            this._qR_ = new _H_o("play", 36, true);
            this._p6 = new _H_o("main", 22, false);
            this._0G_N_ = new _H_o("classes", 22, false);
            super(CurrentCharacterScreen);
            this._n7 = new _aJ_();
            this.newCharacter = new _aJ_();
            this.close = new _u3(this._p6, MouseEvent.CLICK);
            this._1V_ = new _u3(this._0G_N_, MouseEvent.CLICK);
            this._O_v = new _aJ_();
            this._D_u = new _aJ_();
        }
        override public function initialize(_arg1:_0K_R_):void{
            this._T_1 = _arg1;
            super.initialize(_arg1);
            this.nameText_ = new SimpleText(22, 0xB3B3B3, false, 0, 0, "Myriad Pro");
            this.nameText_.setBold(true);
            this.nameText_.text = _arg1.name_;
            this.nameText_.updateMetrics();
            this.nameText_.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
            this.nameText_.y = 24;
            stage;
            this.nameText_.x = ((800 / 2) - (this.nameText_.width / 2));
            addChild(this.nameText_);
            if (!_arg1._hv)
            {
                this._0_9 = new TextButton(16, false, "choose name");
                this._0_9.addEventListener(MouseEvent.CLICK, this._fT_);
                this._0_9.y = 50;
                stage;
                this._0_9.x = ((800 / 2) - (this._0_9.width / 2));
                addChild(this._0_9);
            };
            this._H_t = new _0B_v();
            this._H_t.draw(_arg1.credits_, _arg1._Q_7);
            stage;
            this._H_t.x = 800;
            this._H_t.y = 20;
            addChild(this._H_t);
            this._A_e = new SimpleText(18, 0xB3B3B3, false, 0, 0, "Myriad Pro");
            this._A_e.setBold(true);
            this._A_e.text = "Select a Character";
            this._A_e.updateMetrics();
            this._A_e.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
            this._A_e.x = 18;
            this._A_e.y = 72;
            addChild(this._A_e);
            this._Q_f = new SimpleText(18, 0xB3B3B3, false, 0, 0, "Myriad Pro");
            this._Q_f.setBold(true);
            this._Q_f.text = "News";
            this._Q_f.updateMetrics();
            this._Q_f.filters = [new DropShadowFilter(0, 0, 0, 1, 8, 8)];
            this._Q_f.x = 344;
            this._Q_f.y = 72;
            addChild(this._Q_f);
            this._X_0 = new CharsAndNews(_arg1, this);
            this._X_0.y = 101;
            this._77 = this._X_0.height;
            addChild(this._X_0);
            if (this._77 > 400)
            {
                this._E_k = new _0K_B_(16, 400);
                this._E_k.x = ((800 - this._E_k.width) - 4);
                this._E_k.y = 104;
                this._E_k._fA_(400, this._X_0.height);
                this._E_k.addEventListener(Event.CHANGE, this._A_E_);
                addChild(this._E_k);
            };
            addChild(new ScreenGraphic());
            this._dL_ = new Shape();
            var _local2:Graphics = this._dL_.graphics;
            _local2.clear();
            _local2.lineStyle(2, 0x545454);
            _local2.moveTo(0, 100);
            stage;
            _local2.lineTo(800, 100);
            _local2.moveTo((CharacterRect.WIDTH + 8), 100);
            _local2.lineTo((CharacterRect.WIDTH + 8), 526);
            _local2.lineStyle();
            addChild(this._dL_);
            this._qR_.addEventListener(MouseEvent.CLICK, this._04P_);
            stage;
            this._qR_.x = ((800 / 2) - (this._qR_.width / 2));
            this._qR_.y = 520;
            addChild(this._qR_);
            stage;
            this._p6.x = (((800 / 2) - (this._p6.width / 2)) - 94);
            this._p6.y = 532;
            addChild(this._p6);
            stage;
            this._0G_N_.x = (((800 / 2) - (this._0G_N_.width / 2)) + 96);
            this._0G_N_.y = 532;
            addChild(this._0G_N_);
        }
        private function _fT_(_arg1:Event):void{
            var _local3:_4B_;
            if (!Account._get().isRegistered())
            {
                _local3 = new _4B_();
                _local3.addEventListener(_qO_.BUTTON1_EVENT, this._4);
                _local3.addEventListener(_qO_.BUTTON2_EVENT, this.onDialogRegister);
                addChild(_local3);
                return;
            };
            var _local2:_T_K_ = new _T_K_();
            _local2.addEventListener(Event.COMPLETE, this._0G_V_);
            addChild(_local2);
        }
        private function _0G_V_(_arg1:Event):void{
            this._O_v.dispatch();
        }
        private function _4(_arg1:Event):void{
            var _local2:_qO_ = (_arg1.currentTarget as _qO_);
            removeChild(_local2);
        }
        private function onDialogRegister(_arg1:Event):void{
            var _local2:_qO_ = (_arg1.currentTarget as _qO_);
            removeChild(_local2);
            _0j();
        }
        private function _A_E_(_arg1:Event):void{
            this._X_0._0D__((-(this._E_k._Q_D_()) * (this._77 - 400)));
        }
        public function _u_():void{
            this._lR_ = new _ax();
            this._lR_.x = 35;
            this._lR_.y = 32;
            addChild(this._lR_);
        }
        private function _04P_(_arg1:Event):void{
            if (this._T_1.numChars_ == 0)
            {
                this.newCharacter.dispatch();
            } else
            {
                this._D_u.dispatch();
            };
        }

    }
}//package _F_1

