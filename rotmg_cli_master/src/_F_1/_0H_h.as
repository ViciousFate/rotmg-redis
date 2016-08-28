// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_F_1._0H_h

package _F_1{
    import flash.display.Sprite;
    import _sp._aJ_;
    import com.company.ui.SimpleText;
    import com.company.rotmg.graphics.StackedLogoR;
    import _02t._R_f;
    import com.company.rotmg.graphics.ScreenGraphic;
    import flash.filters.DropShadowFilter;
    import flash.events.MouseEvent;
    import flash.net.navigateToURL;
    import flash.net.URLRequest;
    import flash.events.Event;
    import flash.display.Bitmap;
    import com.company.rotmg.graphics.NillyLogo;

    public class _0H_h extends Sprite {

        private static const _088:String = "http://www.wildshadow.com/";
        private static const _0L_O_:String = "http://www.kabam.com/";

        public var close:_aJ_;
        private var _0H_L_:SimpleText;
        private var _01q:StackedLogoR;
        private var _00z:Bitmap;
        private var _045:_H_o;

        public function _0H_h(){
            this.close = new _aJ_();
            addChild(new _R_f());
            addChild(new ScreenGraphic());
            this._0H_L_ = new SimpleText(16, 0xB3B3B3, false, 0, 0, "Myriad Pro");
            this._0H_L_.setBold(true);
            this._0H_L_.text = "Developed by:";
            this._0H_L_.updateMetrics();
            this._0H_L_.filters = [new DropShadowFilter(0, 0, 0)];
            addChild(this._0H_L_);
            this._01q = new StackedLogoR();
            this._01q.scaleX = (this._01q.scaleY = 1.2);
            this._01q.addEventListener(MouseEvent.CLICK, this._W_w);
            this._01q.buttonMode = true;
            this._01q.useHandCursor = true;
            addChild(this._01q);
			this._00z = new Bitmap(new NillyLogo());
            this._00z.scaleX = (this._00z.scaleY = 1);
            this._00z.addEventListener(MouseEvent.CLICK, this._i0);
            //this._00z.buttonMode = true;
            //this._00z.useHandCursor = true;
            addChild(this._00z);
            this._045 = new _H_o("close", 36, false);
            this._045.addEventListener(MouseEvent.CLICK, this._ly);
            addChild(this._045);
        }
        public function initialize():void{
            stage;
            this._0H_L_.x = ((800 / 2) - (this._0H_L_.width / 2));
            this._0H_L_.y = 10;
            stage;
            this._01q.x = ((800 / 2) - (this._01q.width / 2));
            this._01q.y = 50;
            stage;
            this._00z.x = ((800 / 2) - (this._00z.width / 2));
            this._00z.y = 290;
            stage;
            this._045.x = ((800 / 2) - (this._045.width / 2));
            this._045.y = 524;
        }
        protected function _W_w(_arg1:Event):void{
            navigateToURL(new URLRequest(_088), "_blank");
        }
        protected function _i0(_arg1:Event):void{
            navigateToURL(new URLRequest(_0L_O_), "_blank");
        }
        private function _ly(_arg1:Event):void{
            this.close.dispatch();
        }

    }
}//package _F_1

