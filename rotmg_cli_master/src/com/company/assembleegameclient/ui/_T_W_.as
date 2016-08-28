// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.ui._T_W_

package com.company.assembleegameclient.ui{
    import flash.display.Sprite;
    import com.company.assembleegameclient.game.GameSprite;
    import com.company.assembleegameclient.objects.Player;
    import flash.display.Bitmap;
    import com.company.ui.SimpleText;
    import flash.filters.DropShadowFilter;
    import com.company.assembleegameclient.parameters.Parameters;
    import com.company.util.AssetLibrary;
    import flash.events.MouseEvent;
    import _0C_P_.Options;

    public class _T_W_ extends Sprite {

        private var gs_:GameSprite;
        private var go_:Player;
        private var w_:int;
        private var h_:int;
        private var _tm:Bitmap;
        private var nameText_:SimpleText;
        private var _L_P_:Sprite;
        private var _nw:_rN_ = null;
        private var _sI_:_0M_Y_;
        private var _U_U_:_0M_Y_;
        private var _023:_0M_Y_;
        private var _F_C_:_0M_Y_;
        private var _086:Stats;
        public var _e9:Inventory;
        public var _0E__:int;

        public function _T_W_(_arg1:GameSprite, _arg2:Player, _arg3:int, _arg4:int){
            this.gs_ = _arg1;
            this.go_ = _arg2;
            this.w_ = _arg3;
            this.h_ = _arg4;
            this._tm = new Bitmap(null);
            this._tm.x = -2;
            this._tm.y = -8;
            addChild(this._tm);
            this.nameText_ = new SimpleText(20, 0xB3B3B3, false, 0, 0, "Myriad Pro");
            this.nameText_.setBold(true);
            this.nameText_.x = 36;
            this.nameText_.y = 0;
            if (this.gs_.charList_.name_ == null)
            {
                this.nameText_.text = this.go_.name_;
            } else
            {
                this.nameText_.text = this.gs_.charList_.name_;
            };
            this.nameText_.updateMetrics();
            this.nameText_.filters = [new DropShadowFilter(0, 0, 0)];
            addChild(this.nameText_);
            if (this.gs_.gsc_.gameId_ != Parameters.NEXUS_ID)
            {
                this._nw = new _rN_(AssetLibrary._xK_("lofiInterfaceBig", 6), "Nexus", "escapeToNexus");
                this._nw.addEventListener(MouseEvent.CLICK, this._Q_C_);
                this._nw.x = 172;
                this._nw.y = 4;
                addChild(this._nw);
            } else
            {
                this._nw = new _rN_(AssetLibrary._xK_("lofiInterfaceBig", 5), "Options", "options");
                this._nw.addEventListener(MouseEvent.CLICK, this._nD_);
                this._nw.x = 172;
                this._nw.y = 4;
                addChild(this._nw);
            };
            this._sI_ = new _0M_Y_(176, 16, 5931045, 0x545454, "Lvl X");
            this._sI_.x = 12;
            this._sI_.y = 32;
            addChild(this._sI_);
            this._sI_.visible = true;
            this._U_U_ = new _0M_Y_(176, 16, 0xE25F00, 0x545454, "Fame");
            this._U_U_.x = 12;
            this._U_U_.y = 32;
            addChild(this._U_U_);
            this._U_U_.visible = false;
            this._023 = new _0M_Y_(176, 16, 14693428, 0x545454, "HP");
            this._023.x = 12;
            this._023.y = 56;
            addChild(this._023);
            this._F_C_ = new _0M_Y_(176, 16, 6325472, 0x545454, "MP");
            this._F_C_.x = 12;
            this._F_C_.y = 80;
            addChild(this._F_C_);
            this._086 = new Stats(180, 46);
            this._086.x = 22;
            this._086.y = 88;
            addChild(this._086);
            this._e9 = new Inventory(_arg1, _arg2, "Inventory", _arg2._9A_, 12, true);
            this._e9.x = 14;
            this._e9.y = 164;
            addChild(this._e9);
            mouseEnabled = false;
            this.draw();
        }
        public function setName(_arg1:String):void{
            this.nameText_.text = _arg1;
            this.nameText_.updateMetrics();
        }
        private function _Q_C_(_arg1:MouseEvent):void{
            this.gs_.gsc_._M_6();
            Parameters.data_.needsRandomRealm = false;
            Parameters.save();
        }
        private function _nD_(_arg1:MouseEvent):void{
            this.gs_.mui_.clearInput();
            this.gs_.addChild(new Options(this.gs_));
        }
        public function draw():void{
            this._tm.bitmapData = this.go_.getPortrait();
            var _local1:String = ("Lvl " + this.go_._81);
            if (_local1 != this._sI_.labelText_.text)
            {
                this._sI_.labelText_.text = _local1;
                this._sI_.labelText_.updateMetrics();
            };
            if (this.go_._81 != 20)
            {
                if (!this._sI_.visible)
                {
                    this._sI_.visible = true;
                    this._U_U_.visible = false;
                };
                this._sI_.draw(this.go_.exp_, this.go_._7V_, 0);
                if (this._0E__ != this.go_._gz)
                {
                    this._0E__ = this.go_._gz;
                    this._sI_._Y_r(this._0E__);
                };
            } else
            {
                if (!this._U_U_.visible)
                {
                    this._U_U_.visible = true;
                    this._sI_.visible = false;
                };
                this._U_U_.draw(this.go_._0L_o, this.go_._n8, 0);
            };
            this._023.draw(this.go_._aY_, this.go_._L_T_, this.go_._P_7, this.go_._uR_);
            this._F_C_.draw(this.go_._86, this.go_._a7, this.go_._0D_G_, this.go_._dt);
            this._086.draw(this.go_);
            this._e9.draw(this.go_._zq);
        }
        public function destroy():void{
        }

    }
}//package com.company.assembleegameclient.ui

