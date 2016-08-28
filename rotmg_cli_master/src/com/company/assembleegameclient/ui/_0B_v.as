// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.ui._0B_v

package com.company.assembleegameclient.ui{
    import flash.display.Sprite;
    import com.company.ui.SimpleText;
    import flash.display.Bitmap;
    import com.company.assembleegameclient.game.GameSprite;
    import flash.filters.DropShadowFilter;
    import com.company.util.AssetLibrary;
    import flash.display.BitmapData;
    import com.company.assembleegameclient.util.TextureRedrawer;
    import com.company.assembleegameclient.util._Z_B_;
    import flash.events.MouseEvent;
    import com.company.assembleegameclient.parameters.Parameters;
    import _qN_.Account;

    public class _0B_v extends Sprite {

        private static const _Y_J_:int = 18;

        private var _ho:SimpleText;
        private var _1x:SimpleText;
        private var _0_V_:Bitmap;
        private var _0H_:Bitmap;
        private var credits_:int = -1;
        private var _Q_7:int = -1;
        private var _sx:GameSprite;

        public function _0B_v(_arg1:GameSprite=null){
            this._sx = _arg1;
            this._ho = new SimpleText(_Y_J_, 0xFFFFFF, false, 0, 0, "Myriad Pro");
            this._ho.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4, 2)];
            addChild(this._ho);
            var _local2:BitmapData = AssetLibrary._xK_("lofiObj3", 225);
            _local2 = TextureRedrawer.redraw(_local2, 40, true, 0, 0);
            this._0_V_ = new Bitmap(_local2);
            addChild(this._0_V_);
            this._1x = new SimpleText(_Y_J_, 0xFFFFFF, false, 0, 0, "Myriad Pro");
            this._1x.filters = [new DropShadowFilter(0, 0, 0, 1, 4, 4, 2)];
            addChild(this._1x);
            this._0H_ = new Bitmap(_Z_B_._qf());
            addChild(this._0H_);
            this.draw(0, 0);
            mouseEnabled = true;
            doubleClickEnabled = true;
            addEventListener(MouseEvent.DOUBLE_CLICK, this._D_7, false, 0, true);
        }
        private function _D_7(_arg1:MouseEvent):void{
            if (((((!(this._sx)) || (this._sx._vw()))) || ((Parameters.data_.clickForGold == true))))
            {
                Account._get().showMoneyManagement(stage);
            };
        }
        public function draw(_arg1:int, _arg2:int):void{
            if ((((_arg1 == this.credits_)) && ((_arg2 == this._Q_7))))
            {
                return;
            };
            this.credits_ = _arg1;
            this._Q_7 = _arg2;
            this._0_V_.x = -(this._0_V_.width);
            this._ho.text = this.credits_.toString();
            this._ho.updateMetrics();
            this._ho.x = ((this._0_V_.x - this._ho.width) + 8);
            this._ho.y = ((this._0_V_.height / 2) - (this._ho.height / 2));
            this._0H_.x = (this._ho.x - this._0H_.width);
            this._1x.text = this._Q_7.toString();
            this._1x.updateMetrics();
            this._1x.x = ((this._0H_.x - this._1x.width) + 8);
            this._1x.y = ((this._0H_.height / 2) - (this._1x.height / 2));
        }

    }
}//package com.company.assembleegameclient.ui

