// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.ui.Slot

package com.company.assembleegameclient.ui{
    import flash.display.Sprite;
    import flash.filters.ColorMatrixFilter;
    import com.company.util.MoreColorUtil;
    import flash.display.Bitmap;
    import flash.display.GraphicsSolidFill;
    import flash.display.GraphicsPath;

    import flash.display.IGraphicsData;
    import com.company.util.GraphicHelper;
    import flash.geom.Point;
    import com.company.ui.SimpleText;
    import flash.geom.Matrix;
    import flash.display.BitmapData;
    import com.company.util.AssetLibrary;


    public class Slot extends Sprite {

        public static const __get:int = 0;
        public static const _Z_f:int = 1;
        public static const _J_S_:int = 2;
        public static const _rm:int = 3;
        public static const _L_0:int = 4;
        public static const _ca:int = 5;
        public static const _jP_:int = 6;
        public static const _05C_:int = 7;
        public static const _0J_a:int = 8;
        public static const _yW_:int = 9;
        public static const _U_l:int = 10;
        public static const _3n:int = 11;
        public static const _G_V_:int = 12;
        public static const _06u:int = 13;
        public static const _5l:int = 14;
        public static const _I_f:int = 15;
        public static const _0M_o:int = 16;
        public static const _nv:int = 17;
        public static const _0C_9:int = 18;
        public static const _wf:int = 19;
        public static const _xv:int = 20;
        public static const _hR_:int = 21;
        public static const _2X_:int = 22;
        public static const _lc:int = 23;
        public static const WIDTH:int = 40;
        public static const HEIGHT:int = 40;
        public static const BORDER:int = 4;
        private static const _0I_E_:ColorMatrixFilter = new ColorMatrixFilter(MoreColorUtil._fL_(0x363636));

        public var type_:int;
        public var _ws:int;
        public var _07i:Array;
        public var _0H_K_:Bitmap;
        protected var _04c:GraphicsSolidFill;
        protected var path_:GraphicsPath;
        private var graphicsData_:Vector.<IGraphicsData>;

        public function Slot(_arg1:int, _arg2:int, _arg3:Array){
            this._04c = new GraphicsSolidFill(0x545454, 1);
            this.path_ = new GraphicsPath(new Vector.<int>(), new Vector.<Number>());
            this.graphicsData_ = new <IGraphicsData>[this._04c, this.path_, GraphicHelper.END_FILL];
            super();
            this.type_ = _arg1;
            this._ws = _arg2;
            this._07i = _arg3;
            this._rC_();
        }
        public static function _mb(_arg1:int):String{
            switch (_arg1)
            {
                case __get:
                    return ("Any");
                case _Z_f:
                    return ("Sword");
                case _J_S_:
                    return ("Dagger");
                case _rm:
                    return ("Bow");
                case _L_0:
                    return ("Tome");
                case _ca:
                    return ("Shield");
                case _jP_:
                    return ("Leather Armor");
                case _05C_:
                    return ("Armor");
                case _0J_a:
                    return ("Wand");
                case _yW_:
                    return ("Accessory");
                case _U_l:
                    return ("Potion");
                case _3n:
                    return ("Spell");
                case _G_V_:
                    return ("Holy Seal");
                case _06u:
                    return ("Cloak");
                case _5l:
                    return ("Robe");
                case _I_f:
                    return ("Quiver");
                case _0M_o:
                    return ("Helm");
                case _nv:
                    return ("Staff");
                case _0C_9:
                    return ("Poison");
                case _wf:
                    return ("Skull");
                case _xv:
                    return ("Trap");
                case _hR_:
                    return ("Orb");
                case _2X_:
                    return ("Prism");
                case _lc:
                    return ("Scepter");
            };
            return ("Invalid Type!");
        }

        protected function _H_K_(_arg1:int, _arg2:int, _arg3:Boolean):Point{
            var _local4:Point = new Point();
            switch (_arg2)
            {
                case _yW_:
                    _local4.x = (((_arg1)==2878) ? 0 : -2);
                    _local4.y = ((_arg3) ? -2 : 0);
                    break;
                case _3n:
                    _local4.y = -2;
                    break;
            };
            return (_local4);
        }
        protected function _rC_():void{
            var _local4:Point;
            var _local5:SimpleText;
            var _local6:Matrix;
            GraphicHelper._0L_6(this.path_);
            GraphicHelper.drawUI(0, 0, WIDTH, HEIGHT, 4, this._07i, this.path_);
            graphics.clear();
            graphics.drawGraphicsData(this.graphicsData_);
            var _local1:BitmapData;
            var _local2:int;
            var _local3:int;
            switch (this.type_)
            {
                case __get:
                    break;
                case _Z_f:
                    _local1 = AssetLibrary._xK_("lofiObj5", 48);
                    break;
                case _J_S_:
                    _local1 = AssetLibrary._xK_("lofiObj5", 96);
                    break;
                case _rm:
                    _local1 = AssetLibrary._xK_("lofiObj5", 80);
                    break;
                case _L_0:
                    _local1 = AssetLibrary._xK_("lofiObj6", 80);
                    break;
                case _ca:
                    _local1 = AssetLibrary._xK_("lofiObj6", 112);
                    break;
                case _jP_:
                    _local1 = AssetLibrary._xK_("lofiObj5", 0);
                    break;
                case _05C_:
                    _local1 = AssetLibrary._xK_("lofiObj5", 32);
                    break;
                case _0J_a:
                    _local1 = AssetLibrary._xK_("lofiObj5", 64);
                    break;
                case _yW_:
                    _local1 = AssetLibrary._xK_("lofiObj", 44);
                    break;
                case _3n:
                    _local1 = AssetLibrary._xK_("lofiObj6", 64);
                    break;
                case _G_V_:
                    _local1 = AssetLibrary._xK_("lofiObj6", 160);
                    break;
                case _06u:
                    _local1 = AssetLibrary._xK_("lofiObj6", 32);
                    break;
                case _5l:
                    _local1 = AssetLibrary._xK_("lofiObj5", 16);
                    break;
                case _I_f:
                    _local1 = AssetLibrary._xK_("lofiObj6", 48);
                    break;
                case _0M_o:
                    _local1 = AssetLibrary._xK_("lofiObj6", 96);
                    break;
                case _nv:
                    _local1 = AssetLibrary._xK_("lofiObj5", 112);
                    break;
                case _0C_9:
                    _local1 = AssetLibrary._xK_("lofiObj6", 128);
                    break;
                case _wf:
                    _local1 = AssetLibrary._xK_("lofiObj6", 0);
                    break;
                case _xv:
                    _local1 = AssetLibrary._xK_("lofiObj6", 16);
                    break;
                case _hR_:
                    _local1 = AssetLibrary._xK_("lofiObj6", 144);
                    break;
                case _2X_:
                    _local1 = AssetLibrary._xK_("lofiObj6", 176);
                    break;
                case _lc:
                    _local1 = AssetLibrary._xK_("lofiObj6", 192);
                    break;
            };
            if (this._0H_K_ == null)
            {
                if (_local1 != null)
                {
                    _local4 = this._H_K_(-1, this.type_, true);
                    this._0H_K_ = new Bitmap(_local1);
                    this._0H_K_.x = (BORDER + _local4.x);
                    this._0H_K_.y = (BORDER + _local4.y);
                    this._0H_K_.scaleX = 4;
                    this._0H_K_.scaleY = 4;
                    this._0H_K_.filters = [_0I_E_];
                    addChild(this._0H_K_);
                } else
                {
                    if (this._ws > 0)
                    {
                        _local5 = new SimpleText(26, 0x363636, false, 0, 0, "Myriad Pro");
                        _local5.text = String(this._ws);
                        _local5.setBold(true);
                        _local5.updateMetrics();
                        _local1 = new BitmapData(26, 30, true, 0);
                        _local6 = new Matrix();
                        _local1.draw(_local5, _local6);
                        this._0H_K_ = new Bitmap(_local1);
                        this._0H_K_.x = ((WIDTH / 2) - (_local5.width / 2));
                        this._0H_K_.y = ((HEIGHT / 2) - 18);
                        addChild(this._0H_K_);
                    };
                };
            };
        }

    }
}//package com.company.assembleegameclient.ui

